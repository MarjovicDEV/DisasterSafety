using System;
using System.Collections;
using System.IO;
using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// Connects the Level scene to an Attempt (Part 3 of Chapter III) and to LevelCompletion
    /// (Part 4). All rules live in Core; this class only shows screens and forwards taps.
    /// Every Scenario uses the Student flow: the Caption, the Action Bar and the Student acting out each Choice.
    /// </summary>
    public sealed class LevelController : MonoBehaviour
    {
        public const float OpenedLineDelay = 2.5f;
        const float ReactionSeconds = 1f;

        [Header("Scene")]
        public LevelSetting[] settings;
        public RectTransform world;
        public Text levelLabel;
        public Button pauseButton;
        public Button hintButton;
        public ScreenPanel dimmer;

        [Header("Student flow")]
        public RectTransform caption;
        public Text captionGoal;
        public Text captionBody;
        public ActionBar actionBar;
        public Vector2 studentHintPosition = new Vector2(0f, -240f);

        [Header("Feedback")]
        public ScreenPanel feedbackPanel;
        public Image feedbackBanner;
        public Sprite safeBanner;
        public Sprite unsafeBanner;
        public Text feedbackHeading;
        public Text feedbackText;
        public Button tryAgainButton;
        public Button finishButton;

        [Header("Level Completed")]
        public ScreenPanel completedPanel;
        [Tooltip("The Student who cheers in Level Completed.")]
        public StudentActor completedStudent;
        public SpeechBubble completedBubble;
        public StarReveal stars;
        public GameObject openedRibbon;
        public Text openedText;
        public Text safeChoiceText;
        public Text lessonText;
        public Button replayButton;
        public Button nextButton;
        public Button nextEpisodeButton;
        public Button levelsButton;
        public Button menuButton;
        public Confetti confetti;

        [Header("Pause")]
        public ScreenPanel pausePanel;
        public Button resumeButton;
        public Button restartButton;
        public Text pauseLevelLabel;
        public Button pauseLevelsButton;
        public Button pauseMainMenuButton;

        LevelId level;
        Scenario scenario;
        Attempt attempt;
        LevelId nextLevel;
        bool hasNext;

        public Attempt Attempt => attempt;
        public LevelId Level => level;
        public LevelSetting ActiveSetting { get; private set; }
        public CompletionResult LastResult { get; private set; }
        public bool IsPaused => pausePanel != null && pausePanel.IsVisible;

        public StudentActor Student => ActiveSetting != null ? ActiveSetting.student : null;

        void Awake()
        {
            GameServices.Initialize();
            AudioManager.EnsureExists();
            level = GameServices.CurrentLevel;
            if (GameServices.Scenarios.TryGet(level, out scenario))
                attempt = new Attempt(scenario);
            Wire();
        }

        void OnDestroy() => Time.timeScale = 1f;

        IEnumerator Start()
        {
            foreach (var p in new[] { feedbackPanel, completedPanel, pausePanel, dimmer, actionBar != null ? actionBar.panel : null })
                if (p != null) p.SetInstant(false);

            if (attempt == null)
            {
                Debug.LogError("No Playable Level for " + level.Key + "; returning to the Main Menu.");
                GameScenes.ToMainMenu();
                yield break;
            }

            yield return StartStudentLevel();
        }

        // The Student flow -------------------------------------------------------
        IEnumerator StartStudentLevel()
        {
            ShowSetting(ScenarioSettings.ActionArtKey(scenario.setting));
            UseStudentLayout();
            SetLevelLabels(level);
            captionGoal.text = scenario.goal.text;
            captionBody.text = scenario.intro.text;
            FitCaption();
            actionBar.Setup(scenario, ChooseAction);

            ActiveSetting.PrepareStudentStart(level.Key, GameServices.Progress.Student);
            var student = Student;

            var shaker = ActiveSetting.shaker;
            if (scenario.Shakes && shaker != null)
            {
                shaker.StartShake();
                AudioManager.Play(Sfx.Rumble, 0.9f);
                AudioManager.DuckMusic(shaker.shakeSeconds);
                student.Play(StudentPose.Surprised);
                student.Say(UIStrings.ReactionWhoa, shaker.shakeSeconds);
                yield return new WaitForSeconds(shaker.shakeSeconds);
                student.Play(ActiveSetting.StartPose);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
            attempt.StartChoosing();
            actionBar.Show();
        }

        public void SetLevelLabels(LevelId id)
        {
            levelLabel.text = UIStrings.LevelLabel(id);
            pauseLevelLabel.text = UIStrings.PauseLevel(id);
        }

        // The Caption's built width (960) and the room its text has on each side.
        float captionBaseWidth = -1f, captionBaseHeight, captionBodyBaseHeight;
        Font captionBaseGoalFont;
        const float CaptionTextMargin = 80f, CaptionMaxWidth = 1400f, CaptionExtraLine = 48f;

        /// <summary>
        /// Widens the Caption when the Goal would not fit on one line, and makes it taller (moving the Hint down)
        /// when the intro needs two lines, so no text runs into the next or out of the banner. Call it after
        /// setting the Goal and the intro, and after UseStudentLayout.
        /// </summary>
        public void FitCaption()
        {
            var body = captionBody.rectTransform;
            if (captionBaseWidth < 0f)
            {
                captionBaseWidth = caption.sizeDelta.x;
                captionBaseHeight = caption.sizeDelta.y;
                captionBodyBaseHeight = body.sizeDelta.y;
            }
            var goalWidth = PixelText.Width(captionGoal.font, captionGoal.text) + 16f;
            var width = Mathf.Clamp(Mathf.Ceil((goalWidth + CaptionTextMargin) / 8f) * 8f, captionBaseWidth, CaptionMaxWidth);
            var textWidth = width - CaptionTextMargin;
            var extra = PixelText.Lines(captionBody.font, captionBody.text, textWidth) > 1 ? CaptionExtraLine : 0f;

            caption.sizeDelta = new Vector2(width, captionBaseHeight + extra);
            var goal = captionGoal.rectTransform;
            goal.sizeDelta = new Vector2(textWidth, goal.sizeDelta.y);
            body.sizeDelta = new Vector2(textWidth, captionBodyBaseHeight + extra);
            ((RectTransform)hintButton.transform).anchoredPosition = studentHintPosition - new Vector2(0f, extra);
        }

        /// <summary>Shows the Caption in the active Setting's Goal font and puts the Hint under it.</summary>
        public void UseStudentLayout()
        {
            caption.gameObject.SetActive(true);
            var settingGoalFont = ActiveSetting?.captionGoalFont;
            if (settingGoalFont != null)
            {
                captionBaseGoalFont ??= captionGoal.font;
                captionGoal.font = settingGoalFont;
            }
            else if (captionBaseGoalFont != null)
            {
                captionGoal.font = captionBaseGoalFont;
                captionBaseGoalFont = null;
            }

            var hint = (RectTransform)hintButton.transform;
            // The Hint sits under the Caption, over the blackboard: the door, desk, window and cabinet stay clear.
            hint.anchorMin = hint.anchorMax = hint.pivot = new Vector2(0.5f, 1f);
            hint.anchoredPosition = studentHintPosition;
            hint.sizeDelta = new Vector2(260f, 96f);
        }

        void ChooseAction(int index)
        {
            if (attempt == null || IsPaused || !attempt.Choose(index)) return;
            StartCoroutine(ActOut(index));
        }

        IEnumerator ActOut(int index)
        {
            var choice = scenario.choices[index];
            var setting = ActiveSetting;
            var student = setting.student;
            var actOut = setting.ActOutFor(choice.action);
            var safe = attempt.LastChoiceSafe;
            actionBar.Hide();
            if (!safe) actionBar.SetTried(index, true);

            // Safe: every step. Unsafe: the steps up to the stop, then "Hmm, wait...".
            if (actOut == null) Debug.LogError("No Act-out '" + choice.action + "' in Setting '" + setting.key + "'.");
            else
            {
                var last = actOut.LastStep(!safe);
                for (var i = 0; i <= last; i++)
                {
                    var step = actOut.steps[i];
                    if (!PlayInstantStep(step, setting)) yield return PlayTimedStep(step, setting);
                }
            }

            setting.PositionReaction(actOut);
            if (safe)
            {
                if (scenario.Shakes) setting.shaker?.Calm();
                student.Say(scenario.phase == ScenarioPhases.Before ? UIStrings.ReactionReady : UIStrings.ReactionSafe, ReactionSeconds + 1f);
                AudioManager.Play(Sfx.Safe);
                Juice.Sparkles((RectTransform)student.transform, 14);
            }
            else
            {
                student.Play(actOut?.stopPose ?? StudentPose.Wait);
                student.Say(UIStrings.ReactionWait, ReactionSeconds + 1f);
                AudioManager.Play(Sfx.Unsafe);
            }

            yield return new WaitForSeconds(ReactionSeconds);
            if (!safe && scenario.Shakes) setting.shaker?.Calm();
            FillFeedback(choice);
            dimmer.Show();
            feedbackPanel.Show();
        }

        // Overlays an Act-out showed, so Try Again can hide them again.
        readonly System.Collections.Generic.List<GameObject> shownOverlays = new System.Collections.Generic.List<GameObject>();

        // The steps that take no time run in the same frame, so a pose, the calm and the Reaction land together.
        bool PlayInstantStep(ActOutStep step, LevelSetting setting)
        {
            var student = setting.student;
            switch (step.kind)
            {
                case ActOutStepKind.Pose:
                    student.Play(step.pose);
                    return step.seconds <= 0f;
                case ActOutStepKind.Calm:
                    if (scenario.Shakes) setting.shaker?.Calm();
                    return true;
                case ActOutStepKind.Face:
                    student.Face(step.faceLeft);
                    return true;
                case ActOutStepKind.Overlay:
                    if (step.target == null) { Debug.LogError("An Overlay step in '" + setting.key + "' has no target."); return true; }
                    step.target.SetActive(true);
                    shownOverlays.Add(step.target);
                    return true;
                case ActOutStepKind.GrownUp:
                    if (step.grownUp == null) { Debug.LogError("A GrownUp step in '" + setting.key + "' has no Grown-up."); return true; }
                    if (step.grownUpAction == GrownUpAction.Turn) { step.grownUp.Face(step.faceLeft); return true; }
                    if (step.grownUpAction == GrownUpAction.Pose)
                    {
                        step.grownUp.Play(step.grownUpPose);
                        return step.seconds <= 0f;
                    }
                    return false;
                case ActOutStepKind.PropShow:
                case ActOutStepKind.PropHide:
                case ActOutStepKind.PropPickUp:
                case ActOutStepKind.PropPlace:
                    var prop = PropOf(step, setting);
                    if (prop == null) return true;
                    if (step.kind == ActOutStepKind.PropShow) prop.Show();
                    else if (step.kind == ActOutStepKind.PropHide) prop.Hide();
                    else if (step.kind == ActOutStepKind.PropPickUp) prop.PickUp(student.Hands);
                    else prop.Place(step.place);
                    return true;
                default:
                    return false;
            }
        }

        IEnumerator PlayTimedStep(ActOutStep step, LevelSetting setting)
        {
            var student = setting.student;
            switch (step.kind)
            {
                case ActOutStepKind.Walk:
                case ActOutStepKind.CarryTo:
                    Coroutine grownUpWalk = null;
                    if (step.grownUp != null)
                    {
                        var destination = step.grownUpDestination;
                        grownUpWalk = StartCoroutine(step.grownUp.WalkTo(destination.x, destination.y));
                    }
                    if (step.kind == ActOutStepKind.CarryTo)
                        yield return student.CarryTo(step.x, step.y);
                    else
                        yield return student.WalkTo(step.x, step.y, PoseActor.WalkSpeed);
                    if (grownUpWalk != null) yield return grownUpWalk;
                    break;
                case ActOutStepKind.FastWalk:
                    yield return student.WalkTo(step.x, step.y, PoseActor.FastWalkSpeed);
                    break;
                case ActOutStepKind.Run:
                    yield return student.WalkTo(step.x, step.y, PoseActor.RunSpeed);
                    break;
                case ActOutStepKind.Crouch:
                    yield return student.CrouchTo(step.x, step.seconds);
                    break;
                case ActOutStepKind.Carry:
                    yield return student.CarryTo(step.x);
                    break;
                case ActOutStepKind.Push:
                    var pushed = PropOf(step, setting);
                    if (pushed == null) break;
                    // The Student keeps its palms on the prop and steps along with it.
                    student.Play(StudentPose.Push);
                    var slide = StartCoroutine(pushed.Slide(step.x * student.unitsPerPixel, step.seconds));
                    var fromX = student.X;
                    for (var t = 0f; t < step.seconds; t += Time.deltaTime)
                    {
                        student.PlaceAt(Mathf.Lerp(fromX, fromX + step.x, t / step.seconds));
                        yield return null;
                    }
                    student.PlaceAt(fromX + step.x);
                    yield return slide;
                    break;
                case ActOutStepKind.Pose:
                case ActOutStepKind.Wait:
                    yield return new WaitForSeconds(step.seconds);
                    break;
                case ActOutStepKind.Say:
                    var seconds = Mathf.Max(1f, step.seconds);
                    student.Say(step.line, seconds + 0.5f);
                    yield return new WaitForSeconds(seconds);
                    break;
                case ActOutStepKind.GrownUp:
                    if (step.grownUpAction == GrownUpAction.Walk) yield return step.grownUp.WalkTo(step.x, step.y);
                    else yield return new WaitForSeconds(step.seconds);
                    break;
                case ActOutStepKind.PropSlide:
                    var prop = PropOf(step, setting);
                    if (prop != null) yield return prop.Slide(step.x * student.unitsPerPixel, step.seconds);
                    break;
            }
        }

        static LevelProp PropOf(ActOutStep step, LevelSetting setting)
        {
            var prop = step.target != null ? step.target.GetComponent<LevelProp>() : null;
            if (prop == null) Debug.LogError("A " + step.kind + " step in '" + setting.key + "' has no LevelProp target.");
            return prop;
        }

        // Try Again: the Student walks back while the props and overlays return to how the Level started.
        IEnumerator ReturnToStart()
        {
            var setting = ActiveSetting;
            var student = Student;
            student.bubble.Hide();
            foreach (var overlay in shownOverlays) if (overlay != null) overlay.SetActive(false);
            shownOverlays.Clear();
            var resets = new System.Collections.Generic.List<Coroutine>();
            foreach (var prop in setting.ActiveProps)
                if (prop != null && !prop.AtStart) resets.Add(StartCoroutine(prop.ResetToStart()));

            foreach (var grownUp in setting.ActiveGrownUps)
                if (grownUp != null && !grownUp.AtStart) resets.Add(StartCoroutine(grownUp.ReturnToStart()));

            if (setting.startCrouchFromX >= 0f)
            {
                // A Student already under the desk only needs the short shuffle back to the leg.
                if (student.X < setting.startCrouchFromX)
                    yield return student.WalkTo(setting.startCrouchFromX, setting.startY, PoseActor.WalkSpeed);
                yield return student.CrouchTo(setting.StartX);
            }
            else
            {
                var act = setting.retraceActOutOnRetry
                    ? setting.ActOutFor(scenario.choices[attempt.LastChoice].action)
                    : null;
                if (act != null)
                {
                    for (var i = act.LastStep(stops: true); i >= 0; i--)
                    {
                        var step = act.steps[i];
                        if (step.kind != ActOutStepKind.Walk && step.kind != ActOutStepKind.FastWalk &&
                            step.kind != ActOutStepKind.Run) continue;
                        if (student.X == step.x && student.Y == step.y) continue;
                        yield return student.WalkTo(step.x, step.y, PoseActor.WalkSpeed);
                    }
                }
                yield return student.WalkTo(setting.StartX, setting.startY, PoseActor.WalkSpeed);
            }

            foreach (var reset in resets) yield return reset;
            if (setting.StartPose != StudentPose.Idle) student.Play(setting.StartPose);
            actionBar.Show();
        }

        public void ShowSetting(string key)
        {
            ActiveSetting = Array.Find(settings, setting => setting.key == key);
            if (ActiveSetting == null)
            {
                Debug.LogError("No Level art group for Setting '" + key + "'.");
                if (settings.Length > 0) ActiveSetting = settings[0];
            }

            foreach (var setting in settings)
                setting.gameObject.SetActive(setting == ActiveSetting);

            if (ActiveSetting == null) return;

            var artAnchor = new Vector2(0.5f, ActiveSetting.artAnchorY);
            world.anchorMin = world.anchorMax = world.pivot = artAnchor;
            world.anchoredPosition = Vector2.zero;
        }

        void Wire()
        {
            hintButton.onClick.AddListener(ShowHint);
            pauseButton.onClick.AddListener(Pause);
            tryAgainButton.onClick.AddListener(TryAgain);
            finishButton.onClick.AddListener(Finish);

            replayButton.onClick.AddListener(() => GameScenes.PlayLevel(level));
            nextButton.onClick.AddListener(() => { if (hasNext) GameScenes.PlayLevel(nextLevel); });
            nextEpisodeButton.onClick.AddListener(() => GameScenes.ToEpisodeSelect(Episode.Fire));
            levelsButton.onClick.AddListener(() => GameScenes.ToLevelSelect(level.Episode, level.Difficulty));
            menuButton.onClick.AddListener(GameScenes.ToMainMenu);

            resumeButton.onClick.AddListener(Resume);
            restartButton.onClick.AddListener(() => GameScenes.PlayLevel(level));
            pauseLevelsButton.onClick.AddListener(() => GameScenes.ToLevelSelect(level.Episode, level.Difficulty));
            pauseMainMenuButton.onClick.AddListener(GameScenes.ToMainMenu);
        }

        void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null || !keyboard.escapeKey.wasPressedThisFrame) return;
            AudioManager.Play(Sfx.Back);
            if (IsPaused) Resume();
            else if (completedPanel.IsVisible) GameScenes.ToMainMenu();
            else Pause();
        }

        // Taps -------------------------------------------------------------------
        public void ShowHint()
        {
            if (attempt == null || IsPaused || attempt.Phase == AttemptPhase.ShowingFeedback || attempt.Phase == AttemptPhase.Completed) return;
            attempt.UseHint();
            AudioManager.Play(Sfx.Sparkle, 0.6f);
            Juice.Sparkles((RectTransform)hintButton.transform, 10);
            captionBody.text = UIStrings.HintLine(scenario.hint.text);
            FitCaption();
        }

        void FillFeedback(ScenarioChoice choice)
        {
            feedbackBanner.sprite = attempt.LastChoiceSafe ? safeBanner : unsafeBanner;
            feedbackHeading.text = !string.IsNullOrEmpty(choice.heading) ? choice.heading
                : attempt.LastChoiceSafe ? UIStrings.SafeHeading : UIStrings.UnsafeHeading;
            feedbackText.text = choice.feedback;
            tryAgainButton.gameObject.SetActive(!attempt.LastChoiceSafe);
            finishButton.gameObject.SetActive(attempt.LastChoiceSafe);
        }

        void TryAgain()
        {
            if (attempt.Phase != AttemptPhase.ShowingFeedback || attempt.LastChoiceSafe) return;
            attempt.TryAgain();
            feedbackPanel.Hide();
            dimmer.Hide();
            if (scenario.Shakes) ActiveSetting?.shaker?.ResumeWobble();
            StartCoroutine(ReturnToStart());
        }

        void Finish()
        {
            if (attempt.Phase != AttemptPhase.ShowingFeedback || !attempt.LastChoiceSafe) return;
            attempt.Complete();

            // Save completion, Stars, and new unlocks before Level Completed appears.
            var saved = false;
            try
            {
                LastResult = GameServices.CompleteLevel(level, attempt);
                saved = true;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                // The result still shows, and the guide says plainly that it was not saved.
                Debug.LogError("Could not save the Completed Level: " + ex.Message);
            }

            ShowCompleted(saved);
        }

        void ShowCompleted(bool saved)
        {
            feedbackPanel.Hide();
            actionBar.Hide();
            var name = GameServices.Progress.DisplayName;
            var headline = saved ? UIStrings.LevelCompletedHeadline(name) : UIStrings.SaveProblem(name);
            var safe = scenario.SafeChoice;
            safeChoiceText.text = UIStrings.SafeChoiceLine(safe.label);
            lessonText.text = scenario.lesson.text;

            SetCompletionNavigation(level, saved && LastResult.OffersNextEpisode, GameServices.Progress, GameServices.Content);

            dimmer.Show();
            completedPanel.Show();
            AudioManager.Play(Sfx.Complete);
            stars.Play(attempt.Stars);
            if (confetti != null) confetti.Burst();
            Student.bubble.Hide();
            completedStudent.SetKind(GameServices.Progress.Student);
            completedStudent.Play(StudentPose.Happy);
            completedBubble.Say(headline, -1f);

            var openedKey = saved && LastResult.Opened.Count > 0 ? LastResult.Opened[0] : null;
            var openedName = OpenedName(openedKey, out var openedEpisode, out var openedDifficulty);
            openedRibbon.SetActive(openedName != null);
            if (openedName == null) return;

            openedText.text = UIStrings.Opened(openedName);
            StartCoroutine(PlayOpenedSound());
            StartCoroutine(SayOpened(openedName, openedEpisode, openedDifficulty));
        }

        public void SetCompletionNavigation(LevelId completed, bool offerNextEpisode, ProgressState progress, PlayableLevels content)
        {
            hasNext = !offerNextEpisode && LevelCompletion.TryNext(completed, out nextLevel)
                      && progress.IsOpen(nextLevel.Episode, nextLevel.Difficulty) && content.IsPlayable(nextLevel);
            nextButton.gameObject.SetActive(hasNext);
            nextEpisodeButton.gameObject.SetActive(offerNextEpisode);
        }

        static string OpenedName(string key, out Episode episode, out Difficulty difficulty)
        {
            if (!Curriculum.TryParseKey(key, out episode, out difficulty)) return null;
            return difficulty == Difficulty.Easy ? UIStrings.EpisodeName(episode) : UIStrings.DifficultyName(difficulty);
        }

        IEnumerator PlayOpenedSound()
        {
            yield return new WaitUntil(() => stars.Done);
            yield return new WaitForSecondsRealtime(0.2f);
            AudioManager.Play(Sfx.Opened);
        }

        IEnumerator SayOpened(string openedName, Episode episode, Difficulty difficulty)
        {
            yield return new WaitForSecondsRealtime(OpenedLineDelay);
            var line = OpenedLine(openedName, episode, difficulty, GameServices.Content);
            completedBubble.Say(line, -1f);
        }

        public static string OpenedLine(string openedName, Episode episode, Difficulty difficulty, PlayableLevels content) =>
            content.AnyIn(episode, difficulty) ? UIStrings.StudentOpened(openedName) : UIStrings.StudentOpenedSoon(openedName);

        void Pause()
        {
            if (IsPaused || completedPanel.IsVisible) return;
            dimmer.Show();
            pausePanel.Show();
            Time.timeScale = 0f;
        }

        void Resume()
        {
            if (!IsPaused) return;
            Time.timeScale = 1f;
            pausePanel.Hide();
            if (!feedbackPanel.IsVisible) dimmer.Hide();
        }
    }
}

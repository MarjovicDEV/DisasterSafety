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
    /// A Scenario whose Choices all have an action uses the Student flow (Caption, Action Bar and
    /// the Student acting out each Choice); every other Scenario keeps the Choice cards and Bantay.
    /// </summary>
    public sealed class LevelController : MonoBehaviour
    {
        public const float OpenedLineDelay = 2.5f;
        const float ReactionSeconds = 1f;

        [Header("Scene")]
        public LevelSetting[] settings;
        public RectTransform world;
        public RectTransform goalBanner;
        public RectTransform guidance;
        public Text goalText;
        public Text levelLabel;
        public Button pauseButton;
        public Button hintButton;
        public BantayGuide bantay;
        public ScreenPanel choicesPanel;
        public LevelChoicesLayout choicesLayout;
        public ChoiceCard[] choiceCards;
        [Tooltip("Choice pictures; matched to a Scenario Choice by sprite name.")]
        public Sprite[] pictures;
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
        public GameObject feedbackPictureHolder;
        public Image feedbackPicture;
        public Text feedbackText;
        public Button tryAgainButton;
        public Button finishButton;

        [Header("Level Completed")]
        public ScreenPanel completedPanel;
        [Tooltip("A second Bantay inside Level Completed who cheers with the Display Name.")]
        public BantayGuide cheerBantay;
        [Tooltip("The Student who cheers in Level Completed in the Student flow.")]
        public StudentActor completedStudent;
        public SpeechBubble completedBubble;
        public StarReveal stars;
        public GameObject openedRibbon;
        public Text openedText;
        public Image safePicture;
        public Text safeChoiceText;
        public Text lessonText;
        public Button replayButton;
        public Button nextButton;
        public Button levelsButton;
        public Button menuButton;
        public Confetti confetti;

        [Header("Pause")]
        public ScreenPanel pausePanel;
        public Button resumeButton;
        public Button restartButton;
        public Button quitButton;

        LevelId level;
        Scenario scenario;
        Attempt attempt;
        LevelId nextLevel;
        bool hasNext;
        bool studentFlow;

        public Attempt Attempt => attempt;
        public LevelId Level => level;
        public LevelSetting ActiveSetting { get; private set; }
        public CompletionResult LastResult { get; private set; }
        public bool IsPaused => pausePanel != null && pausePanel.IsVisible;

        /// <summary>True when the Student acts out each Choice (every Choice of the Scenario has an action).</summary>
        public bool UsesStudentFlow => studentFlow;
        public StudentActor Student => ActiveSetting != null ? ActiveSetting.student : null;

        void Awake()
        {
            GameServices.Initialize();
            AudioManager.EnsureExists();
            level = GameServices.CurrentLevel;
            if (GameServices.Scenarios.TryGet(level, out scenario))
            {
                attempt = new Attempt(scenario);
                studentFlow = scenario.UsesActions;
            }
            Wire();
        }

        void OnDestroy() => Time.timeScale = 1f;

        IEnumerator Start()
        {
            foreach (var p in new[] { feedbackPanel, completedPanel, pausePanel, dimmer, choicesPanel, actionBar != null ? actionBar.panel : null })
                if (p != null) p.SetInstant(false);

            if (attempt == null)
            {
                Debug.LogError("No Playable Level for " + level.Key + "; returning to the Main Menu.");
                GameScenes.ToMainMenu();
                yield break;
            }

            if (studentFlow)
            {
                yield return StartStudentLevel();
                yield break;
            }

            ShowSetting(scenario.setting);
            goalText.text = scenario.goal.text;
            levelLabel.text = UIStrings.LevelLabel(level);
            for (var i = 0; i < choiceCards.Length; i++)
            {
                var used = i < scenario.choices.Length;
                choiceCards[i].gameObject.SetActive(used);
                if (used) choiceCards[i].Show(scenario.choices[i], PictureFor(scenario.choices[i].picture));
            }
            bantay.tapLine = () => attempt.HintUsed ? UIStrings.HintLine(scenario.hint.text) : scenario.intro.text;

            if (scenario.Shakes)
            {
                ActiveSetting?.shaker?.StartShake();
                AudioManager.Play(Sfx.Rumble, 0.9f);
                AudioManager.DuckMusic(ActiveSetting?.shaker != null ? ActiveSetting.shaker.shakeSeconds : 2f);
                yield return new WaitForSeconds(0.4f);
            }
            bantay.Say(scenario.intro.text, true, 5f);
            yield return new WaitForSeconds(1.1f);
            choicesPanel.Show();
            attempt.StartChoosing();
            yield return new WaitForSeconds(6f);
            if (attempt.Phase == AttemptPhase.Choosing && !attempt.HintUsed && attempt.UnsafeChoices == 0)
                bantay.Say(UIStrings.BantayHintReady);
        }

        // The Student flow -------------------------------------------------------
        IEnumerator StartStudentLevel()
        {
            ShowSetting(ScenarioSettings.ActionArtKey(scenario.setting));
            UseStudentLayout();
            levelLabel.text = UIStrings.LevelLabel(level);
            captionGoal.text = scenario.goal.text;
            captionBody.text = scenario.intro.text;
            actionBar.Setup(scenario, ChooseAction);

            var student = Student;
            student.SetKind(GameServices.Progress.Student);
            student.PlaceAt(ActiveSetting.startX, ActiveSetting.startY);
            student.Face(false);
            student.Play(ActiveSetting.startPose);
            foreach (var prop in ActiveSetting.props)
                if (prop != null) prop.CaptureStart();

            var shaker = ActiveSetting.shaker;
            if (scenario.Shakes && shaker != null)
            {
                shaker.StartShake();
                AudioManager.Play(Sfx.Rumble, 0.9f);
                AudioManager.DuckMusic(shaker.shakeSeconds);
                student.Play(StudentPose.Surprised);
                student.Say(UIStrings.ReactionWhoa, shaker.shakeSeconds);
                yield return new WaitForSeconds(shaker.shakeSeconds);
                student.Play(ActiveSetting.startPose);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
            attempt.StartChoosing();
            actionBar.Show();
        }

        /// <summary>Hides the Choice cards, the Goal banner and Bantay, and lays out the Caption, the Feedback and Level Completed for the Student.</summary>
        public void UseStudentLayout()
        {
            goalBanner.gameObject.SetActive(false);
            bantay.gameObject.SetActive(false);
            choicesPanel.gameObject.SetActive(false);
            caption.gameObject.SetActive(true);

            var hint = (RectTransform)hintButton.transform;
            // The Hint sits under the Caption, over the blackboard: the door, desk, window and cabinet stay clear.
            hint.anchorMin = hint.anchorMax = hint.pivot = new Vector2(0.5f, 1f);
            hint.anchoredPosition = studentHintPosition;
            hint.sizeDelta = new Vector2(260f, 96f);

            feedbackPictureHolder.SetActive(false);
            var text = (RectTransform)feedbackText.transform;
            text.anchoredPosition = new Vector2(0f, text.anchoredPosition.y);
            text.sizeDelta = new Vector2(1200f, text.sizeDelta.y);
            feedbackText.alignment = TextAnchor.MiddleCenter;

            safePicture.transform.parent.gameObject.SetActive(false);
            cheerBantay.gameObject.SetActive(false);
            completedStudent.transform.parent.gameObject.SetActive(true);
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

            if (safe)
            {
                if (scenario.Shakes) setting.shaker?.Calm();
                student.Say(scenario.phase == ScenarioPhases.Before ? UIStrings.ReactionReady : UIStrings.ReactionSafe, ReactionSeconds + 1f);
                AudioManager.Play(Sfx.Safe);
                Juice.Sparkles((RectTransform)student.transform, 14);
            }
            else
            {
                student.Play(StudentPose.Wait);
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
                case ActOutStepKind.Overlay:
                    if (step.target == null) { Debug.LogError("An Overlay step in '" + setting.key + "' has no target."); return true; }
                    step.target.SetActive(true);
                    shownOverlays.Add(step.target);
                    return true;
                case ActOutStepKind.PropShow:
                case ActOutStepKind.PropHide:
                case ActOutStepKind.PropPickUp:
                case ActOutStepKind.PropPlace:
                    var prop = PropOf(step, setting);
                    if (prop == null) return true;
                    if (step.kind == ActOutStepKind.PropShow) prop.Show();
                    else if (step.kind == ActOutStepKind.PropHide) prop.Hide();
                    else if (step.kind == ActOutStepKind.PropPickUp) prop.PickUp(student.transform);
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
                    yield return student.WalkTo(step.x, step.y, PoseActor.WalkSpeed);
                    break;
                case ActOutStepKind.FastWalk:
                    yield return student.WalkTo(step.x, step.y, PoseActor.FastWalkSpeed);
                    break;
                case ActOutStepKind.Run:
                    yield return student.WalkTo(step.x, step.y, PoseActor.RunSpeed);
                    break;
                case ActOutStepKind.Crouch:
                    yield return student.CrouchTo(step.x);
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
            foreach (var prop in setting.props)
                if (prop != null && !prop.AtStart) resets.Add(StartCoroutine(prop.ResetToStart()));

            yield return student.WalkTo(setting.startX, setting.startY, PoseActor.WalkSpeed);
            foreach (var reset in resets) yield return reset;
            if (setting.startPose != StudentPose.Idle) student.Play(setting.startPose);
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

            var artAnchor = new Vector2(0.5f, ActiveSetting.alignArtTop ? 1f : ActiveSetting.artAnchorY);
            world.anchorMin = world.anchorMax = world.pivot = artAnchor;
            world.anchoredPosition = Vector2.zero;
            goalBanner.anchoredPosition = ActiveSetting.goalPosition;
            guidance.anchoredPosition = new Vector2(ActiveSetting.guidanceOffsetX, 0f);
            choicesLayout?.UseCompactLayoutOnWideScreens(ActiveSetting.compactChoicesOnWideScreens);
        }

        void Wire()
        {
            for (var i = 0; i < choiceCards.Length; i++)
            {
                var n = i;
                choiceCards[i].button.onClick.AddListener(() => Choose(n));
            }
            hintButton.onClick.AddListener(ShowHint);
            pauseButton.onClick.AddListener(Pause);
            tryAgainButton.onClick.AddListener(TryAgain);
            finishButton.onClick.AddListener(Finish);

            replayButton.onClick.AddListener(() => GameScenes.PlayLevel(level));
            nextButton.onClick.AddListener(() => { if (hasNext) GameScenes.PlayLevel(nextLevel); });
            levelsButton.onClick.AddListener(() => GameScenes.ToLevelSelect(level.Episode, level.Difficulty));
            menuButton.onClick.AddListener(GameScenes.ToMainMenu);

            resumeButton.onClick.AddListener(Resume);
            restartButton.onClick.AddListener(() => GameScenes.PlayLevel(level));
            quitButton.onClick.AddListener(GameScenes.ToMainMenu);
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

        Sprite PictureFor(string key)
        {
            if (pictures == null || string.IsNullOrEmpty(key)) return null;
            foreach (var p in pictures)
                if (p != null && p.name == key) return p;
            return null;
        }

        // Taps -------------------------------------------------------------------
        public void ShowHint()
        {
            if (attempt == null || IsPaused || attempt.Phase == AttemptPhase.ShowingFeedback || attempt.Phase == AttemptPhase.Completed) return;
            attempt.UseHint();
            AudioManager.Play(Sfx.Sparkle, 0.6f);
            Juice.Sparkles((RectTransform)hintButton.transform, 10);
            if (studentFlow) captionBody.text = UIStrings.HintLine(scenario.hint.text);
            else bantay.Say(UIStrings.HintLine(scenario.hint.text), true, 7f);
        }

        void Choose(int index)
        {
            if (attempt == null || IsPaused || !attempt.Choose(index)) return;
            var choice = scenario.choices[index];
            if (scenario.Shakes) ActiveSetting?.shaker?.Calm();
            bantay.HideBubble();

            FillFeedback(choice);
            feedbackPicture.sprite = PictureFor(choice.picture);

            if (attempt.LastChoiceSafe)
            {
                AudioManager.Play(Sfx.Safe);
                Juice.Sparkles((RectTransform)choiceCards[index].transform, 14);
            }
            else
            {
                AudioManager.Play(Sfx.Unsafe);
                choiceCards[index].SetTried(true);
            }
            dimmer.Show();
            feedbackPanel.Show();
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
            if (studentFlow) StartCoroutine(ReturnToStart());
            else bantay.Say(UIStrings.BantayThinkAgain, true);
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
            choicesPanel.Hide();
            if (studentFlow) actionBar.Hide();
            var name = GameServices.Progress.DisplayName;
            var headline = saved ? UIStrings.LevelCompletedHeadline(name) : UIStrings.SaveProblem(name);
            var safe = scenario.SafeChoice;
            if (!studentFlow) safePicture.sprite = PictureFor(safe.picture);
            safeChoiceText.text = UIStrings.SafeChoiceLine(safe.label);
            lessonText.text = scenario.lesson.text;

            hasNext = LevelCompletion.TryNext(level, out nextLevel)
                      && GameServices.Progress.IsOpen(nextLevel.Episode, nextLevel.Difficulty)
                      && GameServices.Content.IsPlayable(nextLevel);
            nextButton.gameObject.SetActive(hasNext);

            dimmer.Show();
            completedPanel.Show();
            AudioManager.Play(Sfx.Complete);
            stars.Play(attempt.Stars);
            if (confetti != null) confetti.Burst();
            if (studentFlow)
            {
                Student.bubble.Hide();
                completedStudent.SetKind(GameServices.Progress.Student);
                completedStudent.Play(StudentPose.Happy);
                completedBubble.Say(headline, -1f);
            }
            else
            {
                bantay.HideBubble();
                cheerBantay.tapLine = () => UIStrings.BantayCheer(GameServices.Progress.DisplayName);
                cheerBantay.Celebrate(headline);
            }

            var openedKey = saved && LastResult.Opened.Count > 0 ? LastResult.Opened[0] : null;
            var openedName = OpenedName(openedKey, out var openedEpisode, out var openedDifficulty);
            openedRibbon.SetActive(openedName != null);
            if (openedName == null) return;

            openedText.text = UIStrings.Opened(openedName);
            StartCoroutine(PlayOpenedSound());
            StartCoroutine(SayOpened(openedName, openedEpisode, openedDifficulty));
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
            if (studentFlow) completedBubble.Say(line, -1f);
            else cheerBantay.Say(line);
        }

        public static string OpenedLine(string openedName, Episode episode, Difficulty difficulty, PlayableLevels content) =>
            content.AnyIn(episode, difficulty) ? UIStrings.BantayOpened(openedName) : UIStrings.BantayOpenedSoon(openedName);

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

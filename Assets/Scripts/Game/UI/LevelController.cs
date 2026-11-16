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
    /// </summary>
    public sealed class LevelController : MonoBehaviour
    {
        [Header("Scene")]
        public SceneShaker shaker;
        public Text goalText;
        public Text levelLabel;
        public Button pauseButton;
        public Button hintButton;
        public BantayGuide bantay;
        public ScreenPanel choicesPanel;
        public ChoiceCard[] choiceCards;
        [Tooltip("Choice pictures; matched to a Scenario Choice by sprite name.")]
        public Sprite[] pictures;
        public ScreenPanel dimmer;

        [Header("Feedback")]
        public ScreenPanel feedbackPanel;
        public Image feedbackBanner;
        public Sprite safeBanner;
        public Sprite unsafeBanner;
        public Text feedbackHeading;
        public Image feedbackPicture;
        public Text feedbackText;
        public Button tryAgainButton;
        public Button finishButton;

        [Header("Level Completed")]
        public ScreenPanel completedPanel;
        [Tooltip("A second Bantay inside Level Completed who cheers with the Display Name.")]
        public BantayGuide cheerBantay;
        public StarReveal stars;
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

        public Attempt Attempt => attempt;
        public LevelId Level => level;
        public CompletionResult LastResult { get; private set; }
        public bool IsPaused => pausePanel != null && pausePanel.IsVisible;

        void Awake()
        {
            GameServices.Initialize();
            AudioManager.EnsureExists();
            level = GameServices.CurrentLevel;
            if (GameServices.Scenarios.TryGet(level, out scenario)) attempt = new Attempt(scenario);
            Wire();
        }

        void OnDestroy() => Time.timeScale = 1f;

        IEnumerator Start()
        {
            foreach (var p in new[] { feedbackPanel, completedPanel, pausePanel, dimmer, choicesPanel })
                if (p != null) p.SetInstant(false);

            if (attempt == null)
            {
                Debug.LogError("No Playable Level for " + level.Key + "; returning to the Main Menu.");
                GameScenes.ToMainMenu();
                yield break;
            }

            goalText.text = scenario.goal.text;
            levelLabel.text = UIStrings.LevelLabel(level);
            for (var i = 0; i < choiceCards.Length; i++)
            {
                var used = i < scenario.choices.Length;
                choiceCards[i].gameObject.SetActive(used);
                if (used) choiceCards[i].Show(scenario.choices[i], PictureFor(scenario.choices[i].picture));
            }
            bantay.tapLine = () => attempt.HintUsed ? UIStrings.HintLine(scenario.hint.text) : scenario.intro.text;

            // The ground starts shaking, Bantay reads the situation, then the Choices pop in.
            shaker.StartShake();
            AudioManager.Play(Sfx.Rumble, 0.9f);
            yield return new WaitForSeconds(0.4f);
            bantay.Say(scenario.intro.text, true, 5f);
            yield return new WaitForSeconds(1.1f);
            choicesPanel.Show();
            attempt.StartChoosing();
            yield return new WaitForSeconds(6f);
            if (attempt.Phase == AttemptPhase.Choosing && !attempt.HintUsed && attempt.UnsafeChoices == 0)
                bantay.Say(UIStrings.BantayHintReady);
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
            bantay.Say(UIStrings.HintLine(scenario.hint.text), true, 7f);
        }

        void Choose(int index)
        {
            if (attempt == null || IsPaused || !attempt.Choose(index)) return;
            var choice = scenario.choices[index];
            shaker.Calm();
            bantay.HideBubble();

            feedbackBanner.sprite = attempt.LastChoiceSafe ? safeBanner : unsafeBanner;
            feedbackHeading.text = !string.IsNullOrEmpty(choice.heading) ? choice.heading
                : attempt.LastChoiceSafe ? UIStrings.SafeHeading : UIStrings.UnsafeHeading;
            feedbackPicture.sprite = PictureFor(choice.picture);
            feedbackText.text = choice.feedback;
            tryAgainButton.gameObject.SetActive(!attempt.LastChoiceSafe);
            finishButton.gameObject.SetActive(attempt.LastChoiceSafe);

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

        void TryAgain()
        {
            if (attempt.Phase != AttemptPhase.ShowingFeedback || attempt.LastChoiceSafe) return;
            attempt.TryAgain();
            feedbackPanel.Hide();
            dimmer.Hide();
            shaker.ResumeWobble();
            bantay.Say(UIStrings.BantayThinkAgain, true);
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
                // The result still shows, and Bantay says plainly that it was not saved.
                Debug.LogError("Could not save the Completed Level: " + ex.Message);
            }

            ShowCompleted(saved);
        }

        void ShowCompleted(bool saved)
        {
            feedbackPanel.Hide();
            choicesPanel.Hide();
            var name = GameServices.Progress.DisplayName;
            var headline = saved ? UIStrings.LevelCompletedHeadline(name) : UIStrings.SaveProblem(name);
            var safe = scenario.SafeChoice;
            safePicture.sprite = PictureFor(safe.picture);
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
            bantay.HideBubble();
            cheerBantay.tapLine = () => UIStrings.BantayCheer(GameServices.Progress.DisplayName);
            cheerBantay.Celebrate(headline);
        }

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

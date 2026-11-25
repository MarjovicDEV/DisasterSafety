using System;
using System.Collections;
using System.IO;
using DisasterSafety.Core;
using DisasterSafety.Game;
using DisasterSafety.Game.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DisasterSafety.Tests
{
    /// <summary>
    /// Plays Earthquake / Easy / Level 1 in the real scenes: Choices, Feedback, Hint, Stars,
    /// saving before Level Completed, Pause, and the routes back to the Main Menu.
    /// Saves go to a temp folder, never the Learner's real persistentDataPath.
    /// </summary>
    public class LevelFlowPlayTests
    {
        static readonly LevelId First = new LevelId(Episode.Earthquake, Difficulty.Easy, 1);

        string dir;

        [SetUp]
        public void SetUp()
        {
            dir = Path.Combine(Path.GetTempPath(), "ds-level-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            GameServices.Reset();
            Time.timeScale = 1f;
        }

        [TearDown]
        public void TearDown()
        {
            Time.timeScale = 1f;
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }

        static IEnumerator Wait(float seconds)
        {
            for (var t = 0f; t < seconds; t += Time.unscaledDeltaTime) yield return null;
        }

        static IEnumerator WaitFor(Func<bool> condition, string what, float timeout = 8f)
        {
            for (var t = 0f; t < timeout && !condition(); t += Time.unscaledDeltaTime) yield return null;
            Assert.That(condition(), Is.True, "timed out waiting for " + what);
        }

        static IEnumerator WaitForScene(string name)
        {
            yield return WaitFor(() => SceneManager.GetActiveScene().name == name, "scene " + name);
            yield return Wait(0.3f);
        }

        static IEnumerator ReadyToChoose(Func<LevelController> find)
        {
            yield return WaitFor(() => find() != null && find().Attempt != null && find().Attempt.Phase == AttemptPhase.Choosing, "the Choices");
            yield return Wait(0.4f);
        }

        static LevelController Level() => UnityEngine.Object.FindAnyObjectByType<LevelController>();
        static MainMenuController Menu() => UnityEngine.Object.FindAnyObjectByType<MainMenuController>();

        // Easy 1 uses the Student flow: a tap on the Action Bar starts the Student walking, and Feedback shows about a second later.

        static void Tap(LevelController c, int choice) => c.actionBar.buttons[choice].button.onClick.Invoke();

        static void AssertActionBarLayout(LevelController controller, int expected)
        {
            Canvas.ForceUpdateCanvases();
            var buttons = Array.FindAll(controller.actionBar.buttons, button => button.gameObject.activeInHierarchy);
            Assert.That(buttons.Length, Is.EqualTo(expected));
            var row = (RectTransform)buttons[0].transform.parent;
            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var target = (RectTransform)button.button.targetGraphic.transform;
                var centre = row.InverseTransformPoint(target.TransformPoint(target.rect.center));
                Assert.That(centre.x, Is.EqualTo(row.rect.center.x + (i - (expected - 1) / 2f) * 450f).Within(0.1f));
                Assert.That(centre.y, Is.EqualTo(row.rect.center.y).Within(0.1f));
                Assert.That(target.rect.size, Is.EqualTo(new Vector2(430f, 124f)));
                Assert.That(button.button.targetGraphic.raycastTarget && button.button.IsInteractable(), Is.True);
                var label = button.label;
                Assert.That(label.preferredHeight, Is.LessThanOrEqualTo(label.rectTransform.rect.height + 0.1f), label.text);
                var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(target, label.rectTransform);
                Assert.That(bounds.min.y, Is.GreaterThanOrEqualTo(target.rect.yMin), label.text);
                Assert.That(bounds.max.y, Is.LessThanOrEqualTo(target.rect.yMax), label.text);
            }
        }

        static ActionButton ButtonFor(LevelController controller, string action)
        {
            var index = Array.FindIndex(controller.Attempt.Scenario.choices, choice => choice.action == action);
            Assert.That(index, Is.GreaterThanOrEqualTo(0), controller.Level + " has no action " + action);
            return controller.actionBar.buttons[index];
        }

        static void Tap(LevelController controller, string action) => ButtonFor(controller, action).button.onClick.Invoke();

        static IEnumerator FeedbackShown(LevelController c) =>
            WaitFor(() => c.feedbackPanel.IsVisible, "the Feedback panel", 12f);

        static IEnumerator ActionBarShown(LevelController c) =>
            WaitFor(() => c.actionBar.IsVisible && !c.Student.IsWalking, "the Action Bar", 12f);

        /// <summary>Taps a Choice of an Easy 1 Level and waits for its Feedback.</summary>
        static IEnumerator ChooseAndWait(LevelController c, string choice)
        {
            Tap(c, choice);
            yield return FeedbackShown(c);
            yield return Wait(0.3f);
        }

        ProgressState SavedOnDisk()
        {
            var result = new SaveService(dir).Load();
            Assert.That(result.Status, Is.EqualTo(SaveLoadStatus.Loaded), "save.json should be readable");
            return result.Progress;
        }

        [UnityTest]
        public IEnumerator FreshInstall_PlaysLevelOne_FromNameToContinue()
        {
            GameServices.Initialize(dir);
            SceneManager.LoadScene(GameScenes.Boot);
            yield return WaitForScene(GameScenes.MainMenu);
            yield return Wait(0.3f);
            var m = Menu();
            Assert.That(m.continueButton.interactable, Is.False);

            // Enter a name, then Earthquake > Easy > Level 1 > Start.
            m.playButton.onClick.Invoke();
            yield return Wait(0.4f);
            m.nameInput.text = "Ana";
            m.nameOk.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(m.studentPanel.IsVisible, Is.True, "a new name leads to the Student picker");
            m.studentBoy.onClick.Invoke();
            yield return Wait(0.5f);
            m.episodeCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            m.difficultyCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            m.levelSlots[0].button.onClick.Invoke();
            yield return null;
            Assert.That(m.startButton.interactable, Is.True);
            m.startButton.onClick.Invoke();

            yield return WaitForScene(GameScenes.Level);
            yield return ReadyToChoose(Level);
            var c = Level();
            Assert.That(c.Level, Is.EqualTo(First));
            Assert.That(c.captionGoal.text, Is.EqualTo("Stay safe when the ground shakes."));
            Assert.That(ButtonFor(c, "desk").label.text, Is.EqualTo("Duck under the desk"));
            Assert.That(c.ActiveSetting.shaker.wobblers, Has.All.Matches<SpriteAnimator>(w => w.enabled), "fan and cabinet wobble while it shakes");

            var attempt = c.Attempt;
            // An Unsafe Choice shows kind Feedback and can be tried again; the button is marked tried.
            yield return ChooseAndWait(c, "door");
            Assert.That(c.feedbackPanel.IsVisible, Is.True);
            Assert.That(c.feedbackHeading.text, Is.EqualTo("Not safe yet"), "Run outside has its own heading");
            Assert.That(c.ActiveSetting.shaker.wobblers, Has.All.Matches<SpriteAnimator>(w => !w.enabled), "the shaking calms after a Choice");
            Assert.That(c.tryAgainButton.gameObject.activeSelf, Is.True);
            Assert.That(c.finishButton.gameObject.activeSelf, Is.False);
            Assert.That(ButtonFor(c, "door").button.interactable, Is.False, "a tried Unsafe Choice cannot be picked again");
            Assert.That(File.Exists(Path.Combine(dir, "save.json")), Is.True, "the name was saved");
            Assert.That(SavedOnDisk().HasSavedProgress, Is.False, "nothing is saved for an unfinished Attempt");
            c.tryAgainButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.feedbackPanel.IsVisible, Is.False);
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing));
            yield return ActionBarShown(c);

            Assert.That(c.Attempt, Is.SameAs(attempt));

            // The Hint, then the Safe Choice.
            c.hintButton.onClick.Invoke();
            Assert.That(c.Attempt.HintUsed, Is.True);
            Assert.That(c.captionBody.text, Does.StartWith("Hint:"));
            yield return ChooseAndWait(c, "desk");
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.SafeHeading));
            Assert.That(c.finishButton.gameObject.activeSelf, Is.True);
            c.finishButton.onClick.Invoke();
            yield return Wait(0.5f);

            // Level Completed: 3 - 1 Unsafe Choice - 1 Hint = 1 Star, already on disk.
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult, Is.Not.Null);
            Assert.That(c.LastResult.Stars, Is.EqualTo(1));
            Assert.That(c.completedBubble.Line, Does.Contain("Ana"));
            Assert.That(c.completedStudent.Pose, Is.EqualTo(StudentPose.Happy));
            Assert.That(c.safeChoiceText.text, Does.Contain("Duck under the desk"));
            Assert.That(c.nextButton.gameObject.activeSelf, Is.True, "Next is shown when Level 2 is playable");
            var saved = SavedOnDisk();
            Assert.That(saved.IsCompleted(First), Is.True);
            Assert.That(saved.StarsFor(First), Is.EqualTo(1));
            yield return WaitFor(() => c.stars.Done, "the Stars to finish");
            Assert.That(c.stars.Shown, Is.EqualTo(1));

            // Levels returns to Level Selection for Earthquake / Easy, showing the Star.
            c.levelsButton.onClick.Invoke();
            yield return WaitForScene(GameScenes.MainMenu);
            yield return Wait(0.4f);
            m = Menu();
            Assert.That(m.levelPanel.IsVisible, Is.True);
            Assert.That(m.levelHeader.text, Is.EqualTo("Earthquake - Easy"));
            Assert.That(m.levelSlots[0].stars[0].sprite, Is.SameAs(m.levelSlots[0].starOn));
            Assert.That(m.levelSlots[0].stars[1].sprite, Is.SameAs(m.levelSlots[0].starOff));

            // Back to the Main Menu: Continue is now enabled and opens the same Level Selection.
            m.levelBack.onClick.Invoke();
            yield return Wait(0.5f);
            m.difficultyBack.onClick.Invoke();
            yield return Wait(0.5f);
            m.episodeBack.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(m.mainPanel.IsVisible, Is.True);
            Assert.That(m.continueButton.interactable, Is.True, "Continue opens after the first Completed Level");
            m.continueButton.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(m.levelPanel.IsVisible, Is.True);
            Assert.That(m.levelHeader.text, Is.EqualTo("Earthquake - Easy"));
        }

        [UnityTest]
        public IEnumerator EasyDifficulty_CompletingAllThreeLevels_OpensNormal()
        {
            var second = new LevelId(Episode.Earthquake, Difficulty.Easy, 2);
            var third = Third;

            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.SaveProgress();
            GameScenes.PlayLevel(First);
            yield return ReadyToChoose(Level);

            var c = Level();
            Assert.That(c.Level, Is.EqualTo(First));
            yield return ChooseAndWait(c, "desk");
            c.finishButton.onClick.Invoke();
            Assert.That(c.nextButton.gameObject.activeSelf, Is.True, "Level 1 leads to Level 2");
            c.nextButton.onClick.Invoke();

            yield return WaitFor(() => Level()?.Level.Equals(second) == true, "Level 2 to load");
            yield return ReadyToChoose(Level);
            c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("corridor_v2"));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.True);

            // Easy 2 is acted out by the Student in the corridor.
            Tap(c, "plants");
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.UnsafeHeading));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Tap(c, "cover");
            yield return FeedbackShown(c);
            c.finishButton.onClick.Invoke();
            Assert.That(c.nextButton.gameObject.activeSelf, Is.True, "Level 2 leads to Level 3");
            c.nextButton.onClick.Invoke();

            yield return WaitFor(() => Level()?.Level.Equals(third) == true && Level().ActiveSetting != null, "Level 3 to load");
            c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("classroom_after_v2"));
            yield return ReadyToChoose(Level);

            yield return ChooseAndWait(c, "walk");
            c.finishButton.onClick.Invoke();
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult.Opened, Does.Contain("Earthquake/Normal"));
            Assert.That(c.openedRibbon.activeInHierarchy, Is.True);
            Assert.That(c.openedText.text, Is.EqualTo("Normal is open!"));
            Assert.That(c.nextButton.gameObject.activeSelf, Is.False);
            var saved = SavedOnDisk();
            Assert.That(saved.IsCompleted(third), Is.True);
            Assert.That(saved.IsOpen(Episode.Earthquake, Difficulty.Normal), Is.True);

            Assert.That(c.completedBubble.Line, Is.EqualTo("You did it, Ana!"));
            const string openedLine = "Normal is open! Let's play!";
            yield return WaitFor(() => c.completedBubble.gameObject.activeInHierarchy && c.completedBubble.Line == openedLine,
                "the Student to announce that Normal is open", 5f);

            var completed = c;
            c.replayButton.onClick.Invoke();
            yield return WaitFor(() => completed == null, "the old Level to unload");
            yield return ReadyToChoose(Level);
            c = Level();
            Assert.That(c.Level, Is.EqualTo(third));
            yield return ChooseAndWait(c, "walk");
            c.finishButton.onClick.Invoke();
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult.Opened, Is.Empty);
            Assert.That(c.openedRibbon.activeSelf, Is.False);
            yield return Wait(LevelController.OpenedLineDelay + 0.2f);
            Assert.That(c.completedBubble.Line, Is.EqualTo("You did it, Ana!"));
        }

        [UnityTest]
        public IEnumerator NormalDifficulty_CompletingAllFiveLevels_OpensHard()
        {
            var settings = new[] { "bedroom_v2", "home_v2", "home_v2", "schoolyard_v2", "beach_v2" };
            var shakes = new[] { false, false, true, true, false };

            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Normal);
            GameServices.SaveProgress();
            GameScenes.PlayLevel(new LevelId(Episode.Earthquake, Difficulty.Normal, 1));

            LevelController c = null;
            for (var n = 1; n <= settings.Length; n++)
            {
                var level = new LevelId(Episode.Earthquake, Difficulty.Normal, n);
                yield return WaitFor(() => Level()?.Level.Equals(level) == true, "Normal " + n + " to load");
                yield return ReadyToChoose(Level);
                c = Level();
                AssertActionBarLayout(c, 3);
                Assert.That(c.ActiveSetting.key, Is.EqualTo(settings[n - 1]), "Setting of Normal " + n);
                var shaking = c.ActiveSetting.shaker != null && c.ActiveSetting.shaker.IsShaking;
                Assert.That(shaking, Is.EqualTo(shakes[n - 1]), "shaking in Normal " + n);

                Assert.That(GameServices.Scenarios.TryGet(level, out var scenario), Is.True);
                Tap(c, scenario.SafeChoiceIndex);
                yield return FeedbackShown(c);
                Assert.That(c.finishButton.gameObject.activeSelf, Is.True, "the Safe Choice of Normal " + n + " can Finish");
                c.finishButton.onClick.Invoke();
                Assert.That(c.completedPanel.IsVisible, Is.True, "Level Completed after Normal " + n);
                if (n < settings.Length)
                {
                    Assert.That(c.nextButton.gameObject.activeSelf, Is.True, "Normal " + n + " leads to Normal " + (n + 1));
                    c.nextButton.onClick.Invoke();
                }
            }

            Assert.That(c.LastResult.Opened, Does.Contain("Earthquake/Hard"));
            Assert.That(c.openedRibbon.activeInHierarchy, Is.True);
            Assert.That(c.openedText.text, Is.EqualTo("Hard is open!"));
            Assert.That(c.nextButton.gameObject.activeSelf, Is.False, "Normal 5 is the last Normal Level");
            var saved = SavedOnDisk();
            Assert.That(saved.IsCompleted(new LevelId(Episode.Earthquake, Difficulty.Normal, 5)), Is.True);
            Assert.That(saved.IsOpen(Episode.Earthquake, Difficulty.Hard), Is.True);

            const string openedLine = "Hard is open! Let's play!";
            yield return WaitFor(() => c.completedBubble.gameObject.activeInHierarchy && c.completedBubble.Line == openedLine,
                "the Student to announce that Hard is open", LevelController.OpenedLineDelay + 2f);
        }

        [UnityTest]
        public IEnumerator HardDifficulty_CompletingAllSevenLevels_CompletesTheEpisode()
        {
            var settings = new[] { "home_v2", "schoolyard_v2", "mall_a_v2", "mall_b_v2", "home_after_v2", "home_after_v2", "evacuation_area_v2" };
            var shakes = new[] { false, false, true, false, false, false, false };

            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Hard);
            GameServices.SaveProgress();
            GameScenes.PlayLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 1));

            LevelController c = null;
            for (var n = 1; n <= settings.Length; n++)
            {
                var level = new LevelId(Episode.Earthquake, Difficulty.Hard, n);
                yield return WaitFor(() => Level()?.Level.Equals(level) == true, "Hard " + n + " to load");
                yield return ReadyToChoose(Level);
                c = Level();
                Assert.That(c.ActiveSetting.key, Is.EqualTo(settings[n - 1]), "Setting of Hard " + n);
                var shaking = c.ActiveSetting.shaker != null && c.ActiveSetting.shaker.IsShaking;
                Assert.That(shaking, Is.EqualTo(shakes[n - 1]), "shaking in Hard " + n);

                Assert.That(GameServices.Scenarios.TryGet(level, out var scenario), Is.True);
                Tap(c, scenario.SafeChoiceIndex);
                yield return FeedbackShown(c);
                Assert.That(c.finishButton.gameObject.activeSelf, Is.True, "the Safe Choice of Hard " + n + " can Finish");
                c.finishButton.onClick.Invoke();
                Assert.That(c.completedPanel.IsVisible, Is.True, "Level Completed after Hard " + n);
                if (n < settings.Length)
                {
                    Assert.That(c.nextButton.gameObject.activeSelf, Is.True, "Hard " + n + " leads to Hard " + (n + 1));
                    c.nextButton.onClick.Invoke();
                }
            }

            Assert.That(c.LastResult.EpisodeCompleted, Is.True, "Hard 7 completes the Earthquake Episode");
            Assert.That(c.LastResult.Opened, Does.Contain("Fire/Easy"));
            Assert.That(c.openedRibbon.activeInHierarchy, Is.True);
            Assert.That(c.openedText.text, Is.EqualTo("Fire is open!"));
            Assert.That(c.nextButton.gameObject.activeSelf, Is.False);
            Assert.That(c.nextEpisodeButton.gameObject.activeInHierarchy, Is.True);
            var saved = SavedOnDisk();
            Assert.That(saved.IsCompleted(new LevelId(Episode.Earthquake, Difficulty.Hard, 7)), Is.True);
            Assert.That(saved.IsOpen(Episode.Fire, Difficulty.Easy), Is.True);

            const string openedLine = "Fire is open! New levels are coming soon.";
            yield return WaitFor(() => c.completedBubble.gameObject.activeInHierarchy && c.completedBubble.Line == openedLine,
                "the Student to announce that Fire is open", LevelController.OpenedLineDelay + 2f);
        }

        [UnityTest]
        public IEnumerator LastOutstandingHardLevel_OffersNextEpisode()
        {
            GameServices.Initialize(dir);
            for (var n = 2; n <= 7; n++)
                LevelCompletion.Record(GameServices.Progress, new LevelId(Episode.Earthquake, Difficulty.Hard, n), 3);
            yield return StartRolloutLevel(HardOne);
            var controller = Level();
            yield return ReadyToChoose(Level);
            Tap(controller, controller.Attempt.Scenario.SafeChoiceIndex);
            yield return FeedbackShown(controller);
            controller.finishButton.onClick.Invoke();
            Assert.That(controller.LastResult.Opened, Does.Contain("Fire/Easy"));
            var nextEpisode = controller.nextEpisodeButton;
            Assert.That(nextEpisode.gameObject.activeInHierarchy, Is.True);
            Assert.That(controller.nextButton.gameObject.activeSelf, Is.False, "the shortcut takes the Next slot");
            Assert.That(nextEpisode.GetComponentInChildren<Text>().text, Is.EqualTo("Next Episode"));
            Assert.That(Array.Exists(nextEpisode.GetComponentsInChildren<Image>(),
                image => image.sprite != null && image.sprite.name == "episode_fire"), Is.True);
            yield return Wait(0.4f);
            var row = nextEpisode.transform.parent;
            var replayBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(row, controller.replayButton.transform);
            var episodeBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(row, nextEpisode.transform);
            var levelsBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(row, controller.levelsButton.transform);
            Assert.That(episodeBounds.min.x, Is.GreaterThan(replayBounds.max.x));
            Assert.That(episodeBounds.max.x, Is.LessThan(levelsBounds.min.x));
            var label = nextEpisode.GetComponentInChildren<Text>();
            var buttonRect = (RectTransform)nextEpisode.transform;
            var labelCentre = buttonRect.InverseTransformPoint(label.rectTransform.TransformPoint(label.rectTransform.rect.center));
            var rightEdge = labelCentre.x + PixelText.Width(label.font, label.text) / 2f;
            Assert.That(rightEdge, Is.LessThan(buttonRect.rect.xMax - 8f), "the label stays inside the button face");
            nextEpisode.onClick.Invoke();
            yield return WaitForScene(GameScenes.MainMenu);
            var menu = Menu();
            yield return WaitFor(() => menu.episodePanel.IsVisible, "Episode selection");
            var fire = Array.Find(menu.episodeCards, card => card.episode == Episode.Fire);
            Assert.That(fire.gameObject.activeInHierarchy && !fire.lockBadge.activeSelf, Is.True);
            fire.button.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(menu.difficultyHeader.text, Is.EqualTo("Fire"));
            menu.difficultyCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            menu.levelSlots[0].button.onClick.Invoke();
            Assert.That(menu.startButton.interactable, Is.False);
            Assert.That(menu.student.Bubble.Line, Is.EqualTo(UIStrings.StudentLevelSoon));
            menu.levelBack.onClick.Invoke();
            yield return Wait(0.5f);
            menu.difficultyBack.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(menu.episodePanel.IsVisible, Is.True);
            menu.episodeBack.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(menu.mainPanel.IsVisible, Is.True);
        }

        [UnityTest]
        public IEnumerator HardSevenEarlyAndAfterReload_OfferDifferentCompletionActions()
        {
            yield return StartRolloutLevel(HardSeven);
            var controller = Level();
            yield return ReadyToChoose(Level);
            Tap(controller, controller.Attempt.Scenario.SafeChoiceIndex);
            yield return FeedbackShown(controller);
            controller.finishButton.onClick.Invoke();
            Assert.That(controller.nextEpisodeButton.gameObject.activeSelf, Is.False);
            Assert.That(controller.nextButton.gameObject.activeSelf, Is.False);
            Assert.That(SavedOnDisk().IsOpen(Episode.Fire), Is.False);

            for (var n = 1; n <= 6; n++)
                LevelCompletion.Record(GameServices.Progress, new LevelId(Episode.Earthquake, Difficulty.Hard, n), 3);
            GameServices.SaveProgress();
            GameServices.Reset();
            GameServices.Initialize(dir);
            Assert.That(GameServices.Progress.IsOpen(Episode.Fire), Is.True);
            controller.replayButton.onClick.Invoke();
            yield return WaitFor(() => controller == null, "Hard 7 replay to reload");
            yield return ReadyToChoose(Level);
            controller = Level();
            Tap(controller, controller.Attempt.Scenario.SafeChoiceIndex);
            yield return FeedbackShown(controller);
            controller.finishButton.onClick.Invoke();
            Assert.That(controller.nextEpisodeButton.gameObject.activeInHierarchy, Is.True);
            Assert.That(controller.LastResult.Opened, Is.Empty);
            Assert.That(controller.openedRibbon.activeSelf, Is.False);
        }

        IEnumerator StartEasyOne(StudentKind kind = StudentKind.None)
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            if (kind != StudentKind.None) GameServices.Progress.SetStudent(kind);
            GameServices.SaveProgress();
            GameScenes.PlayLevel(First);
            yield return WaitFor(() => Level() != null && Level().ActiveSetting != null && Level().Student != null
                                       && Level().Attempt != null && Level().captionGoal.text.Length > 0, "Easy 1 to start");
        }

        [UnityTest]
        public IEnumerator EasyLevel1_NewFlow_SafeChoice_StudentDucksAndCompletes()
        {
            yield return StartEasyOne();
            var c = Level();
            var student = c.Student;
            Assert.That(c.ActiveSetting.key, Is.EqualTo("classroom_v2"));
            Assert.That(c.captionGoal.text, Is.EqualTo("Stay safe when the ground shakes."));
            Assert.That(c.captionBody.text, Is.EqualTo("The ground is shaking! What will you do?"));

            // The first shake: the Student is surprised and says Whoa, and the Action Bar waits.
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.True);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Surprised));
            Assert.That(student.bubble.Line, Is.EqualTo("Whoa!"));
            Assert.That(student.X, Is.EqualTo(c.ActiveSetting.startX));
            Assert.That(c.actionBar.IsVisible, Is.False);
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Reading));

            yield return ReadyToChoose(Level);
            Assert.That(c.actionBar.IsVisible, Is.True);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            AssertActionBarLayout(c, 2);
            Assert.That(ButtonFor(c, "desk").label.text, Is.EqualTo("Duck under the desk"));
            Assert.That(ButtonFor(c, "desk").icon.sprite.name, Is.EqualTo("icon_desk"));

            // The desk: the Action Bar hides, the Student walks up beside the desk, crouches and
            // shuffles in under it, then holds the desk leg.
            Tap(c, "desk");
            Assert.That(c.actionBar.IsVisible, Is.False);
            var desk = c.ActiveSetting.ActOutFor("desk");
            var deskX = desk.EndX(c.ActiveSetting.startX);
            yield return WaitFor(() => student.Pose == StudentPose.Crouch, "the Student to crouch", 12f);
            Assert.That(student.X, Is.LessThan(deskX));
            Assert.That(desk.FirstOverlay.activeSelf, Is.True, "the front of the desk is drawn from the crouch on");
            yield return WaitFor(() => student.Pose == StudentPose.Hold, "the Student to hold the desk", 12f);
            Assert.That(student.X, Is.EqualTo(deskX));
            Assert.That(desk.FirstOverlay.activeSelf, Is.True, "the front of the desk is drawn over the Student");
            Assert.That(student.bubble.Line, Is.EqualTo("I'm safe!"));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.False, "the shaking calms");
            Assert.That(c.feedbackPanel.IsVisible, Is.False, "Feedback follows a second later");

            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.SafeHeading));
            Assert.That(c.finishButton.gameObject.activeSelf, Is.True);
            Assert.That(c.tryAgainButton.gameObject.activeSelf, Is.False);

            c.finishButton.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult.Stars, Is.EqualTo(3));
            Assert.That(c.completedStudent.Pose, Is.EqualTo(StudentPose.Happy));
            Assert.That(c.completedBubble.Line, Is.EqualTo("You did it, Ana!"));
        }

        [UnityTest]
        public IEnumerator EasyLevel1_NewFlow_UnsafeChoice_StudentStopsAndTriesAgain()
        {
            yield return StartEasyOne();
            var c = Level();
            var student = c.Student;
            yield return ReadyToChoose(Level);

            Tap(c, "door");
            yield return WaitFor(() => student.Pose == StudentPose.Wait, "the Student to stop", 12f);
            var door = c.ActiveSetting.ActOutFor("door");
            Assert.That(student.X, Is.EqualTo(door.StopX(c.ActiveSetting.startX)).Within(0.01f));
            Assert.That(student.X - 16f, Is.GreaterThan(46f), "the Student stops before the door");
            Assert.That(student.bubble.Line, Is.EqualTo("Hmm, wait..."));

            yield return FeedbackShown(c);
            Assert.That(c.tryAgainButton.gameObject.activeSelf, Is.True);
            Assert.That(c.finishButton.gameObject.activeSelf, Is.False);
            Assert.That(c.feedbackText.text, Does.Contain("Stay inside while it shakes"));
            Assert.That(c.Attempt.UnsafeChoices, Is.EqualTo(1));

            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Assert.That(student.X, Is.EqualTo(c.ActiveSetting.startX));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing));
            Assert.That(ButtonFor(c, "door").button.interactable, Is.False, "the door is marked tried");
            Assert.That(ButtonFor(c, "desk").button.interactable, Is.True);
        }

        static readonly LevelId Second = new LevelId(Episode.Earthquake, Difficulty.Easy, 2);

        IEnumerator StartEasyTwo()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.SaveProgress();
            GameScenes.PlayLevel(Second);
            yield return WaitFor(() => Level() != null && Level().ActiveSetting != null && Level().Student != null
                                       && Level().Attempt != null && Level().captionGoal.text.Length > 0, "Easy 2 to start");
        }

        [UnityTest]
        public IEnumerator EasyLevel2_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartEasyTwo();
            var c = Level();
            var student = c.Student;
            var setting = c.ActiveSetting;
            Assert.That(setting.key, Is.EqualTo("corridor_v2"));
            Assert.That(c.captionGoal.text, Is.EqualTo("Keep your head safe when the ground shakes."));
            Assert.That(setting.shaker.IsShaking, Is.True);
            Assert.That(student.X, Is.EqualTo(setting.startX));

            yield return ReadyToChoose(Level);
            Assert.That(ButtonFor(c, "cover").label.text, Is.EqualTo("Duck and cover your head"));
            Assert.That(ButtonFor(c, "cover").icon.sprite.name, Is.EqualTo("icon_cover"));
            Assert.That(ButtonFor(c, "plants").icon.sprite.name, Is.EqualTo("icon_plants"));
            // The long Goal and labels still fit: the Goal on one line, each label in two.
            Assert.That(PixelText.Lines(c.captionGoal.font, c.captionGoal.text, c.captionGoal.rectTransform.sizeDelta.x), Is.EqualTo(1));
            foreach (var b in c.actionBar.buttons)
                Assert.That(PixelText.Lines(b.label.font, b.label.text, c.actionBar.labelWidth), Is.LessThanOrEqualTo(2), b.label.text);

            // Walk to the inner wall, crouch, then cover the head and hold until the shaking calms.
            Tap(c, "cover");
            Assert.That(c.actionBar.IsVisible, Is.False);
            yield return WaitFor(() => student.IsWalking, "the Student to walk", 4f);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Walk));
            yield return WaitFor(() => student.Pose == StudentPose.Crouch, "the Student to crouch", 12f);
            Assert.That(student.X, Is.GreaterThanOrEqualTo(269.99f), "the crouch starts at the inner wall");
            yield return WaitFor(() => student.Pose == StudentPose.Cover, "the Student to cover the head", 12f);
            Assert.That(student.X, Is.EqualTo(286f).Within(0.01f));
            Assert.That(setting.shaker.IsShaking, Is.True, "the Student holds the cover while it still shakes");
            yield return WaitFor(() => !setting.shaker.IsShaking, "the shaking to calm", 6f);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Cover));
            Assert.That(student.bubble.Line, Is.EqualTo("I'm safe!"));

            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.SafeHeading));
            Assert.That(c.finishButton.gameObject.activeSelf, Is.True);
            c.finishButton.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult.Stars, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator EasyLevel2_UnsafeChoice_StopsAndResets()
        {
            yield return StartEasyTwo();
            var c = Level();
            var student = c.Student;
            var setting = c.ActiveSetting;
            yield return ReadyToChoose(Level);

            Tap(c, "plants");
            yield return WaitFor(() => student.IsWalking, "the Student to walk", 4f);
            yield return WaitFor(() => student.Pose == StudentPose.Wait, "the Student to stop", 12f);
            Assert.That(student.X, Is.EqualTo(setting.ActOutFor("plants").StopX(setting.startX)).Within(0.01f));
            Assert.That(student.X - 16f, Is.GreaterThan(63f), "the Student stops clear of the pots");
            Assert.That(student.bubble.Line, Is.EqualTo("Hmm, wait..."));

            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.UnsafeHeading));
            Assert.That(c.tryAgainButton.gameObject.activeSelf, Is.True);
            Assert.That(c.Attempt.UnsafeChoices, Is.EqualTo(1));

            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Assert.That(student.X, Is.EqualTo(setting.startX));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing));
            Assert.That(ButtonFor(c, "plants").button.interactable, Is.False, "the plants are marked tried");
            Assert.That(ButtonFor(c, "cover").button.interactable, Is.True);
        }

        static readonly LevelId Third = new LevelId(Episode.Earthquake, Difficulty.Easy, 3);

        IEnumerator StartEasyThree()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.SaveProgress();
            GameScenes.PlayLevel(Third);
            yield return WaitFor(() => Level()?.Level.Equals(Third) == true && Level().ActiveSetting != null,
                "Easy 3 to start");
        }

        [UnityTest]
        public IEnumerator EasyLevel3_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartEasyThree();
            var c = Level();
            var student = c.Student;
            Assert.That(c.ActiveSetting.key, Is.EqualTo("classroom_after_v2"));
            Assert.That(c.ActiveSetting.shaker == null || !c.ActiveSetting.shaker.IsShaking, Is.True);
            Assert.That(c.captionBody.text, Is.EqualTo("The shaking stopped. Your teacher is at the door."));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Hold));
            Assert.That(student.X, Is.EqualTo(222f));
            Assert.That(c.ActiveSetting.grownUps, Has.Length.EqualTo(1));
            var teacher = c.ActiveSetting.grownUps[0];
            Assert.That(teacher.kind, Is.EqualTo(GrownUpKind.Teacher));
            Assert.That(teacher.Pose, Is.EqualTo(GrownUpPose.Wave));
            Assert.That(teacher.X, Is.EqualTo(77f));
            var firstFrame = teacher.CurrentSprite;
            Assert.That(firstFrame, Is.Not.Null);
            yield return WaitFor(() => teacher.CurrentSprite != firstFrame, "the Teacher to wave");
            yield return ReadyToChoose(Level);
            Assert.That(ButtonFor(c, "stay").icon.sprite.name, Is.EqualTo("icon_stay"));
            Assert.That(ButtonFor(c, "walk").icon.sprite.name, Is.EqualTo("icon_teacher_walk"));

            Tap(c, "walk");
            Assert.That(c.actionBar.IsVisible, Is.False);
            yield return WaitFor(() => student.Pose == StudentPose.Crouch && student.IsWalking,
                "the Student to shuffle out from under the desk");
            Assert.That(teacher.AtStart, Is.True);
            yield return WaitFor(() => student.Pose == StudentPose.Idle && student.X == 182f,
                "the Student to stand beside the desk");
            yield return WaitFor(() => student.X < 182f && student.IsWalking && student.Pose == StudentPose.Walk,
                "the Student to walk to the Teacher");
            Assert.That(teacher.AtStart, Is.True);
            yield return WaitFor(() => student.IsWalking && teacher.IsWalking,
                "the Student and the Teacher to walk out together");
            Assert.That(student.FacingLeft, Is.True);
            Assert.That(teacher.FacingLeft, Is.True);
            Assert.That(student.X - teacher.X, Is.GreaterThan(14f), "the Teacher leads through the door");

            yield return WaitFor(() => student.bubble.Line == "I'm safe!", "the Safe Reaction");
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Happy));
            Assert.That(student.X, Is.EqualTo(51f));
            Assert.That(student.Y, Is.EqualTo(21f));
            Assert.That(teacher.X, Is.EqualTo(36f));
            Assert.That(teacher.Y, Is.EqualTo(35f));

            yield return FeedbackShown(c);
            Assert.That(c.finishButton.gameObject.activeSelf, Is.True);
            Assert.That(c.feedbackText.text, Is.EqualTo("Walk out with your class. Follow your teacher to the evacuation area."));
            c.finishButton.onClick.Invoke();
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult.Stars, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator EasyLevel3_UnsafeChoice_StopsAndResets()
        {
            yield return StartEasyThree();
            var c = Level();
            var student = c.Student;
            var teacher = c.ActiveSetting.grownUps[0];
            yield return ReadyToChoose(Level);

            Tap(c, "stay");
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Hold));
            yield return WaitFor(() => student.bubble.Line == "Hmm, wait...", "the stop under the desk");
            Assert.That(student.X, Is.EqualTo(220f), "the Student only peeks out");
            Assert.That(teacher.AtStart, Is.True);
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo("Time to go"));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Assert.That(student.X, Is.EqualTo(222f));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Hold));
            Assert.That(ButtonFor(c, "stay").button.interactable, Is.False);

            Assert.That(ButtonFor(c, "walk").button.interactable, Is.True);
        }

        static readonly LevelId NormalOne = new LevelId(Episode.Earthquake, Difficulty.Normal, 1);

        IEnumerator StartNormalOne(StudentKind kind = StudentKind.None)
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            if (kind != StudentKind.None) GameServices.Progress.SetStudent(kind);
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Normal);
            GameServices.SaveProgress();
            GameScenes.PlayLevel(NormalOne);
            yield return WaitFor(() => Level()?.Level.Equals(NormalOne) == true && Level().ActiveSetting != null,
                "Normal 1 to start");
        }

        static readonly LevelId NormalTwo = new LevelId(Episode.Earthquake, Difficulty.Normal, 2);
        static readonly LevelId NormalThree = new LevelId(Episode.Earthquake, Difficulty.Normal, 3);
        static readonly LevelId HardOne = new LevelId(Episode.Earthquake, Difficulty.Hard, 1);

        IEnumerator StartRolloutLevel(LevelId level)
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.Open(Episode.Earthquake, level.Difficulty);
            GameServices.SaveProgress();
            GameScenes.PlayLevel(level);
            yield return WaitFor(() => Level()?.Level.Equals(level) == true && Level().ActiveSetting != null,
                level.Key + " to start");
        }

        [UnityTest]
        public IEnumerator HomeNormal2_StartsWithItsOwnCast()
        {
            yield return StartRolloutLevel(NormalTwo);
            var c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("home_v2"));
            Assert.That(c.Student.X, Is.EqualTo(281f));
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.False);
            var props = c.ActiveSetting.GetComponentsInChildren<LevelProp>();
            Assert.That(Array.ConvertAll(props, p => p.key), Is.EquivalentTo(new[]
            {
                "bag_open", "water", "can", "flashlight", "whistle", "snack", "toy",
            }));
            var grownUps = c.ActiveSetting.GetComponentsInChildren<GrownUpActor>();
            Assert.That(grownUps.Length, Is.EqualTo(1));
            Assert.That(grownUps[0].kind, Is.EqualTo(GrownUpKind.Mom));
            Assert.That(grownUps[0].X, Is.EqualTo(158f));
        }

        [UnityTest]
        public IEnumerator NormalLevel2_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(NormalTwo);
            var c = Level();
            yield return ReadyToChoose(Level);
            Assert.That(c.ActiveSetting.ActOutFor("go_bag"), Is.Not.Null, "the Safe Choice has an Act-out");
            var student = c.Student;
            var closed = PropIn(c, "bag_closed");
            var open = PropIn(c, "bag_open");
            var mom = c.ActiveSetting.ActiveGrownUps[0];
            AssertActionBarLayout(c, 3);
            Tap(c, "go_bag");

            foreach (var key in new[] { "water", "can", "flashlight", "whistle" })
            {
                var item = PropIn(c, key);
                Assert.That(item.gameObject.activeSelf, Is.True, key + " is still unpacked");
                yield return WaitFor(() => !item.gameObject.activeSelf, key + " to go into the bag");
                Assert.That(student.Pose, Is.EqualTo(StudentPose.Pack));
            }

            yield return WaitFor(() => closed.transform.IsChildOf(student.transform) && student.IsWalking,
                "the Student to carry the closed bag");
            Assert.That(open.gameObject.activeSelf, Is.False);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Carry));
            Assert.That(student.FacingLeft, Is.True);
            Assert.That(CentreX(closed), Is.LessThan(student.transform.position.x));

            yield return FeedbackShown(c);
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.Happy));
            Assert.That(closed.gameObject.activeInHierarchy, Is.True);
            AssertPropCentre(closed, 83f, 202.5f);
            Assert.That(PropIn(c, "toy").gameObject.activeSelf, Is.True);
            Assert.That(PropIn(c, "snack").gameObject.activeSelf, Is.True);
            FinishWithThreeStars(c);
        }

        static void AssertPropCentre(LevelProp prop, float artX, float artY)
        {
            var rect = (RectTransform)prop.transform;
            var settingPoint = rect.parent.InverseTransformPoint(rect.TransformPoint(rect.rect.center));
            var parent = (RectTransform)rect.parent;
            var fromBottomLeft = (Vector2)settingPoint - parent.rect.min;
            Assert.That(fromBottomLeft.x, Is.EqualTo(artX * 4f).Within(0.02f));
            Assert.That(fromBottomLeft.y, Is.EqualTo((270f - artY) * 4f).Within(0.02f));
        }

        static void FinishWithThreeStars(LevelController c)
        {
            Assert.That(c.finishButton.gameObject.activeSelf, Is.True);
            c.finishButton.onClick.Invoke();
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult.Stars, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator NormalLevel2_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(NormalTwo);
            var c = Level();
            yield return ReadyToChoose(Level);
            Assert.That(c.ActiveSetting.ActOutFor("hidden"), Is.Not.Null, "the boxes Choice has an Act-out");
            var student = c.Student;
            var open = PropIn(c, "bag_open");
            var toy = PropIn(c, "toy");
            var mom = c.ActiveSetting.ActiveGrownUps[0];
            var props = c.ActiveSetting.ActiveProps;

            Tap(c, "hidden");
            yield return WaitFor(() => open.transform.IsChildOf(student.transform) && student.IsWalking,
                "the Student to carry the bag toward the boxes");
            yield return WaitFor(() => student.bubble.Line == UIStrings.ReactionWait, "the boxes stop");
            Assert.That(student.X, Is.EqualTo(425f));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Crouch));
            AssertPropCentre(open, 448f, 202.5f);
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.UnsafeHeading));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            foreach (var prop in props) Assert.That(prop.AtStart, Is.True, prop.key);
            Assert.That(mom.AtStart, Is.True);
            Assert.That(student.X, Is.EqualTo(281f));
            Assert.That(ButtonFor(c, "hidden").button.interactable, Is.False);

            Tap(c, "toys");
            yield return WaitFor(() => student.Pose == StudentPose.Pack && toy.transform.IsChildOf(student.transform),
                "the Student to start packing the toy");
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo("Not ready yet"));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            foreach (var prop in props) Assert.That(prop.AtStart, Is.True, prop.key);
            Assert.That(mom.AtStart, Is.True);
            Assert.That(student.X, Is.EqualTo(281f));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(ButtonFor(c, "toys").button.interactable, Is.False);
        }

        [UnityTest]
        public IEnumerator NormalLevel3_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(NormalThree);
            var c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("home_v2"));
            Assert.That(c.ActiveSetting.GetComponentsInChildren<LevelProp>(), Is.Empty);
            Assert.That(c.ActiveSetting.GetComponentsInChildren<GrownUpActor>(), Is.Empty);
            Assert.That(c.Student.X, Is.EqualTo(294f));
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Surprised));
            var reaction = (RectTransform)c.Student.bubble.transform;
            Assert.That(reaction.anchoredPosition.y, Is.EqualTo(8f), "the Normal 3 Reaction stays below Hint");
            var shaker = c.ActiveSetting.shaker;
            Assert.That(shaker.IsShaking, Is.True);
            Assert.That(shaker.wobblers[0].enabled, Is.True);
            yield return ReadyToChoose(Level);
            var table = c.ActiveSetting.ActOutFor("table");
            Assert.That(table, Is.Not.Null);
            var overlay = table.FirstOverlay;
            Assert.That(overlay.activeSelf, Is.False);
            Tap(c, "table");
            yield return WaitFor(() => c.Student.Pose == StudentPose.Crouch && overlay.activeSelf, "the front of the table");
            yield return WaitFor(() => c.Student.Pose == StudentPose.Hold, "the Student to hold the table leg");
            Assert.That(c.Student.X, Is.EqualTo(240f));
            Assert.That(c.Student.FacingLeft, Is.True);
            Assert.That(overlay.activeSelf, Is.True);
            yield return WaitFor(() => !shaker.IsShaking, "the shaking to calm under the table");
            Assert.That(shaker.wobblers[0].enabled, Is.False);
            yield return FeedbackShown(c);
            FinishWithThreeStars(c);
        }

        [UnityTest]
        public IEnumerator NormalLevel3_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(NormalThree);
            var c = Level();
            yield return ReadyToChoose(Level);
            Assert.That(c.ActiveSetting.ActOutFor("glass"), Is.Not.Null, "the glass cabinet Choice has an Act-out");
            Tap(c, "glass");
            yield return WaitFor(() => c.Student.IsWalking && !c.Student.FacingLeft, "the Student to walk toward the cabinet");
            yield return WaitFor(() => c.Student.bubble.Line == UIStrings.ReactionWait, "the walk to stop");
            Assert.That(c.Student.X, Is.EqualTo(387f), "the Student stops inside the living room");
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.True);
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.UnsafeHeading));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Assert.That(c.Student.X, Is.EqualTo(294f));
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.True);
            Assert.That(ButtonFor(c, "glass").button.interactable, Is.False);
        }

        [UnityTest]
        public IEnumerator HardLevel1_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(HardOne);
            var c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("home_v2"));
            Assert.That(c.Student.X, Is.EqualTo(154f));
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.False);
            var props = c.ActiveSetting.GetComponentsInChildren<LevelProp>();
            Assert.That(Array.ConvertAll(props, p => p.key), Is.EquivalentTo(new[] { "park_card", "house_card" }));
            var park = PropIn(c, "park_card");
            var mom = c.ActiveSetting.ActiveGrownUps[0];
            Assert.That(mom.kind, Is.EqualTo(GrownUpKind.Mom));
            Assert.That(mom.X, Is.EqualTo(290f));

            yield return ReadyToChoose(Level);
            Assert.That(c.ActiveSetting.ActOutFor("meeting_place"), Is.Not.Null);
            Tap(c, 1);
            yield return WaitFor(() => park.transform.IsChildOf(c.Student.transform) && c.Student.IsWalking,
                "the Student to carry the park card");
            yield return WaitFor(() => c.Student.Pose == StudentPose.ReachUp, "the reach toward the board");
            Assert.That(c.Student.X, Is.EqualTo(136f));
            Assert.That(c.Student.FacingLeft, Is.True);
            Assert.That(c.Student.body.sprite.name, Does.Contain("_home_reachup_"));
            yield return FeedbackShown(c);
            AssertPropCentre(park, 117f, 178.5f);
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.Happy));
            Assert.That(PropIn(c, "house_card").AtStart, Is.True);
            FinishWithThreeStars(c);
        }

        [UnityTest]
        public IEnumerator HardLevel1_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(HardOne);
            var c = Level();
            yield return ReadyToChoose(Level);
            Assert.That(c.ActiveSetting.ActOutFor("meet_home"), Is.Not.Null, "the house Choice has an Act-out");
            var house = PropIn(c, "house_card");
            var park = PropIn(c, "park_card");
            var mom = c.ActiveSetting.ActiveGrownUps[0];
            Tap(c, 2);
            yield return WaitFor(() => c.Student.bubble.Line == UIStrings.ReactionWait, "the reach-up stop");
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.ReachUp));
            Assert.That(c.Student.X, Is.EqualTo(136f));
            Assert.That(house.AtStart, Is.False);
            AssertPropCentre(house, 117f, 178.5f);
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.UnsafeHeading));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Assert.That(house.AtStart, Is.True);
            Assert.That(park.AtStart, Is.True);
            Assert.That(mom.AtStart, Is.True);
            Assert.That(c.Student.X, Is.EqualTo(154f));
            Assert.That(c.actionBar.buttons[2].button.interactable, Is.False);
        }

        [UnityTest]
        public IEnumerator HomeActOuts_AreComplete()
        {
            foreach (var level in new[] { NormalTwo, NormalThree, HardOne })
            {
                yield return StartRolloutLevel(level);
                var c = Level();
                var setting = c.ActiveSetting;
                Assert.That(GameServices.Scenarios.TryGet(level, out var scenario), Is.True);

                foreach (var choice in scenario.choices)
                {
                    var act = setting.ActOutFor(choice.action);
                    Assert.That(act, Is.Not.Null, choice.id);
                    Assert.That(act.steps, Is.Not.Empty, choice.id);
                    if (!choice.safe)
                        Assert.That(act.stopAfter, Is.InRange(0, act.steps.Length - 1), choice.id);

                    foreach (var step in act.steps)
                    {
                        switch (step.kind)
                        {
                            case ActOutStepKind.PropShow:
                            case ActOutStepKind.PropHide:
                            case ActOutStepKind.PropPickUp:
                            case ActOutStepKind.PropPlace:
                            case ActOutStepKind.PropSlide:
                            case ActOutStepKind.Push:
                                Assert.That(step.target, Is.Not.Null, choice.id);
                                Assert.That(setting.ActiveProps, Does.Contain(step.target.GetComponent<LevelProp>()), choice.id);
                                break;
                            case ActOutStepKind.GrownUp:
                                Assert.That(setting.ActiveGrownUps, Does.Contain(step.grownUp), choice.id);
                                break;
                            case ActOutStepKind.Overlay:
                                Assert.That(step.target, Is.Not.Null, choice.id);
                                Assert.That(step.target.transform.IsChildOf(setting.transform), Is.True, choice.id);
                                break;
                        }
                    }
                }
            }
        }

        static readonly LevelId NormalFour = new LevelId(Episode.Earthquake, Difficulty.Normal, 4);
        static readonly LevelId HardTwo = new LevelId(Episode.Earthquake, Difficulty.Hard, 2);
        static readonly LevelId NormalFive = new LevelId(Episode.Earthquake, Difficulty.Normal, 5);

        [UnityTest]
        public IEnumerator Schoolyard_LevelsSelectTheirOwnCastAndStart()
        {
            yield return StartRolloutLevel(NormalFour);
            var c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("schoolyard_v2"));
            Assert.That(c.Student.X, Is.EqualTo(302f), "Normal 4 starts at the recess spot");
            Assert.That(c.Student.Y, Is.Zero);
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Surprised));
            Assert.That(c.actionBar.IsVisible, Is.False);
            Assert.That(c.ActiveSetting.GetComponentsInChildren<GrownUpActor>(), Is.Empty);
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.True);
            yield return ActionBarShown(c);
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));

            yield return StartRolloutLevel(HardTwo);
            c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("schoolyard_v2"));
            Assert.That(c.Student.X, Is.EqualTo(306f));
            Assert.That(c.Student.Y, Is.Zero);
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.False);
            foreach (var wobbler in c.ActiveSetting.shaker.wobblers)
                Assert.That(wobbler.enabled, Is.False, "the schoolyard stays still in Hard 2");
            var grownUps = c.ActiveSetting.GetComponentsInChildren<GrownUpActor>();
            Assert.That(grownUps, Has.Length.EqualTo(1));
            Assert.That(grownUps[0].kind, Is.EqualTo(GrownUpKind.Teacher));
            Assert.That(grownUps[0].X, Is.EqualTo(217f), "the Teacher stands in the doorway");
            Assert.That(grownUps[0].Y, Is.Zero);
            Assert.That(grownUps[0].Pose, Is.EqualTo(GrownUpPose.Wave));
        }

        [UnityTest]
        public IEnumerator NormalLevel4_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(NormalFour);
            var c = Level();
            yield return ActionBarShown(c);
            Assert.That(c.ActiveSetting.ActOutFor("field"), Is.Not.Null, "the field Choice has an Act-out");
            var student = c.Student;
            var shaker = c.ActiveSetting.shaker;
            Assert.That(shaker.wobblers, Has.Length.EqualTo(2));
            foreach (var wobbler in shaker.wobblers)
            {
                Assert.That(wobbler.enabled, Is.True);
                var image = wobbler.GetComponent<Image>();
                yield return WaitFor(() => image.sprite == wobbler.frames[1], wobbler.name + " to sway");
            }

            Tap(c, "field");
            yield return WaitFor(() => student.IsWalking && student.FacingLeft, "the walk into the field");
            yield return WaitFor(() => student.Pose == StudentPose.Crouch, "the crouch in the field");
            Assert.That(student.X, Is.EqualTo(110f));
            yield return WaitFor(() => student.Pose == StudentPose.Cover, "the Student to cover");
            Assert.That(student.FacingLeft, Is.True);
            Assert.That(shaker.IsShaking, Is.True, "the Student covers while the ground still shakes");
            yield return WaitFor(() => !shaker.IsShaking, "the schoolyard to calm");
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Cover));
            Assert.That(student.X, Is.EqualTo(110f));
            Assert.That(student.Y, Is.Zero);
            foreach (var wobbler in shaker.wobblers)
            {
                Assert.That(wobbler.enabled, Is.False);
                Assert.That(wobbler.GetComponent<Image>().sprite, Is.EqualTo(wobbler.frames[0]));
            }

            yield return FeedbackShown(c);
            FinishWithThreeStars(c);
        }

        [UnityTest]
        public IEnumerator NormalLevel4_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(NormalFour);
            var c = Level();
            yield return ActionBarShown(c);
            Assert.That(c.ActiveSetting.ActOutFor("building"), Is.Not.Null, "the building Choice has an Act-out");
            Assert.That(c.ActiveSetting.ActOutFor("tree"), Is.Not.Null, "the tree Choice has an Act-out");
            Assert.That(GameServices.Scenarios.TryGet(NormalFour, out var scenario), Is.True);
            var student = c.Student;
            var shaker = c.ActiveSetting.shaker;

            Tap(c, "building");
            yield return WaitFor(() => student.IsWalking && student.FacingLeft, "the run toward the door");
            yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait, "the building stop");
            Assert.That(student.X, Is.EqualTo(260f), "the Student stops outside the doorway");
            Assert.That(student.Y, Is.Zero);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Wait));
            Assert.That(shaker.IsShaking, Is.True);

            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(scenario.choices[2].heading));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            AssertSchoolyardReset(c, 302f, 2);
            Assert.That(shaker.IsShaking, Is.True);

            Tap(c, "tree");
            yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait, "the tree stop");
            Assert.That(student.X, Is.EqualTo(320f), "the Student stops at the edge of the shade");
            Assert.That(student.Y, Is.EqualTo(31f));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Wait));
            Assert.That(shaker.IsShaking, Is.True);

            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.UnsafeHeading));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            AssertSchoolyardReset(c, 302f, 0);
            Assert.That(shaker.IsShaking, Is.True);
        }

        static void AssertSchoolyardReset(LevelController c, float startX, int tried)
        {
            Assert.That(c.Student.X, Is.EqualTo(startX));
            Assert.That(c.Student.Y, Is.Zero);
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.actionBar.buttons[tried].button.interactable, Is.False);
        }

        [UnityTest]
        public IEnumerator NormalLevel5_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(NormalFive);
            var c = Level();
            yield return ActionBarShown(c);
            var route = c.ActiveSetting.ActOutFor("high_ground");
            Assert.That(route, Is.Not.Null, "the high ground Choice has an Act-out");
            Assert.That(c.ActiveSetting.key, Is.EqualTo("beach_v2"));
            Assert.That(c.caption.rect.width, Is.LessThanOrEqualTo(960f),
                "the beach Caption leaves room for the Level label and hilltop Reaction");
            Assert.That(c.captionGoal.rectTransform.rect.width,
                Is.GreaterThanOrEqualTo(PixelText.Width(c.captionGoal.font, c.captionGoal.text)),
                "the full Goal fits the Caption");
            Assert.That(c.ActiveSetting.GetComponentsInChildren<GrownUpActor>(), Is.Empty);
            Assert.That(c.Student.X, Is.EqualTo(260f));
            Assert.That(c.Student.Y, Is.Zero);
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));

            var student = c.Student;
            Tap(c, "high_ground");
            yield return WaitFor(() => student.IsWalking && student.X > 300f && student.Y > 5f, "the climb up the beach path");
            Assert.That(student.FacingLeft, Is.False);
            Assert.That(Array.FindAll(route.steps, step => step.kind == ActOutStepKind.FastWalk).Length,
                Is.GreaterThanOrEqualTo(3), "the Student fast-walks around the turns of the uphill path");
            yield return FeedbackShown(c);
            Assert.That(student.X, Is.EqualTo(386f));
            Assert.That(student.Y, Is.EqualTo(79f));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            var halfWidth = ((RectTransform)student.transform).rect.width / student.unitsPerPixel / 2f;
            Assert.That(student.X + halfWidth, Is.LessThan(405f), "the Student stands beside the sign");
            FinishWithThreeStars(c);
        }

        [UnityTest]
        public IEnumerator NormalLevel5_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(NormalFive);
            var c = Level();
            yield return ActionBarShown(c);

            foreach (var stop in new[]
            {
                (Action: "wait", X: 200f, Y: 11f, Pose: StudentPose.Sit, Left: false),
                (Action: "watch", X: 72f, Y: 14f, Pose: StudentPose.Wait, Left: true),
            })
            {
                Tap(c, stop.Action);
                yield return WaitFor(() => c.Student.IsWalking && c.Student.FacingLeft, "the walk toward the Unsafe stop");
                yield return WaitFor(() => c.Student.bubble.gameObject.activeSelf &&
                    c.Student.bubble.Line == UIStrings.ReactionWait, "the beach stop");
                Assert.That(c.Student.X, Is.EqualTo(stop.X));
                Assert.That(c.Student.Y, Is.EqualTo(stop.Y));
                Assert.That(c.Student.Pose, Is.EqualTo(stop.Pose));
                Assert.That(c.Student.FacingLeft, Is.EqualTo(stop.Left));

                yield return FeedbackShown(c);
                Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.UnsafeHeading));
                c.tryAgainButton.onClick.Invoke();
                yield return ActionBarShown(c);
                Assert.That(c.Student.X, Is.EqualTo(260f));
                Assert.That(c.Student.Y, Is.Zero);
                Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
                Assert.That(ButtonFor(c, stop.Action).button.interactable, Is.False);
            }
        }

        [UnityTest]
        public IEnumerator NormalLevel5_WaterLineAnimatesWhileTheBeachIsStill()
        {
            yield return StartRolloutLevel(NormalFive);
            var c = Level();
            yield return ActionBarShown(c);
            var water = Array.Find(c.ActiveSetting.GetComponentsInChildren<SpriteAnimator>(),
                animator => animator.name == "Water Line");
            Assert.That(water, Is.Not.Null, "the beach has an animated water line");
            Assert.That(c.ActiveSetting.shaker?.IsShaking ?? false, Is.False);
            var image = water.GetComponent<Image>();
            var first = image.sprite;
            yield return WaitFor(() => image.sprite != first && c.actionBar.IsVisible, "the water line to move", 3f);
            Assert.That(c.ActiveSetting.shaker?.IsShaking ?? false, Is.False);
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
        }

        [UnityTest]
        public IEnumerator HardLevel3_SafeChoice_ActsOutAndCompletes()
        {
            var level = new LevelId(Episode.Earthquake, Difficulty.Hard, 3);
            yield return StartRolloutLevel(level);
            var c = Level();
            var setting = c.ActiveSetting;
            var student = c.Student;
            Assert.That(setting.key, Is.EqualTo("mall_a_v2"));
            Assert.That(setting.shaker.IsShaking, Is.True);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Surprised));
            Assert.That(student.bubble.Line, Is.EqualTo(UIStrings.ReactionWhoa));
            Assert.That(setting.ActiveGrownUps, Is.Empty);
            yield return ActionBarShown(c);

            var cover = setting.ActOutFor("table");
            Assert.That(cover, Is.Not.Null, "the food court table Choice has an Act-out");
            var overlay = cover.FirstOverlay;
            Assert.That(overlay, Is.Not.Null);
            Assert.That(overlay.activeSelf, Is.False);
            Assert.That(overlay.transform.GetSiblingIndex(), Is.GreaterThan(student.transform.GetSiblingIndex()));

            Tap(c, 3);
            yield return WaitFor(() => student.IsWalking, "the walk to the food court table");
            yield return WaitFor(() => student.Pose == StudentPose.Crouch, "the Student to duck under the table");
            Assert.That(overlay.activeInHierarchy, Is.True, "the table front covers the Student while crouching");
            yield return WaitFor(() => student.Pose == StudentPose.Hold, "the hold under the table");
            Assert.That(student.X, Is.EqualTo(168f));
            Assert.That(student.Y, Is.Zero);
            yield return FeedbackShown(c);
            Assert.That(setting.shaker.IsShaking, Is.False);
            FinishWithThreeStars(c);
        }

        [UnityTest]
        public IEnumerator HardLevel3_UnsafeChoice_StopsAndResets()
        {
            var level = new LevelId(Episode.Earthquake, Difficulty.Hard, 3);
            yield return StartRolloutLevel(level);
            var c = Level();
            var setting = c.ActiveSetting;
            var student = c.Student;
            yield return ActionBarShown(c);

            foreach (var stop in new[]
            {
                (Index: 1, Key: "elevator", X: 363f, Y: 0f, Pose: StudentPose.Reach, Heading: UIStrings.UnsafeHeading),
                (Index: 2, Key: "shop_window", X: 307f, Y: 0f, Pose: StudentPose.Wait, Heading: UIStrings.UnsafeHeading),
                (Index: 0, Key: "run_exit", X: 66f, Y: -17f, Pose: StudentPose.Wait, Heading: "Not safe yet"),
            })
            {
                Assert.That(setting.ActOutFor(stop.Key), Is.Not.Null, "the food court Unsafe Choice has an Act-out");
                Tap(c, stop.Index);
                yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait,
                    "the food court stop at " + stop.Key);
                Assert.That(student.X, Is.EqualTo(stop.X));
                Assert.That(student.Y, Is.EqualTo(stop.Y));
                Assert.That(student.Pose, Is.EqualTo(stop.Pose));
                if (stop.Key == "elevator")
                    Assert.That(student.X + 16f, Is.LessThan(387f), "the Student stays outside the elevator doors");
                if (stop.Key == "shop_window")
                {
                    Assert.That(230f - student.Y, Is.GreaterThan(207f), "the Student stops in the aisle in front of the glass");
                    // The glazing spans art x 278 to 354; the Reaction must leave its right half in view.
                    var reaction = BoundsInSetting(setting, (RectTransform)student.bubble.transform);
                    Assert.That(reaction.xMax, Is.LessThan(316f * 4f), "the Reaction leaves the shop glass visible");
                }
                if (stop.Key == "run_exit")
                    Assert.That(student.X + 16f, Is.LessThan(83f), "the run stops on open floor, left of the stool");

                yield return FeedbackShown(c);
                Assert.That(c.feedbackHeading.text, Is.EqualTo(stop.Heading));
                c.tryAgainButton.onClick.Invoke();
                yield return ActionBarShown(c);
                Assert.That(student.X, Is.EqualTo(230f));
                Assert.That(student.Y, Is.Zero);
                Assert.That(student.Pose, Is.EqualTo(setting.StartPose));
                Assert.That(c.actionBar.buttons[stop.Index].button.interactable, Is.False);
            }
        }

        [UnityTest]
        public IEnumerator HardLevel3_HangingSign_SwaysOnlyWhileShaking()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 3));
            var c = Level();
            var shaker = c.ActiveSetting.shaker;
            var sign = Array.Find(c.ActiveSetting.GetComponentsInChildren<SpriteAnimator>(), a => a.name == "Food Court Sign");
            Assert.That(sign, Is.Not.Null, "the food court has an animated hanging sign");
            Assert.That(shaker.wobblers, Does.Contain(sign));
            Assert.That(shaker.IsShaking, Is.True);
            var image = sign.GetComponent<Image>();
            var initial = image.sprite;
            yield return WaitFor(() => image.sprite != initial, "the hanging sign to sway");
            yield return ActionBarShown(c);

            Tap(c, 3);
            yield return FeedbackShown(c);
            Assert.That(shaker.IsShaking, Is.False);
            Assert.That(sign.enabled, Is.False);
            var resting = image.sprite;
            yield return Wait(0.4f);
            Assert.That(image.sprite, Is.SameAs(resting), "the sign holds its resting frame after the shaking stops");
        }

        [UnityTest]
        public IEnumerator HardLevel4_StartsOnTheUpperFloor()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 4));
            var c = Level();
            var setting = c.ActiveSetting;
            Assert.That(setting.key, Is.EqualTo("mall_b_v2"));
            Assert.That(setting.startY, Is.EqualTo(52f), "the landing is 52 art pixels above the floor line");
            Assert.That(c.Student.X, Is.EqualTo(129f));
            Assert.That(c.Student.Y, Is.EqualTo(setting.startY));
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(setting.shaker, Is.Null);
            Assert.That(setting.ActiveGrownUps, Is.Empty);

            var student = BoundsInSetting(setting, (RectTransform)c.Student.transform);
            Assert.That(student.Overlaps(BoundsInSetting(setting, c.caption)), Is.False);
            Assert.That(student.Overlaps(BoundsInSetting(setting, (RectTransform)c.hintButton.transform)), Is.False);
            Assert.That(student.Overlaps(BoundsInSetting(setting, (RectTransform)c.pauseButton.transform)), Is.False);
        }

        // Between x 203 and 227 the tread line descends from 27 to 13 pixels above the floor.
        static bool OnMallStairs(StudentActor student)
        {
            if (!student.IsWalking || student.X < 203f || student.X > 227f) return false;
            var expectedY = Mathf.Lerp(27f, 13f, (student.X - 203f) / 24f);
            return Mathf.Abs(student.Y - expectedY) <= 1f;
        }

        [UnityTest]
        public IEnumerator HardLevel4_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 4));
            var c = Level();
            var student = c.Student;
            yield return ActionBarShown(c);
            Assert.That(c.ActiveSetting.ActOutFor("stairs"), Is.Not.Null, "the stairs Choice has an Act-out");

            Tap(c, 0);
            yield return WaitFor(() => OnMallStairs(student) && !student.FacingLeft, "the Student to descend the stairs");
            var higherY = student.Y;
            var earlierX = student.X;
            yield return WaitFor(() => student.IsWalking && student.X >= 240f, "the Student to reach the foot of the stairs");
            Assert.That(student.X, Is.GreaterThan(earlierX));
            Assert.That(student.Y, Is.LessThan(higherY));
            yield return WaitFor(() => student.IsWalking && student.X > 331f, "the Student to leave through the exit");
            yield return FeedbackShown(c);
            Assert.That(student.X, Is.EqualTo(430f));
            Assert.That(student.Y, Is.Zero);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            var halfWidth = ((RectTransform)student.transform).rect.width / student.unitsPerPixel / 2f;
            Assert.That(student.X + halfWidth, Is.LessThan(452f), "the Student stands beside the parking sign");
            // The outer wall ends at art x 373 and the pavement edge at about x 384 on the standing row.
            Assert.That(student.X - halfWidth, Is.GreaterThan(405f), "the Student stands out in the open parking, clear of the mall");
            FinishWithThreeStars(c);
        }

        [UnityTest]
        public IEnumerator HardLevel4_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 4));
            var c = Level();
            var setting = c.ActiveSetting;
            var student = c.Student;
            yield return ActionBarShown(c);

            foreach (var stop in new[]
            {
                (Index: 2, Key: "run_push", X: 270f, Y: 9f, Pose: StudentPose.Wait, Heading: "Not safe yet"),
                (Index: 3, Key: "mall_wall", X: 357f, Y: 0f, Pose: StudentPose.Wait, Heading: "Not safe yet"),
                (Index: 1, Key: "elevator", X: 96f, Y: 52f, Pose: StudentPose.Reach, Heading: UIStrings.UnsafeHeading),
            })
            {
                Assert.That(setting.ActOutFor(stop.Key), Is.Not.Null, "the way out Unsafe Choice has an Act-out");
                Tap(c, stop.Index);
                if (stop.Key == "run_push")
                    yield return WaitFor(() => OnMallStairs(student) && !student.FacingLeft, "the run down the stairs");
                yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait,
                    "the way out stop at " + stop.Key);
                Assert.That(student.X, Is.EqualTo(stop.X));
                Assert.That(student.Y, Is.EqualTo(stop.Y));
                Assert.That(student.Pose, Is.EqualTo(stop.Pose));
                if (stop.Key == "run_push")
                    Assert.That(student.X + 16f, Is.LessThan(289f), "the Student stays short of the crowd");
                if (stop.Key == "mall_wall")
                    Assert.That(student.X, Is.InRange(333f, 373f), "the Student waits beside the outer wall");
                if (stop.Key == "elevator")
                    Assert.That(student.FacingLeft, Is.True, "the Student reaches toward the call button");

                yield return FeedbackShown(c);
                Assert.That(c.feedbackHeading.text, Is.EqualTo(stop.Heading));
                c.tryAgainButton.onClick.Invoke();
                if (stop.Y < setting.startY)
                    yield return WaitFor(() => OnMallStairs(student) && student.FacingLeft,
                        "the Try Again return to retrace the stairs from " + stop.Key);
                yield return ActionBarShown(c);
                Assert.That(student.X, Is.EqualTo(129f));
                Assert.That(student.Y, Is.EqualTo(52f));
                Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
                Assert.That(c.actionBar.buttons[stop.Index].button.interactable, Is.False);
            }
        }

        // C11: the exterior in Hard 4 (mall_b). The mall's corner at x 373 runs from the cornice to the
        // pavement, a low curb closes the far edge of the lot, and the painted parking lines are two art
        // pixels thick. The Safe ending stands on open asphalt inside a green painted assembly box, and the
        // wall stop on the pavement beside the mall, with open sky above the assembly sign.
        [UnityTest]
        public IEnumerator HardLevel4_Exterior_ShowsTheMallCornerCurbAndParking()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 4));
            var setting = Level().ActiveSetting;
            var student = Level().Student;
            var safeSteps = setting.ActOutFor("stairs").steps;
            var wallSteps = setting.ActOutFor("mall_wall").steps;
            var parkingX = Mathf.RoundToInt(safeSteps[safeSteps.Length - 1].x);
            var wallStopX = Mathf.RoundToInt(wallSteps[wallSteps.Length - 1].x);
            Assert.That(parkingX, Is.EqualTo(430), "the Safe ending is out in the parking");
            Assert.That(wallStopX, Is.EqualTo(357), "the wall stop is beside the mall");
            var halfWidth = Mathf.RoundToInt(((RectTransform)student.transform).rect.width / student.unitsPerPixel / 2f);

            var art = LevelArt("bg_mall_b");
            for (var row = 0; row <= 215; row++)
            {
                var edge = false;
                for (var x = 373; x <= 377; x++) edge |= SameColour(art[x, row], ArtOutline);
                Assert.That(edge, Is.True, "the mall's exterior corner is outlined on row " + row);
            }

            // The assembly sign stands at x 452 to 476 and covers the curb there.
            for (var x = 376; x < 480; x++)
            {
                if (x >= 452 && x <= 476) continue;
                Assert.That(SameColour(art[x, 189], ArtOutline) && SameColour(art[x, 195], ArtOutline), Is.True,
                    "the curb is outlined above and below at x " + x);
                Assert.That(SameColour(art[x, 192], ArtRoad) || SameColour(art[x, 192], ArtGrass), Is.False, "the curb face at x " + x);
                Assert.That(SameColour(art[x, 196], ArtRoad), Is.True, "the asphalt starts below the curb at x " + x);
            }

            var dividers = 0;
            for (var x = 380; x < 480;)
            {
                var start = x;
                while (x < 480 && SameColour(art[x, 205], ArtWhite)) x++;
                if (x - start >= 2) dividers++;
                if (x == start) x++;
            }
            Assert.That(dividers, Is.GreaterThanOrEqualTo(3), "the back stalls have dividers two art pixels thick");
            for (var x = 390; x <= 455; x++)
                Assert.That(SameColour(art[x, 213], ArtWhite) && SameColour(art[x, 214], ArtWhite), Is.True,
                    "the back stalls end in a line two art pixels thick at x " + x);

            for (var x = 452; x <= 476; x++)
                for (var row = 0; row <= 140; row++)
                    Assert.That(SameColour(art[x, row], ArtSky), Is.True, "open sky above the assembly sign at " + x + ", " + row);

            // Row 231 is just below the Student's feet at row 230.
            for (var x = parkingX - halfWidth + 6; x <= parkingX + halfWidth - 6; x++)
                Assert.That(SameColour(art[x, 231], ArtRoad), Is.True, "the Safe ending stands on open asphalt at x " + x);
            Assert.That(wallStopX, Is.InRange(333, 373), "the wall stop is in front of the mall's outer wall");
            Assert.That(SameColour(art[wallStopX, 231], ArtRoad), Is.False, "the wall stop stands on the pavement");
            Assert.That(parkingX - halfWidth, Is.GreaterThan(377), "the Safe ending is clear of the mall's corner");

            // A green painted box on the asphalt marks where people gather around the Safe ending. Its left
            // edge is up to 8 art pixels left of the Student's frame and its right edge up to 16 right of it.
            // The 7 art pixels each side of the feet centre are the asphalt checked above.
            bool BoxIn(int from, int to)
            {
                for (var x = from; x <= to; x++)
                    if (SameColour(art[x, 231], ArtSign)) return true;
                return false;
            }
            Assert.That(BoxIn(parkingX - halfWidth - 8, parkingX - 7), Is.True, "the assembly box left of the Safe ending");
            Assert.That(BoxIn(parkingX + 7, parkingX + halfWidth + 16), Is.True, "the assembly box right of the Safe ending");
        }

        // The Hard 4 exterior the researcher approved for C11 (variant B, with the green assembly box).
        [Test]
        public void HardLevel4_ExteriorArt_IsTheApprovedPicture()
        {
            Assert.That(ArtHash("bg_mall_b"), Is.EqualTo("2837ece26b9ff19bf5f04381100c06f4be852f839d1049a6b259357802b55384"));
        }

        // C11 changes only mall_b. Hard 3's food court and its overlays come from the same script and stay
        // the same, pixel for pixel.
        [Test]
        public void HardLevel3_MallArt_IsUnchanged()
        {
            Assert.That(ArtHash("bg_mall_a"), Is.EqualTo("e493330e96fb9763a00c663b75faac5ff0d6775bdfdae42b25b33084bf2df03e"));
            Assert.That(ArtHash("foodtable_front"), Is.EqualTo("236cd4938748836a6eda7170bdaf84e0dd888146926c4d42795ac4b5fcc26694"));
            Assert.That(ArtHash("mall_foodcourt_sign_0"), Is.EqualTo("538d232bd4cfca421aefd97f928d8210f16cb2a7a78a503b8e040fdd7e1bc4fe"));
            Assert.That(ArtHash("mall_foodcourt_sign_1"), Is.EqualTo("380a4d9761058abefaca3bb7450b2e9e1f81c5b270279f3381e064b5df84e850"));
            Assert.That(ArtHash("mall_foodcourt_sign_2"), Is.EqualTo("538d232bd4cfca421aefd97f928d8210f16cb2a7a78a503b8e040fdd7e1bc4fe"));
            Assert.That(ArtHash("mall_foodcourt_sign_3"), Is.EqualTo("fa27f739dca07e0ee64054791a3ef2c8e2629bd0586c7edd3ca03c9eb14857ed"));
        }

        // Colours from ArtSource/scripts: the outline, the asphalt (ROAD), white paint, the sky, the grass
        // and the green of the signs (SIGN).
        static readonly Color32 ArtOutline = new Color32(40, 28, 44, 255);
        static readonly Color32 ArtRoad = new Color32(152, 156, 168, 255);
        static readonly Color32 ArtWhite = new Color32(250, 252, 255, 255);
        static readonly Color32 ArtSky = new Color32(196, 232, 248, 255);
        static readonly Color32 ArtGrass = new Color32(124, 180, 150, 255);
        static readonly Color32 ArtSign = new Color32(52, 108, 82, 255);

        static bool SameColour(Color32 a, Color32 b) => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;

        static Color32[] ArtPixels(string name, out int width, out int height)
        {
            var path = Path.Combine(Application.dataPath, "Art", "Level", "v2", name + ".png");
            var texture = new Texture2D(2, 2);
            Assert.That(texture.LoadImage(File.ReadAllBytes(path)), Is.True, "read " + path);
            width = texture.width;
            height = texture.height;
            var pixels = texture.GetPixels32();
            UnityEngine.Object.Destroy(texture);
            return pixels;
        }

        // A v2 Level picture as [x, row], with row 0 at the top as in the art scripts. The import is not
        // readable, so this reads the PNG.
        static Color32[,] LevelArt(string name)
        {
            var pixels = ArtPixels(name, out var width, out var height);
            var art = new Color32[width, height];
            for (var row = 0; row < height; row++)
                for (var x = 0; x < width; x++)
                    art[x, row] = pixels[(height - 1 - row) * width + x];
            return art;
        }

        // The SHA-256 of a v2 Level picture's RGBA pixels, bottom row first.
        static string ArtHash(string name)
        {
            var pixels = ArtPixels(name, out _, out _);
            var bytes = new byte[pixels.Length * 4];
            for (var i = 0; i < pixels.Length; i++)
            {
                bytes[i * 4] = pixels[i].r;
                bytes[i * 4 + 1] = pixels[i].g;
                bytes[i * 4 + 2] = pixels[i].b;
                bytes[i * 4 + 3] = pixels[i].a;
            }
            using var sha = System.Security.Cryptography.SHA256.Create();
            return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
        }

        [UnityTest]
        public IEnumerator HardLevel5_StartsWithMomHurtAndDadAway()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 5));
            var c = Level();
            var setting = c.ActiveSetting;
            Assert.That(setting.key, Is.EqualTo("home_after_v2"));
            Assert.That(setting.ActiveGrownUps, Has.Length.EqualTo(2), "Hard 5 has Mom and Dad");
            var mom = System.Array.Find(setting.ActiveGrownUps, actor => actor.kind == GrownUpKind.Mom);
            var dad = System.Array.Find(setting.ActiveGrownUps, actor => actor.kind == GrownUpKind.Dad);
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.HurtSit));
            Assert.That(mom.X, Is.EqualTo(322f));
            Assert.That(mom.Y, Is.EqualTo(-3f));
            Assert.That(BoundsInSetting(setting, dad.body.rectTransform).xMax, Is.LessThan(0f));
            Assert.That(c.Student.X, Is.EqualTo(286f));
            Assert.That(c.Student.Y, Is.Zero);
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(setting.shaker, Is.Null, "the room does not shake");

            var drip = setting.GetComponentInChildren<SpriteAnimator>();
            Assert.That(drip, Is.Not.Null, "the pipe drips while the room is still");
            var firstFrame = drip.GetComponent<Image>().sprite;
            yield return WaitFor(() => drip.GetComponent<Image>().sprite != firstFrame, "the pipe drip to advance");
        }

        [UnityTest]
        public IEnumerator HardLevel5_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 5));
            var c = Level();
            var setting = c.ActiveSetting;
            var student = c.Student;
            var mom = System.Array.Find(setting.ActiveGrownUps, actor => actor.kind == GrownUpKind.Mom);
            var dad = System.Array.Find(setting.ActiveGrownUps, actor => actor.kind == GrownUpKind.Dad);
            yield return ActionBarShown(c);
            Assert.That(setting.ActOutFor("call_adult"), Is.Not.Null, "calling a grown-up has an Act-out");

            Tap(c, 1);
            yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == "Stay still, Mom.",
                "the Student to speak to Mom");
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Crouch));
            Assert.That(student.X, Is.EqualTo(284f));
            Assert.That(dad.AtStart, Is.True);
            yield return WaitFor(() => student.IsWalking && student.Pose == StudentPose.Walk && student.FacingLeft,
                "the Student to stand and walk to the door");
            yield return WaitFor(() => student.bubble.Line == "Help!", "the Student to call for help");
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Call));
            Assert.That(student.X, Is.EqualTo(79f));
            Assert.That(dad.AtStart, Is.True, "Dad stays away until the Student calls");
            yield return WaitFor(() => dad.IsWalking && dad.X > 0f, "Dad to enter after the call");

            yield return FeedbackShown(c);
            Assert.That(dad.Pose, Is.EqualTo(GrownUpPose.Kneel));
            Assert.That(dad.X, Is.EqualTo(286f));
            Assert.That(dad.Y, Is.EqualTo(-14f));
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.HurtSit));
            var dadBounds = BoundsInSetting(setting, dad.body.rectTransform);
            Assert.That(dadBounds.Overlaps(BoundsInSetting(setting, (RectTransform)mom.transform)), Is.False);
            Assert.That(dadBounds.Overlaps(BoundsInSetting(setting, (RectTransform)student.transform)), Is.False);
            AssertHomeAfterReactionClear(setting, mom);
            FinishWithThreeStars(c);
        }

        static void AssertHomeAfterReactionClear(LevelSetting setting, GrownUpActor mom)
        {
            const float px = 4f;
            const float artHeight = 270f;
            var reaction = BoundsInSetting(setting, (RectTransform)setting.student.bubble.transform);
            var frontDoor = new Rect(16f * px, (artHeight - 212f) * px, 56f * px, 105f * px);
            var pipe = new Rect(123f * px, (artHeight - 217f) * px, 29f * px, 24f * px);
            Assert.That(reaction.Overlaps(frontDoor), Is.False, "the Reaction leaves the front door in view");
            Assert.That(reaction.Overlaps(pipe), Is.False, "the Reaction leaves the pipe and puddle in view");
            Assert.That(reaction.Overlaps(BoundsInSetting(setting, (RectTransform)mom.transform)), Is.False,
                "the Reaction leaves Mom in view");
        }

        [UnityTest]
        public IEnumerator HardLevel5_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 5));
            var c = Level();
            var setting = c.ActiveSetting;
            var student = c.Student;
            var mom = System.Array.Find(setting.ActiveGrownUps, actor => actor.kind == GrownUpKind.Mom);
            var dad = System.Array.Find(setting.ActiveGrownUps, actor => actor.kind == GrownUpKind.Dad);
            yield return ActionBarShown(c);

            foreach (var stop in new[]
            {
                (Index: 0, Key: "pull_up", X: 284f, Y: -4f, Pose: StudentPose.Reach, Heading: "Not safe yet"),
                (Index: 2, Key: "fix_pipe", X: 167f, Y: -6f, Pose: StudentPose.Reach, Heading: UIStrings.UnsafeHeading),
                (Index: 3, Key: "leave", X: 100f, Y: -2f, Pose: StudentPose.Wait, Heading: UIStrings.UnsafeHeading),
            })
            {
                Assert.That(setting.ActOutFor(stop.Key), Is.Not.Null, stop.Key + " has an Unsafe Act-out");
                Tap(c, stop.Index);
                yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait,
                    "the stop at " + stop.Key);
                Assert.That(student.X, Is.EqualTo(stop.X));
                Assert.That(student.Y, Is.EqualTo(stop.Y));
                Assert.That(student.Pose, Is.EqualTo(stop.Pose));
                Assert.That(mom.AtStart, Is.True);
                Assert.That(dad.AtStart, Is.True);
                if (stop.Key == "pull_up")
                    Assert.That(student.X + 16f, Is.LessThan(mom.X - 18f), "the Student stops short of Mom's hand");
                if (stop.Key == "fix_pipe")
                    Assert.That(student.X - 15f, Is.GreaterThan(151f), "the Student stops short of the pipe");
                if (stop.Key == "leave")
                    Assert.That(student.X - 16f, Is.GreaterThan(72f), "the Student stops inside the room");
                AssertHomeAfterReactionClear(setting, mom);

                yield return FeedbackShown(c);
                Assert.That(c.feedbackHeading.text, Is.EqualTo(stop.Heading));
                c.tryAgainButton.onClick.Invoke();
                yield return ActionBarShown(c);
                Assert.That(student.X, Is.EqualTo(286f));
                Assert.That(student.Y, Is.Zero);
                Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
                Assert.That(mom.AtStart, Is.True);
                Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.HurtSit));
                Assert.That(dad.AtStart, Is.True);
                Assert.That(c.actionBar.buttons[stop.Index].button.interactable, Is.False);
            }
        }

        [UnityTest]
        public IEnumerator HardLevel6_StartsWithTheBagAndMomAtTheDoor()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 6));
            var c = Level();
            var setting = c.ActiveSetting;
            Assert.That(setting.key, Is.EqualTo("home_after_v2"));
            Assert.That(setting.ActiveGrownUps, Has.Length.EqualTo(1), "Hard 6 has Mom at the door");
            var mom = setting.ActiveGrownUps[0];
            Assert.That(mom.kind, Is.EqualTo(GrownUpKind.Mom));
            Assert.That(mom.X, Is.EqualTo(38f));
            Assert.That(mom.Y, Is.EqualTo(15f));
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.Idle));
            Assert.That(c.Student.X, Is.EqualTo(286f));
            Assert.That(c.Student.Y, Is.Zero);
            Assert.That(setting.shaker, Is.Null);

            AssertPropAtArtPoint(setting, PropIn(c, "bag_closed"), 73f, 191f);
            AssertPropAtArtPoint(setting, PropIn(c, "paper"), 232f, 180f);
            AssertPropAtArtPoint(setting, PropIn(c, "pen"), 245f, 181f);
            Assert.That(PropIn(c, "note").gameObject.activeSelf, Is.False);
        }

        static void AssertPropAtArtPoint(LevelSetting setting, LevelProp prop, float x, float row)
        {
            Assert.That(prop.gameObject.activeInHierarchy, Is.True);
            var bounds = BoundsInSetting(setting, (RectTransform)prop.transform);
            Assert.That(bounds.xMin, Is.EqualTo(x * 4f).Within(0.01f));
            Assert.That(bounds.yMax, Is.EqualTo((270f - row) * 4f).Within(0.01f));
        }

        [UnityTest]
        public IEnumerator HardLevel6_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 6));
            var c = Level();
            var setting = c.ActiveSetting;
            var student = c.Student;
            var mom = setting.ActiveGrownUps[0];
            var bag = PropIn(c, "bag_closed");
            var paper = PropIn(c, "paper");
            var note = PropIn(c, "note");
            yield return ActionBarShown(c);
            Assert.That(setting.ActOutFor("go_bag"), Is.Not.Null, "taking the go bag has an Act-out");

            Tap(c, 3);
            yield return WaitFor(() => student.Pose == StudentPose.Pack, "the Student to write at the table");
            Assert.That(note.gameObject.activeSelf, Is.False, "the note follows the writing");
            Assert.That(student.X, Is.EqualTo(238f));
            Assert.That(student.Y, Is.EqualTo(22f));
            yield return WaitFor(() => note.gameObject.activeSelf, "the written note to appear");
            AssertPropAtArtPoint(setting, note, 232f, 180f);
            Assert.That(paper.gameObject.activeSelf, Is.False);
            Assert.That(bag.AtStart, Is.True, "writing comes before picking up the bag");
            yield return WaitFor(() => student.Pose == StudentPose.Carry && student.IsWalking && mom.IsWalking,
                "Mom and the Student to walk out together with the bag");
            Assert.That(bag.transform.parent, Is.SameAs(student.Hands));
            Assert.That(student.FacingLeft, Is.True);

            yield return FeedbackShown(c);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Carry));
            Assert.That(student.X, Is.EqualTo(56f));
            Assert.That(student.Y, Is.EqualTo(24f));
            Assert.That(mom.X, Is.EqualTo(40f));
            Assert.That(mom.Y, Is.EqualTo(55f));
            Assert.That(note.gameObject.activeSelf, Is.True);
            Assert.That(bag.transform.parent, Is.SameAs(student.Hands));
            AssertHomeAfterReactionClear(setting, mom);
            FinishWithThreeStars(c);
        }

        [UnityTest]
        public IEnumerator HardLevel6_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 6));
            var c = Level();
            var setting = c.ActiveSetting;
            var student = c.Student;
            var mom = setting.ActiveGrownUps[0];
            var bag = PropIn(c, "bag_closed");
            var note = PropIn(c, "note");
            yield return ActionBarShown(c);

            foreach (var stop in new[]
            {
                (Index: 0, Key: "no_go_bag", X: 114f, Y: -2f, Pose: StudentPose.Wait, Heading: "Not ready yet"),
                (Index: 1, Key: "toys", X: 441f, Y: -5f, Pose: StudentPose.Wait, Heading: "Not safe yet"),
                (Index: 2, Key: "no_note", X: 91f, Y: -2f, Pose: StudentPose.Carry, Heading: "Not ready yet"),
            })
            {
                Assert.That(setting.ActOutFor(stop.Key), Is.Not.Null, stop.Key + " has an Unsafe Act-out");
                Tap(c, stop.Index);
                if (stop.Key == "toys")
                    yield return WaitFor(() => student.IsWalking && student.X > 363f, "the run toward the inner room");
                yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait,
                    "the stop at " + stop.Key);
                Assert.That(student.X, Is.EqualTo(stop.X));
                Assert.That(student.Y, Is.EqualTo(stop.Y));
                Assert.That(student.Pose, Is.EqualTo(stop.Pose));
                Assert.That(mom.AtStart, Is.True);
                Assert.That(note.gameObject.activeSelf, Is.False);
                if (stop.Key == "no_note")
                {
                    Assert.That(bag.transform.parent, Is.SameAs(student.Hands));
                    Assert.That(230f - student.Y, Is.GreaterThan(212f), "the Student stays inside the threshold");
                }
                else
                {
                    Assert.That(bag.AtStart, Is.True, "the Student's hands are empty");
                }
                if (stop.Key == "toys")
                {
                    var reaction = BoundsInSetting(setting, (RectTransform)student.bubble.transform);
                    Assert.That(reaction.xMax, Is.LessThanOrEqualTo(480f * 4f), "the Reaction stays inside the screen");
                    Assert.That(reaction.xMax, Is.LessThan(429f * 4f), "the Reaction leaves the inner door in view");
                    Assert.That(230f - student.Y, Is.GreaterThan(212f), "the Student stops before the inner room");
                }
                AssertHomeAfterReactionClear(setting, mom);

                yield return FeedbackShown(c);
                Assert.That(c.feedbackHeading.text, Is.EqualTo(stop.Heading));
                c.tryAgainButton.onClick.Invoke();
                yield return ActionBarShown(c);
                Assert.That(student.X, Is.EqualTo(286f));
                Assert.That(student.Y, Is.Zero);
                Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
                Assert.That(note.gameObject.activeSelf, Is.False);
                Assert.That(mom.AtStart, Is.True);
                Assert.That(bag.AtStart, Is.True);
                AssertPropAtArtPoint(setting, bag, 73f, 191f);
                Assert.That(c.actionBar.buttons[stop.Index].button.interactable, Is.False);
            }
        }

        static readonly LevelId HardSeven = new LevelId(Episode.Earthquake, Difficulty.Hard, 7);

        static SpriteAnimator Neighbours(LevelSetting setting) =>
            Array.Find(setting.GetComponentsInChildren<SpriteAnimator>(true), a => a.name == "Neighbours");

        [UnityTest]
        public IEnumerator HardLevel7_StartsWithMomOnTheMatAndTheRadioOff()
        {
            yield return StartRolloutLevel(HardSeven);
            var c = Level();
            var setting = c.ActiveSetting;
            Assert.That(setting.key, Is.EqualTo("evacuation_area_v2"));
            Assert.That(setting.shaker, Is.Null, "the field does not shake");
            Assert.That(setting.ActiveGrownUps, Has.Length.EqualTo(1), "Hard 7 has Mom on the mat");
            var mom = setting.ActiveGrownUps[0];
            Assert.That(mom.kind, Is.EqualTo(GrownUpKind.Mom));
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.Sit));
            Assert.That(mom.X, Is.EqualTo(118f));
            Assert.That(mom.Y, Is.EqualTo(-6f));
            Assert.That(c.Student.X, Is.EqualTo(228f));
            Assert.That(c.Student.Y, Is.Zero);
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
            AssertPropAtArtPoint(setting, PropIn(c, "radio"), 136f, 208f);
            Assert.That(PropIn(c, "radio_on").gameObject.activeSelf, Is.False, "the radio starts off");

            var neighbours = Neighbours(setting);
            Assert.That(neighbours, Is.Not.Null, "the neighbours chat in the background");
            var firstFrame = neighbours.GetComponent<Image>().sprite;
            yield return WaitFor(() => neighbours.GetComponent<Image>().sprite != firstFrame, "the neighbours to move");
        }

        [UnityTest]
        public IEnumerator HardLevel7_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(HardSeven);
            var c = Level();
            var setting = c.ActiveSetting;
            var student = c.Student;
            var mom = setting.ActiveGrownUps[0];
            var radio = PropIn(c, "radio");
            var radioOn = PropIn(c, "radio_on");
            yield return ActionBarShown(c);
            Assert.That(setting.ActOutFor("radio"), Is.Not.Null, "listening to the radio has an Act-out");

            Tap(c, 1);
            yield return WaitFor(() => student.Pose == StudentPose.Sit, "the Student to sit on the mat");
            Assert.That(student.X, Is.EqualTo(182f));
            Assert.That(student.Y, Is.EqualTo(-6f));
            yield return WaitFor(() => radioOn.gameObject.activeSelf, "the radio to turn on beside Mom");

            yield return FeedbackShown(c);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Sit));
            Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.Sit));
            Assert.That(mom.AtStart, Is.True);
            Assert.That(radio.gameObject.activeSelf, Is.False);
            Assert.That(radioOn.image.sprite.name, Does.StartWith("evacuation_area_radio_on"));
            var studentBounds = BoundsInSetting(setting, (RectTransform)student.transform);
            Assert.That(studentBounds.Overlaps(BoundsInSetting(setting, mom.body.rectTransform)), Is.False,
                "the Student sits beside Mom");
            var reaction = BoundsInSetting(setting, (RectTransform)student.bubble.transform);
            Assert.That(reaction.Overlaps(BoundsInSetting(setting, (RectTransform)radioOn.transform)), Is.False,
                "the Reaction leaves the radio in view");
            FinishWithThreeStars(c);
        }

        [UnityTest]
        public IEnumerator HardLevel7_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(HardSeven);
            var c = Level();
            var setting = c.ActiveSetting;
            var student = c.Student;
            var mom = setting.ActiveGrownUps[0];
            var radio = PropIn(c, "radio");
            var radioOn = PropIn(c, "radio_on");
            var neighbours = (RectTransform)Neighbours(setting).transform;
            const float pathStart = 395f;
            yield return ActionBarShown(c);

            foreach (var stop in new[]
            {
                (Index: 0, Key: "go_home", X: 376f, Y: -18f, Pose: StudentPose.Wait, Heading: "Not safe yet"),
                (Index: 2, Key: "rumor", X: 254f, Y: 2f, Pose: StudentPose.Call, Heading: UIStrings.UnsafeHeading),
                (Index: 3, Key: "phone_all_day", X: 228f, Y: 0f, Pose: StudentPose.Phone, Heading: "Not ready yet"),
            })
            {
                Assert.That(setting.ActOutFor(stop.Key), Is.Not.Null, stop.Key + " has an Unsafe Act-out");
                Tap(c, stop.Index);
                if (stop.Key == "rumor")
                {
                    yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ActOutDidYouHear,
                        "the Student to pass on the rumor");
                    Assert.That(student.Pose, Is.EqualTo(StudentPose.Call));
                    Assert.That(student.FacingLeft, Is.False, "the Student faces the neighbours");
                }
                yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait,
                    "the stop at " + stop.Key);
                Assert.That(student.X, Is.EqualTo(stop.X));
                Assert.That(student.Y, Is.EqualTo(stop.Y));
                Assert.That(student.Pose, Is.EqualTo(stop.Pose));
                Assert.That(mom.AtStart, Is.True);
                Assert.That(radioOn.gameObject.activeSelf, Is.False, "the radio stays off");
                var studentBounds = BoundsInSetting(setting, (RectTransform)student.transform);
                if (stop.Key == "go_home")
                    Assert.That(student.X + 16f, Is.LessThan(pathStart), "the Student stops on the field, before the path");
                if (stop.Key == "rumor")
                {
                    Assert.That(studentBounds.Overlaps(BoundsInSetting(setting, neighbours)), Is.False,
                        "the Student stands beside the neighbours");
                    Assert.That(student.FacingLeft, Is.False);
                }
                var reaction = BoundsInSetting(setting, (RectTransform)student.bubble.transform);
                Assert.That(reaction.Overlaps(BoundsInSetting(setting, neighbours)), Is.False,
                    "the Reaction at " + stop.Key + " leaves the neighbours in view");
                Assert.That(reaction.xMax, Is.LessThanOrEqualTo(480f * 4f), "the Reaction stays inside the screen");
                Assert.That(reaction.xMin, Is.GreaterThanOrEqualTo(0f), "the Reaction stays inside the screen");

                yield return FeedbackShown(c);
                Assert.That(c.feedbackHeading.text, Is.EqualTo(stop.Heading));
                c.tryAgainButton.onClick.Invoke();
                yield return ActionBarShown(c);
                Assert.That(student.X, Is.EqualTo(228f));
                Assert.That(student.Y, Is.Zero);
                Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
                Assert.That(radio.gameObject.activeSelf, Is.True);
                Assert.That(radioOn.gameObject.activeSelf, Is.False);
                Assert.That(mom.AtStart, Is.True);
                Assert.That(mom.Pose, Is.EqualTo(GrownUpPose.Sit));
                Assert.That(c.actionBar.buttons[stop.Index].button.interactable, Is.False);
            }
        }

        [UnityTest]
        public IEnumerator HardLevel2_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartRolloutLevel(HardTwo);
            var c = Level();
            yield return ActionBarShown(c);
            Assert.That(c.ActiveSetting.ActOutFor("route_field"), Is.Not.Null, "the field route has an Act-out");
            var student = c.Student;
            var teacher = c.ActiveSetting.ActiveGrownUps[0];

            Tap(c, 2);
            yield return WaitFor(() => student.IsWalking && teacher.IsWalking, "the Student and Teacher to walk together");
            Assert.That(student.FacingLeft, Is.True);
            Assert.That(teacher.FacingLeft, Is.True);
            yield return WaitFor(() => student.Y > 0f && teacher.Y > 5f && student.IsWalking && teacher.IsWalking,
                "the Student and Teacher to turn into the field path");

            yield return FeedbackShown(c);
            Assert.That(student.X, Is.EqualTo(133f));
            Assert.That(student.Y, Is.EqualTo(32f));
            Assert.That(teacher.X, Is.EqualTo(101f));
            Assert.That(teacher.Y, Is.EqualTo(47f));
            Assert.That(teacher.Pose, Is.EqualTo(GrownUpPose.Wave));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.False);
            FinishWithThreeStars(c);
        }

        [UnityTest]
        public IEnumerator HardLevel2_UnsafeChoice_StopsAndResets()
        {
            yield return StartRolloutLevel(HardTwo);
            var c = Level();
            yield return ActionBarShown(c);
            Assert.That(c.ActiveSetting.ActOutFor("route_post"), Is.Not.Null, "the post route has an Act-out");
            Assert.That(c.ActiveSetting.ActOutFor("route_later"), Is.Not.Null, "the later Choice has an Act-out");
            Assert.That(GameServices.Scenarios.TryGet(HardTwo, out var scenario), Is.True);
            var student = c.Student;
            var teacher = c.ActiveSetting.ActiveGrownUps[0];

            Tap(c, 0);
            yield return WaitFor(() => student.IsWalking && student.Y < 0f, "the Student to follow the foreground path");
            yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait, "the post route stop");
            // The post route stops short of the pole and its lines.
            Assert.That(student.X, Is.EqualTo(393f));
            Assert.That(student.Y, Is.EqualTo(-14f));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Wait));
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.UnsafeHeading));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            AssertSchoolyardReset(c, 306f, 0);
            Assert.That(teacher.AtStart, Is.True);
            Assert.That(teacher.Pose, Is.EqualTo(GrownUpPose.Wave));

            Tap(c, 3);
            yield return WaitFor(() => student.IsWalking && student.FacingLeft, "the return toward the school");
            yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait, "the door route stop");
            Assert.That(student.X, Is.EqualTo(260f), "the Student stops outside the doorway");
            Assert.That(student.Y, Is.Zero);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Wait));
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(scenario.choices[3].heading));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            AssertSchoolyardReset(c, 306f, 3);
            Assert.That(teacher.AtStart, Is.True);
            Assert.That(teacher.Pose, Is.EqualTo(GrownUpPose.Wave));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.False);
        }

        [UnityTest]
        public IEnumerator SchoolyardActOuts_AreComplete()
        {
            foreach (var level in new[] { NormalFour, HardTwo })
            {
                yield return StartRolloutLevel(level);
                var setting = Level().ActiveSetting;
                Assert.That(GameServices.Scenarios.TryGet(level, out var scenario), Is.True);
                foreach (var choice in scenario.choices)
                {
                    var act = setting.ActOutFor(choice.action);
                    Assert.That(act, Is.Not.Null, choice.id);
                    Assert.That(act.steps, Is.Not.Empty, choice.id);
                    if (!choice.safe)
                        Assert.That(act.stopAfter, Is.InRange(0, act.steps.Length - 1), choice.id);
                    foreach (var step in act.steps)
                        if (step.grownUp != null)
                            Assert.That(setting.ActiveGrownUps, Does.Contain(step.grownUp), choice.id);
                }
            }
        }

        [UnityTest]
        public IEnumerator SchoolyardReactions_LeaveTheSignAndTeacherVisible()
        {
            yield return StartRolloutLevel(NormalFour);
            var c = Level();
            yield return ActionBarShown(c);
            Tap(c, 1);
            yield return WaitFor(() => c.Student.bubble.Line == UIStrings.ReactionSafe, "the Normal 4 Safe Reaction");
            var sign = new Rect(15f * 4f, (270f - 176f) * 4f, 68f * 4f, 34f * 4f);
            var reaction = (RectTransform)c.Student.bubble.transform;
            Assert.That(BoundsInSetting(c.ActiveSetting, reaction).Overlaps(sign),
                Is.False, "the Normal 4 Reaction leaves the Evacuation Area sign visible");

            yield return StartRolloutLevel(HardTwo);
            c = Level();
            yield return ActionBarShown(c);
            Tap(c, 2);
            yield return WaitFor(() => c.Student.bubble.Line == UIStrings.ReactionReady, "the Hard 2 Safe Reaction");
            reaction = (RectTransform)c.Student.bubble.transform;
            var teacher = (RectTransform)c.ActiveSetting.ActiveGrownUps[0].transform;
            var bounds = BoundsInSetting(c.ActiveSetting, reaction);
            Assert.That(bounds.Overlaps(sign), Is.False, "the Hard 2 Reaction leaves the sign visible");
            Assert.That(bounds.Overlaps(BoundsInSetting(c.ActiveSetting, teacher)),
                Is.False, "the Hard 2 Reaction leaves the Teacher visible");
        }

        static Rect BoundsInSetting(LevelSetting setting, RectTransform element)
        {
            var group = (RectTransform)setting.transform;
            var corners = new Vector3[4];
            element.GetWorldCorners(corners);
            var min = (Vector2)group.InverseTransformPoint(corners[0]) - group.rect.min;
            var max = (Vector2)group.InverseTransformPoint(corners[2]) - group.rect.min;
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        static LevelProp PropIn(LevelController c, string key)
        {
            var prop = Array.Find(c.ActiveSetting.ActiveProps, p => p != null && p.key == key);
            Assert.That(prop, Is.Not.Null, "the Setting has a prop '" + key + "'");
            return prop;
        }


        // The world x of a prop's centre: its pivot is its bottom-left corner, which mirrors with the Student.
        static float CentreX(LevelProp prop)
        {
            var r = (RectTransform)prop.transform;
            return r.TransformPoint(r.rect.center).x;
        }

        [UnityTest]
        public IEnumerator NormalLevel1_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartNormalOne();
            var c = Level();
            var student = c.Student;
            Assert.That(c.ActiveSetting.key, Is.EqualTo("bedroom_v2"));
            Assert.That(c.ActiveSetting.shaker == null || !c.ActiveSetting.shaker.IsShaking, Is.True, "nothing shakes before an earthquake");
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(student.X, Is.EqualTo(306f));
            var box = PropIn(c, "box");
            var placed = PropIn(c, "box_placed");
            Assert.That(box.gameObject.activeSelf, Is.True);
            Assert.That(placed.gameObject.activeSelf, Is.False, "the shelf starts empty");
            yield return ReadyToChoose(Level);
            Assert.That(ButtonFor(c, "picture").icon.sprite.name, Is.EqualTo("icon_picture"));
            Assert.That(ButtonFor(c, "low_shelf").icon.sprite.name, Is.EqualTo("icon_low_shelf"));
            Assert.That(ButtonFor(c, "books_top").icon.sprite.name, Is.EqualTo("icon_books"));

            Tap(c, "low_shelf");
            Assert.That(c.actionBar.IsVisible, Is.False);
            yield return WaitFor(() => student.Pose == StudentPose.Carry && student.IsWalking, "the Student to carry the box");
            Assert.That(box.transform.IsChildOf(student.transform), Is.True, "the box is in the Student's hands");
            Assert.That(student.FacingLeft, Is.False);
            Assert.That(CentreX(box), Is.GreaterThan(student.transform.position.x), "the box is held in front");

            yield return WaitFor(() => student.bubble.Line == UIStrings.ReactionReady, "the Safe Reaction");
            Assert.That(student.X, Is.EqualTo(384f), "the Student stands beside the low shelf");
            Assert.That(placed.gameObject.activeSelf, Is.True, "the box shows on the low shelf");
            Assert.That(box.gameObject.activeSelf, Is.False);

            yield return FeedbackShown(c);
            Assert.That(c.finishButton.gameObject.activeSelf, Is.True);
            Assert.That(c.feedbackText.text, Is.EqualTo("Keep heavy things on low shelves, not high ones."));
            c.finishButton.onClick.Invoke();
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult.Stars, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator NormalLevel1_UnsafeChoice_StopsAndResets()
        {
            yield return StartNormalOne();
            var c = Level();
            var student = c.Student;
            var bed = PropIn(c, "bed");
            var props = new[] { bed, PropIn(c, "box"), PropIn(c, "picture"), PropIn(c, "books"), PropIn(c, "box_placed") };
            yield return ReadyToChoose(Level);

            Tap(c, "books_top");
            var books = PropIn(c, "books");
            yield return WaitFor(() => student.Pose == StudentPose.Carry && student.IsWalking && student.FacingLeft,
                "the Student to carry the books left to the cabinet");
            Assert.That(CentreX(books), Is.LessThan(student.transform.position.x), "the books are held in front");
            yield return WaitFor(() => student.Pose == StudentPose.ReachUp, "the Student to reach up at the cabinet");
            Assert.That(student.X, Is.EqualTo(272f));
            yield return FeedbackShown(c);
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);

            Tap(c, "picture");
            yield return WaitFor(() => student.Pose == StudentPose.ReachUp, "the Student to reach up over the bed");
            Assert.That(student.X, Is.EqualTo(140f));
            Assert.That(PropIn(c, "picture").AtStart, Is.False);
            yield return FeedbackShown(c);
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);

            foreach (var prop in props) Assert.That(prop.AtStart, Is.True, prop.key + " is back at its start");
            Assert.That(student.X, Is.EqualTo(306f));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing));
            Assert.That(ButtonFor(c, "low_shelf").button.interactable, Is.True);
        }

        // C7: the generic reach-up frames in Normal 1, for the boy and the girl. The picture goes up over the
        // bed facing right and the books go up at the cabinet facing left. In both 300 ms frames both palms
        // meet the bottom edge of the prop: no gap under it, and at most 1 art pixel of fingers over its edge.
        [UnityTest]
        public IEnumerator NormalLevel1_ReachUp_BothPalmsHoldTheProp([Values(StudentKind.Boy, StudentKind.Girl)] StudentKind kind)
        {
            yield return StartNormalOne(kind);
            var c = Level();
            var student = c.Student;
            Assert.That(student.Kind, Is.EqualTo(kind));
            var name = kind == StudentKind.Girl ? "girl" : "boy";
            var frames = student.Frames.For(StudentPose.ReachUp);
            Assert.That(Array.ConvertAll(frames, f => f.name), Is.EqualTo(new[] { name + "_reachup_0", name + "_reachup_1" }));
            var approved = kind == StudentKind.Girl ? ApprovedGirlHands : ApprovedBoyHands;
            for (var f = 0; f < frames.Length; f++)
                Assert.That(HandRegion(frames[f]), Is.EqualTo(approved[f]), "frame " + (f + 1) + "'s hands match the approved art");
            yield return ReadyToChoose(Level);

            yield return ReachUpHolding(c, "picture", "picture", 140f, facingLeft: false);
            yield return ResetAfterReach(c, "picture");
            yield return ReachUpHolding(c, "books_top", "books", 272f, facingLeft: true);
            yield return ResetAfterReach(c, "books");
        }

        static IEnumerator ReachUpHolding(LevelController c, string action, string propKey, float x, bool facingLeft)
        {
            var student = c.Student;
            var prop = PropIn(c, propKey);
            var frames = student.Frames.For(StudentPose.ReachUp);
            Tap(c, action);
            yield return WaitFor(() => student.Pose == StudentPose.ReachUp && !prop.transform.IsChildOf(student.transform),
                "the " + propKey + " held up");
            Assert.That(student.X, Is.EqualTo(x));
            Assert.That(student.FacingLeft, Is.EqualTo(facingLeft));

            // The stop plays the reach again from its first frame; the frames then swap every 300 ms, and the
            // Student keeps reaching until Try Again. Try Again hides the bubble, so a shown "Hmm, wait..." is
            // this stop and not the last one.
            yield return WaitFor(() => student.bubble.gameObject.activeSelf && student.bubble.Line == UIStrings.ReactionWait,
                "the stop");
            yield return WaitFor(() => student.body.sprite == frames[1], "the peak frame", 1f);
            var peakShown = Time.time;
            yield return WaitFor(() => student.body.sprite == frames[0], "the first frame again", 1f);
            var firstShown = Time.time;
            yield return WaitFor(() => student.body.sprite == frames[1], "the peak frame again", 1f);
            Assert.That(firstShown - peakShown, Is.EqualTo(0.3f).Within(0.08f), "the peak frame shows for 300 ms");
            Assert.That(Time.time - firstShown, Is.EqualTo(0.3f).Within(0.08f), "the first frame shows for 300 ms");

            // Reading the frames is slow, so the palms are checked after the timing.
            AssertPalmsOn(student, prop, "the " + propKey + " at the peak");
            yield return WaitFor(() => student.body.sprite == frames[0], "the first frame", 1f);
            AssertPalmsOn(student, prop, "the " + propKey + " on the first frame");
            yield return FeedbackShown(c);
        }

        static IEnumerator ResetAfterReach(LevelController c, string propKey)
        {
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Assert.That(PropIn(c, propKey).AtStart, Is.True, propKey + " is back at its start");
            Assert.That(c.Student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.Student.body.sprite.name, Does.Not.Contain("reachup"));
            Assert.That(c.Student.bubble.gameObject.activeSelf, Is.False, "the Reaction is hidden");
            Assert.That(c.Student.X, Is.EqualTo(306f));
        }

        // The Student's palette (PAL in ArtSource/scripts/student.lua) as the letters the art script uses.
        static readonly (char letter, Color32 colour)[] StudentPalette =
        {
            ('o', new Color32(40, 28, 44, 255)), ('j', new Color32(92, 70, 98, 255)), ('H', new Color32(52, 38, 60, 255)),
            ('h', new Color32(30, 22, 40, 255)), ('t', new Color32(246, 192, 150, 255)), ('S', new Color32(228, 156, 116, 255)),
            ('s', new Color32(186, 112, 86, 255)), ('W', new Color32(250, 252, 255, 255)), ('w', new Color32(220, 228, 240, 255)),
            ('v', new Color32(160, 176, 206, 255)), ('m', new Color32(72, 98, 160, 255)), ('N', new Color32(42, 58, 114, 255)),
            ('n', new Color32(26, 36, 76, 255)), ('b', new Color32(80, 70, 90, 255)), ('B', new Color32(46, 38, 54, 255)),
            ('x', new Color32(66, 44, 40, 255)), ('e', new Color32(30, 22, 38, 255)), ('k', new Color32(255, 255, 255, 255)),
            ('q', new Color32(128, 40, 52, 255)), ('r', new Color32(240, 130, 126, 255)), ('p', new Color32(214, 64, 64, 255)),
        };

        // A Student frame as palette letters, row 0 at the top. The import is not readable, so this reads the PNG.
        static string[] StudentRows(Sprite sprite)
        {
            var path = Path.Combine(Application.dataPath, "Art", "Student", sprite.name + ".png");
            var texture = new Texture2D(2, 2);
            Assert.That(texture.LoadImage(File.ReadAllBytes(path)), Is.True, "read " + path);
            var rows = new string[texture.height];
            for (var r = 0; r < texture.height; r++)
            {
                var line = new char[texture.width];
                for (var x = 0; x < texture.width; x++)
                {
                    Color32 p = texture.GetPixel(x, texture.height - 1 - r);
                    line[x] = p.a == 0 ? '.' : '?';
                    if (p.a == 0) continue;
                    foreach (var entry in StudentPalette)
                        if (entry.colour.r == p.r && entry.colour.g == p.g && entry.colour.b == p.b) line[x] = entry.letter;
                }
                rows[r] = new string(line);
            }
            UnityEngine.Object.Destroy(texture);
            return rows;
        }

        // The hands and forearms of a reach-up frame: rows 0-11, columns 20-31.
        static string[] HandRegion(Sprite sprite)
        {
            var rows = StudentRows(sprite);
            var region = new string[12];
            for (var r = 0; r < region.Length; r++) region[r] = rows[r].Substring(20, 12);
            return region;
        }

        // The hands of both reach-up frames the researcher approved for C7: two palms on separate wrists with
        // an outline between them, the far arm bent at the elbow and the near forearm clear of the face. The
        // fingertips are on row 4 in frame 1 and on row 3 in frame 2, the peak.
        static readonly string[][] ApprovedBoyHands =
        {
            new[]
            {
                "............", "............", "............", "o..ooo..ooo.", "hootSsooSsso", "HhotSsooSsso",
                "HHtSSsoSssso", "Hhotso.oSso.", "Hhotso.oSso.", "ssotso.oSso.", "esotso.oSsso", "esotso..oSso",
            },
            new[]
            {
                "............", "............", "...ooo..ooo.", "o.otSsooSsso", "hootSsooSsso", "HHtSSsoSssso",
                "Hhotso.oSso.", "Hhotso.oSso.", "Hhotso.oSso.", "ssotso.oSso.", "esotso.oSsso", "esotso..oSso",
            },
        };
        static readonly string[][] ApprovedGirlHands =
        {
            new[]
            {
                "............", "............", "............", "...ooo..ooo.", "..otSsooSsso", "..otSsooSsso",
                "ootSSsoSssso", "hootso.oSso.", "Hhotso.oSso.", "Hhotso.oSso.", "Hhotso.oSsso", "Hhotso..oSso",
            },
            new[]
            {
                "............", "............", "...ooo..ooo.", "..otSsooSsso", "..otSsooSsso", ".otSSsoSssso",
                "o.otso.oSso.", "hootso.oSso.", "Hhotso.oSso.", "Hhotso.oSso.", "Hhotso.oSsso", "Hhotso..oSso",
            },
        };

        static bool IsSkin(char c) => c == 't' || c == 'S' || c == 's';

        // Both palms (the separate skin shapes in the top 4 rows that hold skin) reach the bottom edge of the
        // prop inside its width: the top of each palm is 0 or 1 art pixel above the prop's bottom.
        static void AssertPalmsOn(StudentActor student, LevelProp prop, string what)
        {
            var rows = StudentRows(student.body.sprite);
            var fingertipRow = Array.FindIndex(rows, row => Array.Exists(row.ToCharArray(), IsSkin));
            Assert.That(fingertipRow, Is.GreaterThanOrEqualTo(0), what + ": " + student.body.sprite.name + " shows skin");
            var bandEnd = Math.Min(fingertipRow + 4, rows.Length);
            var seen = new bool[rows.Length, rows[0].Length];
            var palms = new System.Collections.Generic.List<(int top, int minX, int maxX)>();
            for (var r = fingertipRow; r < bandEnd; r++)
                for (var x = 0; x < rows[r].Length; x++)
                {
                    if (!IsSkin(rows[r][x]) || seen[r, x]) continue;
                    var palm = (top: r, minX: x, maxX: x);
                    var open = new System.Collections.Generic.Stack<(int r, int x)>();
                    open.Push((r, x));
                    seen[r, x] = true;
                    while (open.Count > 0)
                    {
                        var (pr, px) = open.Pop();
                        palm = (Math.Min(palm.top, pr), Math.Min(palm.minX, px), Math.Max(palm.maxX, px));
                        foreach (var (nr, nx) in new[] { (pr - 1, px), (pr + 1, px), (pr, px - 1), (pr, px + 1) })
                        {
                            if (nr < fingertipRow || nr >= bandEnd || nx < 0 || nx >= rows[nr].Length) continue;
                            if (!IsSkin(rows[nr][nx]) || seen[nr, nx]) continue;
                            seen[nr, nx] = true;
                            open.Push((nr, nx));
                        }
                    }
                    palms.Add(palm);
                }
            Assert.That(palms.Count, Is.EqualTo(2), what + ": two separate palms in " + student.body.sprite.name);

            // World corners: 0 bottom-left, 1 top-left, 2 top-right, 3 bottom-right. A Student facing left has
            // its body mirrored, so corner 0 is then on the right; lerping from corner 0 handles both.
            var bodyCorners = new Vector3[4];
            student.body.rectTransform.GetWorldCorners(bodyCorners);
            var artPixel = Mathf.Abs(bodyCorners[3].x - bodyCorners[0].x) / 32f;
            float ColumnEdgeX(int column) => Mathf.Lerp(bodyCorners[0].x, bodyCorners[3].x, column / 32f);
            float RowTopY(int row) => Mathf.Lerp(bodyCorners[0].y, bodyCorners[1].y, (50f - row) / 50f);
            var propCorners = new Vector3[4];
            ((RectTransform)prop.transform).GetWorldCorners(propCorners);
            foreach (var palm in palms)
            {
                var pixelsOver = (RowTopY(palm.top) - propCorners[0].y) / artPixel;
                Assert.That(pixelsOver, Is.InRange(-0.01f, 1.01f), what + ": art pixels of palm over the prop's bottom edge");
                var leftEdge = ColumnEdgeX(palm.minX);
                var rightEdge = ColumnEdgeX(palm.maxX + 1);
                Assert.That(Mathf.Min(leftEdge, rightEdge), Is.GreaterThanOrEqualTo(propCorners[0].x - 0.01f),
                    what + ": the palm is under the prop");
                Assert.That(Mathf.Max(leftEdge, rightEdge), Is.LessThanOrEqualTo(propCorners[2].x + 0.01f),
                    what + ": the palm is under the prop");
            }
        }

        [UnityTest]
        public IEnumerator EasyLevel1_NewFlow_Hint_ShowsInCaption()
        {
            yield return StartEasyOne();
            var c = Level();
            yield return ReadyToChoose(Level);

            c.hintButton.onClick.Invoke();
            Assert.That(c.Attempt.HintUsed, Is.True);
            Assert.That(c.captionBody.text, Is.EqualTo("Hint: Find something strong to hide under."));
            Assert.That(c.captionGoal.text, Is.EqualTo("Stay safe when the ground shakes."), "the Goal stays");
            Assert.That(c.Attempt.Stars, Is.EqualTo(2), "the Hint costs a Star");
        }

        [UnityTest]
        public IEnumerator Student_HasEveryRolloutPose_ForTheBoyAndTheGirl()
        {
            yield return StartEasyOne();
            var student = Level().Student;
            var poses = new[]
            {
                StudentPose.Carry, StudentPose.Reach, StudentPose.ReachUp, StudentPose.Push, StudentPose.Call,
                StudentPose.Cover, StudentPose.Pack, StudentPose.Phone, StudentPose.Sit,
            };
            foreach (var frames in new[] { student.boy, student.girl })
                foreach (var pose in poses)
                {
                    var sprites = frames.For(pose);
                    Assert.That(sprites, Is.Not.Null.And.Not.Empty, pose + " has frames");
                    Assert.That(sprites, Has.None.Null, pose + " has every frame");
                    Assert.That(sprites.Length, Is.EqualTo(pose == StudentPose.Carry ? 4 : 2), pose + " frame count");
                }
        }

        [UnityTest]
        public IEnumerator Reaction_PlaysTheVoiceBlip_OnlyWhenSoundIsOn()
        {
            // GameServices exists once the game has started.
            yield return StartEasyOne();
            var student = Level().Student;
            var soundWas = GameServices.Audio.SoundOn;
            try
            {
                GameServices.Audio.SoundOn = true;
                var before = AudioManager.BlipsPlayed;
                student.Say(UIStrings.ReactionWhoa);
                Assert.That(AudioManager.BlipsPlayed, Is.EqualTo(before + 1), "a Reaction plays one blip");

                GameServices.Audio.SoundOn = false;
                before = AudioManager.BlipsPlayed;
                student.Say(UIStrings.ReactionWait);
                Assert.That(AudioManager.BlipsPlayed, Is.EqualTo(before), "no blip when sound is off");
                Assert.That(student.bubble.Line, Is.EqualTo(UIStrings.ReactionWait), "the line still shows");
            }
            finally
            {
                GameServices.Audio.SoundOn = soundWas;
            }
        }

        [UnityTest]
        public IEnumerator Level_PlaysItsOwnMusic_AndTheMenuMusicComesBack()
        {
            var library = Resources.Load<AudioLibrary>(AudioLibrary.ResourcePath);
            Assert.That(library.levelMusic, Is.Not.Null, "bgm_level is in the Audio Library");
            Assert.That(library.levelMusic, Is.Not.SameAs(library.menuMusic));

            yield return StartEasyOne();
            yield return ReadyToChoose(Level);
            yield return WaitFor(() => AudioManager.CurrentMusic == library.levelMusic, "the Level music");

            GameScenes.ToMainMenu();
            yield return WaitForScene(GameScenes.MainMenu);
            yield return WaitFor(() => AudioManager.CurrentMusic == library.menuMusic, "the menu music");
        }

        [UnityTest]
        public IEnumerator EasyLevel1_NewFlow_Pause_FreezesTheStudent()
        {
            yield return StartEasyOne();
            var c = Level();
            yield return ReadyToChoose(Level);

            Tap(c, "door");
            yield return WaitFor(() => c.Student.IsWalking, "the Student to walk");
            yield return Wait(0.3f);
            c.pauseButton.onClick.Invoke();
            yield return Wait(0.4f);
            var x = c.Student.X;
            var pose = c.Student.Pose;
            var sprite = c.Student.CurrentSprite;
            yield return Wait(0.8f);
            Assert.That(c.Student.X, Is.EqualTo(x), "the Student stands still while paused");
            Assert.That(c.Student.Pose, Is.EqualTo(pose));
            Assert.That(c.Student.CurrentSprite, Is.SameAs(sprite), "the walk animation stops too");

            // Batch mode has no focused Game view to receive the simulated Android Back key.
#if UNITY_EDITOR
            var settings = InputSystem.settings;
            var previousFocus = settings.editorInputBehaviorInPlayMode;
            var previousBackground = settings.backgroundBehavior;
            var ranInBackground = Application.runInBackground;
            Application.runInBackground = true;
            settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
            var keyboard = InputSystem.AddDevice<Keyboard>();
            try
            {
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Escape));
                yield return WaitFor(() => !c.IsPaused, "Android Back to resume the Act-out");
                InputSystem.QueueStateEvent(keyboard, new KeyboardState());
                yield return WaitFor(() => c.Student.X != x, "the Student to continue the same walk");
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Escape));
                yield return WaitFor(() => c.IsPaused, "Android Back to open Pause again");
                c.resumeButton.onClick.Invoke();
                yield return FeedbackShown(c);
            }
            finally
            {
                InputSystem.RemoveDevice(keyboard);
#if UNITY_EDITOR
                settings.editorInputBehaviorInPlayMode = previousFocus;
                settings.backgroundBehavior = previousBackground;
                Application.runInBackground = ranInBackground;
#endif
            }
        }

        [UnityTest]
        public IEnumerator Replay_KeepsTheBestStars()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            SceneManager.LoadScene(GameScenes.Level);
            yield return WaitForScene(GameScenes.Level);
            yield return ReadyToChoose(Level);

            // First try, no Hint: 3 Stars.
            var c = Level();
            yield return ChooseAndWait(c, "desk");
            c.finishButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.LastResult.Stars, Is.EqualTo(3));

            // Replay with one Unsafe Choice: 2 Stars this time, but the save keeps 3.
            var before = c;
            c.replayButton.onClick.Invoke();
            yield return WaitFor(() => before == null, "the old Level to unload");
            yield return WaitForScene(GameScenes.Level);
            yield return ReadyToChoose(Level);
            c = Level();
            Assert.That(c.Attempt.UnsafeChoices, Is.EqualTo(0), "Replay starts a new Attempt");
            yield return ChooseAndWait(c, "door");
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            yield return ChooseAndWait(c, "desk");
            c.finishButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.LastResult.Stars, Is.EqualTo(2));
            Assert.That(c.LastResult.BestStars, Is.EqualTo(3));
            Assert.That(SavedOnDisk().StarsFor(First), Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator Pause_Hard4_ShowsCurrentLevelAndFourButtonsInTwoRows()
        {
            yield return StartRolloutLevel(new LevelId(Episode.Earthquake, Difficulty.Hard, 4));
            var c = Level();
            yield return ReadyToChoose(Level);
            c.pauseButton.onClick.Invoke();
            yield return Wait(0.4f);
            Canvas.ForceUpdateCanvases();

            var panel = c.pausePanel;
            Assert.That(panel.IsVisible, Is.True);
            var labels = Array.ConvertAll(panel.GetComponentsInChildren<Text>(), label => label.text);
            Assert.That(labels, Does.Contain("Earthquake - Hard 4"));
            var buttons = panel.GetComponentsInChildren<Button>();
            Assert.That(buttons.Length, Is.EqualTo(4));

            // Read the controls as a Learner would, top to bottom and left to right.
            Array.Sort(buttons, (left, right) =>
            {
                var a = left.transform.position;
                var b = right.transform.position;
                return a.y == b.y ? a.x.CompareTo(b.x) : b.y.CompareTo(a.y);
            });
            var names = new[] { "RESUME", "RESTART", "LEVELS", "MAIN MENU" };
            var icons = new[] { "icon_play", "icon_replay", "icon_levels", "icon_home" };
            var bounds = new Bounds[4];
            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var label = button.GetComponentInChildren<Text>();
                Assert.That(label.text, Is.EqualTo(names[i]));
                Assert.That(button.IsInteractable() && button.targetGraphic.raycastTarget, Is.True, names[i]);
                Assert.That(Array.Exists(button.GetComponentsInChildren<Image>(),
                    image => image.sprite != null && image.sprite.name == icons[i]), Is.True, names[i]);
                Assert.That(label.preferredWidth, Is.LessThanOrEqualTo(label.rectTransform.rect.width + 1f), names[i]);
                bounds[i] = RectTransformUtility.CalculateRelativeRectTransformBounds(panel.transform, button.transform);
            }
            Assert.That(bounds[0].center.y, Is.EqualTo(bounds[1].center.y).Within(1f));
            Assert.That(bounds[2].center.y, Is.EqualTo(bounds[3].center.y).Within(1f));
            Assert.That(bounds[0].min.y, Is.GreaterThan(bounds[2].max.y));
            Assert.That(bounds[0].max.x, Is.LessThan(bounds[1].min.x));
            Assert.That(bounds[2].max.x, Is.LessThan(bounds[3].min.x));

            buttons[2].onClick.Invoke();
            yield return WaitForScene(GameScenes.MainMenu);
            var menu = Menu();
            Assert.That(menu.levelPanel.IsVisible, Is.True);
            Assert.That(menu.levelHeader.text, Is.EqualTo("Earthquake - Hard"));
            Assert.That(Time.timeScale, Is.EqualTo(1f));
        }

        [UnityTest]
        public IEnumerator Pause_Hard4_ResumeRestartAndMainMenu_KeepSavedProgress()
        {
            // Saved Progress from an earlier session.
            var hard4 = new LevelId(Episode.Earthquake, Difficulty.Hard, 4);
            GameServices.Initialize(dir);
            GameServices.Progress.Open(hard4.Episode, hard4.Difficulty);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.RecordCompletion(hard4, 3);
            GameServices.SaveProgress();

            GameScenes.PlayLevel(hard4);
            yield return WaitForScene(GameScenes.Level);
            yield return ReadyToChoose(Level);
            var c = Level();

            // Pause freezes the scene; Resume continues the same Attempt.
            var attempt = c.Attempt;
            yield return ChooseAndWait(c, "elevator");
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            c.pauseButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.IsPaused, Is.True);
            Assert.That(Time.timeScale, Is.EqualTo(0f));
            Tap(c, "stairs");
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing), "Choices wait while paused");
            c.resumeButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.IsPaused, Is.False);
            Assert.That(Time.timeScale, Is.EqualTo(1f));
            Assert.That(c.Attempt, Is.SameAs(attempt));
            Assert.That(c.Attempt.UnsafeChoices, Is.EqualTo(1), "Resume keeps the Attempt");

            // Restart reloads the Level as a new Attempt and keeps saved progress.
            c.pauseButton.onClick.Invoke();
            yield return Wait(0.4f);
            var before = c;
            c.restartButton.onClick.Invoke();
            yield return WaitFor(() => before == null, "the old Level to unload");
            yield return WaitForScene(GameScenes.Level);
            yield return ReadyToChoose(Level);
            c = Level();
            Assert.That(c.Level, Is.EqualTo(hard4));
            Assert.That(c.Attempt, Is.Not.SameAs(attempt));
            Assert.That(c.Attempt.UnsafeChoices, Is.EqualTo(0));
            Assert.That(SavedOnDisk().StarsFor(hard4), Is.EqualTo(3));

            // Main Menu returns to the title screen with Continue available.
            c.pauseButton.onClick.Invoke();
            yield return Wait(0.4f);
            c.pauseMainMenuButton.onClick.Invoke();
            yield return WaitForScene(GameScenes.MainMenu);
            yield return Wait(0.4f);
            var m = Menu();
            Assert.That(m.mainPanel.IsVisible, Is.True);
            Assert.That(m.continueButton.interactable, Is.True);
            Assert.That(SavedOnDisk().StarsFor(hard4), Is.EqualTo(3));
        }
    }
}

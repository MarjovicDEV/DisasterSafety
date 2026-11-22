using System;
using System.Collections;
using System.IO;
using DisasterSafety.Core;
using DisasterSafety.Game;
using DisasterSafety.Game.UI;
using NUnit.Framework;
using UnityEngine;
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
        const int Run = 0, Desk = 1, Window = 2;

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
        const int Cabinet = 3;

        static void Tap(LevelController c, int choice) => c.actionBar.buttons[choice].button.onClick.Invoke();

        static IEnumerator FeedbackShown(LevelController c) =>
            WaitFor(() => c.feedbackPanel.IsVisible, "the Feedback panel", 12f);

        static IEnumerator ActionBarShown(LevelController c) =>
            WaitFor(() => c.actionBar.IsVisible && !c.Student.IsWalking, "the Action Bar", 12f);

        /// <summary>Taps a Choice of an Easy 1 Level and waits for its Feedback.</summary>
        static IEnumerator ChooseAndWait(LevelController c, int choice)
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
            Assert.That(c.UsesStudentFlow, Is.True, "Easy 1 is acted out by the Student");
            Assert.That(c.captionGoal.text, Is.EqualTo("Stay safe when the ground shakes."));
            Assert.That(c.actionBar.buttons[Desk].label.text, Is.EqualTo("Duck under the desk"));
            Assert.That(c.ActiveSetting.shaker.wobblers, Has.All.Matches<SpriteAnimator>(w => w.enabled), "fan and cabinet wobble while it shakes");

            // An Unsafe Choice shows kind Feedback and can be tried again; the button is marked tried.
            yield return ChooseAndWait(c, Run);
            Assert.That(c.feedbackPanel.IsVisible, Is.True);
            Assert.That(c.feedbackHeading.text, Is.EqualTo("Not safe yet"), "Run outside has its own heading");
            Assert.That(c.ActiveSetting.shaker.wobblers, Has.All.Matches<SpriteAnimator>(w => !w.enabled), "the shaking calms after a Choice");
            Assert.That(c.tryAgainButton.gameObject.activeSelf, Is.True);
            Assert.That(c.finishButton.gameObject.activeSelf, Is.False);
            Assert.That(c.actionBar.buttons[Run].button.interactable, Is.False, "a tried Unsafe Choice cannot be picked again");
            Assert.That(File.Exists(Path.Combine(dir, "save.json")), Is.True, "the name was saved");
            Assert.That(SavedOnDisk().HasSavedProgress, Is.False, "nothing is saved for an unfinished Attempt");
            c.tryAgainButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.feedbackPanel.IsVisible, Is.False);
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing));
            yield return ActionBarShown(c);

            // The Hint, then the Safe Choice.
            c.hintButton.onClick.Invoke();
            Assert.That(c.Attempt.HintUsed, Is.True);
            Assert.That(c.captionBody.text, Does.StartWith("Hint:"));
            yield return ChooseAndWait(c, Desk);
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
            yield return ChooseAndWait(c, Desk);
            c.finishButton.onClick.Invoke();
            Assert.That(c.nextButton.gameObject.activeSelf, Is.True, "Level 1 leads to Level 2");
            c.nextButton.onClick.Invoke();

            yield return WaitFor(() => Level()?.Level.Equals(second) == true, "Level 2 to load");
            yield return ReadyToChoose(Level);
            c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("corridor_v2"));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.True);

            // Easy 2 is acted out by the Student in the corridor.
            Assert.That(c.UsesStudentFlow, Is.True);
            const int Stairs = 0, Cover = 2;
            Tap(c, Stairs);
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo("Not safe yet"));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Tap(c, Cover);
            yield return FeedbackShown(c);
            c.finishButton.onClick.Invoke();
            Assert.That(c.nextButton.gameObject.activeSelf, Is.True, "Level 2 leads to Level 3");
            c.nextButton.onClick.Invoke();

            yield return WaitFor(() => Level()?.Level.Equals(third) == true && Level().ActiveSetting != null, "Level 3 to load");
            c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("classroom_after_v2"));
            yield return ReadyToChoose(Level);

            yield return ChooseAndWait(c, ClassroomWalk);
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
            yield return ChooseAndWait(c, ClassroomWalk);
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
            var settings = new[] { "bedroom_v2", "home", "home", "schoolyard", "beach" };
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
                Assert.That(c.ActiveSetting.key, Is.EqualTo(settings[n - 1]), "Setting of Normal " + n);
                var shaking = c.ActiveSetting.shaker != null && c.ActiveSetting.shaker.IsShaking;
                Assert.That(shaking, Is.EqualTo(shakes[n - 1]), "shaking in Normal " + n);

                Assert.That(GameServices.Scenarios.TryGet(level, out var scenario), Is.True);
                // Normal 1 is acted out by the Student in the bedroom; the other Normal Levels use the cards.
                Assert.That(c.UsesStudentFlow, Is.EqualTo(n == 1), "the Student flow in Normal " + n);
                if (c.UsesStudentFlow)
                {
                    Tap(c, scenario.SafeChoiceIndex);
                    yield return FeedbackShown(c);
                }
                else
                {
                    c.choiceCards[scenario.SafeChoiceIndex].button.onClick.Invoke();
                }
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
            yield return WaitFor(() => c.cheerBantay.bubble.IsVisible && c.cheerBantay.bubbleText.text == openedLine,
                "Bantay to announce that Hard is open", LevelController.OpenedLineDelay + 2f);
        }

        [UnityTest]
        public IEnumerator HardDifficulty_CompletingAllSevenLevels_CompletesTheEpisode()
        {
            var settings = new[] { "home", "schoolyard", "mall_a", "mall_b", "home_after", "home_after", "evacuation_area" };
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
                c.choiceCards[scenario.SafeChoiceIndex].button.onClick.Invoke();
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
            Assert.That(c.nextButton.gameObject.activeSelf, Is.False, "Hard 7 is the last Earthquake Level");
            var saved = SavedOnDisk();
            Assert.That(saved.IsCompleted(new LevelId(Episode.Earthquake, Difficulty.Hard, 7)), Is.True);
            Assert.That(saved.IsOpen(Episode.Fire, Difficulty.Easy), Is.True);

            const string openedLine = "Fire is open! New levels are coming soon.";
            yield return WaitFor(() => c.cheerBantay.bubble.IsVisible && c.cheerBantay.bubbleText.text == openedLine,
                "Bantay to announce that Fire is open", LevelController.OpenedLineDelay + 2f);
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
            Assert.That(c.UsesStudentFlow, Is.True);
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
            var shown = 0;
            foreach (var b in c.actionBar.buttons) if (b.gameObject.activeSelf) shown++;
            Assert.That(shown, Is.EqualTo(4));
            Assert.That(c.actionBar.buttons[Desk].label.text, Is.EqualTo("Duck under the desk"));
            Assert.That(c.actionBar.buttons[Desk].icon.sprite.name, Is.EqualTo("icon_desk"));

            // The desk: the Action Bar hides, the Student walks up beside the desk, crouches and
            // shuffles in under it, then holds the desk leg.
            Tap(c, Desk);
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
            Assert.That(c.feedbackPictureHolder.activeSelf, Is.False, "Feedback has no picture in this flow");

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

            Tap(c, Window);
            yield return WaitFor(() => student.Pose == StudentPose.Wait, "the Student to stop", 12f);
            var window = c.ActiveSetting.ActOutFor("window");
            Assert.That(student.X, Is.EqualTo(window.StopX(c.ActiveSetting.startX)).Within(0.01f));
            Assert.That(student.X, Is.LessThan(372f), "the Student stops short of the window (x 372)");
            Assert.That(student.bubble.Line, Is.EqualTo("Hmm, wait..."));

            yield return FeedbackShown(c);
            Assert.That(c.tryAgainButton.gameObject.activeSelf, Is.True);
            Assert.That(c.finishButton.gameObject.activeSelf, Is.False);
            Assert.That(c.feedbackText.text, Does.Contain("Window glass can break"));
            Assert.That(c.Attempt.UnsafeChoices, Is.EqualTo(1));

            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Assert.That(student.X, Is.EqualTo(c.ActiveSetting.startX));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing));
            Assert.That(c.actionBar.buttons[Window].button.interactable, Is.False, "the window is marked tried");
            Assert.That(c.actionBar.buttons[Desk].button.interactable, Is.True);
        }

        static readonly LevelId Second = new LevelId(Episode.Earthquake, Difficulty.Easy, 2);
        const int CorridorStairs = 0, CorridorWindow = 1, CorridorCover = 2, CorridorPlants = 3;

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
            Assert.That(c.UsesStudentFlow, Is.True);
            Assert.That(setting.key, Is.EqualTo("corridor_v2"));
            Assert.That(c.captionGoal.text, Is.EqualTo("Keep your head safe when the ground shakes."));
            Assert.That(setting.shaker.IsShaking, Is.True);
            Assert.That(student.X, Is.EqualTo(setting.startX));

            yield return ReadyToChoose(Level);
            Assert.That(c.actionBar.buttons[CorridorCover].label.text, Is.EqualTo("Duck and cover your head"));
            Assert.That(c.actionBar.buttons[CorridorStairs].icon.sprite.name, Is.EqualTo("icon_stairs"));
            Assert.That(c.actionBar.buttons[CorridorWindow].icon.sprite.name, Is.EqualTo("icon_window"));
            Assert.That(c.actionBar.buttons[CorridorCover].icon.sprite.name, Is.EqualTo("icon_cover"));
            Assert.That(c.actionBar.buttons[CorridorPlants].icon.sprite.name, Is.EqualTo("icon_plants"));
            // The long Goal and labels still fit: the Goal on one line, each label in two.
            Assert.That(PixelText.Lines(c.captionGoal.font, c.captionGoal.text, c.captionGoal.rectTransform.sizeDelta.x), Is.EqualTo(1));
            foreach (var b in c.actionBar.buttons)
                Assert.That(PixelText.Lines(b.label.font, b.label.text, c.actionBar.labelWidth), Is.LessThanOrEqualTo(2), b.label.text);

            // Walk to the inner wall, crouch, then cover the head and hold until the shaking calms.
            Tap(c, CorridorCover);
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

            // Run-start toward the stairs, stop at the top step, then "Hmm, wait...".
            Tap(c, CorridorStairs);
            yield return WaitFor(() => student.IsWalking, "the Student to run", 4f);
            yield return WaitFor(() => student.Pose == StudentPose.Wait, "the Student to stop", 12f);
            Assert.That(student.X, Is.EqualTo(setting.ActOutFor("stairs").StopX(setting.startX)).Within(0.01f));
            Assert.That(student.X + 16f, Is.LessThanOrEqualTo(410f), "the Student stops on the top step (x 404), not down the stairs");
            Assert.That(student.bubble.Line, Is.EqualTo("Hmm, wait..."));

            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo("Not safe yet"));
            Assert.That(c.tryAgainButton.gameObject.activeSelf, Is.True);
            Assert.That(c.Attempt.UnsafeChoices, Is.EqualTo(1));

            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Assert.That(student.X, Is.EqualTo(setting.startX));
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing));
            Assert.That(c.actionBar.buttons[CorridorStairs].button.interactable, Is.False, "the stairs are marked tried");
            Assert.That(c.actionBar.buttons[CorridorCover].button.interactable, Is.True);

            // The other Unsafe Choices stop short of their danger: the window (sill ends at x 180)
            // and the spot under the pots (the right pot ends at x 63).
            var window = setting.ActOutFor("window").StopX(setting.startX);
            Assert.That(window, Is.LessThan(setting.startX));
            Assert.That(window - 16f, Is.GreaterThanOrEqualTo(170f));
            var plants = setting.ActOutFor("plants").StopX(setting.startX);
            Assert.That(plants - 16f, Is.GreaterThan(63f));
        }

        static readonly LevelId Third = new LevelId(Episode.Earthquake, Difficulty.Easy, 3);
        const int ClassroomStay = 0, ClassroomWalk = 1, ClassroomPush = 2;

        IEnumerator StartEasyThree()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.SaveProgress();
            GameScenes.PlayLevel(Third);
            yield return WaitFor(() => Level()?.Level.Equals(Third) == true && Level().ActiveSetting != null,
                "Easy 3 to start");
            Assert.That(Level().UsesStudentFlow, Is.True, "Easy 3 shows the Action Bar");
        }

        [UnityTest]
        public IEnumerator EasyLevel3_SafeChoice_ActsOutAndCompletes()
        {
            yield return StartEasyThree();
            var c = Level();
            var student = c.Student;
            Assert.That(c.ActiveSetting.key, Is.EqualTo("classroom_after_v2"));
            Assert.That(c.bantay.gameObject.activeInHierarchy, Is.False);
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
            Assert.That(c.actionBar.buttons[ClassroomStay].icon.sprite.name, Is.EqualTo("icon_stay"));
            Assert.That(c.actionBar.buttons[ClassroomWalk].icon.sprite.name, Is.EqualTo("icon_teacher_walk"));
            Assert.That(c.actionBar.buttons[ClassroomPush].icon.sprite.name, Is.EqualTo("icon_push"));

            Tap(c, ClassroomWalk);
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

            Tap(c, ClassroomStay);
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
            Assert.That(c.actionBar.buttons[ClassroomStay].button.interactable, Is.False);

            Tap(c, ClassroomPush);
            yield return WaitFor(() => student.Pose == StudentPose.Crouch && student.IsWalking,
                "the Student to shuffle out from under the desk");
            yield return WaitFor(() => student.IsWalking && student.Pose == StudentPose.Walk,
                "the Student to start running toward the door");
            yield return WaitFor(() => student.Pose == StudentPose.Wait, "the Student to stop behind the Teacher");
            Assert.That(student.X, Is.EqualTo(110f));
            Assert.That(student.X - 16f, Is.GreaterThanOrEqualTo(teacher.X + 18f - 1f),
                "the Student stops with room between the two figures");
            Assert.That(teacher.AtStart, Is.True, "the Student does not push or move the Teacher");
            Assert.That(student.bubble.Line, Is.EqualTo("Hmm, wait..."));

            yield return FeedbackShown(c);
            Assert.That(c.tryAgainButton.gameObject.activeSelf, Is.True);
            c.tryAgainButton.onClick.Invoke();
            yield return WaitFor(() => student.Pose == StudentPose.Crouch && student.IsWalking,
                "the Student to crouch before shuffling back under the desk");
            yield return ActionBarShown(c);
            Assert.That(student.X, Is.EqualTo(222f));
            Assert.That(student.Y, Is.Zero);
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Hold));
            Assert.That(teacher.AtStart, Is.True);
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing));
            Assert.That(c.actionBar.buttons[ClassroomStay].button.interactable, Is.False);
            Assert.That(c.actionBar.buttons[ClassroomPush].button.interactable, Is.False);
            Assert.That(c.actionBar.buttons[ClassroomWalk].button.interactable, Is.True);
        }

        static readonly LevelId NormalOne = new LevelId(Episode.Earthquake, Difficulty.Normal, 1);
        const int BedroomPicture = 0, BedroomLowShelf = 1, BedroomBedWindow = 2, BedroomBooksTop = 3;

        IEnumerator StartNormalOne()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Normal);
            GameServices.SaveProgress();
            GameScenes.PlayLevel(NormalOne);
            yield return WaitFor(() => Level()?.Level.Equals(NormalOne) == true && Level().ActiveSetting != null,
                "Normal 1 to start");
            Assert.That(Level().UsesStudentFlow, Is.True, "Normal 1 shows the Action Bar");
        }

        static LevelProp PropIn(LevelController c, string key)
        {
            var prop = Array.Find(c.ActiveSetting.props, p => p != null && p.key == key);
            Assert.That(prop, Is.Not.Null, "the bedroom has a prop '" + key + "'");
            return prop;
        }

        static float AnchoredX(LevelProp prop) => ((RectTransform)prop.transform).anchoredPosition.x;

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
            Assert.That(c.bantay.gameObject.activeInHierarchy, Is.False);
            Assert.That(c.ActiveSetting.shaker == null || !c.ActiveSetting.shaker.IsShaking, Is.True, "nothing shakes before an earthquake");
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Idle));
            Assert.That(student.X, Is.EqualTo(306f));
            var box = PropIn(c, "box");
            var placed = PropIn(c, "box_placed");
            Assert.That(box.gameObject.activeSelf, Is.True);
            Assert.That(placed.gameObject.activeSelf, Is.False, "the shelf starts empty");
            yield return ReadyToChoose(Level);
            Assert.That(c.actionBar.buttons[BedroomPicture].icon.sprite.name, Is.EqualTo("icon_picture"));
            Assert.That(c.actionBar.buttons[BedroomLowShelf].icon.sprite.name, Is.EqualTo("icon_low_shelf"));
            Assert.That(c.actionBar.buttons[BedroomBedWindow].icon.sprite.name, Is.EqualTo("icon_bed"));
            Assert.That(c.actionBar.buttons[BedroomBooksTop].icon.sprite.name, Is.EqualTo("icon_books"));

            Tap(c, BedroomLowShelf);
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
            var bedStartX = AnchoredX(bed);
            yield return ReadyToChoose(Level);

            Tap(c, BedroomBedWindow);
            yield return WaitFor(() => student.Pose == StudentPose.Push, "the Student to push the bed");
            Assert.That(student.X, Is.EqualTo(229f));
            Assert.That(student.FacingLeft, Is.True, "the Student pushes the bed toward the window");
            yield return WaitFor(() => student.bubble.Line == UIStrings.ReactionWait, "the stop");
            Assert.That(AnchoredX(bed), Is.EqualTo(bedStartX - 8f * student.unitsPerPixel).Within(0.01f),
                "the bed slid 8 art pixels toward the window");
            Assert.That(student.X, Is.EqualTo(221f), "the Student steps with the bed, palms on the headboard");
            Assert.That(student.Pose, Is.EqualTo(StudentPose.Push));
            yield return FeedbackShown(c);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.UnsafeHeading));
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            Assert.That(AnchoredX(bed), Is.EqualTo(bedStartX), "Try Again slides the bed back");
            Assert.That(student.X, Is.EqualTo(306f));
            Assert.That(c.actionBar.buttons[BedroomBedWindow].button.interactable, Is.False);

            Tap(c, BedroomBooksTop);
            var books = PropIn(c, "books");
            yield return WaitFor(() => student.Pose == StudentPose.Carry && student.IsWalking && student.FacingLeft,
                "the Student to carry the books left to the cabinet");
            Assert.That(CentreX(books), Is.LessThan(student.transform.position.x), "the books are held in front");
            yield return WaitFor(() => student.Pose == StudentPose.ReachUp, "the Student to reach up at the cabinet");
            Assert.That(student.X, Is.EqualTo(272f));
            yield return FeedbackShown(c);
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);

            Tap(c, BedroomPicture);
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
            Assert.That(c.actionBar.buttons[BedroomLowShelf].button.interactable, Is.True);
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

            Tap(c, Cabinet);
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

            c.resumeButton.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.Student.X, Is.Not.EqualTo(x), "the Student walks on after Resume");
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
            yield return ChooseAndWait(c, Desk);
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
            yield return ChooseAndWait(c, Window);
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            yield return ChooseAndWait(c, Desk);
            c.finishButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.LastResult.Stars, Is.EqualTo(2));
            Assert.That(c.LastResult.BestStars, Is.EqualTo(3));
            Assert.That(SavedOnDisk().StarsFor(First), Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator Pause_ResumeRestartAndQuit_KeepSavedProgress()
        {
            // Saved Progress from an earlier session.
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.RecordCompletion(First, 3);
            GameServices.SaveProgress();

            SceneManager.LoadScene(GameScenes.Level);
            yield return WaitForScene(GameScenes.Level);
            yield return ReadyToChoose(Level);
            var c = Level();

            // Pause freezes the scene; Resume continues the same Attempt.
            yield return ChooseAndWait(c, Run);
            c.tryAgainButton.onClick.Invoke();
            yield return ActionBarShown(c);
            c.pauseButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.IsPaused, Is.True);
            Assert.That(Time.timeScale, Is.EqualTo(0f));
            Tap(c, Desk);
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing), "Choices wait while paused");
            c.resumeButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.IsPaused, Is.False);
            Assert.That(Time.timeScale, Is.EqualTo(1f));
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
            Assert.That(c.Attempt.UnsafeChoices, Is.EqualTo(0));
            Assert.That(SavedOnDisk().StarsFor(First), Is.EqualTo(3));

            // Quit to Menu returns through the Continue check: Continue is on.
            c.pauseButton.onClick.Invoke();
            yield return Wait(0.4f);
            c.quitButton.onClick.Invoke();
            yield return WaitForScene(GameScenes.MainMenu);
            yield return Wait(0.4f);
            var m = Menu();
            Assert.That(m.mainPanel.IsVisible, Is.True);
            Assert.That(m.continueButton.interactable, Is.True);
            Assert.That(SavedOnDisk().StarsFor(First), Is.EqualTo(3));
        }
    }
}

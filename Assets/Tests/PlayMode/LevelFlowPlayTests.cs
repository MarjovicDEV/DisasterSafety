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
            Assert.That(c.goalText.text, Is.EqualTo("Stay safe when the ground shakes."));
            Assert.That(c.choiceCards[Desk].label.text, Is.EqualTo("Duck under the desk"));
            Assert.That(c.ActiveSetting.shaker.wobblers, Has.All.Matches<SpriteAnimator>(w => w.enabled), "fan and cabinet wobble while it shakes");

            // An Unsafe Choice shows kind Feedback and can be tried again; the card is marked tried.
            c.choiceCards[Run].button.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.feedbackPanel.IsVisible, Is.True);
            Assert.That(c.feedbackHeading.text, Is.EqualTo("Not safe yet"), "Run outside has its own heading");
            Assert.That(c.ActiveSetting.shaker.wobblers, Has.All.Matches<SpriteAnimator>(w => !w.enabled), "the shaking calms after a Choice");
            Assert.That(c.tryAgainButton.gameObject.activeSelf, Is.True);
            Assert.That(c.finishButton.gameObject.activeSelf, Is.False);
            Assert.That(c.choiceCards[Run].button.interactable, Is.False, "a tried Unsafe Choice cannot be picked again");
            Assert.That(File.Exists(Path.Combine(dir, "save.json")), Is.True, "the name was saved");
            Assert.That(SavedOnDisk().HasSavedProgress, Is.False, "nothing is saved for an unfinished Attempt");
            c.tryAgainButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.feedbackPanel.IsVisible, Is.False);
            Assert.That(c.Attempt.Phase, Is.EqualTo(AttemptPhase.Choosing));

            // The Hint, then the Safe Choice.
            c.hintButton.onClick.Invoke();
            Assert.That(c.Attempt.HintUsed, Is.True);
            Assert.That(c.bantay.bubbleText.text, Does.StartWith("Hint:"));
            c.choiceCards[Desk].button.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.feedbackHeading.text, Is.EqualTo(UIStrings.SafeHeading));
            Assert.That(c.finishButton.gameObject.activeSelf, Is.True);
            c.finishButton.onClick.Invoke();
            yield return Wait(0.5f);

            // Level Completed: 3 - 1 Unsafe Choice - 1 Hint = 1 Star, already on disk.
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult, Is.Not.Null);
            Assert.That(c.LastResult.Stars, Is.EqualTo(1));
            Assert.That(c.cheerBantay.bubbleText.text, Does.Contain("Ana"));
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
            var third = new LevelId(Episode.Earthquake, Difficulty.Easy, 3);

            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.SaveProgress();
            GameScenes.PlayLevel(First);
            yield return ReadyToChoose(Level);

            var c = Level();
            Assert.That(c.Level, Is.EqualTo(First));
            c.choiceCards[Desk].button.onClick.Invoke();
            c.finishButton.onClick.Invoke();
            Assert.That(c.nextButton.gameObject.activeSelf, Is.True, "Level 1 leads to Level 2");
            c.nextButton.onClick.Invoke();

            yield return WaitFor(() => Level()?.Level.Equals(second) == true, "Level 2 to load");
            yield return ReadyToChoose(Level);
            c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("corridor"));
            Assert.That(c.ActiveSetting.shaker.IsShaking, Is.True);

            const int Stairs = 0, Cover = 2;
            c.choiceCards[Stairs].button.onClick.Invoke();
            Assert.That(c.feedbackHeading.text, Is.EqualTo("Not safe yet"));
            c.tryAgainButton.onClick.Invoke();
            yield return ReadyToChoose(Level);
            c.choiceCards[Cover].button.onClick.Invoke();
            c.finishButton.onClick.Invoke();
            Assert.That(c.nextButton.gameObject.activeSelf, Is.True, "Level 2 leads to Level 3");
            c.nextButton.onClick.Invoke();

            yield return WaitFor(() => Level()?.Level.Equals(third) == true && Level().ActiveSetting != null, "Level 3 to load");
            c = Level();
            Assert.That(c.ActiveSetting.key, Is.EqualTo("classroom_after"));
            Assert.That(c.ActiveSetting.shaker == null || !c.ActiveSetting.shaker.IsShaking, Is.True);
            Assert.That(c.bantay.bubbleText.text, Is.EqualTo("The shaking stopped. Your teacher is at the door."));

            var teacher = c.ActiveSetting.GetComponentInChildren<SpriteAnimator>();
            Assert.That(teacher, Is.Not.Null, "the teacher waves in the still classroom");
            var teacherImage = teacher.GetComponent<Image>();
            var firstFrame = teacherImage.sprite;
            yield return WaitFor(() => teacherImage.sprite != firstFrame, "the teacher to wave");
            yield return ReadyToChoose(Level);

            const int Walk = 1;
            c.choiceCards[Walk].button.onClick.Invoke();
            c.finishButton.onClick.Invoke();
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult.Opened, Does.Contain("Earthquake/Normal"));
            Assert.That(c.openedRibbon.activeInHierarchy, Is.True);
            Assert.That(c.openedText.text, Is.EqualTo("Normal is open!"));
            Assert.That(c.nextButton.gameObject.activeSelf, Is.False);
            var saved = SavedOnDisk();
            Assert.That(saved.IsCompleted(third), Is.True);
            Assert.That(saved.IsOpen(Episode.Earthquake, Difficulty.Normal), Is.True);

            Assert.That(c.cheerBantay.bubbleText.text, Is.EqualTo("You did it, Ana!"));
            const string openedLine = "Normal is open! Let's play!";
            yield return WaitFor(() => c.cheerBantay.bubble.IsVisible && c.cheerBantay.bubbleText.text == openedLine,
                "Bantay to announce that Normal is open", 5f);

            c.cheerBantay.OnPointerClick(null);
            Assert.That(c.cheerBantay.bubbleText.text, Is.EqualTo("Woof woof! Great job, Ana!"));

            var completed = c;
            c.replayButton.onClick.Invoke();
            yield return WaitFor(() => completed == null, "the old Level to unload");
            yield return ReadyToChoose(Level);
            c = Level();
            Assert.That(c.Level, Is.EqualTo(third));
            c.choiceCards[Walk].button.onClick.Invoke();
            c.finishButton.onClick.Invoke();
            Assert.That(c.completedPanel.IsVisible, Is.True);
            Assert.That(c.LastResult.Opened, Is.Empty);
            Assert.That(c.openedRibbon.activeSelf, Is.False);
            yield return Wait(LevelController.OpenedLineDelay + 0.2f);
            Assert.That(c.cheerBantay.bubbleText.text, Is.EqualTo("You did it, Ana!"));
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
            c.choiceCards[Desk].button.onClick.Invoke();
            yield return Wait(0.4f);
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
            c.choiceCards[Window].button.onClick.Invoke();
            yield return Wait(0.4f);
            c.tryAgainButton.onClick.Invoke();
            yield return Wait(0.4f);
            c.choiceCards[Desk].button.onClick.Invoke();
            yield return Wait(0.4f);
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
            c.choiceCards[Run].button.onClick.Invoke();
            yield return Wait(0.4f);
            c.tryAgainButton.onClick.Invoke();
            yield return Wait(0.4f);
            c.pauseButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.IsPaused, Is.True);
            Assert.That(Time.timeScale, Is.EqualTo(0f));
            c.choiceCards[Desk].button.onClick.Invoke();
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

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

namespace DisasterSafety.Tests
{
    /// <summary>
    /// Drives the real Boot and MainMenu scenes through the Figure 1 and 2 routes.
    /// Saves go to a temp folder, never the Learner's real persistentDataPath.
    /// </summary>
    public class MenuFlowPlayTests
    {
        string dir;

        [SetUp]
        public void SetUp()
        {
            dir = Path.Combine(Path.GetTempPath(), "ds-play-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            GameServices.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }

        static IEnumerator Wait(float seconds)
        {
            for (var t = 0f; t < seconds; t += Time.unscaledDeltaTime) yield return null;
        }

        IEnumerator BootToMenu()
        {
            GameServices.Initialize(dir);
            SceneManager.LoadScene("Boot");
            for (var i = 0; i < 300 && SceneManager.GetActiveScene().name != "MainMenu"; i++) yield return null;
            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("MainMenu"));
            yield return Wait(0.5f);
        }

        [UnityTest]
        public IEnumerator FreshInstall_WalksEveryRouteAndBack()
        {
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            Assert.That(c, Is.Not.Null);
            Assert.That(c.mainPanel.IsVisible, Is.True);
            Assert.That(c.continueButton.interactable, Is.False, "Continue is disabled on a fresh install");

            // Settings and How to Play open over the menu and Back closes them.
            c.settingsButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.settingsPanel.IsVisible, Is.True);
            var musicBefore = GameServices.Audio.MusicOn;
            c.musicSwitch.button.onClick.Invoke();
            Assert.That(GameServices.Audio.MusicOn, Is.EqualTo(!musicBefore));
            c.musicSwitch.button.onClick.Invoke(); // restore the device setting
            c.settingsBack.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.settingsPanel.IsVisible, Is.False);

            c.howToPlayButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.howToPlayPanel.IsVisible, Is.True);

            // The picture tutorial pages forward with Next, back with Back, and ends on DONE.
            var t = c.tutorial;
            Assert.That(t.PageIndex, Is.EqualTo(0));
            Assert.That(t.previous.interactable, Is.False);
            Assert.That(t.title.text, Does.StartWith("Step 1"));
            t.next.onClick.Invoke();
            Assert.That(t.PageIndex, Is.EqualTo(1));
            Assert.That(t.Back(), Is.True);
            Assert.That(t.Back(), Is.False, "Back on page 1 leaves How to Play");
            for (var i = 0; i < UIStrings.TutorialPages.Length; i++) t.next.onClick.Invoke();
            Assert.That(t.PageIndex, Is.EqualTo(UIStrings.TutorialPages.Length - 1));
            Assert.That(t.next.interactable, Is.False);
            Assert.That(t.closeLabel.text, Is.EqualTo(UIStrings.Done));
            Assert.That(t.picture.sprite, Is.Not.Null);
            c.howToPlayBack.onClick.Invoke();
            yield return Wait(0.4f);

            // Reopening starts again at page 1.
            c.howToPlayButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(t.PageIndex, Is.EqualTo(0));
            c.howToPlayBack.onClick.Invoke();
            yield return Wait(0.4f);

            // Exit asks first; No returns to the menu.
            c.exitButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.exitPanel.IsVisible, Is.True);
            c.exitNo.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.exitPanel.IsVisible, Is.False);
            Assert.That(c.mainPanel.IsVisible, Is.True);

            // Play without a name asks for one; bad names are refused.
            c.playButton.onClick.Invoke();
            yield return Wait(0.4f);
            Assert.That(c.namePanel.IsVisible, Is.True);
            c.nameInput.text = "   ";
            c.nameOk.onClick.Invoke();
            Assert.That(c.namePanel.IsVisible, Is.True);
            Assert.That(c.nameError.text, Is.Not.Empty);
            c.nameInput.text = "  Niño ";
            c.nameOk.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(GameServices.Progress.DisplayName, Is.EqualTo("Niño"));
            Assert.That(File.Exists(Path.Combine(dir, "save.json")), Is.True);

            // A new Display Name leads to the Student picker; picking the girl saves Girl.
            Assert.That(c.studentPanel.IsVisible, Is.True);
            Assert.That(c.episodePanel.IsVisible, Is.False);
            Assert.That(GameServices.Progress.Student, Is.EqualTo(StudentKind.None));
            c.studentGirl.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(GameServices.Progress.Student, Is.EqualTo(StudentKind.Girl));
            Assert.That(new SaveService(dir).Load().Progress.Student, Is.EqualTo(StudentKind.Girl));
            Assert.That(c.studentPanel.IsVisible, Is.False);
            Assert.That(c.episodePanel.IsVisible, Is.True);

            // Locked Episode is refused; Earthquake opens Difficulty Selection.
            c.episodeCards[1].button.onClick.Invoke();
            yield return null;
            Assert.That(c.episodePanel.IsVisible, Is.True);
            c.episodeCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.difficultyPanel.IsVisible, Is.True);

            c.difficultyCards[2].button.onClick.Invoke();
            yield return null;
            Assert.That(c.difficultyPanel.IsVisible, Is.True, "Hard is locked");
            c.difficultyCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.levelPanel.IsVisible, Is.True);
            Assert.That(c.levelSlots[2].gameObject.activeSelf, Is.True);
            Assert.That(c.levelSlots[3].gameObject.activeSelf, Is.False, "Easy has 3 Levels");

            // Each Easy Level has a valid Scenario.
            for (var i = 0; i < 3; i++)
            {
                c.levelSlots[i].button.onClick.Invoke();
                yield return null;
                Assert.That(c.startButton.interactable, Is.True, $"Earthquake/Easy/{i + 1} is a Playable Level");
            }

            // Back walks Level -> Difficulty -> Episode -> Main, and Continue stays off for a name-only save.
            c.levelBack.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.difficultyPanel.IsVisible, Is.True);
            c.difficultyBack.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.episodePanel.IsVisible, Is.True);
            c.episodeBack.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.mainPanel.IsVisible, Is.True);
            Assert.That(c.continueButton.interactable, Is.False, "a name alone is not Saved Progress");

            // Play again skips name entry.
            c.playButton.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.namePanel.IsVisible, Is.False);
            Assert.That(c.episodePanel.IsVisible, Is.True);
        }

        [UnityTest]
        public IEnumerator NormalOpen_LevelSelection_StartsEveryNormalLevel()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.SetStudent(StudentKind.Boy);
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Normal);
            GameServices.SaveProgress();
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();

            c.playButton.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.episodePanel.IsVisible, Is.True);
            c.episodeCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.difficultyPanel.IsVisible, Is.True);
            c.difficultyCards[1].button.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.levelPanel.IsVisible, Is.True, "Normal is open");

            Assert.That(c.levelSlots.Length, Is.GreaterThanOrEqualTo(5));
            for (var i = 0; i < c.levelSlots.Length; i++)
                Assert.That(c.levelSlots[i].gameObject.activeSelf, Is.EqualTo(i < 5), "Normal has 5 Levels");

            for (var i = 0; i < 5; i++)
            {
                c.levelSlots[i].button.onClick.Invoke();
                yield return null;
                Assert.That(c.levelSlots[i].selectedFrame.activeSelf, Is.True, $"Normal {i + 1} is selected");
                Assert.That(c.startButton.interactable, Is.True, $"Earthquake/Normal/{i + 1} is a Playable Level");
            }
        }

        [UnityTest]
        public IEnumerator HardOpen_LevelSelection_StartsEveryHardLevel()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.SetStudent(StudentKind.Boy);
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Hard);
            GameServices.SaveProgress();
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();

            c.playButton.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.episodePanel.IsVisible, Is.True);
            c.episodeCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.difficultyPanel.IsVisible, Is.True);
            c.difficultyCards[2].button.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.levelPanel.IsVisible, Is.True, "Hard is open");

            Assert.That(c.levelSlots.Length, Is.GreaterThanOrEqualTo(7));
            for (var i = 0; i < c.levelSlots.Length; i++)
                Assert.That(c.levelSlots[i].gameObject.activeSelf, Is.EqualTo(i < 7), "Hard has 7 Levels");

            for (var i = 0; i < 7; i++)
            {
                c.levelSlots[i].button.onClick.Invoke();
                yield return null;
                Assert.That(c.levelSlots[i].selectedFrame.activeSelf, Is.True, $"Hard {i + 1} is selected");
                Assert.That(c.startButton.interactable, Is.True, $"Earthquake/Hard/{i + 1} is a Playable Level");
            }
        }

        [UnityTest]
        public IEnumerator NameWithoutStudent_ShowsPickerOnceAtMainMenu()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.SaveProgress();
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();

            Assert.That(c.studentPanel.IsVisible, Is.True, "an old save with a name and no pick shows the picker");
            c.studentBoy.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(GameServices.Progress.Student, Is.EqualTo(StudentKind.Boy));
            Assert.That(c.studentPanel.IsVisible, Is.False);
            Assert.That(c.mainPanel.IsVisible, Is.True);
        }

        [UnityTest]
        public IEnumerator GirlPick_Easy1UsesTheGirlFrames()
        {
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            c.playButton.onClick.Invoke();
            yield return Wait(0.4f);
            c.nameInput.text = "Ana";
            c.nameOk.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.studentPanel.IsVisible, Is.True);
            c.studentGirl.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(GameServices.Progress.Student, Is.EqualTo(StudentKind.Girl));

            c.episodeCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            c.difficultyCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            c.levelSlots[0].button.onClick.Invoke();
            yield return null;
            c.startButton.onClick.Invoke();

            LevelController level = null;
            for (var t = 0f; t < 10f && (level = UnityEngine.Object.FindAnyObjectByType<LevelController>()) == null; t += Time.unscaledDeltaTime)
                yield return null;
            Assert.That(level, Is.Not.Null);
            for (var t = 0f; t < 5f && level.Student == null; t += Time.unscaledDeltaTime) yield return null;
            yield return Wait(0.3f);

            var student = level.Student;
            Assert.That(student.Kind, Is.EqualTo(StudentKind.Girl));
            Assert.That(student.CurrentSprite.name, Does.StartWith("girl_"));
            Assert.That(student.Frames, Is.SameAs(student.girl));
        }

        [UnityTest]
        public IEnumerator CorruptSave_IsPreserved_AndMenuStillLoads()
        {
            File.WriteAllText(Path.Combine(dir, "save.json"), "{ broken");
            yield return BootToMenu();
            Assert.That(UnityEngine.Object.FindAnyObjectByType<MainMenuController>(), Is.Not.Null);
            Assert.That(Directory.GetFiles(dir, "save.corrupt-*.json"), Has.Length.EqualTo(1));
            Assert.That(GameServices.CanContinue, Is.False);
            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex("could not be read"));
        }
    }
}

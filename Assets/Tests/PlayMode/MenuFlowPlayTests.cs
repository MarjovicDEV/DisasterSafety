using System;
using System.Collections;
using System.IO;
using DisasterSafety.Core;
using DisasterSafety.Game;
using DisasterSafety.Game.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

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
            // The prior menu can still be waiting to greet its Learner when the next test starts.
            var priorMenu = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            if (priorMenu != null) priorMenu.gameObject.SetActive(false);
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
            var previous = SceneManager.GetActiveScene();
            SceneManager.LoadScene("Boot");
            yield return NewActiveScene(GameScenes.MainMenu, previous);
            yield return Wait(0.5f);
        }

        /// <summary>
        /// Waits for a newly loaded scene with this name. A test that starts while the last test's
        /// Main Menu is still active must not take that old scene for the new one.
        /// </summary>
        static IEnumerator NewActiveScene(string name, Scene previous)
        {
            for (var i = 0; i < 600; i++)
            {
                var active = SceneManager.GetActiveScene();
                if (active.name == name && active.handle != previous.handle) yield break;
                yield return null;
            }
            Assert.Fail("Scene " + name + " did not load");
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

        // The Student on the menus (ticket 13) -------------------------------------------------------------

        static StudentActor Named(string name) =>
            Array.Find(UnityEngine.Object.FindObjectsByType<StudentActor>(FindObjectsInactive.Include, FindObjectsSortMode.None),
                s => s.name == name);

        static StudentActor MenuStudent() => Named("Menu Student");
        static StudentActor HowToPlayStudent() => Named("How To Play Student");

        IEnumerator BootWithSave(StudentKind kind, string name = "Ana")
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName(name);
            GameServices.Progress.SetStudent(kind);
            GameServices.SaveProgress();
            yield return BootToMenu();
        }

        [UnityTest]
        public IEnumerator MainMenu_FreshInstall_ShowsTheBoyStudent()
        {
            yield return BootToMenu();
            var student = MenuStudent();
            Assert.That(student, Is.Not.Null, "the Main Menu has the Learner's Student");
            Assert.That(student.gameObject.activeInHierarchy, Is.True);
            Assert.That(student.Kind, Is.EqualTo(StudentKind.Boy), "the boy shows until a pick exists");
            Assert.That(student.CurrentSprite.name, Does.StartWith("boy_"));
        }

        [UnityTest]
        public IEnumerator MainMenu_GirlSave_ShowsTheGirlOnTheMenuAndInHowToPlay()
        {
            yield return BootWithSave(StudentKind.Girl);
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            Assert.That(MenuStudent(), Is.Not.Null, "the Main Menu has the Learner's Student");
            Assert.That(MenuStudent().Kind, Is.EqualTo(StudentKind.Girl));
            Assert.That(MenuStudent().CurrentSprite.name, Does.StartWith("girl_"));

            c.howToPlayButton.onClick.Invoke();
            yield return Wait(0.4f);
            var guide = HowToPlayStudent();
            Assert.That(guide, Is.Not.Null, "How to Play has the Learner's Student");
            Assert.That(guide.gameObject.activeInHierarchy, Is.True);
            Assert.That(guide.Kind, Is.EqualTo(StudentKind.Girl));
            Assert.That(guide.CurrentSprite.name, Does.StartWith("girl_"));

            // Name Entry comes before the pick, so it always shows both Students.
            var names = Array.ConvertAll(c.namePanel.GetComponentsInChildren<Image>(true), i => i.sprite != null ? i.sprite.name : "");
            Assert.That(names, Does.Contain("boy_idle_0"));
            Assert.That(names, Does.Contain("girl_idle_0"));
        }

        [UnityTest]
        public IEnumerator StudentPick_ChangesTheMenuStudentAtOnce()
        {
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            Assert.That(MenuStudent(), Is.Not.Null, "the Main Menu has the Learner's Student");
            Assert.That(MenuStudent().Kind, Is.EqualTo(StudentKind.Boy));
            c.playButton.onClick.Invoke();
            yield return Wait(0.4f);
            c.nameInput.text = "Ana";
            c.nameOk.onClick.Invoke();
            yield return Wait(0.5f);
            c.studentGirl.onClick.Invoke();
            yield return null;
            Assert.That(MenuStudent().Kind, Is.EqualTo(StudentKind.Girl), "the Main Menu Student changes without a reload");
            Assert.That(MenuStudent().CurrentSprite.name, Does.StartWith("girl_"));
        }

        [UnityTest]
        public IEnumerator MenuStudent_Tap_SaysATip()
        {
            yield return BootToMenu();
            yield return Wait(1f);
            var student = MenuStudent();
            Assert.That(student, Is.Not.Null, "the Main Menu has the Learner's Student");
            ExecuteEvents.Execute(student.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            yield return null;
            Assert.That(student.bubble.gameObject.activeSelf, Is.True);
            Assert.That(UIStrings.Tips, Does.Contain(student.bubble.Line), "a tap shows a Tip");
        }

        [UnityTest]
        public IEnumerator MainMenu_Greeting_IsTheStudentsLine()
        {
            yield return BootToMenu();
            yield return Wait(1f);
            Assert.That(MenuStudent(), Is.Not.Null, "the Main Menu has the Learner's Student");
            Assert.That(MenuStudent().bubble.Line, Is.EqualTo(UIStrings.StudentHello));
            Assert.That(MenuStudent().Pose, Is.EqualTo(StudentPose.Happy), "the Student greets in the Happy pose");
        }

        [UnityTest]
        public IEnumerator MainMenu_Greeting_WelcomesBackASavedName()
        {
            yield return BootWithSave(StudentKind.Boy);
            yield return Wait(1f);
            Assert.That(MenuStudent(), Is.Not.Null, "the Main Menu has the Learner's Student");
            Assert.That(MenuStudent().bubble.Line, Is.EqualTo(UIStrings.StudentWelcomeBack("Ana")));
        }

        [UnityTest]
        public IEnumerator HowToPlay_HasSevenPages_ShowingTheActionBarFlow()
        {
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            var pages = UIStrings.TutorialPages;
            Assert.That(Array.ConvertAll(pages, p => p.Art), Is.EqualTo(new[]
            {
                "topic", "difficulty", "v2_actionbar", "v2_actout", "v2_feedback", "v2_hint", "stars",
            }));
            Assert.That(c.tutorial.frames, Has.Length.EqualTo(14));
            Assert.That(c.tutorial.frames, Has.None.Null, "every page has two frames");
            Assert.That(c.tutorial.dots, Has.Length.EqualTo(7));
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

        // Backgrounds (C3, C4) ---------------------------------------------------------------

        // The moving layers each background shows, written out here so a wrong rule in MenuWorld fails.
        static readonly MenuBackground[] WithFlag = { MenuBackground.SchoolGate, MenuBackground.DrillGround };
        static readonly MenuBackground[] Outdoors =
        {
            MenuBackground.SchoolGate, MenuBackground.DrillGround, MenuBackground.FireStation, MenuBackground.Schoolyard,
            MenuBackground.CoveredCourt
        };
        static readonly MenuBackground[] WithTrees = { MenuBackground.DrillGround, MenuBackground.Schoolyard };
        static readonly MenuBackground[] WithWalkers =
            { MenuBackground.SchoolGate, MenuBackground.DrillGround, MenuBackground.Schoolyard };

        static void AssertBackground(MainMenuController c, MenuBackground expected, string sprite, string why)
        {
            Assert.That(c.world.Current, Is.EqualTo(expected), why);
            Assert.That(c.world.background.sprite.name, Is.EqualTo(sprite), why);
            Assert.That(c.world.flag.activeSelf, Is.EqualTo(Array.IndexOf(WithFlag, expected) >= 0), why + ": flag");
            Assert.That(c.world.clouds.activeSelf, Is.EqualTo(Array.IndexOf(Outdoors, expected) >= 0), why + ": clouds");
            Assert.That(c.world.birds.activeSelf, Is.EqualTo(Array.IndexOf(Outdoors, expected) >= 0), why + ": birds");
            Assert.That(c.world.leaves.activeSelf, Is.EqualTo(Array.IndexOf(WithTrees, expected) >= 0), why + ": leaves");
            Assert.That(c.world.walkers.activeSelf, Is.EqualTo(Array.IndexOf(WithWalkers, expected) >= 0), why + ": walkers");
        }

        [UnityTest]
        public IEnumerator Backgrounds_FreshSave_FollowEachScreenAndBack()
        {
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            AssertBackground(c, MenuBackground.SchoolGate, "bg_menu_gate", "Main Menu");

            c.settingsButton.onClick.Invoke();
            yield return Wait(0.4f);
            AssertBackground(c, MenuBackground.SchoolGate, "bg_menu_gate", "Settings over the gate");
            c.settingsBack.onClick.Invoke();
            yield return Wait(0.4f);

            c.playButton.onClick.Invoke();
            yield return Wait(0.4f);
            AssertBackground(c, MenuBackground.SchoolGate, "bg_menu_gate", "name entry over the gate");
            c.nameInput.text = "Ana";
            c.nameOk.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.SchoolGate, "bg_menu_gate", "Student picker over the gate");
            c.studentBoy.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.Noticeboard, "bg_menu_noticeboard", "Episode selection");

            c.episodeCards[1].button.onClick.Invoke();
            yield return null;
            AssertBackground(c, MenuBackground.Noticeboard, "bg_menu_noticeboard", "locked Fire card");
            c.episodeCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.DrillGround, "bg_menu_drill_ground", "Earthquake Difficulty selection");

            c.difficultyCards[2].button.onClick.Invoke();
            yield return null;
            AssertBackground(c, MenuBackground.DrillGround, "bg_menu_drill_ground", "locked Hard card");
            c.difficultyCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.Classroom, "bg_menu_classroom", "Earthquake Easy Level selection");

            c.levelBack.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.DrillGround, "bg_menu_drill_ground", "Back to Difficulty selection");
            c.difficultyBack.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.Noticeboard, "bg_menu_noticeboard", "Back to Episode selection");
            c.episodeBack.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.SchoolGate, "bg_menu_gate", "Back to the Main Menu");
        }

        [UnityTest]
        public IEnumerator Backgrounds_EarthquakeDifficulties_ShowTheirOwnLevelSelection()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.SetStudent(StudentKind.Boy);
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Normal);
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Hard);
            GameServices.SaveProgress();
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();

            c.playButton.onClick.Invoke();
            yield return Wait(0.5f);
            c.episodeCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            var expected = new[]
            {
                (MenuBackground.Classroom, "bg_menu_classroom"),
                (MenuBackground.Schoolyard, "bg_menu_schoolyard"),
                (MenuBackground.CoveredCourt, "bg_menu_covered_court"),
            };
            for (var i = 0; i < 3; i++)
            {
                c.difficultyCards[i].button.onClick.Invoke();
                yield return Wait(0.5f);
                Assert.That(c.levelPanel.IsVisible, Is.True, Curriculum.Difficulties[i] + " opens Level selection");
                AssertBackground(c, expected[i].Item1, expected[i].Item2, Curriculum.Difficulties[i] + " Level selection");
                c.levelBack.onClick.Invoke();
                yield return Wait(0.5f);
                AssertBackground(c, MenuBackground.DrillGround, "bg_menu_drill_ground", "Back from " + Curriculum.Difficulties[i]);
            }
        }

        [UnityTest]
        public IEnumerator Backgrounds_ExistingSave_OpensFireOnTheStationAndAnEmptyNoticeboard()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.SetStudent(StudentKind.Girl);
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Hard);
            GameServices.Progress.Open(Episode.Fire, Difficulty.Easy);
            GameServices.SaveProgress();
            yield return BootToMenu();
            var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();

            c.playButton.onClick.Invoke();
            yield return Wait(0.5f);
            c.episodeCards[1].button.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.difficultyPanel.IsVisible, Is.True, "Fire is open");
            AssertBackground(c, MenuBackground.FireStation, "bg_menu_fire_station", "Fire Difficulty selection");

            c.difficultyCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            Assert.That(c.levelPanel.IsVisible, Is.True);
            AssertBackground(c, MenuBackground.StationNoticeboard, "bg_menu_station_noticeboard", "empty Fire Level selection");
            c.levelSlots[0].button.onClick.Invoke();
            yield return null;
            Assert.That(c.startButton.interactable, Is.False, "Fire has no Playable Level yet");
            Assert.That(MenuStudent().bubble.Line, Is.EqualTo(UIStrings.StudentLevelSoon), "the Student says the Level is coming soon");

            c.levelBack.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.FireStation, "bg_menu_fire_station", "Back to Fire Difficulty selection");
            c.difficultyBack.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.Noticeboard, "bg_menu_noticeboard", "Back to Episode selection");
            c.episodeCards[0].button.onClick.Invoke();
            yield return Wait(0.5f);
            AssertBackground(c, MenuBackground.DrillGround, "bg_menu_drill_ground", "Earthquake after Fire");
        }

        [UnityTest]
        public IEnumerator Backgrounds_ReturnFromALevel_ShowThatDifficultyOrTheGate()
        {
            GameServices.Initialize(dir);
            GameServices.Progress.SetDisplayName("Ana");
            GameServices.Progress.SetStudent(StudentKind.Boy);
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Normal);
            GameServices.Progress.Open(Episode.Earthquake, Difficulty.Hard);
            GameServices.SaveProgress();
            yield return BootToMenu();

            var expected = new[]
            {
                (Difficulty.Easy, MenuBackground.Classroom, "bg_menu_classroom"),
                (Difficulty.Normal, MenuBackground.Schoolyard, "bg_menu_schoolyard"),
                (Difficulty.Hard, MenuBackground.CoveredCourt, "bg_menu_covered_court"),
            };
            foreach (var (difficulty, background, sprite) in expected)
            {
                var loaded = SceneManager.GetActiveScene();
                GameScenes.PlayLevel(new LevelId(Episode.Earthquake, difficulty, 1));
                yield return NewActiveScene(GameScenes.Level, loaded);
                yield return Wait(0.3f);

                // Level Completed's and Pause's Levels button.
                loaded = SceneManager.GetActiveScene();
                GameScenes.ToLevelSelect(Episode.Earthquake, difficulty);
                yield return NewActiveScene(GameScenes.MainMenu, loaded);
                yield return Wait(0.3f);
                var c = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
                Assert.That(c.levelPanel.IsVisible, Is.True, difficulty + " Level selection after its Level");
                AssertBackground(c, background, sprite, difficulty + " after its Level");
                c.levelBack.onClick.Invoke();
                yield return Wait(0.5f);
                AssertBackground(c, MenuBackground.DrillGround, "bg_menu_drill_ground", "Back after " + difficulty);
            }

            var before = SceneManager.GetActiveScene();
            GameScenes.PlayLevel(new LevelId(Episode.Earthquake, Difficulty.Easy, 1));
            yield return NewActiveScene(GameScenes.Level, before);
            yield return Wait(0.3f);
            before = SceneManager.GetActiveScene();
            GameScenes.ToMainMenu();
            yield return NewActiveScene(GameScenes.MainMenu, before);
            yield return Wait(0.3f);
            AssertBackground(UnityEngine.Object.FindAnyObjectByType<MainMenuController>(), MenuBackground.SchoolGate,
                "bg_menu_gate", "Main Menu after a Level");
        }
    }
}

using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class MenuNavigatorTests
    {
        sealed class FakeState : IMenuState
        {
            public bool HasDisplayName { get; set; }
            public bool HasStudent { get; set; }
            public bool CanContinue { get; set; }
            public (Episode, Difficulty) ContinueDestination { get; set; } = (Episode.Earthquake, Difficulty.Easy);
            /// <summary>Every Earthquake Difficulty and Fire Easy open, as after finishing Earthquake Hard.</summary>
            public bool EarthquakeDone { get; set; }
            public bool IsOpen(Episode e) => e == Episode.Earthquake || (EarthquakeDone && e == Episode.Fire);
            public bool IsOpen(Episode e, Difficulty d) =>
                (e == Episode.Earthquake && (EarthquakeDone || d == Difficulty.Easy))
                || (EarthquakeDone && e == Episode.Fire && d == Difficulty.Easy);
        }

        FakeState state;
        MenuNavigator nav;

        [SetUp]
        public void SetUp()
        {
            state = new FakeState();
            nav = new MenuNavigator(state);
        }

        [Test]
        public void StartsOnMainMenu() => Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));

        [Test]
        public void Play_WithoutName_AsksForName()
        {
            nav.Play();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.NameEntry));
            nav.NameSaved();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.StudentPick));
            nav.StudentPicked();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.EpisodeSelect));
        }

        [Test]
        public void OfferStudentPick_NameWithoutStudent_ShowsPickerAndPickReturnsToMain()
        {
            state.HasDisplayName = true;
            Assert.That(nav.OfferStudentPick(), Is.True);
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.StudentPick));
            nav.StudentPicked();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));
        }

        [Test]
        public void OfferStudentPick_WithStudentOrWithoutName_DoesNothing()
        {
            Assert.That(nav.OfferStudentPick(), Is.False, "no Display Name yet");
            state.HasDisplayName = true;
            state.HasStudent = true;
            Assert.That(nav.OfferStudentPick(), Is.False, "already picked");
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));
        }

        [Test]
        public void Back_FromStudentPick_GoesToMain()
        {
            state.HasDisplayName = true;
            nav.OfferStudentPick();
            nav.Back();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));
        }

        [Test]
        public void Play_WithName_GoesToEpisodes()
        {
            state.HasDisplayName = true;
            nav.Play();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.EpisodeSelect));
        }

        [Test]
        public void Selection_WalksEpisodeDifficultyLevel_AndBackReturnsStepByStep()
        {
            state.HasDisplayName = true;
            nav.Play();
            Assert.That(nav.SelectEpisode(Episode.Earthquake), Is.True);
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.DifficultySelect));
            Assert.That(nav.SelectDifficulty(Difficulty.Easy), Is.True);
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.LevelSelect));
            Assert.That(nav.SelectedEpisode, Is.EqualTo(Episode.Earthquake));
            Assert.That(nav.SelectedDifficulty, Is.EqualTo(Difficulty.Easy));

            nav.Back();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.DifficultySelect));
            nav.Back();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.EpisodeSelect));
            nav.Back();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));
        }

        [Test]
        public void LockedChoices_AreRefused()
        {
            state.HasDisplayName = true;
            nav.Play();
            Assert.That(nav.SelectEpisode(Episode.Fire), Is.False);
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.EpisodeSelect));
            nav.SelectEpisode(Episode.Earthquake);
            Assert.That(nav.SelectDifficulty(Difficulty.Hard), Is.False);
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.DifficultySelect));
        }

        [TestCase(MenuScreen.Settings)]
        [TestCase(MenuScreen.HowToPlay)]
        [TestCase(MenuScreen.NameEntry)]
        public void Back_FromMainMenuPanels_ReturnsToMain(MenuScreen panel)
        {
            if (panel == MenuScreen.Settings) nav.OpenSettings();
            if (panel == MenuScreen.HowToPlay) nav.OpenHowToPlay();
            if (panel == MenuScreen.NameEntry) nav.Play();
            Assert.That(nav.Current, Is.EqualTo(panel));
            nav.Back();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));
        }

        [Test]
        public void Back_OnMain_AsksToExit_AndBackAgainCancels()
        {
            nav.Back();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.ExitConfirm));
            nav.Back();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));
        }

        [Test]
        public void ExitConfirm_YesQuits_NoReturns()
        {
            nav.RequestExit();
            nav.CancelExit();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));
            nav.RequestExit();
            Assert.That(nav.ConfirmExit(), Is.True);
        }

        [Test]
        public void Continue_WhenNotAllowed_StaysOnMain()
        {
            Assert.That(nav.Continue(), Is.False);
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));
        }

        [Test]
        public void Continue_WhenAllowed_OpensLevelSelectForLastPlayed()
        {
            state.CanContinue = true;
            state.ContinueDestination = (Episode.Earthquake, Difficulty.Easy);
            Assert.That(nav.Continue(), Is.True);
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.LevelSelect));
            nav.Back();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.DifficultySelect));
        }

        [Test]
        public void OpenLevelSelect_FromLevelCompleted_ShowsThatEpisodeAndDifficulty()
        {
            Assert.That(nav.OpenLevelSelect(Episode.Earthquake, Difficulty.Easy), Is.True);
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.LevelSelect));
            Assert.That(nav.SelectedEpisode, Is.EqualTo(Episode.Earthquake));
            Assert.That(nav.SelectedDifficulty, Is.EqualTo(Difficulty.Easy));
            nav.Back();
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.DifficultySelect));
        }

        [Test]
        public void OpenLevelSelect_ForALockedDifficulty_StaysOnMain()
        {
            Assert.That(nav.OpenLevelSelect(Episode.Earthquake, Difficulty.Hard), Is.False);
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.Main));
        }

        [Test]
        public void Changed_ReportsEveryMove()
        {
            MenuScreen from = MenuScreen.Main, to = MenuScreen.Main;
            nav.Changed += (a, b) => { from = a; to = b; };
            nav.OpenSettings();
            Assert.That(from, Is.EqualTo(MenuScreen.Main));
            Assert.That(to, Is.EqualTo(MenuScreen.Settings));
        }

        // Backgrounds (C3, C4) ------------------------------------------------------------

        [Test]
        public void Background_MainMenuAndItsDialogs_ShowTheSchoolGate()
        {
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.SchoolGate));
            nav.OpenSettings();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.SchoolGate), "Settings");
            nav.Back();
            nav.OpenHowToPlay();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.SchoolGate), "How to Play");
            nav.Back();
            nav.RequestExit();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.SchoolGate), "Exit");
            nav.CancelExit();
            nav.Play();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.SchoolGate), "name entry");
            nav.NameSaved();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.SchoolGate), "Student picker");
        }

        [Test]
        public void Background_FreshSave_EarthquakeEasyWalkAndBack()
        {
            state.HasDisplayName = true;
            nav.Play();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.Noticeboard), "Episode selection");

            Assert.That(nav.SelectEpisode(Episode.Fire), Is.False, "Fire is locked on a fresh save");
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.Noticeboard), "a locked card keeps the noticeboard");

            nav.SelectEpisode(Episode.Earthquake);
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.DrillGround), "Earthquake Difficulty selection");
            Assert.That(nav.SelectDifficulty(Difficulty.Hard), Is.False, "Hard is locked on a fresh save");
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.DrillGround), "a locked card keeps the drill ground");

            nav.SelectDifficulty(Difficulty.Easy);
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.Classroom), "Earthquake Easy Level selection");

            nav.Back();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.DrillGround), "Back to Difficulty selection");
            nav.Back();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.Noticeboard), "Back to Episode selection");
            nav.Back();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.SchoolGate), "Back to the Main Menu");
        }

        [TestCase(Difficulty.Easy, MenuBackground.Classroom)]
        [TestCase(Difficulty.Normal, MenuBackground.Schoolyard)]
        [TestCase(Difficulty.Hard, MenuBackground.CoveredCourt)]
        public void Background_EarthquakeLevelSelection_MatchesTheDifficulty(Difficulty difficulty, MenuBackground expected)
        {
            state.HasDisplayName = true;
            state.EarthquakeDone = true;
            nav.Play();
            nav.SelectEpisode(Episode.Earthquake);
            Assert.That(nav.SelectDifficulty(difficulty), Is.True);
            Assert.That(nav.Background, Is.EqualTo(expected));
            nav.Back();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.DrillGround));
        }

        [Test]
        public void Background_ExistingSave_OpensFireWithTheStationAndAnEmptyNoticeboard()
        {
            state.HasDisplayName = true;
            state.EarthquakeDone = true;
            nav.Play();
            Assert.That(nav.SelectEpisode(Episode.Fire), Is.True, "Fire is open after Earthquake Hard");
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.FireStation), "Fire Difficulty selection");
            Assert.That(nav.SelectDifficulty(Difficulty.Easy), Is.True);
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.StationNoticeboard), "empty Fire Level selection");

            nav.Back();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.FireStation));
            nav.Back();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.Noticeboard));

            // Choosing Earthquake after Fire swaps the Episode background.
            nav.SelectEpisode(Episode.Earthquake);
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.DrillGround));
        }

        [TestCase(Difficulty.Easy, MenuBackground.Classroom)]
        [TestCase(Difficulty.Normal, MenuBackground.Schoolyard)]
        [TestCase(Difficulty.Hard, MenuBackground.CoveredCourt)]
        public void Background_ReturnFromALevel_ShowsThatDifficulty(Difficulty difficulty, MenuBackground expected)
        {
            state.EarthquakeDone = true;
            Assert.That(nav.OpenLevelSelect(Episode.Earthquake, difficulty), Is.True);
            Assert.That(nav.Background, Is.EqualTo(expected));
            nav.Back();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.DrillGround));
        }

        [Test]
        public void Background_Continue_ShowsTheLastPlayedDifficulty()
        {
            state.EarthquakeDone = true;
            state.CanContinue = true;
            state.ContinueDestination = (Episode.Earthquake, Difficulty.Normal);
            nav.Continue();
            Assert.That(nav.Background, Is.EqualTo(MenuBackground.Schoolyard));
        }
    }
}

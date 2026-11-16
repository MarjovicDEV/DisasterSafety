using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class MenuNavigatorTests
    {
        sealed class FakeState : IMenuState
        {
            public bool HasDisplayName { get; set; }
            public bool CanContinue { get; set; }
            public (Episode, Difficulty) ContinueDestination { get; set; } = (Episode.Earthquake, Difficulty.Easy);
            public bool IsOpen(Episode e) => e == Episode.Earthquake;
            public bool IsOpen(Episode e, Difficulty d) => e == Episode.Earthquake && d == Difficulty.Easy;
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
            Assert.That(nav.Current, Is.EqualTo(MenuScreen.EpisodeSelect));
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
        public void Changed_ReportsEveryMove()
        {
            MenuScreen from = MenuScreen.Main, to = MenuScreen.Main;
            nav.Changed += (a, b) => { from = a; to = b; };
            nav.OpenSettings();
            Assert.That(from, Is.EqualTo(MenuScreen.Main));
            Assert.That(to, Is.EqualTo(MenuScreen.Settings));
        }
    }
}

using System;

namespace DisasterSafety.Core
{
    public enum MenuScreen
    {
        Main,
        Settings,
        HowToPlay,
        ExitConfirm,
        NameEntry,
        EpisodeSelect,
        DifficultySelect,
        LevelSelect
    }

    /// <summary>What the navigator needs to know about the Learner record.</summary>
    public interface IMenuState
    {
        bool HasDisplayName { get; }
        bool CanContinue { get; }
        (Episode, Difficulty) ContinueDestination { get; }
        bool IsOpen(Episode episode);
        bool IsOpen(Episode episode, Difficulty difficulty);
    }

    /// <summary>
    /// The menu flow from Figures 1 and 2 of Chapter III, with no Unity dependency.
    /// Views call these methods and redraw when Changed fires.
    /// </summary>
    public sealed class MenuNavigator
    {
        readonly IMenuState state;

        public MenuScreen Current { get; private set; } = MenuScreen.Main;
        public Episode SelectedEpisode { get; private set; }
        public Difficulty SelectedDifficulty { get; private set; }

        /// <summary>(from, to) after every move.</summary>
        public event Action<MenuScreen, MenuScreen> Changed;

        public MenuNavigator(IMenuState state)
        {
            this.state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public void Play() => GoTo(state.HasDisplayName ? MenuScreen.EpisodeSelect : MenuScreen.NameEntry);

        public void NameSaved() => GoTo(MenuScreen.EpisodeSelect);

        public bool Continue()
        {
            if (Current != MenuScreen.Main || !state.CanContinue) return false;
            var (e, d) = state.ContinueDestination;
            SelectedEpisode = e;
            SelectedDifficulty = d;
            GoTo(MenuScreen.LevelSelect);
            return true;
        }

        /// <summary>Level Completed's "Levels" button: Level Selection for the Level just played.</summary>
        public bool OpenLevelSelect(Episode episode, Difficulty difficulty)
        {
            if (Current != MenuScreen.Main || !state.IsOpen(episode, difficulty)) return false;
            SelectedEpisode = episode;
            SelectedDifficulty = difficulty;
            GoTo(MenuScreen.LevelSelect);
            return true;
        }

        public void OpenSettings() => GoTo(MenuScreen.Settings);
        public void OpenHowToPlay() => GoTo(MenuScreen.HowToPlay);
        public void RequestExit() => GoTo(MenuScreen.ExitConfirm);
        public void CancelExit() => GoTo(MenuScreen.Main);

        /// <summary>Returns true when the app should close.</summary>
        public bool ConfirmExit() => Current == MenuScreen.ExitConfirm;

        public bool SelectEpisode(Episode episode)
        {
            if (Current != MenuScreen.EpisodeSelect || !state.IsOpen(episode)) return false;
            SelectedEpisode = episode;
            GoTo(MenuScreen.DifficultySelect);
            return true;
        }

        public bool SelectDifficulty(Difficulty difficulty)
        {
            if (Current != MenuScreen.DifficultySelect || !state.IsOpen(SelectedEpisode, difficulty)) return false;
            SelectedDifficulty = difficulty;
            GoTo(MenuScreen.LevelSelect);
            return true;
        }

        /// <summary>The Back button, on screen or Android.</summary>
        public void Back()
        {
            switch (Current)
            {
                case MenuScreen.Main: GoTo(MenuScreen.ExitConfirm); break;
                case MenuScreen.LevelSelect: GoTo(MenuScreen.DifficultySelect); break;
                case MenuScreen.DifficultySelect: GoTo(MenuScreen.EpisodeSelect); break;
                default: GoTo(MenuScreen.Main); break;
            }
        }

        void GoTo(MenuScreen next)
        {
            var previous = Current;
            Current = next;
            Changed?.Invoke(previous, next);
        }
    }
}

namespace DisasterSafety.Core
{
    /// <summary>
    /// The picture behind a menu screen (C3, C4). The Main Menu shows the school gate and
    /// Episode selection shows a noticeboard. DrillGround and FireStation are Episode backgrounds;
    /// Classroom, Schoolyard and CoveredCourt are Earthquake's Difficulty backgrounds and FireHome,
    /// FireCorridor and FireStreet are Fire's (F9).
    /// </summary>
    public enum MenuBackground
    {
        SchoolGate,
        Noticeboard,
        DrillGround,
        FireStation,
        Classroom,
        Schoolyard,
        CoveredCourt,
        FireHome,
        FireCorridor,
        FireStreet
    }

    public static class MenuBackgrounds
    {
        /// <summary>
        /// The background for a screen and the current Episode and Difficulty. The Main Menu and its
        /// dialogs show the school gate. Fire's Level selection shows the living room on Easy, the school
        /// corridor with the Fire Bell box on Normal and the barangay street at dusk on Hard.
        /// </summary>
        public static MenuBackground For(MenuScreen screen, Episode episode, Difficulty difficulty)
        {
            switch (screen)
            {
                case MenuScreen.EpisodeSelect:
                    return MenuBackground.Noticeboard;
                case MenuScreen.DifficultySelect:
                    return episode == Episode.Earthquake ? MenuBackground.DrillGround
                        : episode == Episode.Fire ? MenuBackground.FireStation
                        : MenuBackground.Noticeboard;
                case MenuScreen.LevelSelect:
                    if (episode == Episode.Fire)
                        return difficulty == Difficulty.Easy ? MenuBackground.FireHome
                            : difficulty == Difficulty.Normal ? MenuBackground.FireCorridor
                            : MenuBackground.FireStreet;
                    if (episode != Episode.Earthquake) return MenuBackground.Noticeboard;
                    return difficulty == Difficulty.Easy ? MenuBackground.Classroom
                        : difficulty == Difficulty.Normal ? MenuBackground.Schoolyard
                        : MenuBackground.CoveredCourt;
                default:
                    return MenuBackground.SchoolGate;
            }
        }
    }
}

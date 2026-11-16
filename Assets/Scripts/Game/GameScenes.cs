using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DisasterSafety.Game
{
    /// <summary>Scene names and the moves between them. Works from Boot and from tests that load scenes directly.</summary>
    public static class GameScenes
    {
        public const string Boot = "Boot";
        public const string MainMenu = "MainMenu";
        public const string Level = "Level";

        /// <summary>Opens the Level scene for this Level. Restart and Replay use it too.</summary>
        public static void PlayLevel(LevelId id)
        {
            Time.timeScale = 1f;
            GameServices.CurrentLevel = id;
            SceneManager.LoadScene(Level);
        }

        /// <summary>Back to the Main Menu, which repeats the Continue check as it opens.</summary>
        public static void ToMainMenu()
        {
            Time.timeScale = 1f;
            GameServices.MenuOpensLevelSelect = null;
            SceneManager.LoadScene(MainMenu);
        }

        /// <summary>Back to the Main Menu, opened straight on Level Selection for this Episode and Difficulty.</summary>
        public static void ToLevelSelect(Episode episode, Difficulty difficulty)
        {
            Time.timeScale = 1f;
            GameServices.MenuOpensLevelSelect = (episode, difficulty);
            SceneManager.LoadScene(MainMenu);
        }
    }
}

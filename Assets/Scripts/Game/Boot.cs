using UnityEngine;
using UnityEngine.SceneManagement;

namespace DisasterSafety.Game
{
    /// <summary>
    /// First scene. Loads audio preferences and the save file (preserving an unreadable one),
    /// starts the music, then opens the Main Menu.
    /// </summary>
    public sealed class Boot : MonoBehaviour
    {
        public const string MainMenuScene = "MainMenu";

        void Start()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            GameServices.Initialize();
            AudioManager.EnsureExists();
            SceneManager.LoadScene(MainMenuScene);
        }
    }
}

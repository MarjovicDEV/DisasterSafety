using DisasterSafety.Core;
using UnityEngine;

namespace DisasterSafety.Game
{
    /// <summary>
    /// Process-wide game state: the Learner record, the save file, audio preferences and
    /// the Playable Levels in this build. Initialised by Boot, or lazily when a scene is
    /// played directly in the Editor.
    /// </summary>
    public static class GameServices
    {
        public static bool Ready { get; private set; }
        public static SaveService Saves { get; private set; }
        public static AudioPreferences Audio { get; private set; }

        /// <summary>The Learner record. A new game until something is saved.</summary>
        public static ProgressState Progress { get; private set; }

        /// <summary>True once a save file exists (loaded, or written this session).</summary>
        public static bool HasSaveFile { get; private set; }

        public static SaveLoadStatus StartupLoadStatus { get; private set; }
        public static string PreservedSavePath { get; private set; }

        /// <summary>No scenario content exists in this milestone, so no Level is playable yet.</summary>
        public static PlayableLevels Content { get; private set; } = PlayableLevels.None;

        /// <summary>Forgets all state. Runs on every Play mode start; tests call it between cases.</summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            Ready = false;
            Saves = null;
            Audio = null;
            Progress = null;
            HasSaveFile = false;
            PreservedSavePath = null;
            Content = PlayableLevels.None;
        }

        /// <param name="saveDirectory">Folder for save.json. Defaults to Application.persistentDataPath; tests pass a temp folder.</param>
        public static void Initialize(string saveDirectory = null)
        {
            if (Ready) return;

            Audio = new AudioPreferences(new PlayerPrefsStore());
            Saves = new SaveService(saveDirectory ?? Application.persistentDataPath);

            var result = Saves.Load();
            StartupLoadStatus = result.Status;
            PreservedSavePath = result.PreservedPath;
            HasSaveFile = result.Progress != null;
            Progress = result.Progress ?? ProgressState.NewGame();

            if (result.Status == SaveLoadStatus.Preserved)
                Debug.LogWarning("Save file could not be read and was kept at: " + result.PreservedPath);

            Ready = true;
        }

        /// <summary>The record ContinueRule should see: null when nothing has ever been saved.</summary>
        public static ProgressState SavedProgressOrNull => HasSaveFile ? Progress : null;

        public static bool CanContinue => ContinueRule.CanContinue(SavedProgressOrNull, Content);

        public static void SaveProgress()
        {
            Saves.Save(Progress);
            HasSaveFile = true;
        }
    }

    public sealed class PlayerPrefsStore : IPreferenceStore
    {
        public int GetInt(string key, int fallback) => PlayerPrefs.GetInt(key, fallback);
        public void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);
        public void Flush() => PlayerPrefs.Save();
    }
}

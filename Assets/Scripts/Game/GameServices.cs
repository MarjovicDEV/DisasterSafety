using System.Collections.Generic;
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

        /// <summary>The authored Scenarios in this build (Resources/Scenarios).</summary>
        public static ScenarioLibrary Scenarios { get; private set; } = ScenarioLibrary.Empty;

        /// <summary>The Playable Levels: every Level with a valid authored Scenario.</summary>
        public static PlayableLevels Content => Scenarios.Playable;

        /// <summary>The Level the Level scene plays. Set by Level Selection before loading it.</summary>
        public static LevelId CurrentLevel { get; set; } = new LevelId(Episode.Earthquake, Difficulty.Easy, 1);

        /// <summary>
        /// Set by Level Completed's "Levels" button: the Main Menu opens straight on Level
        /// Selection for this Episode and Difficulty, then clears it.
        /// </summary>
        public static (Episode, Difficulty)? MenuOpensLevelSelect { get; set; }

        public const string ScenarioFolder = "Scenarios";

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
            Scenarios = ScenarioLibrary.Empty;
            CurrentLevel = new LevelId(Episode.Earthquake, Difficulty.Easy, 1);
            MenuOpensLevelSelect = null;
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

            Scenarios = LoadScenarios();

            Ready = true;
        }

        /// <summary>The record ContinueRule should see: null when nothing has ever been saved.</summary>
        public static ProgressState SavedProgressOrNull => HasSaveFile ? Progress : null;

        public static bool CanContinue => ContinueRule.CanContinue(SavedProgressOrNull, Content);

        static ScenarioLibrary LoadScenarios()
        {
            var assets = Resources.LoadAll<TextAsset>(ScenarioFolder);
            var parsed = new List<Scenario>();
            foreach (var asset in assets)
            {
                var s = Scenario.Parse(asset.text);
                if (s == null) Debug.LogError("Scenario file is not readable: " + asset.name);
                else parsed.Add(s);
            }
            var library = new ScenarioLibrary(parsed);
            foreach (var problem in library.Problems) Debug.LogError("Scenario problem: " + problem);
            return library;
        }

        /// <summary>
        /// Records a Completed Attempt, applies unlocks, and writes save.json before returning,
        /// so Level Completed only ever shows progress that is already saved.
        /// </summary>
        public static CompletionResult CompleteLevel(LevelId id, Attempt attempt)
        {
            var result = LevelCompletion.Complete(Progress, id, attempt, Saves);
            HasSaveFile = true;
            return result;
        }

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

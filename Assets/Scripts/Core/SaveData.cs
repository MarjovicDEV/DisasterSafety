using System;
using System.Collections.Generic;

namespace DisasterSafety.Core
{
    /// <summary>
    /// The JSON shape of save.json. Field names are the file format: rename only with a saveVersion bump.
    /// saveVersion defaults to 0 so that a file missing the field is treated as unreadable.
    /// </summary>
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentVersion = 1;

        public int saveVersion;
        public string displayName = string.Empty;
        public List<string> openEpisodes = new List<string>();
        /// <summary>Open Episode/Difficulty pairs, e.g. "Earthquake/Easy".</summary>
        public List<string> openDifficulties = new List<string>();
        public List<LevelRecord> completedLevels = new List<LevelRecord>();
        public string lastEpisode = string.Empty;
        public string lastDifficulty = string.Empty;
    }

    [Serializable]
    public sealed class LevelRecord
    {
        /// <summary>LevelId key, e.g. "Earthquake/Easy/2".</summary>
        public string level;
        /// <summary>Best stars earned, 0 to 3.</summary>
        public int stars;
    }
}

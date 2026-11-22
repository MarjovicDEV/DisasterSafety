using System;

namespace DisasterSafety.Core
{
    /// <summary>The Learner record: Display Name, Open Episodes and Difficulties, and Completed Levels.</summary>
    public sealed class ProgressState
    {
        public const int MaxStars = 3;

        public SaveData Data { get; }

        public ProgressState(SaveData data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            EnsureStartingContentOpen();
        }

        public static ProgressState NewGame() => new ProgressState(new SaveData { saveVersion = SaveData.CurrentVersion });

        public string DisplayName => Data.displayName ?? string.Empty;
        public bool HasDisplayName => !string.IsNullOrEmpty(Data.displayName);

        /// <summary>Saved Progress exists once at least one Level is a Completed Level.</summary>
        public bool HasSavedProgress => Data.completedLevels.Count > 0;

        public void SetDisplayName(string name)
        {
            if (DisplayNameRule.Check(name, out var normalized) != DisplayNameError.None)
                throw new ArgumentException("Display Name breaks DisplayNameRule", nameof(name));
            Data.displayName = normalized;
        }

        /// <summary>The Student the Learner picked; None until they pick one, and for any text we do not know.</summary>
        public StudentKind Student
        {
            get
            {
                switch (Data.student)
                {
                    case "boy": return StudentKind.Boy;
                    case "girl": return StudentKind.Girl;
                    default: return StudentKind.None;
                }
            }
        }

        public bool HasStudent => Student != StudentKind.None;

        /// <summary>Stores the pick. The caller saves, as with the Display Name.</summary>
        public void SetStudent(StudentKind kind)
        {
            Data.student = kind == StudentKind.Boy ? "boy" : kind == StudentKind.Girl ? "girl" : string.Empty;
        }

        public bool IsOpen(Episode episode) => Data.openEpisodes.Contains(episode.ToString());

        public bool IsOpen(Episode episode, Difficulty difficulty) =>
            IsOpen(episode) && Data.openDifficulties.Contains(Curriculum.Key(episode, difficulty));

        public void Open(Episode episode)
        {
            if (!IsOpen(episode)) Data.openEpisodes.Add(episode.ToString());
        }

        public void Open(Episode episode, Difficulty difficulty)
        {
            Open(episode);
            var key = Curriculum.Key(episode, difficulty);
            if (!Data.openDifficulties.Contains(key)) Data.openDifficulties.Add(key);
        }

        public bool IsCompleted(LevelId id) => Find(id) != null;

        public int StarsFor(LevelId id) => Find(id)?.stars ?? 0;

        /// <summary>Marks a Level as completed, keeping the best stars, and records it as Last Played.</summary>
        public void RecordCompletion(LevelId id, int stars)
        {
            stars = Math.Max(0, Math.Min(MaxStars, stars));
            var record = Find(id);
            if (record == null)
                Data.completedLevels.Add(new LevelRecord { level = id.Key, stars = stars });
            else
                record.stars = Math.Max(record.stars, stars);

            Data.lastEpisode = id.Episode.ToString();
            Data.lastDifficulty = id.Difficulty.ToString();
        }

        public bool TryGetLastPlayed(out Episode episode, out Difficulty difficulty)
        {
            difficulty = default;
            return Curriculum.TryParseEpisode(Data.lastEpisode, out episode)
                   && Curriculum.TryParseDifficulty(Data.lastDifficulty, out difficulty);
        }

        LevelRecord Find(LevelId id)
        {
            var key = id.Key;
            foreach (var r in Data.completedLevels)
                if (r.level == key) return r;
            return null;
        }

        /// <summary>The first Episode and its Easy Difficulty are always Open, whatever the file says.</summary>
        void EnsureStartingContentOpen() => Open(Curriculum.Episodes[0], Difficulty.Easy);
    }
}

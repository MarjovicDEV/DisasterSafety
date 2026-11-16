using System;
using System.Collections.Generic;

namespace DisasterSafety.Core
{
    /// <summary>One disaster topic. Order matters: it is the order shown to the Learner.</summary>
    public enum Episode
    {
        Earthquake,
        Fire,
        Typhoon,
        Flood,
        Landslide
    }

    /// <summary>A tier inside an Episode.</summary>
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    /// <summary>The fixed structure of the game: 5 Episodes x (3 + 5 + 7) Levels = 75 Levels.</summary>
    public static class Curriculum
    {
        public static readonly IReadOnlyList<Episode> Episodes = new[]
        {
            Episode.Earthquake, Episode.Fire, Episode.Typhoon, Episode.Flood, Episode.Landslide
        };

        public static readonly IReadOnlyList<Difficulty> Difficulties = new[]
        {
            Difficulty.Easy, Difficulty.Normal, Difficulty.Hard
        };

        public static int LevelCount(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy: return 3;
                case Difficulty.Normal: return 5;
                case Difficulty.Hard: return 7;
                default: throw new ArgumentOutOfRangeException(nameof(difficulty));
            }
        }

        public static IEnumerable<LevelId> AllLevels()
        {
            foreach (var e in Episodes)
            foreach (var d in Difficulties)
                for (var n = 1; n <= LevelCount(d); n++)
                    yield return new LevelId(e, d, n);
        }

        public static bool TryParseEpisode(string text, out Episode episode)
        {
            episode = default;
            if (string.IsNullOrEmpty(text)) return false;
            foreach (var e in Episodes)
            {
                if (e.ToString() == text)
                {
                    episode = e;
                    return true;
                }
            }
            return false;
        }

        public static bool TryParseDifficulty(string text, out Difficulty difficulty)
        {
            difficulty = default;
            if (string.IsNullOrEmpty(text)) return false;
            foreach (var d in Difficulties)
            {
                if (d.ToString() == text)
                {
                    difficulty = d;
                    return true;
                }
            }
            return false;
        }

        /// <summary>Key used in the save file for an Episode and Difficulty pair, e.g. "Earthquake/Easy".</summary>
        public static string Key(Episode episode, Difficulty difficulty) => episode + "/" + difficulty;

        public static bool TryParseKey(string key, out Episode episode, out Difficulty difficulty)
        {
            episode = default;
            difficulty = default;
            if (string.IsNullOrEmpty(key)) return false;
            var parts = key.Split('/');
            return parts.Length == 2 && TryParseEpisode(parts[0], out episode) && TryParseDifficulty(parts[1], out difficulty);
        }
    }

    /// <summary>Identifies one Level, e.g. Earthquake / Easy / 2.</summary>
    public readonly struct LevelId : IEquatable<LevelId>
    {
        public readonly Episode Episode;
        public readonly Difficulty Difficulty;
        public readonly int Number;

        public LevelId(Episode episode, Difficulty difficulty, int number)
        {
            if (number < 1 || number > Curriculum.LevelCount(difficulty))
                throw new ArgumentOutOfRangeException(nameof(number));
            Episode = episode;
            Difficulty = difficulty;
            Number = number;
        }

        public string Key => Episode + "/" + Difficulty + "/" + Number;

        public static bool TryParse(string key, out LevelId id)
        {
            id = default;
            if (string.IsNullOrEmpty(key)) return false;
            var parts = key.Split('/');
            if (parts.Length != 3) return false;
            if (!Curriculum.TryParseEpisode(parts[0], out var e)) return false;
            if (!Curriculum.TryParseDifficulty(parts[1], out var d)) return false;
            if (!int.TryParse(parts[2], out var n)) return false;
            if (n < 1 || n > Curriculum.LevelCount(d)) return false;
            id = new LevelId(e, d, n);
            return true;
        }

        public bool Equals(LevelId other) => Episode == other.Episode && Difficulty == other.Difficulty && Number == other.Number;
        public override bool Equals(object obj) => obj is LevelId other && Equals(other);
        public override int GetHashCode() => ((int)Episode * 16 + (int)Difficulty) * 16 + Number;
        public override string ToString() => Key;
    }
}

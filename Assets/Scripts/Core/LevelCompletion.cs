using System;
using System.Collections.Generic;

namespace DisasterSafety.Core
{
    /// <summary>What finishing a Level changed in the Learner record.</summary>
    public sealed class CompletionResult
    {
        public LevelId Level;
        /// <summary>Stars earned in this Attempt.</summary>
        public int Stars;
        /// <summary>Best Stars ever earned for this Level (what the save keeps).</summary>
        public int BestStars;
        public bool FirstCompletion;
        /// <summary>Every Level of this Difficulty is now a Completed Level.</summary>
        public bool DifficultyCompleted;
        /// <summary>The Episode's Hard Levels are all completed, which finishes the Episode; a Safety Reminder belongs here.</summary>
        public bool EpisodeCompleted;
        /// <summary>Show the Fire shortcut on Level Completed in place of the next Level.</summary>
        public bool OffersNextEpisode;
        /// <summary>What this completion opened, e.g. "Earthquake/Normal" or "Fire/Easy".</summary>
        public List<string> Opened = new List<string>();
    }

    /// <summary>
    /// Records a Completed Level, applies the unlock rules, and saves, in that order, so the
    /// save file already holds the result when Level Completed appears. There are no locks
    /// inside a Difficulty: completing all Easy Levels opens Normal, all Normal opens Hard, and
    /// all Hard completes the Episode and opens the next one.
    /// </summary>
    public static class LevelCompletion
    {
        /// <summary>Records and saves a Completed Attempt. Throws if the Attempt is not completed.</summary>
        public static CompletionResult Complete(ProgressState progress, LevelId id, Attempt attempt, SaveService saves)
        {
            if (attempt == null) throw new ArgumentNullException(nameof(attempt));
            if (attempt.Phase != AttemptPhase.Completed)
                throw new InvalidOperationException("Only a Completed Attempt can be recorded.");
            return Complete(progress, id, attempt.Stars, saves);
        }

        public static CompletionResult Complete(ProgressState progress, LevelId id, int stars, SaveService saves)
        {
            if (saves == null) throw new ArgumentNullException(nameof(saves));
            var result = Record(progress, id, stars);
            saves.Save(progress);
            return result;
        }

        /// <summary>Updates the Learner record without saving.</summary>
        public static CompletionResult Record(ProgressState progress, LevelId id, int stars)
        {
            if (progress == null) throw new ArgumentNullException(nameof(progress));
            stars = Math.Max(StarRule.MinStars, Math.Min(ProgressState.MaxStars, stars));

            var result = new CompletionResult
            {
                Level = id,
                Stars = stars,
                FirstCompletion = !progress.IsCompleted(id),
            };
            progress.RecordCompletion(id, stars);
            result.BestStars = progress.StarsFor(id);

            result.DifficultyCompleted = AllCompleted(progress, id.Episode, id.Difficulty);
            var earthquakeHard = id.Episode == Episode.Earthquake && id.Difficulty == Difficulty.Hard;
            result.OffersNextEpisode = earthquakeHard && !result.FirstCompletion
                && id.Number == Curriculum.LevelCount(Difficulty.Hard) && progress.IsOpen(Episode.Fire);
            if (!result.DifficultyCompleted) return result;

            if (id.Difficulty == Difficulty.Easy) OpenIfNew(progress, result, id.Episode, Difficulty.Normal);
            else if (id.Difficulty == Difficulty.Normal) OpenIfNew(progress, result, id.Episode, Difficulty.Hard);
            else
            {
                result.EpisodeCompleted = true;
                var next = (int)id.Episode + 1;
                if (next < Curriculum.Episodes.Count)
                {
                    OpenIfNew(progress, result, Curriculum.Episodes[next], Difficulty.Easy);
                    if (earthquakeHard && result.Opened.Count > 0) result.OffersNextEpisode = true;
                }
            }
            return result;
        }

        /// <summary>The following Level in the same Difficulty, if there is one.</summary>
        public static bool TryNext(LevelId id, out LevelId next)
        {
            next = default;
            if (id.Number >= Curriculum.LevelCount(id.Difficulty)) return false;
            next = new LevelId(id.Episode, id.Difficulty, id.Number + 1);
            return true;
        }

        static bool AllCompleted(ProgressState progress, Episode e, Difficulty d)
        {
            for (var n = 1; n <= Curriculum.LevelCount(d); n++)
                if (!progress.IsCompleted(new LevelId(e, d, n))) return false;
            return true;
        }

        static void OpenIfNew(ProgressState progress, CompletionResult result, Episode e, Difficulty d)
        {
            if (progress.IsOpen(e, d)) return;
            progress.Open(e, d);
            result.Opened.Add(Curriculum.Key(e, d));
        }
    }
}

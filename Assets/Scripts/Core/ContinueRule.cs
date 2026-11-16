using System.Collections.Generic;
using System.Linq;

namespace DisasterSafety.Core
{
    /// <summary>The set of Playable Levels in this build. Empty until scenario content exists.</summary>
    public sealed class PlayableLevels
    {
        public static readonly PlayableLevels None = new PlayableLevels();

        readonly HashSet<LevelId> levels;

        public PlayableLevels(params LevelId[] playable)
        {
            levels = new HashSet<LevelId>(playable);
        }

        public bool Any => levels.Count > 0;
        public bool IsPlayable(LevelId id) => levels.Contains(id);

        public bool AnyIn(Episode episode, Difficulty difficulty) =>
            levels.Any(level => level.Episode == episode && level.Difficulty == difficulty);
    }

    /// <summary>Continue is offered only when Saved Progress exists and at least one Playable Level exists.</summary>
    public static class ContinueRule
    {
        public static bool CanContinue(ProgressState progress, PlayableLevels content) =>
            progress != null && progress.HasSavedProgress && content != null && content.Any;

        /// <summary>Where Continue leads: Level Selection for the Last Played Episode and Difficulty.</summary>
        public static (Episode, Difficulty) Destination(ProgressState progress)
        {
            if (progress != null && progress.TryGetLastPlayed(out var e, out var d) && progress.IsOpen(e, d))
                return (e, d);
            return (Curriculum.Episodes[0], Difficulty.Easy);
        }
    }
}

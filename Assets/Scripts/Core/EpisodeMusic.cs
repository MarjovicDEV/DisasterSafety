using System;
using System.Collections.Generic;

namespace DisasterSafety.Core
{
    public enum MusicTrack
    {
        SomewhereSunny,
        EasyLemon,
        CherryBlossom,
        HappyAlley,
        PreludeAndAction,
        LightSting,
        CheeryMonday,
        BRoll,
        Machinations,
        PianoBetween
    }

    /// <summary>Music selection by Episode, Difficulty and the Level Completed panel.</summary>
    public static class EpisodeMusic
    {
        static readonly (MusicTrack Easy, MusicTrack Normal, MusicTrack Hard, MusicTrack Completion) SetA =
            (MusicTrack.CherryBlossom, MusicTrack.HappyAlley, MusicTrack.PreludeAndAction, MusicTrack.LightSting);
        static readonly (MusicTrack Easy, MusicTrack Normal, MusicTrack Hard, MusicTrack Completion) SetB =
            (MusicTrack.CheeryMonday, MusicTrack.BRoll, MusicTrack.Machinations, MusicTrack.PianoBetween);

        static readonly Dictionary<Episode, (MusicTrack Easy, MusicTrack Normal, MusicTrack Hard, MusicTrack Completion)> Sets =
            new Dictionary<Episode, (MusicTrack, MusicTrack, MusicTrack, MusicTrack)>
            {
                { Episode.Earthquake, SetA },
                { Episode.Fire, SetB },
                { Episode.Typhoon, SetA },
                { Episode.Flood, SetB },
                { Episode.Landslide, SetA }
            };

        public static readonly IReadOnlyList<MusicTrack> MenuPlaylist =
            Array.AsReadOnly(new[] { MusicTrack.SomewhereSunny, MusicTrack.EasyLemon });

        public static MusicTrack ForLevel(LevelId level)
        {
            var set = Sets[level.Episode];
            switch (level.Difficulty)
            {
                case Difficulty.Easy: return set.Easy;
                case Difficulty.Normal: return set.Normal;
                case Difficulty.Hard: return set.Hard;
                default: throw new ArgumentOutOfRangeException(nameof(level));
            }
        }

        public static MusicTrack ForCompletion(Episode episode) => Sets[episode].Completion;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;

namespace DisasterSafety.Core
{
    /// <summary>The supported Settings and their shaking capabilities.</summary>
    public static class ScenarioSettings
    {
        public const string Classroom = "classroom";
        public const string Corridor = "corridor";
        public const string ClassroomAfter = "classroom_after";
        public const string Bedroom = "bedroom";
        public const string Home = "home";
        public const string Schoolyard = "schoolyard";
        public const string Beach = "beach";
        public const string MallA = "mall_a";
        public const string MallB = "mall_b";
        public const string HomeAfter = "home_after";
        public const string EvacuationArea = "evacuation_area";

        public static IReadOnlyList<string> All { get; } =
            Array.AsReadOnly(new[]
            {
                Classroom, Corridor, ClassroomAfter, Bedroom, Home, Schoolyard, Beach,
                MallA, MallB, HomeAfter, EvacuationArea,
            });

        public static bool IsKnown(string key) => All.Contains(key);

        /// <summary>The art group the Student's flow uses for a Setting, e.g. "classroom" becomes "classroom_v2".</summary>
        public static string ActionArtKey(string setting) => setting + "_v2";

        /// <summary>The action spots a Scenario with actions may name in a Setting. Only the classroom has them so far.</summary>
        public static IReadOnlyList<string> ActionSpots(string setting) =>
            setting == Classroom ? ClassroomSpots : System.Array.Empty<string>();

        public static bool HasActionSpot(string setting, string action) => ActionSpots(setting).Contains(action);

        static readonly string[] ClassroomSpots = { "door", "desk", "window", "cabinet" };

        public static bool CanShake(string key) =>
            key == Classroom || key == Corridor || key == Home || key == Schoolyard || key == MallA;
    }
}

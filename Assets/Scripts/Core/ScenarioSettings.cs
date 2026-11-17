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

        public static bool CanShake(string key) =>
            key == Classroom || key == Corridor || key == Home || key == Schoolyard || key == MallA;
    }
}

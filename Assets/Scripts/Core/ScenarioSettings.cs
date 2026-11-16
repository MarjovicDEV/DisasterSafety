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

        public static IReadOnlyList<string> All { get; } = Array.AsReadOnly(new[] { Classroom, Corridor, ClassroomAfter });

        public static bool IsKnown(string key) => All.Contains(key);

        public static bool CanShake(string key) => key == Classroom || key == Corridor;
    }
}

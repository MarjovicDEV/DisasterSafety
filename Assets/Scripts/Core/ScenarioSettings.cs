using System;
using System.Collections.Generic;

namespace DisasterSafety.Core
{
    public static class ScenarioSettings
    {
        public const string Classroom = "classroom";
        public const string Corridor = "corridor";
        public const string ClassroomAfter = "classroom_after";

        public static IReadOnlyList<string> All { get; } = Array.AsReadOnly(new[] { Classroom, Corridor, ClassroomAfter });

        public static bool IsKnown(string setting) =>
            setting == Classroom || setting == Corridor || setting == ClassroomAfter;
    }
}

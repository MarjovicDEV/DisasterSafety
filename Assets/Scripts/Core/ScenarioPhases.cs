using System;
using System.Collections.Generic;
using System.Linq;

namespace DisasterSafety.Core
{
    /// <summary>The supported Phases for an authored Scenario.</summary>
    public static class ScenarioPhases
    {
        public const string Before = "before";
        public const string During = "during";
        public const string After = "after";

        public static IReadOnlyList<string> All { get; } = Array.AsReadOnly(new[] { Before, During, After });

        public static bool IsKnown(string phase) => All.Contains(phase);
    }
}

namespace DisasterSafety.Core
{
    public static class ScenarioPhases
    {
        public const string Before = "before";
        public const string During = "during";
        public const string After = "after";

        public static bool IsKnown(string phase) => phase == Before || phase == During || phase == After;
    }
}

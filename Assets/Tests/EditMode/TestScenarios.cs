using DisasterSafety.Core;

namespace DisasterSafety.Tests
{
    /// <summary>Small hand-built Scenarios for rule tests. Real content lives in Resources/Scenarios.</summary>
    static class TestScenarios
    {
        public const int Safe = 1;

        static TextLine Line(string text, params string[] sources) => new TextLine { text = text, sources = sources };

        static ScenarioChoice Choice(string id, bool safe) => new ScenarioChoice
        {
            id = id,
            label = "Label " + id,
            picture = "pic_" + id,
            safe = safe,
            feedback = "Feedback " + id + ".",
            sources = new[] { "EQ-10" },
        };

        /// <summary>Four Choices; index 1 is the Safe Choice.</summary>
        public static Scenario Classroom(string level = "Earthquake/Easy/1") => new Scenario
        {
            level = level,
            phase = "during",
            setting = "classroom",
            goal = Line("Stay safe.", "EQ-28"),
            intro = Line("It shakes!"),
            hint = Line("Hide under something strong.", "EQ-10"),
            lesson = Line("Duck, Cover, and Hold!", "EQ-09"),
            choices = new[] { Choice("run", false), Choice("desk", true), Choice("window", false), Choice("cabinet", false) },
        };
    }
}

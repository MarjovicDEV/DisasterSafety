using DisasterSafety.Core;

namespace DisasterSafety.Tests
{
    /// <summary>Small hand-built Scenarios for rule tests. Real content lives in Resources/Scenarios.</summary>
    static class TestScenarios
    {
        public const int Safe = 1;

        static TextLine Line(string text, params string[] sources) => new TextLine { text = text, sources = sources };

        static ScenarioChoice Choice(string id, string action, bool safe) => new ScenarioChoice
        {
            id = id,
            icon = "icon_" + action,
            action = action,
            label = "Label " + id,
            safe = safe,
            feedback = "Feedback " + id + ".",
            sources = new[] { "EQ-10" },
        };

        public static Scenario Classroom(string level = "Earthquake/Easy/1")
        {
            return WithChoices(level, 2);
        }

        public static Scenario WithChoices(string level, int count)
        {
            var s = new Scenario
            {
                level = level,
                phase = "during",
                setting = "classroom",
                goal = Line("Stay safe.", "EQ-28"),
                intro = Line("It shakes!"),
                hint = Line("Hide under something strong.", "EQ-10"),
                lesson = Line("Duck, Cover, and Hold!", "EQ-09"),
                choices = new ScenarioChoice[count],
            };
            for (var i = 0; i < count; i++)
            {
                s.choices[i] = i == Safe ? Choice("desk", "desk", true)
                    : Choice(i == 0 ? "run" : "other" + i, "door", false);
            }
            return s;
        }
    }
}

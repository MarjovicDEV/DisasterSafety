using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DisasterSafety.Core
{
    /// <summary>
    /// What every authored Scenario must satisfy before its Level counts as a Playable Level:
    /// a Level inside the Curriculum, 3 or 4 Choices with exactly one Safe Choice, Feedback for
    /// every Choice, a cited source for every safety line, and a known Setting and Phase.
    /// </summary>
    public static class ScenarioRules
    {
        public const int MinChoices = 3;
        public const int MaxChoices = 4;

        static readonly Regex SourceId = new Regex(@"^[A-Z]{2}-\d{2}$");

        public static List<string> Problems(Scenario s)
        {
            var problems = new List<string>();
            if (s == null)
            {
                problems.Add("Scenario is missing.");
                return problems;
            }

            if (!s.TryGetLevel(out _)) problems.Add("Level '" + s.level + "' is not in the Curriculum.");
            if (!ScenarioSettings.IsKnown(s.setting)) problems.Add("Setting '" + s.setting + "' has no scene art.");
            if (!ScenarioPhases.IsKnown(s.phase)) problems.Add("Phase '" + s.phase + "' must be before, during or after.");

            CheckLine(problems, s.goal, "goal", needsSource: true);
            CheckLine(problems, s.intro, "intro", needsSource: false);
            CheckLine(problems, s.hint, "Hint", needsSource: true);
            CheckLine(problems, s.lesson, "lesson", needsSource: true);

            var count = s.choices?.Length ?? 0;
            if (count < MinChoices || count > MaxChoices)
                problems.Add("A Scenario needs 3 or 4 Choices, not " + count + ".");

            var safe = 0;
            var ids = new HashSet<string>();
            for (var i = 0; i < count; i++)
            {
                var c = s.choices[i];
                if (c == null)
                {
                    problems.Add("Choice " + (i + 1) + " is empty.");
                    continue;
                }
                var name = string.IsNullOrWhiteSpace(c.id) ? "Choice " + (i + 1) : "Choice '" + c.id + "'";
                if (string.IsNullOrWhiteSpace(c.id)) problems.Add(name + " needs an id.");
                else if (!ids.Add(c.id)) problems.Add(name + ": Choice ids must be unique.");
                if (string.IsNullOrWhiteSpace(c.label)) problems.Add(name + " needs a label.");
                if (string.IsNullOrWhiteSpace(c.picture)) problems.Add(name + " needs a picture.");
                if (string.IsNullOrWhiteSpace(c.feedback)) problems.Add(name + " needs Feedback.");
                if (!HasSources(c.sources)) problems.Add(name + " needs a source for its Feedback.");
                if (c.safe) safe++;
            }
            if (safe != 1) problems.Add("A Scenario needs exactly one Safe Choice, not " + safe + ".");
            return problems;
        }

        static void CheckLine(List<string> problems, TextLine line, string name, bool needsSource)
        {
            if (line == null || string.IsNullOrWhiteSpace(line.text))
            {
                problems.Add("The " + name + " needs text.");
                return;
            }
            if (needsSource && !HasSources(line.sources)) problems.Add("The " + name + " needs a source.");
            else if (line.sources != null)
                foreach (var id in line.sources)
                    if (id == null || !SourceId.IsMatch(id)) problems.Add("The " + name + " has a bad source id '" + id + "'.");
        }

        static bool HasSources(string[] sources)
        {
            if (sources == null || sources.Length == 0) return false;
            foreach (var id in sources)
                if (id == null || !SourceId.IsMatch(id)) return false;
            return true;
        }
    }
}

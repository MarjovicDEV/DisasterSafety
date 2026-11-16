using System.Collections.Generic;

namespace DisasterSafety.Core
{
    /// <summary>
    /// The authored Scenarios in this build. Only a Scenario that passes ScenarioRules makes
    /// its Level a Playable Level; problems are kept for logging instead of reaching a Learner.
    /// </summary>
    public sealed class ScenarioLibrary
    {
        readonly Dictionary<LevelId, Scenario> byLevel = new Dictionary<LevelId, Scenario>();

        public static readonly ScenarioLibrary Empty = new ScenarioLibrary(new Scenario[0]);

        public List<string> Problems { get; } = new List<string>();
        public PlayableLevels Playable { get; }

        public ScenarioLibrary(IEnumerable<Scenario> scenarios)
        {
            foreach (var s in scenarios)
            {
                var problems = ScenarioRules.Problems(s);
                var name = s?.level ?? "(no level)";
                if (problems.Count > 0)
                {
                    foreach (var p in problems) Problems.Add(name + ": " + p);
                    continue;
                }
                s.TryGetLevel(out var id);
                if (byLevel.ContainsKey(id))
                {
                    Problems.Add(name + ": this Level has a Scenario more than once; the first one is used.");
                    continue;
                }
                byLevel.Add(id, s);
            }
            Playable = new PlayableLevels(new List<LevelId>(byLevel.Keys).ToArray());
        }

        public bool TryGet(LevelId id, out Scenario scenario) => byLevel.TryGetValue(id, out scenario);
    }
}

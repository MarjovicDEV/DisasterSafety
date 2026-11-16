using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    /// <summary>
    /// Checks the authored Scenario files the game ships, so a content mistake fails a test
    /// instead of reaching a Learner. Every safety line must cite a finding in the research notes.
    /// </summary>
    public class ScenarioContentTests
    {
        const string ScenarioFolder = "Assets/Resources/Scenarios";
        const string ResearchFolder = "docs/research";
        const string FontScript = "ArtSource/scripts/font.lua";
        const int MaxWordsPerSentence = 12;

        static IEnumerable<string> Files() =>
            Directory.Exists(ScenarioFolder)
                ? Directory.GetFiles(ScenarioFolder, "*.json", SearchOption.AllDirectories)
                : Enumerable.Empty<string>();

        static Scenario Load(string path)
        {
            var s = Scenario.Parse(File.ReadAllText(path));
            Assert.That(s, Is.Not.Null, path + " is not valid JSON");
            return s;
        }

        static IEnumerable<string> AllText(Scenario s)
        {
            yield return s.goal?.text;
            yield return s.intro?.text;
            yield return s.hint?.text;
            yield return s.lesson?.text;
            foreach (var c in s.choices ?? new ScenarioChoice[0])
            {
                yield return c.label;
                yield return c.heading;
                yield return c.feedback;
            }
        }

        static IEnumerable<string> AllSources(Scenario s)
        {
            var lines = new[] { s.goal, s.hint, s.lesson, s.intro }.Where(l => l?.sources != null).SelectMany(l => l.sources);
            var choices = (s.choices ?? new ScenarioChoice[0]).Where(c => c.sources != null).SelectMany(c => c.sources);
            return lines.Concat(choices);
        }

        [Test]
        public void FirstPlayableLevel_Exists()
        {
            var levels = Files().Select(Load).Select(s => s.level).ToList();
            Assert.That(levels, Does.Contain("Earthquake/Easy/1"));
        }

        [Test]
        public void EveryScenario_PassesTheScenarioRules()
        {
            foreach (var path in Files())
                Assert.That(ScenarioRules.Problems(Load(path)), Is.Empty, path);
        }

        [Test]
        public void EveryScenario_IsStoredUnderItsOwnLevelPath()
        {
            foreach (var path in Files())
            {
                var s = Load(path);
                var expected = Path.Combine(ScenarioFolder, s.level + ".json").Replace('\\', '/');
                Assert.That(path.Replace('\\', '/'), Is.EqualTo(expected));
            }
        }

        [Test]
        public void EverySource_IsAFindingInTheResearchNotes()
        {
            var notes = string.Join("\n", Directory.GetFiles(ResearchFolder, "*.md").Select(File.ReadAllText));
            var known = new HashSet<string>(Regex.Matches(notes, @"^- ([A-Z]{2}-\d{2}):", RegexOptions.Multiline)
                .Cast<Match>().Select(m => m.Groups[1].Value));
            Assert.That(known, Is.Not.Empty, "no findings found in " + ResearchFolder);

            foreach (var path in Files())
            foreach (var id in AllSources(Load(path)))
                Assert.That(known, Does.Contain(id), path + " cites an unknown finding");
        }

        [Test]
        public void EveryCharacter_IsInThePixelFont()
        {
            var glyphs = new HashSet<char>(Regex.Matches(File.ReadAllText(FontScript), "def\\(\"(.)\"")
                .Cast<Match>().Select(m => m.Groups[1].Value[0]));
            foreach (var path in Files())
            foreach (var text in AllText(Load(path)).Where(t => t != null))
            foreach (var ch in text)
                Assert.That(glyphs, Does.Contain(ch), path + ": '" + ch + "' in \"" + text + "\"");
        }

        [Test]
        public void Sentences_StayShortForYoungReaders()
        {
            foreach (var path in Files())
            foreach (var text in AllText(Load(path)).Where(t => !string.IsNullOrWhiteSpace(t)))
            foreach (var sentence in Regex.Split(text, @"(?<=[.!?])\s+"))
            {
                var words = sentence.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).Length;
                Assert.That(words, Is.LessThanOrEqualTo(MaxWordsPerSentence), path + ": \"" + sentence + "\"");
            }
        }

        [Test]
        public void SafetyWording_IsDuckCoverAndHold()
        {
            foreach (var path in Files())
            foreach (var text in AllText(Load(path)).Where(t => t != null))
            {
                // The slogan is "Duck, Cover, and Hold". "Hold on to it" alone is PHIVOLCS wording (EQ-11).
                Assert.That(text, Does.Not.Match(@"(?i)\bdrop\b"), path);
                Assert.That(text, Does.Not.Match(@"(?i)cover,? and hold on"), path);
            }
        }
    }
}

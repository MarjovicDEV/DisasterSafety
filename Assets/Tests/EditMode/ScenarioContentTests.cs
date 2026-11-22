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
        public void EasyLevels_AllExist()
        {
            var levels = Files().Select(Load).Select(s => s.level).ToList();
            Assert.That(levels, Is.SupersetOf(new[] { "Earthquake/Easy/1", "Earthquake/Easy/2", "Earthquake/Easy/3" }));
        }

        [Test]
        public void NormalLevels_AllExist()
        {
            var levels = Files().Select(Load).Select(s => s.level).ToList();
            Assert.That(levels, Is.SupersetOf(new[]
            {
                "Earthquake/Normal/1", "Earthquake/Normal/2", "Earthquake/Normal/3",
                "Earthquake/Normal/4", "Earthquake/Normal/5",
            }));
        }

        [Test]
        public void HardLevels_AllExist()
        {
            var levels = Files().Select(Load).Select(s => s.level).ToList();
            Assert.That(levels, Is.SupersetOf(new[]
            {
                "Earthquake/Hard/1", "Earthquake/Hard/2", "Earthquake/Hard/3", "Earthquake/Hard/4",
                "Earthquake/Hard/5", "Earthquake/Hard/6", "Earthquake/Hard/7",
            }));
        }

        [Test]
        public void EveryChoicePicture_HasArt()
        {
            foreach (var path in Files())
            foreach (var choice in Load(path).choices)
            {
                var picture = "Assets/Art/Level/" + choice.picture + ".png";
                Assert.That(File.Exists(picture), Is.True, path + ": missing " + picture);
            }
        }

        [Test]
        public void EveryNewFlowIcon_HasArt()
        {
            foreach (var path in Files())
            {
                var s = Load(path);
                if (!s.UsesActions) continue;
                foreach (var choice in s.choices)
                {
                    var icon = "Assets/Art/UI/" + choice.icon + ".png";
                    Assert.That(File.Exists(icon), Is.True, path + ": missing " + icon);
                }
            }
        }

        [Test]
        public void OnlyTheMovedLevels_UseTheStudentFlow()
        {
            var levels = Files().Select(Load).Where(s => s.UsesActions).Select(s => s.level).OrderBy(l => l).ToList();
            Assert.That(levels, Is.EqualTo(new[] { "Earthquake/Easy/1", "Earthquake/Easy/2", "Earthquake/Easy/3" }));
            foreach (var path in Files())
            {
                var s = Load(path);
                if (s.UsesActions) continue;
                Assert.That(s.ChoicesWithAction, Is.EqualTo(0), path + " must use the old flow, with no action on any Choice");
            }
        }

        [Test]
        public void EasyLevel2_UsesTheStudentFlow_InTheCorridor_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Easy/2.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.Corridor));
            Assert.That(ScenarioSettings.ActionSpots(ScenarioSettings.Corridor),
                Is.EquivalentTo(new[] { "stairs", "window", "cover", "plants" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(new[] { "stairs", "window", "cover", "plants" }));
            Assert.That(s.choices.Select(c => c.icon),
                Is.EqualTo(new[] { "icon_stairs", "icon_window", "icon_cover", "icon_plants" }));
            // The rollout changes no Scenario text (docs/specs/earthquake-rollout.md, Data).
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Run down the stairs", "Stand by the window", "Duck and cover your head", "Stand under the hanging plants",
            }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "Stay where you are while it shakes. Walk out when it stops.",
                "Window glass can break. Stay away from windows.",
                "Duck down low. Cover your head with your arms or a book.",
                "Hanging pots can swing and fall. Stay away from them.",
            }));
        }

        [Test]
        public void EasyLevel3_UsesTheStudentFlow_InTheClassroomAfter_WithItsTextUnchanged()
        {
            var s = Load(Path.Combine(ScenarioFolder, "Earthquake/Easy/3.json"));
            Assert.That(s.UsesActions, Is.True);
            Assert.That(s.phase, Is.EqualTo(ScenarioPhases.After));
            Assert.That(s.setting, Is.EqualTo(ScenarioSettings.ClassroomAfter));
            Assert.That(ScenarioSettings.ActionSpots(s.setting), Is.EquivalentTo(new[] { "stay", "walk", "push" }));
            Assert.That(s.choices.Select(c => c.action), Is.EqualTo(new[] { "stay", "walk", "push" }));
            Assert.That(s.choices.Select(c => c.icon), Is.EqualTo(new[] { "icon_stay", "icon_teacher_walk", "icon_push" }));
            Assert.That(s.SafeChoice.id, Is.EqualTo("walk"));
            Assert.That(s.choices.Select(c => c.picture), Is.EqualTo(new[] { "choice_stay", "choice_walk", "choice_push" }));
            Assert.That(s.choices.Select(c => c.label), Is.EqualTo(new[]
            {
                "Stay under the desk", "Walk out with your teacher", "Run and push to the door",
            }));
            Assert.That(s.choices.Select(c => c.heading ?? ""), Is.EqualTo(new[] { "Time to go", "", "" }));
            Assert.That(s.choices.Select(c => c.feedback), Is.EqualTo(new[]
            {
                "The shaking stopped. Now leave the classroom with your teacher.",
                "Walk out with your class. Follow your teacher to the evacuation area.",
                "Walk. Do not run. Do not push.",
            }));
            Assert.That(s.choices[0].sources, Is.EqualTo(new[] { "EQ-19", "EQ-34", "EQ-35" }));
            Assert.That(s.choices[1].sources, Is.EqualTo(new[] { "EQ-34", "EQ-35", "EQ-37" }));
            Assert.That(s.choices[2].sources, Is.EqualTo(new[] { "EQ-20", "EQ-36" }));
        }

        [Test]
        public void EverySetting_HasArt()
        {
            foreach (var path in Files())
            {
                var background = "Assets/Art/Level/bg_" + Load(path).setting + ".png";
                Assert.That(File.Exists(background), Is.True, path + ": missing " + background);
            }
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

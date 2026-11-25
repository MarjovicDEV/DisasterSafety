using System;
using System.IO;
using System.Linq;
using DisasterSafety.Core;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace DisasterSafety.Tests
{
    public class ScenarioTests
    {
        [Test]
        public void ValidScenario_HasNoProblems()
        {
            Assert.That(ScenarioRules.Problems(TestScenarios.Classroom()), Is.Empty);
        }

        [Test]
        public void UnknownSetting_IsAProblem()
        {
            var s = TestScenarios.Classroom();
            s.setting = "gym";
            Assert.That(ScenarioRules.Problems(s), Does.Contain("Setting 'gym' has no scene art."));
        }

        [TestCase(ScenarioSettings.Classroom, true)]
        [TestCase(ScenarioSettings.Corridor, true)]
        [TestCase(ScenarioSettings.Home, true)]
        [TestCase(ScenarioSettings.Schoolyard, true)]
        [TestCase(ScenarioSettings.MallA, true)]
        [TestCase(ScenarioSettings.MallB, false)]
        [TestCase(ScenarioSettings.HomeAfter, false)]
        [TestCase(ScenarioSettings.EvacuationArea, false)]
        [TestCase(ScenarioSettings.ClassroomAfter, false)]
        [TestCase(ScenarioSettings.Bedroom, false)]
        [TestCase(ScenarioSettings.Beach, false)]
        public void CanShake_OnlyForSettingsWithAShaker(string setting, bool shakes)
        {
            Assert.That(ScenarioSettings.CanShake(setting), Is.EqualTo(shakes));
        }

        [Test]
        public void UnknownPhase_IsAProblem()
        {
            var s = TestScenarios.Classroom();
            s.phase = "later";
            Assert.That(ScenarioRules.Problems(s), Does.Contain("Phase 'later' must be before, during or after."));
        }

        [Test]
        public void DuringScenario_WithStillSetting_IsAProblem()
        {
            var s = TestScenarios.Classroom();
            s.phase = ScenarioPhases.During;
            s.setting = ScenarioSettings.ClassroomAfter;
            Assert.That(ScenarioRules.Problems(s), Does.Contain("Setting 'classroom_after' cannot shake for a during Scenario."));
        }

        [Test]
        public void ScenarioWithoutActions_IsAProblem()
        {
            var s = TestScenarios.Classroom();
            foreach (var c in s.choices) c.action = string.Empty;
            Assert.That(s.UsesActions, Is.False);
            Assert.That(ScenarioRules.Problems(s), Does.Contain("Choice 'run' needs an action."));
        }

        [Test]
        public void ScenarioWithAnActionOnEveryChoice_IsValid()
        {
            var s = TestScenarios.Classroom();
            Assert.That(s.UsesActions, Is.True);
            Assert.That(ScenarioRules.Problems(s), Is.Empty);
        }

        [Test]
        public void OneChoiceWithoutAnAction_IsAProblem()
        {
            var s = TestScenarios.Classroom();
            s.choices[2].action = string.Empty;
            Assert.That(s.UsesActions, Is.False);
            Assert.That(ScenarioRules.Problems(s), Does.Contain("Choice 'window' needs an action."));
        }

        [Test]
        public void Choice_NeedsAnIcon()
        {
            var s = TestScenarios.Classroom();
            s.choices[1].icon = " ";
            Assert.That(ScenarioRules.Problems(s), Does.Contain("Choice 'desk' needs an icon."));
        }

        [Test]
        public void Action_MustBeAnActionSpotOfTheSetting()
        {
            var s = TestScenarios.Classroom();
            s.choices[3].action = "roof";
            Assert.That(ScenarioRules.Problems(s), Has.Some.Contains("action 'roof'"));

            var corridor = TestScenarios.Classroom();
            corridor.setting = ScenarioSettings.Corridor;
            Assert.That(ScenarioRules.Problems(corridor), Has.Some.Contains("is not an action spot in Setting 'corridor'"));
        }

        [TestCase("before", false)]
        [TestCase("during", true)]
        [TestCase("after", false)]
        public void OnlyDuringScenariosShake(string phase, bool shakes)
        {
            var s = TestScenarios.Classroom();
            s.phase = phase;
            Assert.That(s.Shakes, Is.EqualTo(shakes));
        }

        [Test]
        public void ExactlyOneSafeChoice_IsRequired()
        {
            var none = TestScenarios.Classroom();
            none.choices[1].safe = false;
            Assert.That(ScenarioRules.Problems(none), Has.Some.Contains("Safe Choice"));

            var two = TestScenarios.Classroom();
            two.choices[0].safe = true;
            Assert.That(ScenarioRules.Problems(two), Has.Some.Contains("Safe Choice"));
        }

        [Test]
        public void ThreeOrFourChoices_AreRequired()
        {
            var s = TestScenarios.Classroom();
            s.choices = s.choices.Take(2).ToArray();
            Assert.That(ScenarioRules.Problems(s), Has.Some.Contains("3 or 4"));
        }

        [Test]
        public void EveryChoice_NeedsFeedbackAndASource()
        {
            var s = TestScenarios.Classroom();
            s.choices[2].feedback = " ";
            s.choices[3].sources = new string[0];
            var problems = ScenarioRules.Problems(s);
            Assert.That(problems, Has.Some.Matches<string>(p => p.Contains("window") && p.Contains("Feedback")));
            Assert.That(problems, Has.Some.Matches<string>(p => p.Contains("cabinet") && p.Contains("source")));
        }

        [Test]
        public void GoalHintAndLesson_NeedSources()
        {
            var s = TestScenarios.Classroom();
            s.hint.sources = null;
            s.lesson.sources = new[] { "not an id" };
            var problems = ScenarioRules.Problems(s);
            Assert.That(problems, Has.Some.Contains("Hint"));
            Assert.That(problems, Has.Some.Contains("lesson"));
        }

        [Test]
        public void LevelOutsideTheCurriculum_IsAProblem()
        {
            var s = TestScenarios.Classroom("Earthquake/Easy/4");
            Assert.That(ScenarioRules.Problems(s), Has.Some.Contains("Level"));
        }

        [Test]
        public void ChoiceIds_MustBeUnique()
        {
            var s = TestScenarios.Classroom();
            s.choices[3].id = "run";
            Assert.That(ScenarioRules.Problems(s), Has.Some.Contains("unique"));
        }

        [Test]
        public void Parse_ReadsJson()
        {
            const string json = "{\"level\":\"Earthquake/Easy/1\",\"goal\":{\"text\":\"G\",\"sources\":[\"EQ-01\"]}," +
                                "\"choices\":[{\"id\":\"a\",\"label\":\"A\",\"safe\":true,\"feedback\":\"F\",\"sources\":[\"EQ-02\"]}]}";
            var s = Scenario.Parse(json);
            Assert.That(s.level, Is.EqualTo("Earthquake/Easy/1"));
            Assert.That(s.goal.text, Is.EqualTo("G"));
            Assert.That(s.choices, Has.Length.EqualTo(1));
            Assert.That(s.choices[0].safe, Is.True);
            Assert.That(s.TryGetLevel(out var id), Is.True);
            Assert.That(id, Is.EqualTo(new LevelId(Episode.Earthquake, Difficulty.Easy, 1)));
        }

        [Test]
        public void Parse_BrokenJson_ReturnsNull()
        {
            Assert.That(Scenario.Parse("{ nope"), Is.Null);
            Assert.That(Scenario.Parse(""), Is.Null);
        }

        [Test]
        public void SafeChoice_IsFound()
        {
            var s = TestScenarios.Classroom();
            Assert.That(s.SafeChoiceIndex, Is.EqualTo(TestScenarios.Safe));
            Assert.That(s.SafeChoice.id, Is.EqualTo("desk"));
        }

        [Test]
        public void Library_OnlyValidScenariosArePlayable()
        {
            var good = TestScenarios.Classroom();
            var bad = TestScenarios.Classroom("Earthquake/Easy/2");
            bad.choices[1].safe = false;
            var library = new ScenarioLibrary(new[] { good, bad });

            Assert.That(library.Playable.IsPlayable(new LevelId(Episode.Earthquake, Difficulty.Easy, 1)), Is.True);
            Assert.That(library.Playable.IsPlayable(new LevelId(Episode.Earthquake, Difficulty.Easy, 2)), Is.False);
            Assert.That(library.Problems, Has.Some.Contains("Earthquake/Easy/2"));
            Assert.That(library.TryGet(new LevelId(Episode.Earthquake, Difficulty.Easy, 1), out var found), Is.True);
            Assert.That(found, Is.SameAs(good));
        }

        [Test]
        public void Library_DuplicateLevel_KeepsTheFirstAndReportsIt()
        {
            var a = TestScenarios.Classroom();
            var b = TestScenarios.Classroom();
            var library = new ScenarioLibrary(new[] { a, b });
            Assert.That(library.TryGet(new LevelId(Episode.Earthquake, Difficulty.Easy, 1), out var found), Is.True);
            Assert.That(found, Is.SameAs(a));
            Assert.That(library.Problems, Has.Some.Contains("more than once"));
        }

        [Test]
        public void ContinueRule_BecomesTrue_AfterTheFirstCompletedLevel_WithLibraryContent()
        {
            var content = new ScenarioLibrary(new[] { TestScenarios.Classroom() }).Playable;
            var p = ProgressState.NewGame();
            Assert.That(ContinueRule.CanContinue(p, content), Is.False);
            LevelCompletion.Record(p, new LevelId(Episode.Earthquake, Difficulty.Easy, 1), 3);
            Assert.That(ContinueRule.CanContinue(p, content), Is.True);
        }

        // The final check of the Earthquake rollout (ticket 14): every Earthquake Level uses the Student
        // flow, and its Setting has a v2 LevelSetting in the Level scene that ProjectBuilder builds.
        static readonly string[] EarthquakeLevels =
        {
            "Earthquake/Easy/1", "Earthquake/Easy/2", "Earthquake/Easy/3",
            "Earthquake/Normal/1", "Earthquake/Normal/2", "Earthquake/Normal/3", "Earthquake/Normal/4", "Earthquake/Normal/5",
            "Earthquake/Hard/1", "Earthquake/Hard/2", "Earthquake/Hard/3", "Earthquake/Hard/4",
            "Earthquake/Hard/5", "Earthquake/Hard/6", "Earthquake/Hard/7",
        };

        [Test]
        public void EveryEarthquakeLevel_UsesTheStudentFlow_WithAV2Setting()
        {
            // LevelSetting lives in DisasterSafety.Game, which this test assembly does not reference.
            var settingType = Type.GetType("DisasterSafety.Game.UI.LevelSetting, DisasterSafety.Game");
            Assert.That(settingType, Is.Not.Null);
            var scene = EditorSceneManager.OpenScene("Assets/Scenes/Level.unity", OpenSceneMode.Additive);
            try
            {
                var keys = scene.GetRootGameObjects()
                    .SelectMany(root => root.GetComponentsInChildren(settingType, true))
                    .Select(setting => (string)settingType.GetField("key").GetValue(setting))
                    .ToArray();
                foreach (var level in EarthquakeLevels)
                {
                    var path = Path.Combine("Assets/Resources/Scenarios", level + ".json");
                    Assert.That(File.Exists(path), Is.True, level + " has a Scenario file");
                    var s = Scenario.Parse(File.ReadAllText(path));
                    Assert.That(s.UsesActions, Is.True, level + " uses the Student flow");
                    var key = ScenarioSettings.ActionArtKey(s.setting);
                    Assert.That(key, Does.EndWith("_v2"));
                    Assert.That(keys, Does.Contain(key), level + ": the Level scene has no v2 Setting '" + key + "'");
                }
            }
            finally
            {
                EditorSceneManager.CloseScene(scene, true);
            }
        }
    }
}

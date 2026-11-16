using System.Linq;
using DisasterSafety.Core;
using NUnit.Framework;

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
    }
}

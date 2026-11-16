using System.Linq;
using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class CurriculumTests
    {
        [Test]
        public void Episodes_AreTheFiveTopicsInManuscriptOrder()
        {
            Assert.That(Curriculum.Episodes, Is.EqualTo(new[]
            {
                Episode.Earthquake, Episode.Fire, Episode.Typhoon, Episode.Flood, Episode.Landslide
            }));
        }

        [TestCase(Difficulty.Easy, 3)]
        [TestCase(Difficulty.Normal, 5)]
        [TestCase(Difficulty.Hard, 7)]
        public void LevelCount_MatchesDifficulty(Difficulty difficulty, int expected)
        {
            Assert.That(Curriculum.LevelCount(difficulty), Is.EqualTo(expected));
        }

        [Test]
        public void AllLevels_Totals75()
        {
            Assert.That(Curriculum.AllLevels().Count(), Is.EqualTo(75));
        }

        [Test]
        public void LevelId_RoundTripsThroughItsKey()
        {
            var id = new LevelId(Episode.Flood, Difficulty.Hard, 7);
            Assert.That(LevelId.TryParse(id.Key, out var parsed), Is.True);
            Assert.That(parsed, Is.EqualTo(id));
        }

        [TestCase("Flood/Hard/8")]
        [TestCase("Flood/Hard/0")]
        [TestCase("Volcano/Easy/1")]
        [TestCase("Flood/Medium/1")]
        [TestCase("")]
        [TestCase(null)]
        public void LevelId_RejectsKeysOutsideTheCurriculum(string key)
        {
            Assert.That(LevelId.TryParse(key, out _), Is.False);
        }
    }
}

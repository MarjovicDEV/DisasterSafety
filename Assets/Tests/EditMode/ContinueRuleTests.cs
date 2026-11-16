using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class ContinueRuleTests
    {
        static readonly LevelId FirstLevel = new LevelId(Episode.Earthquake, Difficulty.Easy, 1);

        static ProgressState WithProgress()
        {
            var p = ProgressState.NewGame();
            p.SetDisplayName("Ana");
            p.RecordCompletion(FirstLevel, 1);
            return p;
        }

        [Test]
        public void NoSave_CannotContinue()
        {
            Assert.That(ContinueRule.CanContinue(null, new PlayableLevels(FirstLevel)), Is.False);
        }

        [Test]
        public void ValidProgress_WithoutPlayableContent_CannotContinue()
        {
            Assert.That(ContinueRule.CanContinue(WithProgress(), PlayableLevels.None), Is.False);
        }

        [Test]
        public void NameOnlySave_CannotContinue()
        {
            var p = ProgressState.NewGame();
            p.SetDisplayName("Ana");
            Assert.That(ContinueRule.CanContinue(p, new PlayableLevels(FirstLevel)), Is.False);
        }

        [Test]
        public void ValidProgress_WithPlayableContent_CanContinue()
        {
            Assert.That(ContinueRule.CanContinue(WithProgress(), new PlayableLevels(FirstLevel)), Is.True);
        }

        [Test]
        public void Destination_IsLastPlayedEpisodeAndDifficulty()
        {
            var p = WithProgress();
            p.RecordCompletion(new LevelId(Episode.Earthquake, Difficulty.Easy, 3), 2);
            var (e, d) = ContinueRule.Destination(p);
            Assert.That(e, Is.EqualTo(Episode.Earthquake));
            Assert.That(d, Is.EqualTo(Difficulty.Easy));
        }

        [Test]
        public void Destination_FallsBackToFirstOpenDifficulty()
        {
            var (e, d) = ContinueRule.Destination(ProgressState.NewGame());
            Assert.That(e, Is.EqualTo(Episode.Earthquake));
            Assert.That(d, Is.EqualTo(Difficulty.Easy));
        }
    }
}

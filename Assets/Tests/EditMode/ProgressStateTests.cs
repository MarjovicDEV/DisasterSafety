using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class ProgressStateTests
    {
        [Test]
        public void NewGame_OpensOnlyEarthquake()
        {
            var p = ProgressState.NewGame();
            foreach (var e in Curriculum.Episodes)
                Assert.That(p.IsOpen(e), Is.EqualTo(e == Episode.Earthquake), e.ToString());
        }

        [Test]
        public void NewGame_OpensOnlyEarthquakeEasy()
        {
            var p = ProgressState.NewGame();
            Assert.That(p.IsOpen(Episode.Earthquake, Difficulty.Easy), Is.True);
            Assert.That(p.IsOpen(Episode.Earthquake, Difficulty.Normal), Is.False);
            Assert.That(p.IsOpen(Episode.Earthquake, Difficulty.Hard), Is.False);
            Assert.That(p.IsOpen(Episode.Fire, Difficulty.Easy), Is.False);
        }

        [Test]
        public void NewGame_HasNoSavedProgressAndNoName()
        {
            var p = ProgressState.NewGame();
            Assert.That(p.HasSavedProgress, Is.False);
            Assert.That(p.HasDisplayName, Is.False);
        }

        [Test]
        public void DisplayNameAlone_IsNotSavedProgress()
        {
            var p = ProgressState.NewGame();
            p.SetDisplayName("Ana");
            Assert.That(p.HasDisplayName, Is.True);
            Assert.That(p.HasSavedProgress, Is.False);
        }

        [Test]
        public void CompletingALevel_CreatesSavedProgressAndLastPlayed()
        {
            var p = ProgressState.NewGame();
            p.RecordCompletion(new LevelId(Episode.Earthquake, Difficulty.Easy, 2), 2);

            Assert.That(p.HasSavedProgress, Is.True);
            Assert.That(p.TryGetLastPlayed(out var e, out var d), Is.True);
            Assert.That(e, Is.EqualTo(Episode.Earthquake));
            Assert.That(d, Is.EqualTo(Difficulty.Easy));
            Assert.That(p.StarsFor(new LevelId(Episode.Earthquake, Difficulty.Easy, 2)), Is.EqualTo(2));
        }

        [Test]
        public void RecordCompletion_KeepsBestStarsAndClampsToThree()
        {
            var id = new LevelId(Episode.Earthquake, Difficulty.Easy, 1);
            var p = ProgressState.NewGame();
            p.RecordCompletion(id, 3);
            p.RecordCompletion(id, 1);
            Assert.That(p.StarsFor(id), Is.EqualTo(3));

            var q = ProgressState.NewGame();
            q.RecordCompletion(id, 9);
            Assert.That(q.StarsFor(id), Is.EqualTo(3));
        }

        [Test]
        public void StarsFor_UnplayedLevel_IsZero()
        {
            Assert.That(ProgressState.NewGame().StarsFor(new LevelId(Episode.Fire, Difficulty.Hard, 7)), Is.EqualTo(0));
        }
    }
}

using System;
using System.IO;
using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class LevelCompletionTests
    {
        static LevelId Easy(int n) => new LevelId(Episode.Earthquake, Difficulty.Easy, n);
        static LevelId Normal(int n) => new LevelId(Episode.Earthquake, Difficulty.Normal, n);
        static LevelId Hard(int n) => new LevelId(Episode.Earthquake, Difficulty.Hard, n);

        string dir;

        [SetUp]
        public void SetUp()
        {
            dir = Path.Combine(Path.GetTempPath(), "ds-completion-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
        }

        [Test]
        public void Complete_SavesStarsAndCompletionToDisk_BeforeReturning()
        {
            var saves = new SaveService(dir);
            var p = ProgressState.NewGame();
            p.SetDisplayName("Ana");

            var result = LevelCompletion.Complete(p, Easy(1), 2, saves);

            Assert.That(result.Stars, Is.EqualTo(2));
            var reloaded = saves.Load();
            Assert.That(reloaded.Status, Is.EqualTo(SaveLoadStatus.Loaded));
            Assert.That(reloaded.Progress.IsCompleted(Easy(1)), Is.True);
            Assert.That(reloaded.Progress.StarsFor(Easy(1)), Is.EqualTo(2));
        }

        [Test]
        public void Replay_KeepsBestStars_AndReportsThem()
        {
            var p = ProgressState.NewGame();
            LevelCompletion.Record(p, Easy(1), 3);
            var replay = LevelCompletion.Record(p, Easy(1), 1);

            Assert.That(replay.Stars, Is.EqualTo(1), "this Attempt's Stars");
            Assert.That(replay.BestStars, Is.EqualTo(3));
            Assert.That(replay.FirstCompletion, Is.False);
            Assert.That(p.StarsFor(Easy(1)), Is.EqualTo(3));
        }

        [Test]
        public void FirstCompletion_IsReported()
        {
            var r = LevelCompletion.Record(ProgressState.NewGame(), Easy(1), 2);
            Assert.That(r.FirstCompletion, Is.True);
        }

        [Test]
        public void Completion_UpdatesLastPlayed()
        {
            var p = ProgressState.NewGame();
            LevelCompletion.Record(p, Easy(2), 1);
            Assert.That(p.TryGetLastPlayed(out var e, out var d), Is.True);
            Assert.That(e, Is.EqualTo(Episode.Earthquake));
            Assert.That(d, Is.EqualTo(Difficulty.Easy));
        }

        [Test]
        public void NoLocksInsideADifficulty_LevelTwoNeedsNothing()
        {
            // Every Level of an Open Difficulty may be started; completing Level 2 first is fine.
            var p = ProgressState.NewGame();
            var r = LevelCompletion.Record(p, Easy(2), 3);
            Assert.That(r.Opened, Is.Empty);
            Assert.That(p.IsCompleted(Easy(2)), Is.True);
            Assert.That(p.IsCompleted(Easy(1)), Is.False);
        }

        [Test]
        public void CompletingAllEasy_OpensNormal_OnlyOnTheLastOne()
        {
            var p = ProgressState.NewGame();
            Assert.That(LevelCompletion.Record(p, Easy(1), 3).Opened, Is.Empty);
            Assert.That(LevelCompletion.Record(p, Easy(3), 3).Opened, Is.Empty);
            Assert.That(p.IsOpen(Episode.Earthquake, Difficulty.Normal), Is.False);

            var last = LevelCompletion.Record(p, Easy(2), 1);
            Assert.That(last.DifficultyCompleted, Is.True);
            Assert.That(last.Opened, Is.EqualTo(new[] { "Earthquake/Normal" }));
            Assert.That(p.IsOpen(Episode.Earthquake, Difficulty.Normal), Is.True);
            Assert.That(p.IsOpen(Episode.Earthquake, Difficulty.Hard), Is.False);
        }

        [Test]
        public void ReplayingAfterEasyIsDone_OpensNothingNew()
        {
            var p = ProgressState.NewGame();
            for (var n = 1; n <= 3; n++) LevelCompletion.Record(p, Easy(n), 3);
            var again = LevelCompletion.Record(p, Easy(1), 3);
            Assert.That(again.Opened, Is.Empty);
        }

        [Test]
        public void CompletingAllNormal_OpensHard()
        {
            var p = ProgressState.NewGame();
            p.Open(Episode.Earthquake, Difficulty.Normal);
            CompletionResult r = null;
            for (var n = 1; n <= 5; n++) r = LevelCompletion.Record(p, Normal(n), 2);
            Assert.That(r.Opened, Is.EqualTo(new[] { "Earthquake/Hard" }));
            Assert.That(p.IsOpen(Episode.Earthquake, Difficulty.Hard), Is.True);
        }

        [Test]
        public void CompletingAllHard_CompletesTheEpisode_AndOpensTheNextEpisode()
        {
            var p = ProgressState.NewGame();
            p.Open(Episode.Earthquake, Difficulty.Hard);
            CompletionResult r = null;
            for (var n = 1; n <= 7; n++) r = LevelCompletion.Record(p, Hard(n), 2);
            Assert.That(r.EpisodeCompleted, Is.True);
            Assert.That(r.Opened, Is.EqualTo(new[] { "Fire/Easy" }));
            Assert.That(p.IsOpen(Episode.Fire, Difficulty.Easy), Is.True);
        }

        [TestCase(1)]
        [TestCase(4)]
        [TestCase(7)]
        public void NextEpisode_AppearsOnTheUnlockAndOnHardSevenReplay(int last)
        {
            var progress = ProgressState.NewGame();
            progress.Open(Episode.Earthquake, Difficulty.Hard);
            for (var n = 7; n >= 1; n--)
                if (n != last)
                    Assert.That(LevelCompletion.Record(progress, Hard(n), 2).OffersNextEpisode, Is.False);

            var unlock = LevelCompletion.Record(progress, Hard(last), 2);
            Assert.That(unlock.Opened, Is.EqualTo(new[] { "Fire/Easy" }));
            Assert.That(unlock.OffersNextEpisode, Is.True);
            for (var n = 1; n <= 7; n++)
            {
                var replay = LevelCompletion.Record(progress, Hard(n), 1);
                Assert.That(replay.Opened, Is.Empty);
                Assert.That(replay.OffersNextEpisode, Is.EqualTo(n == 7));
                Assert.That(replay.BestStars, Is.EqualTo(2));
            }
        }

        [Test]
        public void CompletingTheLastEpisode_OpensNothing()
        {
            var p = ProgressState.NewGame();
            p.Open(Episode.Landslide, Difficulty.Hard);
            CompletionResult r = null;
            for (var n = 1; n <= 7; n++) r = LevelCompletion.Record(p, new LevelId(Episode.Landslide, Difficulty.Hard, n), 3);
            Assert.That(r.EpisodeCompleted, Is.True);
            Assert.That(r.Opened, Is.Empty);
            Assert.That(r.OffersNextEpisode, Is.False);
            Assert.That(LevelCompletion.Record(p, r.Level, 3).OffersNextEpisode, Is.False);
        }

        [Test]
        public void AbandonedAttempt_SavesNothing()
        {
            var saves = new SaveService(dir);
            var p = ProgressState.NewGame();
            var a = new Attempt(TestScenarios.Classroom());
            a.StartChoosing();
            a.Choose(0);
            // The Learner quits here: nothing calls LevelCompletion.
            Assert.That(File.Exists(saves.SavePath), Is.False);
            Assert.That(p.HasSavedProgress, Is.False);
        }

        [Test]
        public void Complete_BeforeTheSafeChoice_IsRefused()
        {
            var saves = new SaveService(dir);
            var a = new Attempt(TestScenarios.Classroom());
            a.StartChoosing();
            Assert.Throws<InvalidOperationException>(() => LevelCompletion.Complete(ProgressState.NewGame(), Easy(1), a, saves));
            Assert.That(File.Exists(saves.SavePath), Is.False);
        }

        [Test]
        public void Complete_FromACompletedAttempt_UsesItsStars()
        {
            var saves = new SaveService(dir);
            var p = ProgressState.NewGame();
            var a = new Attempt(TestScenarios.Classroom());
            a.StartChoosing();
            a.Choose(0);
            a.TryAgain();
            a.Choose(TestScenarios.Safe);
            a.Complete();
            var r = LevelCompletion.Complete(p, Easy(1), a, saves);
            Assert.That(r.Stars, Is.EqualTo(2));
            Assert.That(saves.Load().Progress.StarsFor(Easy(1)), Is.EqualTo(2));
        }

        [Test]
        public void Next_IsTheFollowingLevelInTheSameDifficulty()
        {
            Assert.That(LevelCompletion.TryNext(Easy(1), out var next), Is.True);
            Assert.That(next, Is.EqualTo(Easy(2)));
            Assert.That(LevelCompletion.TryNext(Easy(3), out _), Is.False, "the last Easy Level has no Next");
        }

        static readonly PlayableLevels Earthquake = new PlayableLevels(
            Easy(1), Easy(2), Easy(3), Normal(1), Normal(2), Normal(3), Normal(4), Normal(5), Hard(1));

        [Test]
        public void LastLevelOfADifficulty_LeadsToTheNextDifficulty_OnlyWhenItIsOpenAndPlayable()
        {
            var p = ProgressState.NewGame();
            Assert.That(LevelCompletion.TryNextDifficulty(Easy(3), p, Earthquake, out _), Is.False, "Normal is not open yet");

            p.Open(Episode.Earthquake, Difficulty.Normal);
            Assert.That(LevelCompletion.TryNextDifficulty(Easy(3), p, Earthquake, out var next), Is.True);
            Assert.That(next, Is.EqualTo(Difficulty.Normal));
            Assert.That(LevelCompletion.TryNextDifficulty(Easy(2), p, Earthquake, out _), Is.False,
                "only the last Level of a Difficulty offers the next one; the others offer the Next Level");

            p.Open(Episode.Earthquake, Difficulty.Hard);
            Assert.That(LevelCompletion.TryNextDifficulty(Normal(5), p, Earthquake, out next), Is.True);
            Assert.That(next, Is.EqualTo(Difficulty.Hard));
            Assert.That(LevelCompletion.TryNextDifficulty(Normal(5), p, new PlayableLevels(Normal(5)), out _), Is.False,
                "a Difficulty with no Playable Level is not offered");
            Assert.That(LevelCompletion.TryNextDifficulty(Hard(7), p, Earthquake, out _), Is.False, "Hard is the last Difficulty");
        }
    }
}

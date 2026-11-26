using System.Collections.Generic;
using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class AudioPreferencesTests
    {
        sealed class MemoryStore : IPreferenceStore
        {
            public readonly Dictionary<string, int> Values = new Dictionary<string, int>();
            public int GetInt(string key, int fallback) => Values.TryGetValue(key, out var v) ? v : fallback;
            public void SetInt(string key, int value) => Values[key] = value;
            public void Flush() { }
        }

        [Test]
        public void Defaults_AreMusicAndSoundOn()
        {
            var prefs = new AudioPreferences(new MemoryStore());
            Assert.That(prefs.MusicOn, Is.True);
            Assert.That(prefs.SoundOn, Is.True);
        }


        [TestCase(Episode.Earthquake, MusicTrack.CherryBlossom, MusicTrack.HappyAlley, MusicTrack.PreludeAndAction, MusicTrack.LightSting)]
        [TestCase(Episode.Fire, MusicTrack.CheeryMonday, MusicTrack.BRoll, MusicTrack.Machinations, MusicTrack.PianoBetween)]
        [TestCase(Episode.Typhoon, MusicTrack.CherryBlossom, MusicTrack.HappyAlley, MusicTrack.PreludeAndAction, MusicTrack.LightSting)]
        [TestCase(Episode.Flood, MusicTrack.CheeryMonday, MusicTrack.BRoll, MusicTrack.Machinations, MusicTrack.PianoBetween)]
        [TestCase(Episode.Landslide, MusicTrack.CherryBlossom, MusicTrack.HappyAlley, MusicTrack.PreludeAndAction, MusicTrack.LightSting)]
        public void Episodes_SelectTracksByDifficultyAndCompletion(Episode episode,
            MusicTrack easy, MusicTrack normal, MusicTrack hard, MusicTrack completed)
        {
            Assert.That(EpisodeMusic.ForLevel(new LevelId(episode, Difficulty.Easy, 1)), Is.EqualTo(easy));
            Assert.That(EpisodeMusic.ForLevel(new LevelId(episode, Difficulty.Normal, 1)), Is.EqualTo(normal));
            Assert.That(EpisodeMusic.ForLevel(new LevelId(episode, Difficulty.Hard, 1)), Is.EqualTo(hard));
            Assert.That(EpisodeMusic.ForCompletion(episode), Is.EqualTo(completed));
        }

        [Test]
        public void MenuPlaylist_PutsSomewhereSunnyBeforeEasyLemon()
        {
            Assert.That(EpisodeMusic.MenuPlaylist, Is.EqualTo(new[] { MusicTrack.SomewhereSunny, MusicTrack.EasyLemon }));
        }

        [Test]
        public void Changes_AreStoredImmediately()
        {
            var store = new MemoryStore();
            var prefs = new AudioPreferences(store);
            prefs.MusicOn = false;
            prefs.SoundOn = false;

            var reloaded = new AudioPreferences(store);
            Assert.That(reloaded.MusicOn, Is.False);
            Assert.That(reloaded.SoundOn, Is.False);
        }

        [Test]
        public void Changed_IsRaisedOnlyOnRealChanges()
        {
            var prefs = new AudioPreferences(new MemoryStore());
            var count = 0;
            prefs.Changed += () => count++;
            prefs.MusicOn = true;
            prefs.MusicOn = false;
            Assert.That(count, Is.EqualTo(1));
        }
    }
}

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

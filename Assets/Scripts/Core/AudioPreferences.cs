using System;

namespace DisasterSafety.Core
{
    /// <summary>Small key/value store for device settings (PlayerPrefs in the game).</summary>
    public interface IPreferenceStore
    {
        int GetInt(string key, int fallback);
        void SetInt(string key, int value);
        void Flush();
    }

    /// <summary>
    /// Music and sound effect switches. Stored apart from save.json so that an unreadable
    /// save never resets the sound settings. Changes are written immediately.
    /// </summary>
    public sealed class AudioPreferences
    {
        public const string MusicKey = "audio.music";
        public const string SoundKey = "audio.sound";

        readonly IPreferenceStore store;
        bool musicOn;
        bool soundOn;

        public event Action Changed;

        public AudioPreferences(IPreferenceStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            musicOn = store.GetInt(MusicKey, 1) != 0;
            soundOn = store.GetInt(SoundKey, 1) != 0;
        }

        public bool MusicOn
        {
            get => musicOn;
            set => Set(ref musicOn, value, MusicKey);
        }

        public bool SoundOn
        {
            get => soundOn;
            set => Set(ref soundOn, value, SoundKey);
        }

        void Set(ref bool field, bool value, string key)
        {
            if (field == value) return;
            field = value;
            store.SetInt(key, value ? 1 : 0);
            store.Flush();
            Changed?.Invoke();
        }
    }
}

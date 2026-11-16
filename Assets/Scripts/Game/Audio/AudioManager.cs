using System.Collections;
using UnityEngine;

namespace DisasterSafety.Game
{
    public enum Sfx
    {
        Click,
        Back,
        Open,
        Confirm,
        Locked,
        Type,
        Toggle,
        Sparkle,
        Bark,
        Soon,
        Rumble,
        Safe,
        Unsafe,
        Star,
        Complete
    }

    /// <summary>Plays the menu music and UI sounds, following the Settings switches.</summary>
    public sealed class AudioManager : MonoBehaviour
    {
        const float MusicVolume = 0.42f;
        const float SfxVolume = 0.8f;

        static AudioManager instance;

        AudioLibrary library;
        AudioSource music;
        AudioSource effects;
        AudioSource pitched;
        Coroutine fade;

        public static void EnsureExists()
        {
            if (instance != null) return;
            GameServices.Initialize();
            var go = new GameObject("AudioManager");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<AudioManager>();
            instance.Setup();
        }

        public static void Play(Sfx sfx, float volume = 1f) => Play(sfx, volume, 1f);

        /// <summary>Plays a sound at a set pitch, e.g. a higher chime for each Star.</summary>
        public static void Play(Sfx sfx, float volume, float pitch)
        {
            EnsureExists();
            instance.PlayOneShot(sfx, volume, pitch);
        }

        void Setup()
        {
            library = Resources.Load<AudioLibrary>(AudioLibrary.ResourcePath);
            music = gameObject.AddComponent<AudioSource>();
            music.loop = true;
            music.playOnAwake = false;
            music.volume = 0f;
            effects = gameObject.AddComponent<AudioSource>();
            effects.playOnAwake = false;
            pitched = gameObject.AddComponent<AudioSource>();
            pitched.playOnAwake = false;

            if (library != null && library.menuMusic != null)
            {
                music.clip = library.menuMusic;
                music.Play();
            }
            GameServices.Audio.Changed += ApplyPreferences;
            ApplyPreferences();
        }

        void OnDestroy()
        {
            if (GameServices.Audio != null) GameServices.Audio.Changed -= ApplyPreferences;
            if (instance == this) instance = null;
        }

        void ApplyPreferences()
        {
            var target = GameServices.Audio.MusicOn ? MusicVolume : 0f;
            if (fade != null) StopCoroutine(fade);
            fade = StartCoroutine(FadeMusic(target, 0.6f));
        }

        IEnumerator FadeMusic(float target, float seconds)
        {
            var start = music.volume;
            for (var t = 0f; t < seconds; t += Time.unscaledDeltaTime)
            {
                music.volume = Mathf.Lerp(start, target, t / seconds);
                yield return null;
            }
            music.volume = target;
        }

        void PlayOneShot(Sfx sfx, float volume, float pitch)
        {
            if (!GameServices.Audio.SoundOn || library == null) return;
            var clip = library.Get(sfx);
            if (clip == null) return;
            // PlayOneShot follows the source's pitch, so a pitched sound gets its own source.
            if (!Mathf.Approximately(pitch, 1f))
            {
                pitched.pitch = pitch;
                pitched.PlayOneShot(clip, SfxVolume * volume);
                return;
            }
            effects.pitch = sfx == Sfx.Type || sfx == Sfx.Click ? Random.Range(0.94f, 1.06f) : 1f;
            effects.PlayOneShot(clip, SfxVolume * volume);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Soon,
        Rumble,
        Safe,
        Unsafe,
        Star,
        Complete,
        Opened,
        /// <summary>The Student's short voice blip under a Reaction or an Act-out line.</summary>
        Blip
    }

    /// <summary>Plays the menu and Level music and the UI sounds, following the Settings switches.</summary>
    public sealed class AudioManager : MonoBehaviour
    {
        const float MusicVolume = 0.42f;
        const float SfxVolume = 0.8f;
        /// <summary>The share of the music volume left while the ground shakes.</summary>
        const float DuckedShare = 0.35f;

        static AudioManager instance;

        AudioLibrary library;
        AudioSource music;
        AudioSource effects;
        AudioSource pitched;
        Coroutine fade;
        float duckUntil;
        bool ducked;
        bool swapping;

        public static void EnsureExists()
        {
            if (instance != null) return;
            GameServices.Initialize();
            var go = new GameObject("AudioManager");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<AudioManager>();
            instance.Setup();
        }

        /// <summary>The song playing now (or fading in), for tests.</summary>
        public static AudioClip CurrentMusic => instance != null ? instance.music.clip : null;

        public static void Play(Sfx sfx, float volume = 1f) => Play(sfx, volume, 1f);

        /// <summary>Plays a sound at a set pitch, e.g. a higher chime for each Star.</summary>
        public static void Play(Sfx sfx, float volume, float pitch)
        {
            EnsureExists();
            instance.PlayOneShot(sfx, volume, pitch);
        }

        /// <summary>How many voice blips actually played (sound on and the clip present), for tests.</summary>
        public static int BlipsPlayed { get; private set; }

        /// <summary>The Student's voice blip, quiet under the Safe and Unsafe sounds, at the speaker's pitch.</summary>
        public static void PlayBlip(float pitch)
        {
            EnsureExists();
            if (!GameServices.Audio.SoundOn || instance.library == null || instance.library.Get(Sfx.Blip) == null) return;
            BlipsPlayed++;
            instance.PlayOneShot(Sfx.Blip, 0.45f, pitch);
        }

        /// <summary>The blip for a Student: the girl's is pitched a little higher.</summary>
        public static void PlayBlip(DisasterSafety.Core.StudentKind kind) =>
            PlayBlip(kind == DisasterSafety.Core.StudentKind.Girl ? 1.25f : 1f);

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

            if (library != null)
            {
                music.clip = library.MusicFor(SceneManager.GetActiveScene().name);
                if (music.clip != null) music.Play();
            }
            SceneManager.activeSceneChanged += OnSceneChanged;
            GameServices.Audio.Changed += ApplyPreferences;
            ApplyPreferences();
        }

        /// <summary>Lowers the music for a while (the earthquake rumble), then brings it back.</summary>
        public static void DuckMusic(float seconds)
        {
            EnsureExists();
            instance.duckUntil = Time.unscaledTime + seconds;
            instance.ApplyPreferences();
        }

        void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            if (GameServices.Audio != null) GameServices.Audio.Changed -= ApplyPreferences;
            if (instance == this) instance = null;
        }

        // The menu and the Level have their own songs: fade out, swap, fade back in.
        void OnSceneChanged(Scene from, Scene to)
        {
            if (library == null) return;
            var clip = library.MusicFor(to.name);
            if (clip == null || clip == music.clip) return;
            duckUntil = 0f;
            if (fade != null) StopCoroutine(fade);
            fade = StartCoroutine(SwapMusic(clip));
        }

        IEnumerator SwapMusic(AudioClip clip)
        {
            swapping = true;
            yield return FadeMusic(0f, 0.4f);
            music.clip = clip;
            music.Play();
            yield return FadeMusic(TargetVolume, 0.8f);
            swapping = false;
            fade = null;
            ApplyPreferences();
        }

        // A missing preference (GameServices reset while a music swap is still fading) counts as music off.
        float TargetVolume => GameServices.Audio == null || !GameServices.Audio.MusicOn ? 0f
            : Time.unscaledTime < duckUntil ? MusicVolume * DuckedShare : MusicVolume;

        void ApplyPreferences()
        {
            if (swapping) return; // the swap fades to the current target when it ends
            if (fade != null) StopCoroutine(fade);
            fade = StartCoroutine(FadeMusic(TargetVolume, 0.6f));
        }

        void Update()
        {
            // The duck ends on its own: fade back up once its time is over.
            if (ducked && Time.unscaledTime >= duckUntil)
            {
                ducked = false;
                ApplyPreferences();
            }
            else if (!ducked && Time.unscaledTime < duckUntil) ducked = true;
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

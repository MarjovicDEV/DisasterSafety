using System.Collections;
using DisasterSafety.Core;
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

        enum MusicMode { Menu, Gameplay, Completed }

        static AudioManager instance;

        AudioLibrary library;
        AudioPreferences preferences;
        MusicMode mode;
        int menuTrack;
        AudioSource music;
        AudioSource effects;
        AudioSource pitched;
        Coroutine fade;
        float duckUntil;
        bool ducked;
        bool swapping;

        public static void EnsureExists()
        {
            if (instance != null)
            {
                instance.BindPreferences();
                return;
            }
            GameServices.Initialize();
            var go = new GameObject("AudioManager");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<AudioManager>();
            instance.Setup();
        }

        /// <summary>The song playing now (or fading in), for tests.</summary>
        public static AudioClip CurrentMusic => instance != null ? instance.music.clip : null;

        public static void PlayCompletionMusic(Episode episode)
        {
            EnsureExists();
            var clip = instance.library?.Get(EpisodeMusic.ForCompletion(episode));
            instance.SelectMusic(clip, MusicMode.Completed);
        }

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
            if (GameServices.Audio?.SoundOn != true || instance.library == null || instance.library.Get(Sfx.Blip) == null) return;
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
            music.playOnAwake = false;
            music.volume = 0f;
            effects = gameObject.AddComponent<AudioSource>();
            effects.playOnAwake = false;
            pitched = gameObject.AddComponent<AudioSource>();
            pitched.playOnAwake = false;

            SceneManager.activeSceneChanged += OnSceneChanged;
            BindPreferences();
            OnSceneChanged(default, SceneManager.GetActiveScene());
        }

        /// <summary>Lowers the music for a while (the earthquake rumble), then brings it back.</summary>
        public static void DuckMusic(float seconds)
        {
            EnsureExists();
            instance.duckUntil = Time.unscaledTime + seconds;
            instance.ducked = true;
            instance.ApplyPreferences();
        }

        void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            if (preferences != null) preferences.Changed -= ApplyPreferences;
            if (instance == this) instance = null;
        }

        void BindPreferences()
        {
            if (preferences == GameServices.Audio) return;
            if (preferences != null) preferences.Changed -= ApplyPreferences;
            preferences = GameServices.Audio;
            if (preferences != null) preferences.Changed += ApplyPreferences;
            ApplyPreferences();
        }

        void OnSceneChanged(Scene from, Scene to)
        {
            BindPreferences();
            if (library == null) return;

            if (to.name == GameScenes.Level)
                SelectMusic(library.Get(EpisodeMusic.ForLevel(GameServices.CurrentLevel)), MusicMode.Gameplay);
            else if (mode != MusicMode.Menu || music.clip == null)
                SelectMusic(MenuClip, MusicMode.Menu);
        }

        AudioClip MenuClip => library.Get(EpisodeMusic.MenuPlaylist[menuTrack]);

        void SelectMusic(AudioClip clip, MusicMode next)
        {
            if (next == mode && clip == music.clip && !swapping) return;
            var canFadeOut = mode != MusicMode.Completed && next != MusicMode.Completed
                && music.clip != null && clip != null;
            mode = next;
            duckUntil = 0f;
            ducked = false;
            if (fade != null) StopCoroutine(fade);

            if (canFadeOut)
            {
                swapping = true;
                fade = StartCoroutine(SwapMusic(clip));
            }
            else
            {
                swapping = false;
                // A completion cue belongs to its panel. Stop it immediately when leaving or replaying.
                music.volume = 0f;
                PlayMusic(clip);
                fade = StartCoroutine(FadeMusic(0.6f));
            }
        }

        IEnumerator SwapMusic(AudioClip clip)
        {
            yield return FadeMusic(0.4f, fadeOut: true);
            PlayMusic(clip);
            yield return FadeMusic(0.8f);
            swapping = false;
            fade = null;
        }

        void PlayMusic(AudioClip clip)
        {
            music.Stop();
            music.clip = clip;
            music.loop = mode == MusicMode.Gameplay;
            if (clip != null) music.Play();
        }

        float TargetVolume => GameServices.Audio?.MusicOn != true ? 0f
            : Time.unscaledTime < duckUntil ? MusicVolume * DuckedShare : MusicVolume;

        void ApplyPreferences()
        {
            if (swapping) return; // The swap reads the latest preference as it fades in.
            if (fade != null) StopCoroutine(fade);
            fade = StartCoroutine(FadeMusic(0.6f));
        }

        void Update()
        {
            if (ducked && Time.unscaledTime >= duckUntil)
            {
                ducked = false;
                ApplyPreferences();
            }

            if (mode == MusicMode.Menu && !swapping && music.clip != null && !music.isPlaying)
            {
                menuTrack = (menuTrack + 1) % EpisodeMusic.MenuPlaylist.Count;
                PlayMusic(MenuClip);
            }
        }

        IEnumerator FadeMusic(float seconds, bool fadeOut = false)
        {
            var start = music.volume;
            for (var t = 0f; t < seconds; t += Time.unscaledDeltaTime)
            {
                music.volume = Mathf.Lerp(start, fadeOut ? 0f : TargetVolume, t / seconds);
                yield return null;
            }
            music.volume = fadeOut ? 0f : TargetVolume;
        }

        void PlayOneShot(Sfx sfx, float volume, float pitch)
        {
            if (GameServices.Audio?.SoundOn != true || library == null) return;
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

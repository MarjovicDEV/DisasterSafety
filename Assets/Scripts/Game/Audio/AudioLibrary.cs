using UnityEngine;

namespace DisasterSafety.Game
{
    /// <summary>All clips in one asset at Resources/AudioLibrary, filled by the Editor build step.</summary>
    [CreateAssetMenu(menuName = "Disaster Safety/Audio Library")]
    public sealed class AudioLibrary : ScriptableObject
    {
        public const string ResourcePath = "AudioLibrary";

        public AudioClip menuMusic;
        /// <summary>The calmer loop for the Level scene.</summary>
        public AudioClip levelMusic;
        public AudioClip click;
        public AudioClip back;
        public AudioClip open;
        public AudioClip confirm;
        public AudioClip locked;
        public AudioClip type;
        public AudioClip toggle;
        public AudioClip sparkle;
        public AudioClip soon;
        public AudioClip rumble;
        public AudioClip safe;
        public AudioClip unsafeChoice;
        public AudioClip star;
        public AudioClip complete;
        public AudioClip opened;
        /// <summary>The Student's voice blip under a Reaction.</summary>
        public AudioClip blip;

        /// <summary>The music for a scene: the Level loop in the Level scene (when it exists), else the menu loop.</summary>
        public AudioClip MusicFor(string sceneName) =>
            sceneName == GameScenes.Level && levelMusic != null ? levelMusic : menuMusic;

        public AudioClip Get(Sfx sfx)
        {
            switch (sfx)
            {
                case Sfx.Click: return click;
                case Sfx.Back: return back;
                case Sfx.Open: return open;
                case Sfx.Confirm: return confirm;
                case Sfx.Locked: return locked;
                case Sfx.Type: return type;
                case Sfx.Toggle: return toggle;
                case Sfx.Sparkle: return sparkle;
                case Sfx.Soon: return soon;
                case Sfx.Rumble: return rumble;
                case Sfx.Safe: return safe;
                case Sfx.Unsafe: return unsafeChoice;
                case Sfx.Star: return star;
                case Sfx.Complete: return complete;
                case Sfx.Opened: return opened;
                case Sfx.Blip: return blip;
                default: return null;
            }
        }
    }
}

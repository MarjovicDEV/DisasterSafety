using UnityEngine;

namespace DisasterSafety.Game
{
    /// <summary>All clips in one asset at Resources/AudioLibrary, filled by the Editor build step.</summary>
    [CreateAssetMenu(menuName = "Disaster Safety/Audio Library")]
    public sealed class AudioLibrary : ScriptableObject
    {
        public const string ResourcePath = "AudioLibrary";

        public AudioClip menuMusic;
        public AudioClip click;
        public AudioClip back;
        public AudioClip open;
        public AudioClip confirm;
        public AudioClip locked;
        public AudioClip type;
        public AudioClip toggle;
        public AudioClip sparkle;
        public AudioClip bark;
        public AudioClip soon;
        public AudioClip rumble;
        public AudioClip safe;
        public AudioClip unsafeChoice;
        public AudioClip star;
        public AudioClip complete;
        public AudioClip opened;

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
                case Sfx.Bark: return bark;
                case Sfx.Soon: return soon;
                case Sfx.Rumble: return rumble;
                case Sfx.Safe: return safe;
                case Sfx.Unsafe: return unsafeChoice;
                case Sfx.Star: return star;
                case Sfx.Complete: return complete;
                case Sfx.Opened: return opened;
                default: return null;
            }
        }
    }
}

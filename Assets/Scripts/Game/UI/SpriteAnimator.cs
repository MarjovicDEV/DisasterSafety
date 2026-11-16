using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Loops a list of sprites on a UI Image.</summary>
    [RequireComponent(typeof(Image))]
    public sealed class SpriteAnimator : MonoBehaviour
    {
        public Sprite[] frames;
        public float framesPerSecond = 8f;
        public bool randomStart = true;

        Image image;
        float time;

        void Awake()
        {
            image = GetComponent<Image>();
            if (randomStart) time = Random.value * 10f;
        }

        /// <summary>Stops on the first frame (used when the shaking calms down).</summary>
        public void Rest()
        {
            enabled = false;
            if (image == null) image = GetComponent<Image>();
            if (frames != null && frames.Length > 0) image.sprite = frames[0];
        }

        void Update()
        {
            if (frames == null || frames.Length == 0 || framesPerSecond <= 0f) return;
            time += Time.deltaTime;
            var index = Mathf.FloorToInt(time * framesPerSecond) % frames.Length;
            if (image.sprite != frames[index]) image.sprite = frames[index];
        }
    }
}

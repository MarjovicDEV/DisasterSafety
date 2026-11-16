using UnityEngine;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// Gently shakes the scene picture, never the buttons or text: a short shake when the
    /// Level opens, then a faint wobble until the Learner makes a Choice. Offsets snap to whole
    /// art pixels (4 UI units) so the pixel art stays crisp.
    /// </summary>
    public sealed class SceneShaker : MonoBehaviour
    {
        public RectTransform target;
        [Tooltip("Strongest offset in UI units (2 art pixels).")]
        public float strength = 8f;
        public float shakeSeconds = 2f;
        [Tooltip("Offset of the faint wobble after the shake, in UI units.")]
        public float wobble = 4f;
        [Tooltip("Things that wobble only while the ground shakes (fan, cabinet).")]
        public SpriteAnimator[] wobblers;

        Vector2 rest;
        float started = -1f;
        bool wobbling;

        public bool IsShaking => started >= 0f;

        void Awake()
        {
            if (target == null) target = (RectTransform)transform;
            rest = target.anchoredPosition;
        }

        public void StartShake()
        {
            started = Time.time;
            wobbling = true;
            SetWobblers(true);
        }

        void SetWobblers(bool on)
        {
            if (wobblers == null) return;
            foreach (var w in wobblers)
            {
                if (w == null) continue;
                if (on) w.enabled = true;
                else w.Rest();
            }
        }

        /// <summary>Back to the faint wobble (after Try Again), without the strong shake.</summary>
        public void ResumeWobble()
        {
            started = Time.time - shakeSeconds;
            wobbling = true;
            SetWobblers(true);
        }

        /// <summary>The shaking calms down once a Choice is made.</summary>
        public void Calm()
        {
            started = -1f;
            wobbling = false;
            target.anchoredPosition = rest;
            SetWobblers(false);
        }

        void Update()
        {
            if (started < 0f) return;
            var t = Time.time - started;
            float amount;
            if (t < shakeSeconds)
            {
                // ease in over 0.3 s, ease out to the wobble level at the end
                var k = Mathf.Clamp01(t / 0.3f) * Mathf.Lerp(1f, wobble / strength, Mathf.Clamp01((t - shakeSeconds + 0.6f) / 0.6f));
                amount = strength * k;
            }
            else if (wobbling)
            {
                amount = wobble * (0.6f + 0.4f * Mathf.Sin(t * 1.3f));
            }
            else
            {
                amount = 0f;
            }

            var x = Mathf.Sin(t * 31f) * amount;
            var y = Mathf.Sin(t * 23f + 1.7f) * amount * 0.5f;
            target.anchoredPosition = rest + new Vector2(Mathf.Round(x / 4f) * 4f, Mathf.Round(y / 4f) * 4f);
        }
    }
}

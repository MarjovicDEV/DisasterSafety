using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Level Completed's Stars: each earned Star pops in with a rising chime and sparkles.</summary>
    public sealed class StarReveal : MonoBehaviour
    {
        public Image[] stars = new Image[3];
        public Sprite starOn;
        public Sprite starOff;
        public float delay = 0.45f;

        public int Shown { get; private set; }
        public bool Done { get; private set; }

        Coroutine running;

        public void Play(int earned)
        {
            if (running != null) StopCoroutine(running);
            running = StartCoroutine(Reveal(earned));
        }

        /// <summary>Shows the result at once (used by screenshots and when the screen closes early).</summary>
        public void ShowInstant(int earned)
        {
            if (running != null) StopCoroutine(running);
            for (var i = 0; i < stars.Length; i++)
            {
                stars[i].sprite = i < earned ? starOn : starOff;
                stars[i].rectTransform.localScale = Vector3.one;
            }
            Shown = earned;
            Done = true;
        }

        IEnumerator Reveal(int earned)
        {
            Done = false;
            Shown = 0;
            foreach (var s in stars)
            {
                s.sprite = starOff;
                s.rectTransform.localScale = Vector3.one;
            }
            yield return new WaitForSecondsRealtime(0.35f);
            for (var i = 0; i < earned && i < stars.Length; i++)
            {
                var r = stars[i].rectTransform;
                stars[i].sprite = starOn;
                AudioManager.Play(Sfx.Star, 0.9f, 1f + i * 0.12f);
                Juice.Sparkles(r, 8);
                for (var t = 0f; t < 0.3f; t += Time.unscaledDeltaTime)
                {
                    var k = t / 0.3f;
                    r.localScale = Vector3.one * Mathf.Max(0.01f, Ease.OutBack(k) * 1.0f + (1f - k) * 0.25f);
                    yield return null;
                }
                r.localScale = Vector3.one;
                Shown = i + 1;
                yield return new WaitForSecondsRealtime(delay);
            }
            Done = true;
            running = null;
        }
    }
}

using System.Collections;
using UnityEngine;

namespace DisasterSafety.Game.UI
{
    /// <summary>A menu screen or dialog that animates in and out.</summary>
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class ScreenPanel : MonoBehaviour
    {
        public enum Style
        {
            Fade,
            Pop,
            Slide
        }

        public Style style = Style.Fade;
        [Tooltip("The part that pops or slides. Defaults to this object.")]
        public RectTransform content;

        CanvasGroup group;
        Vector2 restPosition;
        Coroutine running;
        bool visible;
        bool ready;

        public bool IsVisible => visible;

        void Init()
        {
            if (ready) return;
            ready = true;
            group = GetComponent<CanvasGroup>();
            if (content == null) content = (RectTransform)transform;
            restPosition = content.anchoredPosition;
        }

        public void SetInstant(bool show)
        {
            Init();
            if (running != null) StopCoroutine(running);
            running = null;
            visible = show;
            gameObject.SetActive(show);
            group.alpha = show ? 1f : 0f;
            group.interactable = show;
            group.blocksRaycasts = show;
            content.localScale = Vector3.one;
            content.anchoredPosition = restPosition;
        }

        public void Show()
        {
            Init();
            if (visible) return;
            visible = true;
            gameObject.SetActive(true);
            Run(Animate(true));
        }

        public void Hide()
        {
            Init();
            if (!visible) return;
            if (!gameObject.activeInHierarchy)
            {
                SetInstant(false);
                return;
            }
            visible = false;
            Run(Animate(false));
        }

        /// <summary>Visible but not clickable, e.g. the menu behind a dialog.</summary>
        public void SetInteractable(bool value)
        {
            Init();
            group.interactable = value && visible;
            group.blocksRaycasts = value && visible;
        }

        void Run(IEnumerator routine)
        {
            if (running != null) StopCoroutine(running);
            running = StartCoroutine(routine);
        }

        IEnumerator Animate(bool show)
        {
            group.interactable = false;
            group.blocksRaycasts = show;
            var duration = show ? 0.3f : 0.15f;
            for (var t = 0f; t < duration; t += Time.unscaledDeltaTime)
            {
                Apply(show ? t / duration : 1f - t / duration, show);
                yield return null;
            }
            Apply(show ? 1f : 0f, show);
            group.interactable = show;
            if (!show) gameObject.SetActive(false);
            running = null;
        }

        void Apply(float k, bool showing)
        {
            group.alpha = Mathf.Clamp01(k * 1.6f);
            switch (style)
            {
                case Style.Pop:
                    var s = showing ? Ease.OutBack(k) : k;
                    content.localScale = Vector3.one * Mathf.LerpUnclamped(0.7f, 1f, s);
                    break;
                case Style.Slide:
                    var e = showing ? Ease.OutBack(k) : k;
                    content.anchoredPosition = restPosition + new Vector2(0f, Mathf.LerpUnclamped(-90f, 0f, e));
                    break;
            }
        }
    }

    public static class Ease
    {
        public static float OutBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            t = Mathf.Clamp01(t) - 1f;
            return 1f + c3 * t * t * t + c1 * t * t;
        }
    }
}

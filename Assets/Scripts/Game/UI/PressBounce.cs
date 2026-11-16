using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// Makes a button feel physical: it squashes when pressed, springs back with a wobble,
    /// moves its label down with the pressed sprite, and plays a sound.
    /// </summary>
    public sealed class PressBounce : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Tooltip("Label and icon holder that moves down while pressed.")]
        public RectTransform face;
        [Tooltip("Art pixels the pressed sprite drops by, times 4.")]
        public float pressDepth = 12f;
        public Sfx sound = Sfx.Click;
        [Tooltip("Gentle idle pulse to draw the eye (use on PLAY only).")]
        public bool pulse;

        Selectable selectable;
        Vector2 faceRest;
        float scale = 1f;
        float velocity;
        float target = 1f;
        bool pressed;

        void Awake()
        {
            selectable = GetComponent<Selectable>();
            if (face != null) faceRest = face.anchoredPosition;
        }

        bool Interactable => selectable == null || selectable.IsInteractable();

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable) return;
            pressed = true;
            target = 0.93f;
            if (face != null) face.anchoredPosition = faceRest + new Vector2(0f, -pressDepth);
        }

        public void OnPointerUp(PointerEventData eventData) => Release();
        public void OnPointerExit(PointerEventData eventData) => Release();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!Interactable) return;
            AudioManager.Play(sound);
            velocity += 2.5f; // pop out a little on release
        }

        void Release()
        {
            if (!pressed) return;
            pressed = false;
            target = 1f;
            if (face != null) face.anchoredPosition = faceRest;
        }

        void OnDisable()
        {
            pressed = false;
            target = 1f;
            scale = 1f;
            velocity = 0f;
            transform.localScale = Vector3.one;
            if (face != null) face.anchoredPosition = faceRest;
        }

        void Update()
        {
            var dt = Mathf.Min(Time.unscaledDeltaTime, 0.05f);
            // damped spring toward target
            var force = (target - scale) * 380f - velocity * 18f;
            velocity += force * dt;
            scale += velocity * dt;
            var s = scale;
            if (pulse && !pressed && Interactable) s *= 1f + 0.035f * Mathf.Sin(Time.unscaledTime * 3.2f);
            transform.localScale = new Vector3(s, s, 1f);
        }
    }
}

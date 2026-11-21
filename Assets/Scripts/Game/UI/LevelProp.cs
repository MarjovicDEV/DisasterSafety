using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// A small thing in a Setting that an Act-out can show, hide, pick up, put down or slide (a box, a go
    /// bag, a bed). It remembers where it started, so Try Again can put it back while the Student walks
    /// back to the start. Everything follows Time.timeScale.
    /// </summary>
    public sealed class LevelProp : MonoBehaviour
    {
        public string key;
        public Image image;
        [Tooltip("Where the prop sits while the Student holds it: its anchored position in the Student, UI units.")]
        public Vector2 holdOffset = new Vector2(40f, 72f);

        Transform startParent;
        Vector2 startPosition;
        Vector2 startAnchorMin;
        Vector2 startAnchorMax;
        bool startVisible;
        int startSibling;
        bool captured;
        // Picked up or put down since the start: Try Again fades it back rather than sliding it across the room.
        bool carried;

        RectTransform Rect => (RectTransform)transform;

        /// <summary>True when the prop is where it started and shows as it did.</summary>
        public bool AtStart => !captured || (transform.parent == startParent && Rect.anchoredPosition == startPosition
                                             && gameObject.activeSelf == startVisible && Alpha == 1f);

        float Alpha => image != null ? image.color.a : 1f;

        /// <summary>Remembers the current place and visibility as the start. Call once when the Level starts.</summary>
        public void CaptureStart()
        {
            startParent = transform.parent;
            startPosition = Rect.anchoredPosition;
            startAnchorMin = Rect.anchorMin;
            startAnchorMax = Rect.anchorMax;
            startVisible = gameObject.activeSelf;
            startSibling = transform.GetSiblingIndex();
            captured = true;
            carried = false;
        }

        public void Show() { EnsureCaptured(); gameObject.SetActive(true); }

        public void Hide() { EnsureCaptured(); gameObject.SetActive(false); }

        /// <summary>Moves into the holder's hands (as a child of the holder, at holdOffset) and shows.</summary>
        public void PickUp(Transform holder)
        {
            EnsureCaptured();
            carried = true;
            transform.SetParent(holder, false);
            Rect.anchorMin = Rect.anchorMax = Vector2.zero;
            Rect.anchoredPosition = holdOffset;
            gameObject.SetActive(true);
        }

        /// <summary>Puts the prop down in its start parent at the anchored position (UI units) and shows it.</summary>
        public void Place(Vector2 anchoredPosition)
        {
            EnsureCaptured();
            carried = true;
            PutInStartParent();
            Rect.anchoredPosition = anchoredPosition;
            gameObject.SetActive(true);
        }

        /// <summary>Slides the prop sideways by the distance (UI units) over the seconds.</summary>
        public IEnumerator Slide(float distance, float seconds)
        {
            EnsureCaptured();
            var from = Rect.anchoredPosition;
            var to = from + new Vector2(distance, 0f);
            yield return MoveBetween(from, to, seconds);
        }

        /// <summary>
        /// Puts the prop back as it started: a prop that only slid slides back; a prop that was picked up,
        /// put down, shown or hidden fades out and fades back in at its start place.
        /// </summary>
        public IEnumerator ResetToStart(float seconds = 0.4f)
        {
            if (!captured || AtStart) yield break;
            if (!carried && transform.parent == startParent && gameObject.activeSelf == startVisible)
            {
                yield return MoveBetween(Rect.anchoredPosition, startPosition, seconds);
                yield break;
            }
            if (gameObject.activeSelf) yield return Fade(1f, 0f, seconds / 2f);
            PutInStartParent();
            Rect.anchoredPosition = startPosition;
            gameObject.SetActive(startVisible);
            carried = false;
            if (startVisible) yield return Fade(0f, 1f, seconds / 2f);
            SetAlpha(1f);
        }

        void EnsureCaptured()
        {
            if (!captured) CaptureStart();
        }

        void PutInStartParent()
        {
            transform.SetParent(startParent, false);
            transform.SetSiblingIndex(startSibling);
            Rect.anchorMin = startAnchorMin;
            Rect.anchorMax = startAnchorMax;
        }

        IEnumerator MoveBetween(Vector2 from, Vector2 to, float seconds)
        {
            for (var t = 0f; t < seconds; t += Time.deltaTime)
            {
                Rect.anchoredPosition = Vector2.Lerp(from, to, t / seconds);
                yield return null;
            }
            Rect.anchoredPosition = to;
        }

        IEnumerator Fade(float from, float to, float seconds)
        {
            for (var t = 0f; t < seconds; t += Time.deltaTime)
            {
                SetAlpha(Mathf.Lerp(from, to, t / seconds));
                yield return null;
            }
            SetAlpha(to);
        }

        void SetAlpha(float a)
        {
            if (image == null) return;
            var c = image.color;
            c.a = a;
            image.color = c;
        }
    }
}

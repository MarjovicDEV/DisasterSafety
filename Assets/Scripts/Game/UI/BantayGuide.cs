using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// Bantay, the guide dog: wags, blinks, talks in a speech bubble, and shares a
    /// safety tip when tapped. Decoration and guidance only.
    /// </summary>
    public sealed class BantayGuide : MonoBehaviour, IPointerClickHandler
    {
        // Frame order matches ArtSource build: 0 idle, 1 wag, 2 blink, 3 talk.
        public Sprite[] frames = new Sprite[4];
        public Image body;
        public ScreenPanel bubble;
        public Text bubbleText;
        public float sayDuration = 4.5f;

        int tipIndex;
        float talkUntil;
        float hideAt = -1f;
        float nextBlink;
        Coroutine hop;

        void Start()
        {
            tipIndex = Random.Range(0, UIStrings.Tips.Length);
            nextBlink = Time.time + Random.Range(2f, 4f);
            if (bubble != null) bubble.SetInstant(false);
        }

        public void Say(string text, bool bark = false)
        {
            if (bubbleText != null) bubbleText.text = text;
            if (bubble != null)
            {
                bubble.SetInstant(false);
                bubble.Show();
            }
            talkUntil = Time.time + Mathf.Clamp(text.Length * 0.05f, 0.6f, 2.2f);
            hideAt = Time.time + sayDuration;
            if (bark) AudioManager.Play(Sfx.Bark, 0.7f);
            Hop();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Say(UIStrings.Tips[tipIndex], true);
            tipIndex = (tipIndex + 1) % UIStrings.Tips.Length;
        }

        void Hop()
        {
            if (hop != null) StopCoroutine(hop);
            hop = StartCoroutine(HopRoutine());
        }

        IEnumerator HopRoutine()
        {
            var r = (RectTransform)body.transform;
            var rest = Vector2.zero;
            for (var t = 0f; t < 0.3f; t += Time.deltaTime)
            {
                var y = Mathf.Sin(t / 0.3f * Mathf.PI) * 24f;
                r.anchoredPosition = rest + new Vector2(0f, Mathf.Round(y / 4f) * 4f);
                yield return null;
            }
            r.anchoredPosition = rest;
        }

        void Update()
        {
            if (frames == null || frames.Length < 4 || body == null) return;
            var now = Time.time;
            Sprite frame;
            if (now < talkUntil)
                frame = Mathf.FloorToInt(now * 8f) % 2 == 0 ? frames[3] : frames[0];
            else if (now >= nextBlink && now < nextBlink + 0.14f)
                frame = frames[2];
            else
                frame = Mathf.FloorToInt(now * 2.5f) % 2 == 0 ? frames[0] : frames[1];
            if (now > nextBlink + 0.14f) nextBlink = now + Random.Range(2.5f, 5f);
            body.sprite = frame;

            if (hideAt > 0f && now > hideAt)
            {
                hideAt = -1f;
                if (bubble != null) bubble.Hide();
            }
        }
    }
}

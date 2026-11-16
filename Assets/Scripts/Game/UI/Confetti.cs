using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>A happy burst of square confetti that drifts down over the screen once.</summary>
    public sealed class Confetti : MonoBehaviour
    {
        public Sprite piece;
        public RectTransform layer;
        public int count = 70;
        public float seconds = 3.2f;

        static readonly Color32[] Colors =
        {
            new Color32(0xff, 0xd2, 0x3f, 0xff), new Color32(0x62, 0xc4, 0x4c, 0xff), new Color32(0x3f, 0x8f, 0xe0, 0xff),
            new Color32(0xff, 0x7a, 0xa8, 0xff), new Color32(0xf5, 0x90, 0x2e, 0xff), new Color32(0x8a, 0x5c, 0xc8, 0xff),
        };

        public void Burst()
        {
            if (piece == null || layer == null) return;
            StartCoroutine(Run());
        }

        IEnumerator Run()
        {
            var size = layer.rect.size;
            var parts = new RectTransform[count];
            var velocity = new Vector2[count];
            var spin = new float[count];
            for (var i = 0; i < count; i++)
            {
                var go = new GameObject("Confetti", typeof(RectTransform), typeof(Image));
                var r = (RectTransform)go.transform;
                r.SetParent(layer, false);
                r.anchorMin = r.anchorMax = new Vector2(0.5f, 0.5f);
                var side = Random.Range(3, 6) * 4f;
                r.sizeDelta = new Vector2(side, side * Random.Range(0.5f, 1f));
                r.anchoredPosition = new Vector2(Random.Range(-0.5f, 0.5f) * size.x, size.y * 0.5f + Random.Range(20f, 400f));
                var img = go.GetComponent<Image>();
                img.sprite = piece;
                img.color = Colors[i % Colors.Length];
                img.raycastTarget = false;
                velocity[i] = new Vector2(Random.Range(-60f, 60f), Random.Range(-420f, -220f));
                spin[i] = Random.Range(-240f, 240f);
                parts[i] = r;
            }
            for (var t = 0f; t < seconds; t += Time.unscaledDeltaTime)
            {
                var dt = Time.unscaledDeltaTime;
                for (var i = 0; i < count; i++)
                {
                    var sway = Mathf.Sin((t + i) * 3f) * 90f;
                    parts[i].anchoredPosition += new Vector2(velocity[i].x + sway, velocity[i].y) * dt;
                    parts[i].localRotation = Quaternion.Euler(0f, 0f, Mathf.Round(spin[i] * t / 45f) * 45f);
                }
                yield return null;
            }
            foreach (var p in parts) Destroy(p.gameObject);
        }
    }
}

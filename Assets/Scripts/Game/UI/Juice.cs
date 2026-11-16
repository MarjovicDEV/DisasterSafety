using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Small feedback effects: a "no" shake and a burst of sparkles.</summary>
    public sealed class Juice : MonoBehaviour
    {
        public Sprite sparkle;
        public RectTransform effectsLayer;

        static Juice instance;

        void Awake() => instance = this;

        void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        public static void Shake(RectTransform target)
        {
            if (instance != null && target != null) instance.StartCoroutine(instance.ShakeRoutine(target));
        }

        public static void Sparkles(RectTransform at, int count = 10)
        {
            if (instance != null && at != null) instance.StartCoroutine(instance.SparkleRoutine(at, count));
        }

        IEnumerator ShakeRoutine(RectTransform target)
        {
            var rest = target.anchoredPosition;
            for (var t = 0f; t < 0.35f; t += Time.unscaledDeltaTime)
            {
                var k = 1f - t / 0.35f;
                target.anchoredPosition = rest + new Vector2(Mathf.Round(Mathf.Sin(t * 60f) * 14f * k / 4f) * 4f, 0f);
                yield return null;
            }
            target.anchoredPosition = rest;
        }

        IEnumerator SparkleRoutine(RectTransform at, int count)
        {
            if (sparkle == null || effectsLayer == null) yield break;
            var world = at.TransformPoint(at.rect.center);
            Vector2 origin = effectsLayer.InverseTransformPoint(world);
            var parts = new RectTransform[count];
            var velocity = new Vector2[count];
            for (var i = 0; i < count; i++)
            {
                var go = new GameObject("Sparkle", typeof(RectTransform), typeof(Image));
                var r = (RectTransform)go.transform;
                r.SetParent(effectsLayer, false);
                r.sizeDelta = new Vector2(36f, 36f);
                r.anchoredPosition = origin;
                var img = go.GetComponent<Image>();
                img.sprite = sparkle;
                img.raycastTarget = false;
                var a = (i / (float)count) * Mathf.PI * 2f + Random.value * 0.4f;
                velocity[i] = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * Random.Range(500f, 900f);
                parts[i] = r;
            }
            const float life = 0.6f;
            for (var t = 0f; t < life; t += Time.unscaledDeltaTime)
            {
                var dt = Time.unscaledDeltaTime;
                for (var i = 0; i < count; i++)
                {
                    velocity[i] *= 1f - 4f * dt;
                    velocity[i].y -= 600f * dt;
                    parts[i].anchoredPosition += velocity[i] * dt;
                    parts[i].localScale = Vector3.one * (1f - t / life) * 1.2f;
                }
                yield return null;
            }
            foreach (var p in parts) Destroy(p.gameObject);
        }
    }
}

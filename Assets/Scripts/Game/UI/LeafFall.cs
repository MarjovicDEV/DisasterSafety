using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Leaves that drift down from the tree canopy.</summary>
    public sealed class LeafFall : MonoBehaviour
    {
        public Sprite[] leaves;
        public Rect spawnArea = new Rect(0f, 560f, 260f, 200f);
        public float groundY = 180f;
        public float every = 1.6f;
        public int maxLeaves = 6;

        readonly List<(RectTransform rect, float phase, float speed)> live = new List<(RectTransform, float, float)>();
        float timer;

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f && live.Count < maxLeaves && leaves != null && leaves.Length > 0)
            {
                timer = every * Random.Range(0.6f, 1.4f);
                Spawn();
            }
            for (var i = live.Count - 1; i >= 0; i--)
            {
                var (r, phase, speed) = live[i];
                var p = r.anchoredPosition;
                p.y -= speed * Time.deltaTime;
                p.x += Mathf.Sin(Time.time * 2.2f + phase) * 60f * Time.deltaTime;
                r.anchoredPosition = p;
                r.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * 3f + phase) * 35f);
                if (p.y < groundY)
                {
                    Destroy(r.gameObject);
                    live.RemoveAt(i);
                }
            }
        }

        void Spawn()
        {
            var go = new GameObject("Leaf", typeof(RectTransform), typeof(Image));
            var r = (RectTransform)go.transform;
            r.SetParent(transform, false);
            r.anchorMin = r.anchorMax = Vector2.zero;
            r.sizeDelta = new Vector2(24f, 20f);
            r.anchoredPosition = new Vector2(spawnArea.x + Random.value * spawnArea.width, spawnArea.y + Random.value * spawnArea.height);
            var img = go.GetComponent<Image>();
            img.sprite = leaves[Random.Range(0, leaves.Length)];
            img.raycastTarget = false;
            live.Add((r, Random.value * 10f, Random.Range(60f, 110f)));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Now and then a small flock of birds flies across the sky.</summary>
    public sealed class BirdFlock : MonoBehaviour
    {
        public Sprite[] frames;
        public float minWait = 6f;
        public float maxWait = 14f;
        public Vector2 skyBand = new Vector2(760f, 960f);

        IEnumerator Start()
        {
            var rect = (RectTransform)transform;
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minWait, maxWait));
                var count = Random.Range(2, 5);
                var y = Random.Range(skyBand.x, skyBand.y);
                var speed = Random.Range(110f, 170f);
                var birds = new List<RectTransform>();
                for (var i = 0; i < count; i++)
                {
                    var go = new GameObject("Bird", typeof(RectTransform), typeof(Image), typeof(SpriteAnimator));
                    var r = (RectTransform)go.transform;
                    r.SetParent(rect, false);
                    r.anchorMin = r.anchorMax = Vector2.zero;
                    r.pivot = new Vector2(0.5f, 0.5f);
                    r.sizeDelta = new Vector2(36f, 24f);
                    r.anchoredPosition = new Vector2(-60f - i * 70f, y + (i % 2) * 40f);
                    var img = go.GetComponent<Image>();
                    img.raycastTarget = false;
                    img.sprite = frames[0];
                    var anim = go.GetComponent<SpriteAnimator>();
                    anim.frames = frames;
                    anim.framesPerSecond = 6f;
                    birds.Add(r);
                }
                var t = 0f;
                while (birds[birds.Count - 1].anchoredPosition.x < 2100f)
                {
                    t += Time.deltaTime;
                    for (var i = 0; i < birds.Count; i++)
                    {
                        var p = birds[i].anchoredPosition;
                        p.x += speed * Time.deltaTime;
                        p.y = y + (i % 2) * 40f + Mathf.Sin(t * 2f + i) * 12f;
                        birds[i].anchoredPosition = new Vector2(p.x, Mathf.Round(p.y / 4f) * 4f);
                    }
                    yield return null;
                }
                foreach (var b in birds) Destroy(b.gameObject);
            }
        }
    }
}

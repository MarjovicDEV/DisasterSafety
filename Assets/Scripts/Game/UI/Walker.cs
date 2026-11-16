using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>A student who walks across the courtyard, waits off-screen, and walks again.</summary>
    public sealed class Walker : MonoBehaviour
    {
        public float fromX = -120f;
        public float toX = 2040f;
        public float speed = 150f;
        public float minWait = 1.5f;
        public float maxWait = 6f;
        public float startDelay;

        RectTransform rect;

        IEnumerator Start()
        {
            rect = (RectTransform)transform;
            var direction = Mathf.Sign(toX - fromX);
            rect.localScale = new Vector3(direction * Mathf.Abs(rect.localScale.x), rect.localScale.y, 1f);
            SetX(fromX);
            yield return new WaitForSeconds(startDelay);
            while (true)
            {
                for (var x = fromX; direction > 0 ? x < toX : x > toX; x += direction * speed * Time.deltaTime)
                {
                    SetX(x);
                    yield return null;
                }
                SetX(toX);
                yield return new WaitForSeconds(Random.Range(minWait, maxWait));
                SetX(fromX);
            }
        }

        void SetX(float x)
        {
            var p = rect.anchoredPosition;
            rect.anchoredPosition = new Vector2(Mathf.Round(x / 4f) * 4f, p.y);
        }
    }
}

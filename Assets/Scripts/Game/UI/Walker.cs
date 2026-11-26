using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// A student who walks across the courtyard, waits off-screen, and walks again. When the menu
    /// hides the walkers and shows them again, the student carries on from where they were.
    /// </summary>
    public sealed class Walker : MonoBehaviour
    {
        public float fromX = -120f;
        public float toX = 2040f;
        public float speed = 150f;
        public float minWait = 1.5f;
        public float maxWait = 6f;
        public float startDelay;

        RectTransform rect;
        bool started;
        float x;

        // A coroutine stops when its object is turned off, and Start never runs again, so the walk
        // starts from OnEnable each time and resumes at the saved x.
        void OnEnable() => StartCoroutine(Walk());

        IEnumerator Walk()
        {
            rect = (RectTransform)transform;
            var direction = Mathf.Sign(toX - fromX);
            rect.localScale = new Vector3(direction * Mathf.Abs(rect.localScale.x), rect.localScale.y, 1f);
            if (!started)
            {
                started = true;
                x = fromX;
                SetX(fromX);
                yield return new WaitForSeconds(startDelay);
            }
            while (true)
            {
                for (; direction > 0 ? x < toX : x > toX; x += direction * speed * Time.deltaTime)
                {
                    SetX(x);
                    yield return null;
                }
                SetX(toX);
                yield return new WaitForSeconds(Random.Range(minWait, maxWait));
                x = fromX;
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

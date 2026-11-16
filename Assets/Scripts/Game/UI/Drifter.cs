using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Clouds that drift slowly and wrap around the sky.</summary>
    public sealed class Drifter : MonoBehaviour
    {
        public float speed = 12f;
        public float wrapWidth = 1920f;
        public float margin = 300f;

        RectTransform rect;
        float x;

        void Start()
        {
            rect = (RectTransform)transform;
            x = rect.anchoredPosition.x;
        }

        void Update()
        {
            x += speed * Time.deltaTime;
            if (x > wrapWidth + margin) x = -margin;
            rect.anchoredPosition = new Vector2(Mathf.Round(x / 4f) * 4f, rect.anchoredPosition.y);
        }
    }
}

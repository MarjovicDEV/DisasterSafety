using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Scales a fixed 1920x1080 art layer so it always covers the screen.</summary>
    [ExecuteAlways]
    public sealed class EnvelopeScaler : MonoBehaviour
    {
        public Vector2 artSize = new Vector2(1920f, 1080f);

        void LateUpdate()
        {
            var parent = transform.parent as RectTransform;
            if (parent == null) return;
            var size = parent.rect.size;
            var s = Mathf.Max(size.x / artSize.x, size.y / artSize.y);
            transform.localScale = new Vector3(s, s, 1f);
        }
    }
}

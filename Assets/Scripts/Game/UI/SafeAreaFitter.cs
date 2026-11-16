using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Keeps buttons away from notches and rounded corners.</summary>
    public sealed class SafeAreaFitter : MonoBehaviour
    {
        Rect applied;

        void Update()
        {
            var safe = Screen.safeArea;
            if (safe == applied || Screen.width <= 0 || Screen.height <= 0) return;
            applied = safe;
            var r = (RectTransform)transform;
            r.anchorMin = new Vector2(safe.xMin / Screen.width, safe.yMin / Screen.height);
            r.anchorMax = new Vector2(safe.xMax / Screen.width, safe.yMax / Screen.height);
            r.offsetMin = r.offsetMax = Vector2.zero;
        }
    }
}

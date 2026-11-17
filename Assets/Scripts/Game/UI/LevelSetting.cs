using UnityEngine;

namespace DisasterSafety.Game.UI
{
    /// <summary>Scene art and layout for one Scenario Setting.</summary>
    public sealed class LevelSetting : MonoBehaviour
    {
        public string key;
        public SceneShaker shaker;
        public bool alignArtTop;
        public bool compactChoicesOnWideScreens;
        public float guidanceOffsetX;
        public Vector2 goalPosition = new Vector2(0f, -24f);
    }
}

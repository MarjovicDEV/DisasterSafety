using UnityEngine;

namespace DisasterSafety.Game.UI
{
    public sealed class LevelSetting : MonoBehaviour
    {
        public string key;
        public SceneShaker shaker;
        public bool alignArtTop;
        public float guidanceOffsetX;
        public Vector2 goalPosition = new Vector2(0f, -24f);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>An on/off switch drawn with two sprites.</summary>
    public sealed class SwitchButton : MonoBehaviour
    {
        public Image image;
        public Sprite onSprite;
        public Sprite offSprite;
        public Button button;

        public void Show(bool on) => image.sprite = on ? onSprite : offSprite;
    }
}

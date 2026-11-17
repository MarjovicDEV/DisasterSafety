using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>One Choice in the Action Bar: a 16x16 icon and a short command. A tried Unsafe Choice is greyed and marked.</summary>
    public sealed class ActionButton : MonoBehaviour
    {
        public Button button;
        public Image icon;
        public Text label;
        public GameObject triedMark;
        public CanvasGroup group;

        public void Show(ScenarioChoice choice, Sprite iconSprite)
        {
            label.text = choice.label;
            icon.sprite = iconSprite;
            icon.enabled = iconSprite != null;
            SetTried(false);
        }

        public void SetTried(bool tried)
        {
            if (triedMark != null) triedMark.SetActive(tried);
            if (group != null) group.alpha = tried ? 0.6f : 1f;
            button.interactable = !tried;
        }
    }
}

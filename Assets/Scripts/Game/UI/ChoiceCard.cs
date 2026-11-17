using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>One big picture card for a Choice. A tried Unsafe Choice gets a soft grey "Tried" mark, never a red X.</summary>
    public sealed class ChoiceCard : MonoBehaviour
    {
        public Button button;
        public Image picture;
        public Text label;
        public Font compactLabelFont;
        public float compactLabelWidth;
        public GameObject triedMark;
        public CanvasGroup group;

        Font regularLabelFont;
        float regularLabelWidth;

        public void Show(ScenarioChoice choice, Sprite sprite)
        {
            SetLabel(choice.label);
            picture.sprite = sprite;
            picture.enabled = sprite != null;
            SetTried(false);
        }

        void SetLabel(string text)
        {
            label.text = text;
            if (compactLabelFont == null) return;

            var rect = label.rectTransform;
            if (regularLabelFont == null)
            {
                regularLabelFont = label.font;
                regularLabelWidth = rect.sizeDelta.x;
            }

            label.font = regularLabelFont;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, regularLabelWidth);
            if (label.preferredHeight <= rect.rect.height) return;

            label.font = compactLabelFont;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, compactLabelWidth);
        }

        public void SetTried(bool tried)
        {
            if (triedMark != null) triedMark.SetActive(tried);
            if (group != null) group.alpha = tried ? 0.6f : 1f;
            button.interactable = !tried;
        }
    }
}

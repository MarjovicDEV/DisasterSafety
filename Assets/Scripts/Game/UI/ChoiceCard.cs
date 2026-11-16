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
        public GameObject triedMark;
        public CanvasGroup group;

        public void Show(ScenarioChoice choice, Sprite sprite)
        {
            label.text = choice.label;
            picture.sprite = sprite;
            picture.enabled = sprite != null;
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

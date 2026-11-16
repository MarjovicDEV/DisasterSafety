using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>One Level tile on Level Selection.</summary>
    public sealed class LevelSlot : MonoBehaviour
    {
        public Button button;
        public Text number;
        public Image[] stars = new Image[3];
        public GameObject selectedFrame;
        public Sprite starOn;
        public Sprite starOff;

        public int Number { get; private set; }

        public void Show(int levelNumber, int earnedStars, bool selected)
        {
            Number = levelNumber;
            number.text = levelNumber.ToString();
            for (var i = 0; i < stars.Length; i++) stars[i].sprite = i < earnedStars ? starOn : starOff;
            selectedFrame.SetActive(selected);
        }
    }
}

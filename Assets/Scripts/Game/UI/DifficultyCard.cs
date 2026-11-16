using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>One Difficulty card on Difficulty Selection.</summary>
    public sealed class DifficultyCard : MonoBehaviour
    {
        public Difficulty difficulty;
        public Button button;
        public Image face;
        public Text label;
        public Text count;
        public GameObject lockBadge;
        public Image[] hats;

        public void Show(bool open)
        {
            lockBadge.SetActive(!open);
            face.color = open ? Color.white : new Color(0.62f, 0.62f, 0.66f, 1f);
            foreach (var h in hats) h.color = open ? Color.white : new Color(0.5f, 0.5f, 0.55f, 1f);
        }
    }
}

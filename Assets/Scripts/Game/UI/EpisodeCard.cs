using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>One Episode card on Episode Selection.</summary>
    public sealed class EpisodeCard : MonoBehaviour
    {
        public Episode episode;
        public Button button;
        public Image face;
        public Image emblem;
        public Text label;
        public GameObject lockBadge;

        public void Show(bool open)
        {
            lockBadge.SetActive(!open);
            emblem.color = open ? Color.white : new Color(0.45f, 0.45f, 0.5f, 1f);
            face.color = open ? Color.white : new Color(0.62f, 0.62f, 0.66f, 1f);
        }
    }
}

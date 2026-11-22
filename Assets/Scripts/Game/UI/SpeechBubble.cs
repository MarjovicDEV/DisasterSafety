using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// A small speech bubble for a Reaction or a cheer line. It hides itself after a few seconds
    /// and follows Time.timeScale, so it freezes while the Level is paused.
    /// </summary>
    public sealed class SpeechBubble : MonoBehaviour
    {
        public Text text;

        float hideAfter = -1f;

        public string Line => text != null ? text.text : string.Empty;

        /// <summary>Shows the line; a negative <paramref name="seconds"/> keeps it until Hide.</summary>
        public void Say(string line, float seconds = 2f)
        {
            text.text = line;
            hideAfter = seconds;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            hideAfter = -1f;
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (hideAfter < 0f) return;
            hideAfter -= Time.deltaTime;
            if (hideAfter <= 0f) Hide();
        }
    }
}

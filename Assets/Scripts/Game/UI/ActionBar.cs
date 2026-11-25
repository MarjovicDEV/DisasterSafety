using System;
using DisasterSafety.Core;
using UnityEngine;

namespace DisasterSafety.Game.UI
{
    /// <summary>The bar at the bottom of a Level that lists the Scenario's Choices as short commands and slides up.</summary>
    public sealed class ActionBar : MonoBehaviour
    {
        public ScreenPanel panel;
        public ActionButton[] buttons;
        [Tooltip("Choice icons; matched to a Choice by sprite name.")]
        public Sprite[] icons;
        [Tooltip("The smaller pixel font every label uses when one label would need more than two lines.")]
        public Font smallFont;
        [Tooltip("The width a label has inside its button, in UI units.")]
        public float labelWidth = 314f;

        Font normalFont;

        public bool IsVisible => panel.IsVisible;

        public void Setup(Scenario scenario, Action<int> onChoose)
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                var used = i < scenario.choices.Length;
                buttons[i].gameObject.SetActive(used);
                if (!used) continue;
                buttons[i].Show(scenario.choices[i], IconFor(scenario.choices[i].icon));
                var n = i;
                buttons[i].button.onClick.RemoveAllListeners();
                buttons[i].button.onClick.AddListener(() => onChoose(n));
            }
            FitLabels(scenario);
        }

        // A label longer than two lines would spill out of its button: then every label uses the
        // smaller font, so the bar stays even.
        void FitLabels(Scenario scenario)
        {
            if (buttons.Length == 0 || buttons[0].label == null) return;
            if (normalFont == null) normalFont = buttons[0].label.font;
            var tooLong = false;
            for (var i = 0; i < buttons.Length && i < scenario.choices.Length; i++)
                if (PixelText.Lines(normalFont, scenario.choices[i].label, labelWidth) > 2) tooLong = true;
            var font = tooLong && smallFont != null ? smallFont : normalFont;
            foreach (var button in buttons)
            {
                var label = button.label;
                if (label == null) continue;
                label.font = font;
                // Leave room for the third line of a long Normal Choice above the button lip.
                var needsThirdLine = scenario.choices.Length == 3 && PixelText.Lines(font, label.text, labelWidth) > 2;
                label.lineSpacing = needsThirdLine ? 0.75f : 0.9f;
            }
        }

        public void SetTried(int index, bool tried) => buttons[index].SetTried(tried);

        public void Show() => panel.Show();
        public void Hide() => panel.Hide();

        Sprite IconFor(string key)
        {
            if (icons == null || string.IsNullOrEmpty(key)) return null;
            foreach (var s in icons)
                if (s != null && s.name == key) return s;
            return null;
        }
    }
}

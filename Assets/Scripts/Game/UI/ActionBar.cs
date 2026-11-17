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

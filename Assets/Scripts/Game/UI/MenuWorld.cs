using System;
using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// The art behind the menus (C3, C4): one picture per MenuBackground and the moving layers
    /// that belong to it. The flag flies over the school gate and the drill ground, clouds and
    /// birds cross every outdoor background, the guard lets the Students in at the school gate, the
    /// two walking children cross the drill ground and the schoolyard, and the leaves fall from the
    /// trees on those two. Fire's three Level selection backgrounds stay still.
    /// </summary>
    public sealed class MenuWorld : MonoBehaviour
    {
        public Image background;
        [Tooltip("Pictures in MenuBackground order.")]
        public Sprite[] sprites;
        public GameObject flag;
        public GameObject clouds;
        public GameObject birds;
        public GameObject leaves;
        public GameObject walkers;
        public GameObject gateScene;

        public MenuBackground Current { get; private set; }

        public void Show(MenuBackground which)
        {
            var index = (int)which;
            if (sprites == null || index < 0 || index >= sprites.Length || sprites[index] == null)
                throw new ArgumentOutOfRangeException(nameof(which), which, "No sprite for this background.");
            Current = which;
            background.sprite = sprites[index];
            SetActive(flag, HasFlag(which));
            SetActive(clouds, IsOutdoors(which));
            SetActive(birds, IsOutdoors(which));
            SetActive(leaves, HasTree(which));
            SetActive(walkers, HasWalkers(which));
            SetActive(gateScene, which == MenuBackground.SchoolGate);
        }

        public static bool HasFlag(MenuBackground b) => b == MenuBackground.SchoolGate || b == MenuBackground.DrillGround;

        public static bool IsOutdoors(MenuBackground b) =>
            b == MenuBackground.SchoolGate || b == MenuBackground.DrillGround || b == MenuBackground.FireStation
            || b == MenuBackground.Schoolyard || b == MenuBackground.CoveredCourt;

        public static bool HasTree(MenuBackground b) => b == MenuBackground.DrillGround || b == MenuBackground.Schoolyard;

        /// <summary>The school grounds where the two children walk across (the gate has its own scene).</summary>
        public static bool HasWalkers(MenuBackground b) => b == MenuBackground.DrillGround || b == MenuBackground.Schoolyard;

        static void SetActive(GameObject go, bool on)
        {
            if (go != null && go.activeSelf != on) go.SetActive(on);
        }
    }
}

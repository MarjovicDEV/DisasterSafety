using System;
using UnityEngine;

namespace DisasterSafety.Game.UI
{
    /// <summary>A place the Student walks to for a Choice (a door, the desk, the window, the cabinet).</summary>
    [Serializable]
    public sealed class ActionSpot
    {
        public string key;
        [Tooltip("The Student's centre x in art pixels.")]
        public float x;
        [Tooltip("An Unsafe Choice walks this fraction of the way from the start spot, then stops.")]
        [Range(0f, 1f)] public float stopAt = 0.6f;
        public StudentPose endPose = StudentPose.Idle;
        [Tooltip("Drawn over the Student at the end pose, e.g. the front of the desk.")]
        public GameObject overlay;
    }

    /// <summary>Scene art and layout for one Scenario Setting.</summary>
    public sealed class LevelSetting : MonoBehaviour
    {
        public string key;
        public SceneShaker shaker;
        public bool alignArtTop;
        [Tooltip("Where the art sits on a screen wider than 16:9: 0 aligns its bottom, 1 its top. Ignored when alignArtTop is set.")]
        [Range(0f, 1f)] public float artAnchorY = 0.5f;
        public bool compactChoicesOnWideScreens;
        public float guidanceOffsetX;
        public Vector2 goalPosition = new Vector2(0f, -24f);

        [Header("Student (the new flow)")]
        public StudentActor student;
        public float startX;
        public ActionSpot[] actionSpots;

        public ActionSpot SpotFor(string action) => Array.Find(actionSpots ?? new ActionSpot[0], spot => spot.key == action);
    }
}

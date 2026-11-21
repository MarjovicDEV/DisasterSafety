using System;
using UnityEngine;

namespace DisasterSafety.Game.UI
{
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
        [Tooltip("Where the Student starts: the centre x and the height above the floor line, in art pixels.")]
        public float startX;
        public float startY;
        public StudentPose startPose = StudentPose.Idle;
        [Tooltip("The Grown-ups of the Setting. They stand at their start place and pose, and return there on Try Again.")]
        public GrownUpActor[] grownUps = new GrownUpActor[0];
        [Tooltip("The props the Act-outs move. They return to their start place on Try Again.")]
        public LevelProp[] props = new LevelProp[0];
        [Tooltip("One Act-out per Choice action key that the Setting's Scenarios use.")]
        public ActOut[] actOuts = new ActOut[0];

        public ActOut ActOutFor(string action) => Array.Find(actOuts ?? new ActOut[0], a => a.key == action);
    }
}

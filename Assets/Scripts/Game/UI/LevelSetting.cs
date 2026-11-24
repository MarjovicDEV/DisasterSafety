using System;
using DisasterSafety.Core;
using UnityEngine;

namespace DisasterSafety.Game.UI
{
    [Serializable]
    public sealed class LevelStart
    {
        public string level;
        public GameObject cast;
        public float x;
        public StudentPose pose = StudentPose.Idle;
        public Vector2 reactionPosition = new Vector2(0f, 8f);
        public LevelProp[] props = Array.Empty<LevelProp>();
        public GrownUpActor[] grownUps = Array.Empty<GrownUpActor>();
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
        public Font captionGoalFont;

        [Header("Student (the new flow)")]
        public StudentActor student;
        [Tooltip("Where the Student starts: the centre x and the height above the floor line, in art pixels.")]
        public float startX;
        public float startY;
        public StudentPose startPose = StudentPose.Idle;
        [Tooltip("The x to the left of a low start spot where the Student crouches on Try Again. A negative value keeps the full return upright.")]
        public float startCrouchFromX = -1f;
        [Tooltip("Try Again retraces the Act-out's walk and run destinations on the way back to the start.")]
        public bool retraceActOutOnRetry;
        [Tooltip("The Grown-ups of the Setting. They stand at their start place and pose, and return there on Try Again.")]
        public GrownUpActor[] grownUps = new GrownUpActor[0];
        [Tooltip("The props the Act-outs move. They return to their start place on Try Again.")]
        public LevelProp[] props = new LevelProp[0];
        [Tooltip("One Act-out per Choice action key that the Setting's Scenarios use.")]
        public ActOut[] actOuts = new ActOut[0];

        [Tooltip("Optional starts for the Levels that share this Setting.")]
        public LevelStart[] levelStarts = Array.Empty<LevelStart>();

        LevelStart selectedStart;

        public float StartX => selectedStart?.x ?? startX;
        public StudentPose StartPose => selectedStart?.pose ?? startPose;
        public LevelProp[] ActiveProps => selectedStart?.props ?? props;
        public GrownUpActor[] ActiveGrownUps => selectedStart?.grownUps ?? grownUps;

        /// <summary>Places the Student and the cast for the selected Level, also used by the screenshot tool.</summary>
        public void PrepareStudentStart(string level, StudentKind kind)
        {
            selectedStart = Array.Find(levelStarts, start => start.level == level);
            foreach (var start in levelStarts)
                if (start.cast != null) start.cast.SetActive(start == selectedStart);

            student.SetKind(kind);
            student.PlaceAt(StartX, startY);
            student.Face(false);
            student.Play(StartPose);
            if (selectedStart != null)
                ((RectTransform)student.bubble.transform).anchoredPosition = selectedStart.reactionPosition;
            foreach (var prop in ActiveProps)
                if (prop != null) prop.CaptureStart();
            foreach (var grownUp in ActiveGrownUps)
                if (grownUp != null) grownUp.PlaceAtStart();
        }

        public ActOut ActOutFor(string action) => Array.Find(actOuts ?? new ActOut[0], a => a.key == action);
    }
}

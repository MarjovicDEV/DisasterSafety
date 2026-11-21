using System;
using System.Collections;
using UnityEngine;

namespace DisasterSafety.Game.UI
{
    /// <summary>The Grown-ups of the Earthquake Levels (art in Assets/Art/GrownUp).</summary>
    public enum GrownUpKind
    {
        Teacher,
        Mom,
        Dad
    }

    public enum GrownUpPose
    {
        /// <summary>Standing, facing the front.</summary>
        Idle,
        /// <summary>Waving with one arm (the Teacher).</summary>
        Wave,
        /// <summary>Walking (side view, 4 frames).</summary>
        Walk,
        /// <summary>Both arms up, glad (Mom).</summary>
        Happy,
        /// <summary>Sitting cross-legged on the floor (Mom).</summary>
        Sit,
        /// <summary>Sitting on the floor and holding her leg (Mom).</summary>
        HurtSit,
        /// <summary>Kneeling on one knee, reaching down to someone (Dad, side view).</summary>
        Kneel
    }

    /// <summary>The pose frames of one Grown-up. A pose with no frames (Dad has no idle) shows a standing frame of the walk.</summary>
    [Serializable]
    public sealed class GrownUpFrames
    {
        public Sprite[] idle;
        public Sprite[] wave;
        public Sprite[] walk;
        public Sprite[] happy;
        public Sprite[] sit;
        public Sprite[] hurtSit;
        public Sprite[] kneel;

        Sprite[] standing;

        public Sprite[] For(GrownUpPose pose)
        {
            Sprite[] frames;
            switch (pose)
            {
                case GrownUpPose.Wave: frames = wave; break;
                case GrownUpPose.Walk: frames = walk; break;
                case GrownUpPose.Happy: frames = happy; break;
                case GrownUpPose.Sit: frames = sit; break;
                case GrownUpPose.HurtSit: frames = hurtSit; break;
                case GrownUpPose.Kneel: frames = kneel; break;
                default: frames = idle; break;
            }
            if (frames != null && frames.Length > 0) return frames;
            if (standing == null && walk != null && walk.Length > 1) standing = new[] { walk[1] };
            return standing;
        }
    }

    /// <summary>
    /// A Teacher, Mom or Dad in a Level scene: plays a pose, faces left or right, walks to a point and
    /// goes back to its start place and pose on Try Again (see PoseActor). Grown-ups only act as an
    /// Act-out's steps say; they never judge a Choice. Everything follows Time.timeScale.
    /// </summary>
    public sealed class GrownUpActor : PoseActor
    {
        // Frame durations in seconds, in GrownUpPose order, from the .aseprite files.
        static readonly float[] FrameSeconds = { 0.5f, 0.26f, 0.14f, 0.26f, 0.5f, 0.45f, 0.4f };

        public GrownUpKind kind;
        public GrownUpFrames frames;
        [Header("Start (the Level starts, and Try Again returns, here)")]
        [Tooltip("The centre x and the height above the floor line, in art pixels.")]
        public float startX;
        public float startY;
        public GrownUpPose startPose = GrownUpPose.Idle;
        public bool startFacingLeft;

        GrownUpPose pose = GrownUpPose.Idle;

        public GrownUpPose Pose => pose;

        /// <summary>True when the Grown-up stands at its start place, facing and posing as it did.</summary>
        public bool AtStart => !IsWalking && Mathf.Approximately(X, startX) && Mathf.Approximately(Y, startY)
                               && pose == startPose && FacingLeft == startFacingLeft;

        protected override Sprite[] CurrentFrames => frames?.For(pose);
        protected override float CurrentFrameSeconds => FrameSeconds[(int)pose];
        protected override float BlipPitch => kind == GrownUpKind.Dad ? 0.8f : kind == GrownUpKind.Mom ? 1.15f : 1.05f;

        /// <summary>Plays a pose from its first frame.</summary>
        public void Play(GrownUpPose value)
        {
            pose = value;
            poseTime = 0f;
            Apply();
        }

        /// <summary>Stands at the start place in the start pose, without walking.</summary>
        public void PlaceAtStart()
        {
            PlaceAt(startX, startY);
            Face(startFacingLeft);
            Play(startPose);
        }

        /// <summary>Turns, walks to the point at WalkSpeed (a changing height is a slope), then stands idle facing the front.</summary>
        public IEnumerator WalkTo(float artX, float artY) =>
            Move(artX, artY, WalkSpeed, () => Play(GrownUpPose.Walk), () => Play(GrownUpPose.Idle));

        /// <summary>Walks back to the start place and takes the start pose and facing.</summary>
        public IEnumerator ReturnToStart()
        {
            yield return WalkTo(startX, startY);
            Face(startFacingLeft);
            Play(startPose);
        }
    }
}

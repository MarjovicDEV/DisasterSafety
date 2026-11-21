using System;
using System.Collections;
using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    public enum StudentPose
    {
        Idle,
        Surprised,
        Walk,
        Duck,
        Wait,
        Happy,
        /// <summary>Knees bent and back hunched, shuffling in under the desk (side view).</summary>
        Crouch,
        /// <summary>Under the desk: one hand holds the desk leg, the other arm covers the head.</summary>
        Hold
    }

    /// <summary>The pose frames of one Student (boy or girl).</summary>
    [Serializable]
    public sealed class StudentFrames
    {
        public Sprite[] idle;
        public Sprite[] surprised;
        public Sprite[] walk;
        public Sprite[] duck;
        public Sprite[] wait;
        public Sprite[] happy;
        public Sprite[] crouch;
        public Sprite[] hold;
        public Sprite portrait;

        public Sprite[] For(StudentPose pose)
        {
            switch (pose)
            {
                case StudentPose.Surprised: return surprised;
                case StudentPose.Walk: return walk;
                case StudentPose.Duck: return duck;
                case StudentPose.Wait: return wait;
                case StudentPose.Happy: return happy;
                case StudentPose.Crouch: return crouch;
                case StudentPose.Hold: return hold;
                default: return idle;
            }
        }
    }

    /// <summary>
    /// The Learner's Student: plays a pose, faces left or right, walks to a point and shows a Reaction
    /// in a speech bubble (see PoseActor). Positions are in art pixels, as in ArtSource/README.md.
    /// Everything follows Time.timeScale, so Pause freezes the Student.
    /// </summary>
    public sealed class StudentActor : PoseActor
    {
        /// <summary>Art pixels per second while shuffling in under the desk.</summary>
        public const float CrouchSpeed = 30f;

        // Frame durations in seconds, in StudentPose order, from the .aseprite files.
        static readonly float[] FrameSeconds = { 0.5f, 0.16f, 0.14f, 0.3f, 0.45f, 0.26f, 0.18f, 0.3f };

        public StudentFrames boy;
        public StudentFrames girl;

        StudentKind kind = StudentKind.Boy;
        StudentPose pose = StudentPose.Idle;

        public StudentKind Kind => kind;
        public StudentPose Pose => pose;
        public StudentFrames Frames => kind == StudentKind.Girl ? girl : boy;

        protected override Sprite[] CurrentFrames => Frames?.For(pose);
        protected override float CurrentFrameSeconds => FrameSeconds[(int)pose];

        /// <summary>The Student the Learner picked. None means the boy, until a pick exists.</summary>
        public void SetKind(StudentKind value)
        {
            kind = value == StudentKind.Girl ? StudentKind.Girl : StudentKind.Boy;
            Apply();
        }

        /// <summary>Plays a pose from its first frame.</summary>
        public void Play(StudentPose value)
        {
            pose = value;
            poseTime = 0f;
            Apply();
        }

        /// <summary>Shows one frame of a pose, for the screenshot tool, which does not run Update.</summary>
        public void ShowFrame(StudentPose value, int frame)
        {
            pose = value;
            poseTime = frame * FrameSeconds[(int)value];
            Apply();
        }

        /// <summary>Turns, walks to the x at WalkSpeed on the current level, then stands idle facing the front.</summary>
        public IEnumerator WalkTo(float artX) => WalkTo(artX, Y, WalkSpeed);

        /// <summary>
        /// Turns, walks to the point at the speed (WalkSpeed, FastWalkSpeed or RunSpeed; a changing height is
        /// a slope or stairs), then stands idle facing the front. Faster speeds play the walk frames faster.
        /// </summary>
        public IEnumerator WalkTo(float artX, float artY, float speed) =>
            Move(artX, artY, speed, () => { Play(StudentPose.Walk); frameRate = speed / WalkSpeed; }, () => Play(StudentPose.Idle));

        /// <summary>Crouches and shuffles to the x at CrouchSpeed (under the desk), then stays crouched.</summary>
        public IEnumerator CrouchTo(float artX)
        {
            Play(StudentPose.Crouch);
            yield return Move(artX, Y, CrouchSpeed, null, null);
            Face(false);
        }
    }
}

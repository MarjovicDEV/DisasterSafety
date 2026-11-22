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
        Hold,
        /// <summary>Walking with something held in both arms (side view, 4 frames).</summary>
        Carry,
        /// <summary>One arm out forward at shoulder height (side view).</summary>
        Reach,
        /// <summary>On tiptoe with both arms up and forward (side view).</summary>
        ReachUp,
        /// <summary>Pushing with both hands, legs in a stride (side view).</summary>
        Push,
        /// <summary>Calling out, a hand at the mouth (side view).</summary>
        Call,
        /// <summary>Duck and cover without a desk: kneeling low, arms over the head and neck (side view).</summary>
        Cover,
        /// <summary>Hands at table height, packing or writing (front view).</summary>
        Pack,
        /// <summary>Holding a phone in both hands (front view).</summary>
        Phone,
        /// <summary>Sitting cross-legged (front view).</summary>
        Sit
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
        public Sprite[] carry;
        public Sprite[] reach;
        public Sprite[] reachUp;
        public Sprite[] push;
        public Sprite[] call;
        public Sprite[] cover;
        public Sprite[] pack;
        public Sprite[] phone;
        public Sprite[] sit;
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
                case StudentPose.Carry: return carry;
                case StudentPose.Reach: return reach;
                case StudentPose.ReachUp: return reachUp;
                case StudentPose.Push: return push;
                case StudentPose.Call: return call;
                case StudentPose.Cover: return cover;
                case StudentPose.Pack: return pack;
                case StudentPose.Phone: return phone;
                case StudentPose.Sit: return sit;
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
        static readonly float[] FrameSeconds = { 0.5f, 0.16f, 0.14f, 0.3f, 0.45f, 0.26f, 0.18f, 0.3f, 0.14f, 0.3f, 0.3f, 0.26f, 0.3f, 0.3f, 0.3f, 0.35f, 0.5f };

        public StudentFrames boy;
        public StudentFrames girl;

        StudentKind kind = StudentKind.Boy;
        StudentPose pose = StudentPose.Idle;

        public StudentKind Kind => kind;
        public StudentPose Pose => pose;
        public StudentFrames Frames => kind == StudentKind.Girl ? girl : boy;

        protected override Sprite[] CurrentFrames => Frames?.For(pose);
        protected override float CurrentFrameSeconds => FrameSeconds[(int)pose];
        protected override float BlipPitch => kind == StudentKind.Girl ? 1.25f : 1f;

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

        /// <summary>
        /// Where a held prop goes: the body, which mirrors when the Student turns, so the prop stays in
        /// front of the Student whichever way it faces.
        /// </summary>
        public Transform Hands => body != null ? body.transform : transform;

        /// <summary>Walks to x in the carry pose, holding a prop in both arms, and keeps facing that way.</summary>
        public IEnumerator CarryTo(float artX) => Move(artX, Y, WalkSpeed, () => Play(StudentPose.Carry), null);

        /// <summary>Crouches and shuffles to x, optionally looking that way before facing front.</summary>
        public IEnumerator CrouchTo(float artX, float lookSeconds = 0f)
        {
            Play(StudentPose.Crouch);
            yield return Move(artX, Y, CrouchSpeed, null, null);
            if (lookSeconds > 0f) yield return new WaitForSeconds(lookSeconds);
            Face(false);
        }
    }
}

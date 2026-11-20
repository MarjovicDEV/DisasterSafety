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
    /// The Learner's Student: plays a pose, faces left or right, walks to an x and shows a Reaction
    /// in a speech bubble. The x is in art pixels (the centre of the feet), as in ArtSource/README.md.
    /// Everything follows Time.timeScale, so Pause freezes the Student.
    /// </summary>
    public sealed class StudentActor : MonoBehaviour
    {
        /// <summary>Art pixels per second while walking.</summary>
        public const float WalkSpeed = 80f;
        /// <summary>Art pixels per second while shuffling in under the desk.</summary>
        public const float CrouchSpeed = 30f;

        // Frame durations in seconds, in StudentPose order, from the .aseprite files.
        static readonly float[] FrameSeconds = { 0.5f, 0.16f, 0.14f, 0.3f, 0.45f, 0.26f, 0.18f, 0.3f };

        public StudentFrames boy;
        public StudentFrames girl;
        public Image body;
        public SpeechBubble bubble;
        [Tooltip("UI units per art pixel, and the y of the feet above the bottom of the parent, in UI units.")]
        public float unitsPerPixel = 4f;
        public float feetY = 160f;

        StudentKind kind = StudentKind.Boy;
        StudentPose pose = StudentPose.Idle;
        float poseTime;
        float x;
        bool facingLeft;
        Coroutine walking;

        public StudentKind Kind => kind;
        public StudentPose Pose => pose;
        public float X => x;
        public bool FacingLeft => facingLeft;
        public bool IsWalking => walking != null;
        public StudentFrames Frames => kind == StudentKind.Girl ? girl : boy;
        public Sprite CurrentSprite => body != null ? body.sprite : null;

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

        /// <summary>Stands at the x (art pixels) without walking.</summary>
        public void PlaceAt(float artX)
        {
            x = artX;
            var rect = (RectTransform)transform;
            rect.anchoredPosition = new Vector2(Mathf.Round(artX * unitsPerPixel), feetY);
        }

        public void Face(bool left)
        {
            facingLeft = left;
            if (body != null) body.rectTransform.localScale = new Vector3(left ? -1f : 1f, 1f, 1f);
        }

        /// <summary>Shows one frame of a pose, for the screenshot tool, which does not run Update.</summary>
        public void ShowFrame(StudentPose value, int frame)
        {
            pose = value;
            poseTime = frame * FrameSeconds[(int)value];
            Apply();
        }

        public void Say(string line, float seconds = 2f) => bubble.Say(line, seconds);

        /// <summary>Turns, walks to the x at WalkSpeed, then stands idle facing the front.</summary>
        public IEnumerator WalkTo(float artX)
        {
            walking = StartCoroutine(WalkRoutine(artX));
            yield return walking;
            walking = null;
        }

        IEnumerator WalkRoutine(float artX)
        {
            if (!Mathf.Approximately(artX, x))
            {
                Face(artX < x);
                Play(StudentPose.Walk);
                while (!Mathf.Approximately(x, artX))
                {
                    PlaceAt(Mathf.MoveTowards(x, artX, WalkSpeed * Time.deltaTime));
                    yield return null;
                }
            }
            Face(false);
            Play(StudentPose.Idle);
        }

        /// <summary>Crouches and shuffles to the x at CrouchSpeed (under the desk), then stays crouched.</summary>
        public IEnumerator CrouchTo(float artX)
        {
            walking = StartCoroutine(CrouchRoutine(artX));
            yield return walking;
            walking = null;
        }

        IEnumerator CrouchRoutine(float artX)
        {
            Face(artX < x);
            Play(StudentPose.Crouch);
            while (!Mathf.Approximately(x, artX))
            {
                PlaceAt(Mathf.MoveTowards(x, artX, CrouchSpeed * Time.deltaTime));
                yield return null;
            }
            Face(false);
        }

        void Update()
        {
            poseTime += Time.deltaTime;
            Apply();
        }

        void Apply()
        {
            if (body == null) return;
            var frames = Frames?.For(pose);
            if (frames == null || frames.Length == 0) return;
            var index = Mathf.FloorToInt(poseTime / FrameSeconds[(int)pose]) % frames.Length;
            if (body.sprite != frames[index]) body.sprite = frames[index];
        }
    }
}

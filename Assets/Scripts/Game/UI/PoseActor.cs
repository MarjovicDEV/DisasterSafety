using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// A character in a Level scene (the Student, later the Grown-ups): plays the frames of its current
    /// pose, faces left or right by mirroring, moves along the floor and up or down slopes and stairs,
    /// and shows lines in a speech bubble. Positions are in art pixels: x is the centre of the feet, y is
    /// the height of the feet above the Setting's floor line. Everything follows Time.timeScale.
    /// </summary>
    public abstract class PoseActor : MonoBehaviour
    {
        /// <summary>Art pixels per second while walking.</summary>
        public const float WalkSpeed = 80f;
        /// <summary>Art pixels per second for a hurried walk, such as up the path to high ground.</summary>
        public const float FastWalkSpeed = 120f;
        /// <summary>Art pixels per second for the start of a run.</summary>
        public const float RunSpeed = 150f;

        public Image body;
        public SpeechBubble bubble;
        [Tooltip("UI units per art pixel, and the y of the feet above the bottom of the parent, in UI units.")]
        public float unitsPerPixel = 4f;
        public float feetY = 160f;

        protected float poseTime;
        /// <summary>How fast the frames play: 1 is the pose's own speed. A run plays the walk faster.</summary>
        protected float frameRate = 1f;
        float x;
        float y;
        bool facingLeft;
        Coroutine moving;

        public float X => x;
        public float Y => y;
        public bool FacingLeft => facingLeft;
        public bool IsWalking => moving != null;
        public Sprite CurrentSprite => body != null ? body.sprite : null;

        /// <summary>The frames of the current pose.</summary>
        protected abstract Sprite[] CurrentFrames { get; }
        /// <summary>The seconds each frame of the current pose shows.</summary>
        protected abstract float CurrentFrameSeconds { get; }

        /// <summary>Stands at the x (art pixels) at the current height, without walking.</summary>
        public void PlaceAt(float artX) => PlaceAt(artX, y);

        /// <summary>Stands at the x and the height above the floor line (art pixels), without walking.</summary>
        public void PlaceAt(float artX, float artY)
        {
            x = artX;
            y = artY;
            var rect = (RectTransform)transform;
            rect.anchoredPosition = new Vector2(Mathf.Round(artX * unitsPerPixel), feetY + Mathf.Round(artY * unitsPerPixel));
        }

        public void Face(bool left)
        {
            facingLeft = left;
            if (body != null) body.rectTransform.localScale = new Vector3(left ? -1f : 1f, 1f, 1f);
        }

        /// <summary>Shows the line in the speech bubble and plays the voice blip at this actor's pitch.</summary>
        public void Say(string line, float seconds = 2f)
        {
            bubble.Say(line, seconds);
            AudioManager.PlayBlip(BlipPitch);
        }

        /// <summary>The pitch of this actor's voice blip.</summary>
        protected virtual float BlipPitch => 1f;

        /// <summary>
        /// Moves in a straight line to the point at the speed, facing the way it goes. startPose plays when
        /// the move begins; settlePose (when given) plays at the end, facing the front.
        /// </summary>
        protected IEnumerator Move(float artX, float artY, float speed, Action startPose, Action settlePose)
        {
            moving = StartCoroutine(MoveRoutine(artX, artY, speed, startPose, settlePose));
            yield return moving;
            moving = null;
        }

        IEnumerator MoveRoutine(float artX, float artY, float speed, Action startPose, Action settlePose)
        {
            var to = new Vector2(artX, artY);
            if (new Vector2(x, y) != to)
            {
                if (!Mathf.Approximately(artX, x)) Face(artX < x);
                startPose?.Invoke();
                while (new Vector2(x, y) != to)
                {
                    var p = Vector2.MoveTowards(new Vector2(x, y), to, speed * Time.deltaTime);
                    PlaceAt(p.x, p.y);
                    yield return null;
                }
                PlaceAt(artX, artY);
            }
            frameRate = 1f;
            if (settlePose != null)
            {
                Face(false);
                settlePose();
            }
        }

        protected virtual void Update()
        {
            poseTime += Time.deltaTime;
            Apply();
        }

        protected void Apply()
        {
            if (body == null) return;
            var frames = CurrentFrames;
            if (frames == null || frames.Length == 0) return;
            var index = Mathf.FloorToInt(poseTime * frameRate / CurrentFrameSeconds) % frames.Length;
            if (body.sprite != frames[index]) body.sprite = frames[index];
        }
    }
}

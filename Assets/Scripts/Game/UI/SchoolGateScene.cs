using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DisasterSafety.Game.UI
{
    /// <summary>Where the school gate scene is in its loop.</summary>
    public enum GatePhase { Closed, Opening, Entering, Closing }

    /// <summary>
    /// The morning at the school gate behind the Main Menu: the guard opens the gate, the Students
    /// walk up the driveway and into the school, then the guard closes the gate, and it starts again.
    /// The loop restarts from a closed gate each time the scene is shown.
    /// </summary>
    public sealed class SchoolGateScene : MonoBehaviour
    {
        public Image gate;
        [Tooltip("Closed first, open last.")]
        public Sprite[] gateFrames;
        public SpriteAnimator guard;
        public Sprite[] guardIdle;
        public Sprite guardReach;
        public Sprite[] guardWave;
        [Tooltip("The Students who walk in, bottom-pivoted, in the order they set off.")]
        public RectTransform[] students;
        public float[] studentDelays = { 0f, 1.2f };

        [Tooltip("UI y of the Students' feet: below the screen, at the gate, and where they are inside.")]
        public float startY = -196f;
        public float gateY = 244f;
        public float insideY = 292f;
        public float walkSpeed = 90f;

        public float firstWait = 1.5f;
        public float closedWait = 4f;
        public float gateFrameSeconds = 0.15f;

        public GatePhase Phase { get; private set; }
        public int StudentsInside { get; private set; }

        void OnEnable()
        {
            ShowClosed();
            StartCoroutine(Loop());
        }

        void OnDisable() => StopAllCoroutines();

        void ShowClosed()
        {
            Phase = GatePhase.Closed;
            gate.sprite = gateFrames[0];
            SetGuard(guardIdle);
            StudentsInside = 0;
            foreach (var s in students) Place(s, startY);
        }

        IEnumerator Loop()
        {
            var wait = firstWait;
            while (true)
            {
                yield return new WaitForSeconds(wait);
                wait = closedWait;

                Phase = GatePhase.Opening;
                SetGuard(new[] { guardReach });
                for (var i = 1; i < gateFrames.Length; i++)
                {
                    yield return new WaitForSeconds(gateFrameSeconds);
                    gate.sprite = gateFrames[i];
                }
                yield return new WaitForSeconds(0.3f);

                Phase = GatePhase.Entering;
                SetGuard(guardWave);
                StudentsInside = 0;
                for (var i = 0; i < students.Length; i++)
                    StartCoroutine(WalkIn(students[i], i < studentDelays.Length ? studentDelays[i] : i));
                while (StudentsInside < students.Length) yield return null;
                SetGuard(guardIdle);
                yield return new WaitForSeconds(0.6f);

                Phase = GatePhase.Closing;
                SetGuard(new[] { guardReach });
                for (var i = gateFrames.Length - 2; i >= 0; i--)
                {
                    yield return new WaitForSeconds(gateFrameSeconds);
                    gate.sprite = gateFrames[i];
                }
                yield return new WaitForSeconds(0.3f);
                SetGuard(guardIdle);
                Phase = GatePhase.Closed;
            }
        }

        IEnumerator WalkIn(RectTransform student, float delay)
        {
            yield return new WaitForSeconds(delay);
            var animator = student.GetComponent<SpriteAnimator>();
            if (animator != null) animator.enabled = true;
            for (var y = startY; y < insideY; y += walkSpeed * Time.deltaTime)
            {
                Place(student, y);
                yield return null;
            }
            Place(student, startY);
            if (animator != null) animator.enabled = false;
            StudentsInside++;
        }

        /// <summary>Puts a Student's feet at this height, snapped to the art grid, fading out past the gate.</summary>
        void Place(RectTransform student, float y)
        {
            var p = student.anchoredPosition;
            student.anchoredPosition = new Vector2(p.x, Mathf.Round(y / 4f) * 4f);
            var image = student.GetComponent<Image>();
            var c = image.color;
            c.a = y <= startY ? 0f : 1f - Mathf.Clamp01((y - gateY) / (insideY - gateY));
            image.color = c;
        }

        void SetGuard(Sprite[] frames)
        {
            guard.frames = frames;
            guard.GetComponent<Image>().sprite = frames[0];
        }
    }
}

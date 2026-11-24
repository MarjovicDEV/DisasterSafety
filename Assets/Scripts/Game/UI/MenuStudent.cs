using DisasterSafety.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DisasterSafety.Game.UI
{
    /// <summary>
    /// The Learner's Student on the Main Menu (it replaces Bantay there). It shows the picked kind, the boy
    /// until a pick exists, says the menu lines in its speech bubble with the voice blip, and says the next
    /// Tip when tapped. It plays Happy while greeting and Idle otherwise.
    /// </summary>
    public sealed class MenuStudent : MonoBehaviour, IPointerClickHandler
    {
        public StudentActor actor;
        [Tooltip("Seconds a line stays in the speech bubble.")]
        public float sayDuration = 4.5f;

        int tipIndex;

        public SpeechBubble Bubble => actor.bubble;

        void Awake()
        {
            tipIndex = Random.Range(0, UIStrings.Tips.Length);
        }

        /// <summary>Shows the Student the Learner picked; None shows the boy.</summary>
        public void SetKind(StudentKind kind)
        {
            actor.SetKind(kind);
            actor.Play(actor.Pose);
        }

        /// <summary>Says a greeting in the Happy pose; the Student goes back to Idle when the bubble hides.</summary>
        public void Greet(string line)
        {
            actor.Play(StudentPose.Happy);
            actor.Say(line, sayDuration);
        }

        /// <summary>Says a line in the Idle pose.</summary>
        public void Say(string line)
        {
            actor.Play(StudentPose.Idle);
            actor.Say(line, sayDuration);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Say(UIStrings.Tips[tipIndex]);
            tipIndex = (tipIndex + 1) % UIStrings.Tips.Length;
        }

        void Update()
        {
            if (actor.Pose == StudentPose.Happy && !Bubble.gameObject.activeSelf) actor.Play(StudentPose.Idle);
        }
    }
}

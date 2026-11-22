using System;
using UnityEngine;

namespace DisasterSafety.Game.UI
{
    /// <summary>What one Act-out step does.</summary>
    public enum ActOutStepKind
    {
        /// <summary>Walk to (x, y).</summary>
        Walk,
        /// <summary>Walk fast to (x, y), such as up the path to high ground.</summary>
        FastWalk,
        /// <summary>Start to run to (x, y). Only Unsafe Act-outs run, and they stop soon after.</summary>
        Run,
        /// <summary>Crouch and shuffle to x, optionally looking in the direction of travel for the seconds.</summary>
        Crouch,
        /// <summary>Play a pose and hold it for the seconds.</summary>
        Pose,
        /// <summary>Say the line in the speech bubble for the seconds (at least 1).</summary>
        Say,
        /// <summary>Wait for the seconds.</summary>
        Wait,
        /// <summary>Calm the shaking (a "during" Scenario, once the Student is in cover).</summary>
        Calm,
        /// <summary>Show the target, a front overlay such as the front of the desk.</summary>
        Overlay,
        /// <summary>Show the target prop.</summary>
        PropShow,
        /// <summary>Hide the target prop.</summary>
        PropHide,
        /// <summary>The Student picks up the target prop; it moves with the Student's hands.</summary>
        PropPickUp,
        /// <summary>Put the target prop down at place (UI units in its own parent).</summary>
        PropPlace,
        /// <summary>Slide the target prop by x art pixels over the seconds (furniture being pushed).</summary>
        PropSlide,
        /// <summary>A Grown-up acts: see GrownUpAction.</summary>
        GrownUp,
    }

    /// <summary>What a GrownUp step makes the Grown-up do.</summary>
    public enum GrownUpAction
    {
        /// <summary>Walk to (x, y) and stand idle facing the front.</summary>
        Walk,
        /// <summary>Play a pose, and hold it for the seconds (0: carry on at once).</summary>
        Pose,
        /// <summary>Face left or right.</summary>
        Turn,
    }

    /// <summary>One step of an Act-out. Which fields count depends on the kind.</summary>
    [Serializable]
    public sealed class ActOutStep
    {
        public ActOutStepKind kind;
        [Tooltip("Walk, FastWalk, Run, Crouch: the Student's centre x in art pixels. PropSlide: the distance in art pixels.")]
        public float x;
        [Tooltip("Walk, FastWalk, Run: the height of the feet above the floor line, in art pixels.")]
        public float y;
        public StudentPose pose;
        [Tooltip("Crouch, Pose, Say, Wait, PropSlide: seconds.")]
        public float seconds;
        [Tooltip("Say: the line, from UIStrings.")]
        public string line;
        [Tooltip("Overlay: the object shown. Prop steps: the object with the LevelProp.")]
        public GameObject target;
        [Tooltip("PropPlace: where the prop goes (its anchored position, UI units).")]
        public Vector2 place;

        [Tooltip("GrownUp: who acts. Walk: who walks alongside the Student, if anyone.")]
        public GrownUpActor grownUp;
        [Tooltip("Walk with a Grown-up: their x and height above the floor line, in art pixels.")]
        public Vector2 grownUpDestination;
        [Tooltip("GrownUp: what the Grown-up does. Walk uses x and y; Pose uses grownUpPose and seconds; Turn uses faceLeft.")]
        public GrownUpAction grownUpAction;
        public GrownUpPose grownUpPose;
        [Tooltip("GrownUp, Turn: face left (true) or right.")]
        public bool faceLeft;

        /// <summary>True for the steps that move the Student.</summary>
        public bool Moves => kind == ActOutStepKind.Walk || kind == ActOutStepKind.FastWalk
                             || kind == ActOutStepKind.Run || kind == ActOutStepKind.Crouch;

        public static ActOutStep Walk(float x, float y = 0f) => new ActOutStep { kind = ActOutStepKind.Walk, x = x, y = y };
        public static ActOutStep WalkWith(GrownUpActor who, float x, float y, Vector2 destination) =>
            new ActOutStep { kind = ActOutStepKind.Walk, x = x, y = y, grownUp = who, grownUpDestination = destination };
        public static ActOutStep FastWalk(float x, float y = 0f) => new ActOutStep { kind = ActOutStepKind.FastWalk, x = x, y = y };
        public static ActOutStep Run(float x, float y = 0f) => new ActOutStep { kind = ActOutStepKind.Run, x = x, y = y };
        public static ActOutStep Crouch(float x, float lookSeconds = 0f) =>
            new ActOutStep { kind = ActOutStepKind.Crouch, x = x, seconds = lookSeconds };
        public static ActOutStep Pose(StudentPose pose, float seconds = 0f) => new ActOutStep { kind = ActOutStepKind.Pose, pose = pose, seconds = seconds };
        public static ActOutStep Say(string line, float seconds = 1f) => new ActOutStep { kind = ActOutStepKind.Say, line = line, seconds = seconds };
        public static ActOutStep Wait(float seconds) => new ActOutStep { kind = ActOutStepKind.Wait, seconds = seconds };
        public static ActOutStep Calm() => new ActOutStep { kind = ActOutStepKind.Calm };
        public static ActOutStep Overlay(GameObject target) => new ActOutStep { kind = ActOutStepKind.Overlay, target = target };
        public static ActOutStep PropShow(GameObject prop) => new ActOutStep { kind = ActOutStepKind.PropShow, target = prop };
        public static ActOutStep PropHide(GameObject prop) => new ActOutStep { kind = ActOutStepKind.PropHide, target = prop };
        public static ActOutStep PropPickUp(GameObject prop) => new ActOutStep { kind = ActOutStepKind.PropPickUp, target = prop };
        public static ActOutStep PropPlace(GameObject prop, Vector2 place) => new ActOutStep { kind = ActOutStepKind.PropPlace, target = prop, place = place };
        public static ActOutStep GrownUpWalk(GrownUpActor who, float x, float y = 0f) => new ActOutStep { kind = ActOutStepKind.GrownUp, grownUp = who, grownUpAction = GrownUpAction.Walk, x = x, y = y };
        public static ActOutStep GrownUpPoseStep(GrownUpActor who, GrownUpPose pose, float seconds = 0f) => new ActOutStep { kind = ActOutStepKind.GrownUp, grownUp = who, grownUpAction = GrownUpAction.Pose, grownUpPose = pose, seconds = seconds };
        public static ActOutStep GrownUpTurn(GrownUpActor who, bool left) => new ActOutStep { kind = ActOutStepKind.GrownUp, grownUp = who, grownUpAction = GrownUpAction.Turn, faceLeft = left };
        public static ActOutStep PropSlide(GameObject prop, float artPixels, float seconds) => new ActOutStep { kind = ActOutStepKind.PropSlide, target = prop, x = artPixels, seconds = seconds };
    }

    /// <summary>
    /// How the Student acts out one Choice in a Setting, step by step (see CONTEXT.md, Act-out). A Safe
    /// Choice plays every step; an Unsafe Choice plays up to stopAfter, then the Student says "Hmm, wait...".
    /// </summary>
    [Serializable]
    public sealed class ActOut
    {
        [Tooltip("The Choice's action key in the Scenario file.")]
        public string key;
        public ActOutStep[] steps = new ActOutStep[0];
        [Tooltip("An Unsafe Choice plays the steps up to and including this index, then stops. Below 0: every step.")]
        public int stopAfter = -1;
        [Tooltip("The pose for the Unsafe Reaction. Use a low pose if the Student is still under cover.")]
        public StudentPose stopPose = StudentPose.Wait;

        /// <summary>The index of the last step a Safe (false) or Unsafe (true) Choice plays.</summary>
        public int LastStep(bool stops) =>
            stops && stopAfter >= 0 ? Mathf.Min(stopAfter, steps.Length - 1) : steps.Length - 1;

        /// <summary>Where the Student stands after the steps up to and including last, starting from startX.</summary>
        public float XAfter(int last, float startX)
        {
            var x = startX;
            for (var i = 0; i <= last && i < steps.Length; i++)
                if (steps[i].Moves) x = steps[i].x;
            return x;
        }

        /// <summary>Where a Safe Choice ends.</summary>
        public float EndX(float startX) => XAfter(LastStep(false), startX);

        /// <summary>Where an Unsafe Choice stops.</summary>
        public float StopX(float startX) => XAfter(LastStep(true), startX);

        /// <summary>The last pose a Safe Choice plays (Idle when it plays none).</summary>
        public StudentPose EndPose
        {
            get
            {
                for (var i = steps.Length - 1; i >= 0; i--)
                    if (steps[i].kind == ActOutStepKind.Pose) return steps[i].pose;
                return StudentPose.Idle;
            }
        }

        /// <summary>The first front overlay the Act-out shows, or null.</summary>
        public GameObject FirstOverlay =>
            Array.Find(steps, s => s.kind == ActOutStepKind.Overlay)?.target;
    }
}

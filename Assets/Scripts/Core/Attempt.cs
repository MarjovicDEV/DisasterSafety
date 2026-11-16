using System;

namespace DisasterSafety.Core
{
    public enum AttemptPhase
    {
        /// <summary>The Scenario is being introduced; Choices are not tappable yet.</summary>
        Reading,
        Choosing,
        ShowingFeedback,
        Completed
    }

    /// <summary>
    /// One play of a Level. After an Unsafe Choice the Learner sees Feedback and chooses again
    /// in the same Attempt; a tried Unsafe Choice cannot be picked twice. Nothing here is saved:
    /// only LevelCompletion writes to the save file, and only for a Completed Attempt.
    /// </summary>
    public sealed class Attempt
    {
        bool[] tried;

        public Scenario Scenario { get; }
        public AttemptPhase Phase { get; private set; }
        public int UnsafeChoices { get; private set; }
        public bool HintUsed { get; private set; }
        public int LastChoice { get; private set; }
        public bool LastChoiceSafe { get; private set; }

        public int Stars => StarRule.For(UnsafeChoices, HintUsed);

        public Attempt(Scenario scenario)
        {
            Scenario = scenario ?? throw new ArgumentNullException(nameof(scenario));
            Restart();
        }

        /// <summary>Pause > Restart: the same Level from the beginning.</summary>
        public void Restart()
        {
            tried = new bool[Scenario.choices?.Length ?? 0];
            Phase = AttemptPhase.Reading;
            UnsafeChoices = 0;
            HintUsed = false;
            LastChoice = -1;
            LastChoiceSafe = false;
        }

        public bool IsTried(int index) => index >= 0 && index < tried.Length && tried[index];

        public void StartChoosing()
        {
            if (Phase == AttemptPhase.Reading) Phase = AttemptPhase.Choosing;
        }

        /// <summary>The Hint may be read any time before the Safe Choice. It is counted once.</summary>
        public void UseHint()
        {
            if (Phase == AttemptPhase.Reading || Phase == AttemptPhase.Choosing) HintUsed = true;
        }

        /// <summary>Returns false when the Choice cannot be picked now.</summary>
        public bool Choose(int index)
        {
            if (Phase != AttemptPhase.Choosing || index < 0 || index >= tried.Length || tried[index]) return false;
            LastChoice = index;
            LastChoiceSafe = Scenario.choices[index].safe;
            if (!LastChoiceSafe)
            {
                tried[index] = true;
                UnsafeChoices++;
            }
            Phase = AttemptPhase.ShowingFeedback;
            return true;
        }

        /// <summary>From an Unsafe Choice's Feedback back to the Choices.</summary>
        public void TryAgain()
        {
            if (Phase != AttemptPhase.ShowingFeedback || LastChoiceSafe)
                throw new InvalidOperationException("Try Again follows an Unsafe Choice only.");
            Phase = AttemptPhase.Choosing;
        }

        /// <summary>From the Safe Choice's Feedback to Level Completed.</summary>
        public void Complete()
        {
            if (Phase != AttemptPhase.ShowingFeedback || !LastChoiceSafe)
                throw new InvalidOperationException("A Level is completed only by the Safe Choice.");
            Phase = AttemptPhase.Completed;
        }
    }
}

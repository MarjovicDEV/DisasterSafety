using System;

namespace DisasterSafety.Core
{
    /// <summary>
    /// Stars for a Completed Level: start at 3, lose one for each Unsafe Choice and one for
    /// using the Hint, and never fewer than 1. Stars report how the activity went only.
    /// </summary>
    public static class StarRule
    {
        public const int MinStars = 1;

        public static int For(int unsafeChoices, bool hintUsed)
        {
            var stars = ProgressState.MaxStars - Math.Max(0, unsafeChoices) - (hintUsed ? 1 : 0);
            return Math.Max(MinStars, Math.Min(ProgressState.MaxStars, stars));
        }
    }
}

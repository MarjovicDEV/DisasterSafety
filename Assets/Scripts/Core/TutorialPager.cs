using System;

namespace DisasterSafety.Core
{
    /// <summary>
    /// Which Tutorial Page of How to Play is showing. Back on the first page is refused
    /// so the caller can leave How to Play instead.
    /// </summary>
    public sealed class TutorialPager
    {
        public int Count { get; }
        public int Index { get; private set; }
        public bool IsFirst => Index == 0;
        public bool IsLast => Index == Count - 1;

        /// <summary>Raised with the new page index after every move.</summary>
        public event Action<int> Changed;

        public TutorialPager(int count)
        {
            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count), "A tutorial needs at least one page.");
            Count = count;
        }

        public bool Next() => MoveTo(Index + 1);
        public bool Previous() => MoveTo(Index - 1);
        public void Restart() => MoveTo(0);

        bool MoveTo(int index)
        {
            if (index < 0 || index >= Count || index == Index) return false;
            Index = index;
            Changed?.Invoke(index);
            return true;
        }
    }
}

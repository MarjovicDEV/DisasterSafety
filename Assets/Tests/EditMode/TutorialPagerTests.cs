using System;
using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class TutorialPagerTests
    {
        [Test]
        public void StartsOnFirstPage()
        {
            var p = new TutorialPager(5);
            Assert.That(p.Index, Is.EqualTo(0));
            Assert.That(p.IsFirst, Is.True);
            Assert.That(p.IsLast, Is.False);
        }

        [Test]
        public void Next_MovesForward_AndStopsOnLastPage()
        {
            var p = new TutorialPager(3);
            Assert.That(p.Next(), Is.True);
            Assert.That(p.Next(), Is.True);
            Assert.That(p.IsLast, Is.True);
            Assert.That(p.Next(), Is.False);
            Assert.That(p.Index, Is.EqualTo(2));
        }

        [Test]
        public void Previous_MovesBack_AndRefusesOnFirstPage()
        {
            var p = new TutorialPager(3);
            Assert.That(p.Previous(), Is.False, "Back on page 1 leaves the tutorial instead");
            p.Next();
            Assert.That(p.Previous(), Is.True);
            Assert.That(p.Index, Is.EqualTo(0));
        }

        [Test]
        public void Restart_ReturnsToFirstPage()
        {
            var p = new TutorialPager(4);
            p.Next();
            p.Next();
            p.Restart();
            Assert.That(p.Index, Is.EqualTo(0));
        }

        [Test]
        public void Changed_ReportsTheNewPageOnlyWhenItMoves()
        {
            var p = new TutorialPager(2);
            var seen = -1;
            var calls = 0;
            p.Changed += i => { seen = i; calls++; };
            p.Previous();
            p.Next();
            p.Next();
            Assert.That(calls, Is.EqualTo(1));
            Assert.That(seen, Is.EqualTo(1));
        }

        [Test]
        public void SinglePage_IsBothFirstAndLast()
        {
            var p = new TutorialPager(1);
            Assert.That(p.IsFirst && p.IsLast, Is.True);
        }

        [Test]
        public void RequiresAtLeastOnePage()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TutorialPager(0));
        }
    }
}

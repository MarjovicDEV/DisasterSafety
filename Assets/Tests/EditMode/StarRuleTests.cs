using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class StarRuleTests
    {
        [TestCase(0, false, 3)]
        [TestCase(0, true, 2)]
        [TestCase(1, false, 2)]
        [TestCase(1, true, 1)]
        [TestCase(2, false, 1)]
        [TestCase(2, true, 1)]
        [TestCase(3, false, 1)]
        [TestCase(3, true, 1)]
        public void Stars_LoseOnePerUnsafeChoiceAndOneForTheHint(int unsafeChoices, bool hintUsed, int expected)
        {
            Assert.That(StarRule.For(unsafeChoices, hintUsed), Is.EqualTo(expected));
        }

        [Test]
        public void Stars_NeverDropBelowOneOnCompletion()
        {
            Assert.That(StarRule.For(50, true), Is.EqualTo(StarRule.MinStars));
            Assert.That(StarRule.MinStars, Is.EqualTo(1));
        }

        [Test]
        public void Stars_NeverExceedThree()
        {
            Assert.That(StarRule.For(0, false), Is.EqualTo(ProgressState.MaxStars));
            Assert.That(StarRule.For(-4, false), Is.EqualTo(ProgressState.MaxStars));
        }
    }
}

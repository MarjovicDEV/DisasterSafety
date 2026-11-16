using System;
using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class AttemptTests
    {
        static Attempt Choosing()
        {
            var a = new Attempt(TestScenarios.Classroom());
            a.StartChoosing();
            return a;
        }

        [Test]
        public void NewAttempt_StartsReading_WithNothingTried()
        {
            var a = new Attempt(TestScenarios.Classroom());
            Assert.That(a.Phase, Is.EqualTo(AttemptPhase.Reading));
            Assert.That(a.UnsafeChoices, Is.EqualTo(0));
            Assert.That(a.HintUsed, Is.False);
            Assert.That(a.Stars, Is.EqualTo(3));
        }

        [Test]
        public void CannotChoose_WhileReading()
        {
            var a = new Attempt(TestScenarios.Classroom());
            Assert.That(a.Choose(TestScenarios.Safe), Is.False);
            Assert.That(a.Phase, Is.EqualTo(AttemptPhase.Reading));
        }

        [Test]
        public void SafeChoiceFirst_ShowsFeedback_ThenCompletesWithThreeStars()
        {
            var a = Choosing();
            Assert.That(a.Choose(TestScenarios.Safe), Is.True);
            Assert.That(a.Phase, Is.EqualTo(AttemptPhase.ShowingFeedback));
            Assert.That(a.LastChoiceSafe, Is.True);
            Assert.That(a.LastChoice, Is.EqualTo(TestScenarios.Safe));

            a.Complete();
            Assert.That(a.Phase, Is.EqualTo(AttemptPhase.Completed));
            Assert.That(a.Stars, Is.EqualTo(3));
        }

        [Test]
        public void UnsafeChoice_ThenSafeChoice_CompletesWithTwoStars()
        {
            var a = Choosing();
            Assert.That(a.Choose(0), Is.True);
            Assert.That(a.Phase, Is.EqualTo(AttemptPhase.ShowingFeedback));
            Assert.That(a.LastChoiceSafe, Is.False);
            Assert.That(a.IsTried(0), Is.True);

            a.TryAgain();
            Assert.That(a.Phase, Is.EqualTo(AttemptPhase.Choosing));
            a.Choose(TestScenarios.Safe);
            a.Complete();
            Assert.That(a.UnsafeChoices, Is.EqualTo(1));
            Assert.That(a.Stars, Is.EqualTo(2));
        }

        [Test]
        public void TriedUnsafeChoice_CannotBePickedAgain()
        {
            var a = Choosing();
            a.Choose(2);
            a.TryAgain();
            Assert.That(a.Choose(2), Is.False);
            Assert.That(a.Phase, Is.EqualTo(AttemptPhase.Choosing));
            Assert.That(a.UnsafeChoices, Is.EqualTo(1));
        }

        [Test]
        public void HintUse_IsRecordedOnce_AndCostsOneStar()
        {
            var a = new Attempt(TestScenarios.Classroom());
            a.UseHint();
            a.StartChoosing();
            a.UseHint();
            Assert.That(a.HintUsed, Is.True);
            a.Choose(TestScenarios.Safe);
            a.Complete();
            Assert.That(a.Stars, Is.EqualTo(2));
        }

        [Test]
        public void CannotComplete_WithoutTheSafeChoice()
        {
            var a = Choosing();
            Assert.Throws<InvalidOperationException>(a.Complete);
            a.Choose(0);
            Assert.Throws<InvalidOperationException>(a.Complete);
            Assert.That(a.Phase, Is.EqualTo(AttemptPhase.ShowingFeedback));
        }

        [Test]
        public void TryAgain_AfterSafeChoice_IsRefused()
        {
            var a = Choosing();
            a.Choose(TestScenarios.Safe);
            Assert.Throws<InvalidOperationException>(a.TryAgain);
        }

        [Test]
        public void EveryUnsafeChoiceTried_StillEndsOnTheSafeChoiceWithOneStar()
        {
            var a = Choosing();
            a.UseHint();
            foreach (var i in new[] { 0, 2, 3 })
            {
                a.Choose(i);
                a.TryAgain();
            }
            a.Choose(TestScenarios.Safe);
            a.Complete();
            Assert.That(a.Stars, Is.EqualTo(1));
        }

        [Test]
        public void Restart_ResetsTheAttempt()
        {
            var a = Choosing();
            a.UseHint();
            a.Choose(0);
            a.Restart();
            Assert.That(a.Phase, Is.EqualTo(AttemptPhase.Reading));
            Assert.That(a.UnsafeChoices, Is.EqualTo(0));
            Assert.That(a.HintUsed, Is.False);
            Assert.That(a.IsTried(0), Is.False);
            Assert.That(a.LastChoice, Is.EqualTo(-1));
        }

        [Test]
        public void ChoiceOutOfRange_IsRefused()
        {
            var a = Choosing();
            Assert.That(a.Choose(-1), Is.False);
            Assert.That(a.Choose(4), Is.False);
        }
    }
}

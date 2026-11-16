using DisasterSafety.Core;
using NUnit.Framework;

namespace DisasterSafety.Tests
{
    public class DisplayNameRuleTests
    {
        [TestCase("Ana", "Ana")]
        [TestCase("  Ana  ", "Ana")]
        [TestCase("Ana   Maria", "Ana Maria")]
        [TestCase("Niño", "Niño")]
        [TestCase("PEÑA", "PEÑA")]
        [TestCase("Kid 7", "Kid 7")]
        [TestCase("abcdefghijkl", "abcdefghijkl")]
        public void Accepts_AndNormalises(string raw, string expected)
        {
            Assert.That(DisplayNameRule.Check(raw, out var name), Is.EqualTo(DisplayNameError.None));
            Assert.That(name, Is.EqualTo(expected));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void Rejects_Empty(string raw)
        {
            Assert.That(DisplayNameRule.Check(raw, out _), Is.EqualTo(DisplayNameError.Empty));
        }

        [Test]
        public void Rejects_MoreThanTwelveCharacters()
        {
            Assert.That(DisplayNameRule.Check("abcdefghijklm", out _), Is.EqualTo(DisplayNameError.TooLong));
        }

        [TestCase("Ana!")]
        [TestCase("<b>")]
        [TestCase("Ana_Maria")]
        [TestCase("José")]
        public void Rejects_CharactersOutsideTheFont(string raw)
        {
            Assert.That(DisplayNameRule.Check(raw, out _), Is.EqualTo(DisplayNameError.InvalidCharacter));
        }
    }
}

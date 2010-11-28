using System;
using NUnit.Framework;

namespace DiffLib.Tests
{
    [TestFixture]
    public class LongestCommonSubstringResultTests
    {
        [TestCase(0, 0, 1)]
        [TestCase(17, 0, 1)]
        [TestCase(0, 17, 1)]
        [TestCase(0, 0, 17)]
        public void Constructor_AssignsPropertiesCorrectly(int p1, int p2, int l)
        {
            var lcsr = new LongestCommonSubstringResult(p1, p2, l);

            Assert.That(lcsr.PositionInCollection1, Is.EqualTo(p1));
            Assert.That(lcsr.PositionInCollection2, Is.EqualTo(p2));
            Assert.That(lcsr.Length, Is.EqualTo(l));
        }

        [Test]
        public void Constructor_NegativeLength_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LongestCommonSubstringResult(0, 0, -1));
        }

        [Test]
        public void Constructor_NegativePositionInCollection1_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LongestCommonSubstringResult(-1, 0, 1));
        }

        [Test]
        public void Constructor_NegativePositionInCollection2_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LongestCommonSubstringResult(0, -1, 1));
        }

        [Test]
        public void Constructor_ZeroLength_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LongestCommonSubstringResult(0, 0, 0));
        }
    }
}
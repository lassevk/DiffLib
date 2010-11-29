using System;
using NUnit.Framework;

namespace DiffLib.Tests
{
    [TestFixture]
    public class DiffChangeTests
    {
        [TestCase(true, 1, 1)]
        [TestCase(false, 1, 1)]
        [TestCase(true, 3, 3)]
        [TestCase(false, 5, 7)]
        public void Constructor_InitializesPropertiesCorrectly(bool equal, int length1, int length2)
        {
            var ds = new DiffChange(equal, length1, length2);

            Assert.That(ds.Equal, Is.EqualTo(equal));
            Assert.That(ds.Length1, Is.EqualTo(length1));
            Assert.That(ds.Length2, Is.EqualTo(length2));
        }

        [TestCase(true, 1, 1)]
        [TestCase(false, 1, 1)]
        [TestCase(true, 3, 3)]
        [TestCase(false, 5, 7)]
        public void TwoInstances_WithSameProperties_CompareEqual(bool equal, int length1, int length2)
        {
            var ds1 = new DiffChange(equal, length1, length2);
            var ds2 = new DiffChange(equal, length1, length2);

            Assert.That(ds1.Equals(ds2), Is.True);
        }

        [TestCase(true, 1, 1)]
        [TestCase(false, 1, 1)]
        [TestCase(true, 3, 3)]
        [TestCase(false, 5, 7)]
        public void OneInstances_ComparesEqualToItself(bool equal, int length1, int length2)
        {
            var ds = new DiffChange(equal, length1, length2);

            Assert.That(ds.Equals(ds), Is.True);
        }

        [TestCase(true, 1, 1)]
        [TestCase(false, 1, 1)]
        [TestCase(true, 3, 3)]
        [TestCase(false, 5, 7)]
        public void OneInstances_ComparesUnequalToNull(bool equal, int length1, int length2)
        {
            var ds = new DiffChange(equal, length1, length2);

            Assert.That(ds.Equals(null), Is.False);
        }

        [TestCase(true, 1, 1)]
        [TestCase(false, 1, 1)]
        [TestCase(true, 3, 3)]
        [TestCase(false, 5, 7)]
        public void OneInstances_ComparesEqualToItselfThroughObjectEquals(bool equal, int length1, int length2)
        {
            var ds = new DiffChange(equal, length1, length2);

            Assert.That(ds.Equals((object) ds), Is.True);
        }

        [TestCase(true, 1, 1)]
        [TestCase(false, 1, 1)]
        [TestCase(true, 3, 3)]
        [TestCase(false, 5, 7)]
        public void TwoInstances_WithSameProperties_CompareEqualThroughObjectEquals(bool equal, int length1, int length2)
        {
            var ds1 = new DiffChange(equal, length1, length2);
            object ds2 = new DiffChange(equal, length1, length2);

            Assert.That(ds1.Equals(ds2), Is.True);
        }

        [TestCase(true, 1, 1)]
        [TestCase(false, 1, 1)]
        [TestCase(true, 3, 3)]
        [TestCase(false, 5, 7)]
        public void TwoInstances_WithSameProperties_ProduceSameHashCodes(bool equal, int length1, int length2)
        {
            var ds1 = new DiffChange(equal, length1, length2);
            var ds2 = new DiffChange(equal, length1, length2);

            Assert.That(ds1.GetHashCode(), Is.EqualTo(ds2.GetHashCode()));
        }

        [Test]
        public void Constructor_DifferentLengthsWhenEqual_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new DiffChange(true, 1, 2));
        }

        [Test]
        public void Constructor_NegativeLength1_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DiffChange(false, -1, 1));
        }

        [Test]
        public void Constructor_NegativeLength2_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DiffChange(false, 1, -1));
        }
    }
}
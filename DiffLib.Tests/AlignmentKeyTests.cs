using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace DiffLib.Tests
{
    [TestFixture]
    public class AlignmentKeyTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Constructor_Position1_RetainedInProperty(int value)
        {
            var key = new AlignmentKey(value, 0);

            Assert.That(key.Position1, Is.EqualTo(value));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Constructor_Position2_RetainedInProperty(int value)
        {
            var key = new AlignmentKey(0, value);

            Assert.That(key.Position2, Is.EqualTo(value));
        }

        [Test]
        [TestCase(0, 1, 0, 2)]
        [TestCase(0, 1, 1, 1)]
        [TestCase(0, 1, 1, 2)]
        public void GetHashCode_OfDifferentKeys_ReturnsDifferentValues(int position1, int position2, int position3, int position4)
        {
            var key1 = new AlignmentKey(position1, position2);
            var key2 = new AlignmentKey(position3, position4);

            var hashcode1 = key1.GetHashCode();
            var hashcode2 = key2.GetHashCode();

            Assert.That(hashcode1, Is.Not.EqualTo(hashcode2));
        }

        [Test]
        public void GetHashCode_OfKeysWithSameValues_ReturnsSameValue()
        {
            var key1 = new AlignmentKey(10, 20);
            var key2 = new AlignmentKey(10, 20);

            var hashcode1 = key1.GetHashCode();
            var hashcode2 = key2.GetHashCode();

            Assert.That(hashcode1, Is.EqualTo(hashcode2));
        }

        [Test]
        [TestCase(0, 1, 0, 2)]
        [TestCase(0, 1, 1, 1)]
        [TestCase(0, 1, 1, 2)]
        public void Equals_OfDifferentKeys_ReturnsFalse(int position1, int position2, int position3, int position4)
        {
            var key1 = new AlignmentKey(position1, position2);
            var key2 = new AlignmentKey(position3, position4);

            var output = key1.Equals(key2);

            Assert.That(output, Is.False);
        }

        [Test]
        public void Equals_OfKeysWithSameValues_ReturnsSameValue()
        {
            var key1 = new AlignmentKey(10, 20);
            var key2 = new AlignmentKey(10, 20);

            var output = key1.Equals(key2);

            Assert.That(output, Is.True);
        }

        [Test]
        [TestCase(0, 1, 0, 2)]
        [TestCase(0, 1, 1, 1)]
        [TestCase(0, 1, 1, 2)]
        public void EqualsObject_OfDifferentKeys_ReturnsFalse(int position1, int position2, int position3, int position4)
        {
            var key1 = new AlignmentKey(position1, position2);
            var key2 = new AlignmentKey(position3, position4);

            var output = key1.Equals((object)key2);

            Assert.That(output, Is.False);
        }

        [Test]
        public void EqualsObject_OfKeysWithSameValues_ReturnsSameValue()
        {
            var key1 = new AlignmentKey(10, 20);
            var key2 = new AlignmentKey(10, 20);

            var output = key1.Equals((object)key2);

            Assert.That(output, Is.True);
        }

        [Test]
        public void EqualsObject_NullObject_ReturnsFalse()
        {
            var key1 = new AlignmentKey(10, 20);

            var output = key1.Equals(null);

            Assert.That(output, Is.False);
        }

        [Test]
        public void EqualsObject_StringObject_ReturnsFalse()
        {
            var key1 = new AlignmentKey(10, 20);

            // ReSharper disable once SuspiciousTypeConversion.Global
            var output = key1.Equals("string");

            Assert.That(output, Is.False);
        }
    }
}

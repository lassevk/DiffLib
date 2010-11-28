using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace DiffLib.Tests
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void ToRandomAccess_OnArray_ReturnsSameInstance()
        {
            var collection = new[] { 1, 2, 3 };
            var randomAccess = collection.ToRandomAccess();

            Assert.That(ReferenceEquals(collection, randomAccess));
        }

        [Test]
        public void ToRandomAccess_OnList_ReturnsSameInstance()
        {
            var collection = new List<int>();
            var randomAccess = collection.ToRandomAccess();

            Assert.That(ReferenceEquals(collection, randomAccess));
        }

        [Test]
        public void ToRandomAccess_OnCollection_ReturnsSameInstance()
        {
            var collection = new Collection<int>();
            var randomAccess = collection.ToRandomAccess();

            Assert.That(ReferenceEquals(collection, randomAccess));
        }
    }
}

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DiffLib.Tests
{
    [TestFixture]
    public class LongestCommonSubstringTests
    {
        [Test]
        public void Find_NullCollection1_ThrowsArgumentNullException()
        {
            List<string> collection1 = null;
            var collection2 = new List<string>();
            EqualityComparer<string> comparer = EqualityComparer<string>.Default;

            Assert.Throws<ArgumentNullException>(() => LongestCommonSubstring.Find(collection1, collection2, comparer));
        }

        [Test]
        public void Find_NullCollection2_ThrowsArgumentNullException()
        {
            var collection1 = new List<string>();
            List<string> collection2 = null;
            EqualityComparer<string> comparer = EqualityComparer<string>.Default;

            Assert.Throws<ArgumentNullException>(() => LongestCommonSubstring.Find(collection1, collection2, comparer));
        }

        [Test]
        public void Find_NullComparer_ThrowsArgumentNullException()
        {
            var collection1 = new List<string>();
            var collection2 = new List<string>();
            EqualityComparer<string> comparer = null;

            Assert.Throws<ArgumentNullException>(() => LongestCommonSubstring.Find(collection1, collection2, comparer));
        }
    }
}
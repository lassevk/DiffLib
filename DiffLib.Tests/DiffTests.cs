using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DiffLib.Tests
{
    [TestFixture]
    public class DiffTests
    {
        [Test]
        public void Constructor_NullCollection1_ThrowsArgumentNullException()
        {
            List<string> collection1 = null;
            var collection2 = new List<string>();
            EqualityComparer<string> comparer = EqualityComparer<string>.Default;

            Assert.Throws<ArgumentNullException>(() => new Diff<string>(collection1, collection2, comparer));
        }

        [Test]
        public void Constructor_NullCollection2_ThrowsArgumentNullException()
        {
            var collection1 = new List<string>();
            List<string> collection2 = null;
            EqualityComparer<string> comparer = EqualityComparer<string>.Default;

            Assert.Throws<ArgumentNullException>(() => new Diff<string>(collection1, collection2, comparer));
        }

        [Test]
        public void Constructor_NullComparer_ThrowsArgumentNullException()
        {
            var collection1 = new List<string>();
            var collection2 = new List<string>();
            EqualityComparer<string> comparer = null;

            Assert.Throws<ArgumentNullException>(() => new Diff<string>(collection1, collection2, comparer));
        }

        [Test]
        public void SimpleDiff_ProducesCorrectResults()
        {
            const string text1 = "This is a test of the diff implementation, with some text that is deleted.";
            const string text2 = "This is another test of the same implementation, with some more text.";

            DiffChange[] diff = new Diff<char>(text1, text2).ToArray();

            CollectionAssert.AreEqual(diff, new[]
                {
                    new DiffChange(true, 9, 9), // same        "This is a"
                    new DiffChange(false, 0, 6), // add        "nother"
                    new DiffChange(true, 13, 13), // same      " test of the "
                    new DiffChange(false, 4, 4), // replace    "same" with "diff"
                    new DiffChange(true, 27, 27), // same      " implementation, with some "
                    new DiffChange(false, 0, 5), // add        "more "
                    new DiffChange(true, 4, 4), // same        "text"
                    new DiffChange(false, 16, 0), // delete    " that is deleted"
                    new DiffChange(true, 1, 1), // same        "."
                });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute

namespace DiffLib.Tests
{
    [TestFixture]
    public class DiffTests
    {
        [Test]
        public void CalculateSections_NullCollection1_ThrowsArgumentNullException()
        {
            List<string> collection1 = null;
            var collection2 = new List<string>();
            EqualityComparer<string> comparer = EqualityComparer<string>.Default;

            Assert.Throws<ArgumentNullException>(() => Diff.CalculateSections(collection1, collection2, comparer));
        }

        [Test]
        public void CalculateSections_NullCollection2_ThrowsArgumentNullException()
        {
            var collection1 = new List<string>();
            List<string> collection2 = null;
            EqualityComparer<string> comparer = EqualityComparer<string>.Default;

            Assert.Throws<ArgumentNullException>(() => Diff.CalculateSections(collection1, collection2, comparer));
        }

        [Test]
        public void SimpleDiff_ProducesCorrectResults()
        {
            const string text1 = "This is a test of the diff implementation, with some text that is deleted.";
            const string text2 = "This is another test of the same implementation, with some more text.";

            DiffSection[] diff = Diff.CalculateSections(text1.ToCharArray(), text2.ToCharArray()).ToArray();

            CollectionAssert.AreEqual(diff, new[]
            {
                new DiffSection(true, 9, 9), // same        "This is a"
                new DiffSection(false, 0, 6), // add        "nother"
                new DiffSection(true, 13, 13), // same      " test of the "
                new DiffSection(false, 4, 4), // replace    "same" with "diff"
                new DiffSection(true, 27, 27), // same      " implementation, with some "
                new DiffSection(false, 0, 5), // add        "more "
                new DiffSection(true, 4, 4), // same        "text"
                new DiffSection(false, 16, 0), // delete    " that is deleted"
                new DiffSection(true, 1, 1), // same        "."
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute

namespace DiffLib.Tests
{
    [TestFixture]
    public class StringSimilarityDiffElementAlignerTests
    {
        [Test]
        public void Align_NullCollection1_ThrowsArgumentNullException()
        {
            var aligner = new StringSimilarityDiffElementAligner();
            Assert.Throws<ArgumentNullException>(() => aligner.Align(null, 0, 1, new List<string>(), 0, 1));
        }

        [Test]
        public void Align_NullCollection2_ThrowsArgumentNullException()
        {
            var aligner = new StringSimilarityDiffElementAligner();
            Assert.Throws<ArgumentNullException>(() => aligner.Align(new List<string>(), 0, 1, null, 0, 1));
        }

        [Test]
        public void Align_NoDifferences_ReturnsPerfectlyAlignedElements()
        {
            var c1 = new[]
            {
                "Line 1",
                "Line 2",
                "Line 3"
            };

            var c2 = new[]
            {
                "Line 1",
                "Line 2",
                "Line 3"
            };

            var aligner = new StringSimilarityDiffElementAligner();

            var elements = aligner.Align(c1, 0, c1.Length, c2, 0, c2.Length).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new DiffElement<string>("Line 1", "Line 1", DiffOperation.Modify),
                new DiffElement<string>("Line 2", "Line 2", DiffOperation.Modify),
                new DiffElement<string>("Line 3", "Line 3", DiffOperation.Modify),
            }, elements);
        }

        [Test]
        public void Align_MinorDifferencesInMiddleLine_ReturnsPerfectlyAlignedElements()
        {
            var c1 = new[]
            {
                "Line 1",
                "Line 2",
                null,
                "Line 3"
            };

            var c2 = new[]
            {
                "Line 1",
                "Line+2",
                null,
                "Line 3"
            };

            var aligner = new StringSimilarityDiffElementAligner();

            var elements = aligner.Align(c1, 0, c1.Length, c2, 0, c2.Length).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new DiffElement<string>("Line 1", "Line 1", DiffOperation.Modify),
                new DiffElement<string>("Line 2", "Line+2", DiffOperation.Modify),
                new DiffElement<string>(null, null, DiffOperation.Modify),
                new DiffElement<string>("Line 3", "Line 3", DiffOperation.Modify),
            }, elements);
        }

        [Test]
        public void Align_MajorDifferencesInMiddleLine_ReturnsPerfectlyAlignedElements()
        {
            var c1 = new[]
            {
                "Line 1",
                "Line 2",
                "Line 3"
            };

            var c2 = new[]
            {
                "Line 1",
                "Something else",
                "Line 3"
            };

            var aligner = new StringSimilarityDiffElementAligner();

            var elements = aligner.Align(c1, 0, c1.Length, c2, 0, c2.Length).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new DiffElement<string>("Line 1", "Line 1", DiffOperation.Modify),
                new DiffElement<string>("Line 2", Option<string>.None, DiffOperation.Delete),
                new DiffElement<string>(Option<string>.None, "Something else", DiffOperation.Insert),
                new DiffElement<string>("Line 3", "Line 3", DiffOperation.Modify),
            }, elements);
        }
    }
}

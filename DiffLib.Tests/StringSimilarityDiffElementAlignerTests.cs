using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            string[] c1 = new[]
                          {
                              "Line 1",
                              "Line 2",
                              "Line 3"
                          };

            string[] c2 = new[]
                          {
                              "Line 1",
                              "Line 2",
                              "Line 3"
                          };

            var aligner = new StringSimilarityDiffElementAligner();

            DiffElement<string>[] elements = aligner.Align(c1, 0, c1.Length, c2, 0, c2.Length).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new DiffElement<string>(0, "Line 1", 0, "Line 1", DiffOperation.Modify),
                new DiffElement<string>(1, "Line 2", 1, "Line 2", DiffOperation.Modify),
                new DiffElement<string>(2, "Line 3", 2, "Line 3", DiffOperation.Modify),
            }, elements);
        }

        [Test]
        public void Align_MinorDifferencesInMiddleLine_ReturnsPerfectlyAlignedElements()
        {
            string[] c1 = new[]
                          {
                              "Line 1",
                              "Line 2",
                              null,
                              "Line 3"
                          };

            string[] c2 = new[]
                          {
                              "Line 1",
                              "Line+2",
                              null,
                              "Line 3"
                          };

            var aligner = new StringSimilarityDiffElementAligner();

            DiffElement<string>[] elements = aligner.Align(c1, 0, c1.Length, c2, 0, c2.Length).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new DiffElement<string>(0, "Line 1", 0, "Line 1", DiffOperation.Modify),
                new DiffElement<string>(1, "Line 2", 1, "Line+2", DiffOperation.Modify),
                new DiffElement<string>(2, null, 2, null, DiffOperation.Modify),
                new DiffElement<string>(3, "Line 3", 3, "Line 3", DiffOperation.Modify),
            }, elements);
        }

        [Test]
        public void Align_MajorDifferencesInMiddleLine_ReturnsPerfectlyAlignedElements()
        {
            string[] c1 = new[]
                          {
                              "Line 1",
                              "Line 2",
                              "Line 3"
                          };

            string[] c2 = new[]
                          {
                              "Line 1",
                              "Something else",
                              "Line 3"
                          };

            var aligner = new StringSimilarityDiffElementAligner();

            DiffElement<string>[] elements = aligner.Align(c1, 0, c1.Length, c2, 0, c2.Length).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new DiffElement<string>(0, "Line 1", 0, "Line 1", DiffOperation.Modify),
                new DiffElement<string>(1, "Line 2", null, Option<string>.None, DiffOperation.Delete),
                new DiffElement<string>(null, Option<string>.None, 1, "Something else", DiffOperation.Insert),
                new DiffElement<string>(2, "Line 3", 2, "Line 3", DiffOperation.Modify),
            }, elements);
        }
    }
}

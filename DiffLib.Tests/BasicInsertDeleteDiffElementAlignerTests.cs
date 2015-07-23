using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace DiffLib.Tests
{
    [TestFixture]
    public class BasicInsertDeleteDiffElementAlignerTests
    {
        [Test]
        public void Align_NullCollection1_ThrowsArgumentNullException()
        {
            var aligner = new BasicInsertDeleteDiffElementAligner<int>();

            IList<int> collection1 = null;
            IList<int> collection2 = new List<int>();

            Assert.Throws<ArgumentNullException>(() => aligner.Align(collection1, 0, 1, collection2, 0, 1).ToArray());
        }

        [Test]
        public void Align_NullCollection2_ThrowsArgumentNullException()
        {
            var aligner = new BasicInsertDeleteDiffElementAligner<int>();

            IList<int> collection1 = new List<int>(); 
            IList<int> collection2 = null;

            Assert.Throws<ArgumentNullException>(() => aligner.Align(collection1, 0, 1, collection2, 0, 1).ToArray());
        }

        [Test]
        public void Align_ItemsOnlyInCollection1_OutputsDeletes()
        {
            var aligner = new BasicInsertDeleteDiffElementAligner<int>();

            IList<int> collection1 = new List<int>
            {
                1,
                2,
                3
            };
            IList<int> collection2 = new List<int>();

            var elements = aligner.Align(collection1, 0, collection1.Count, collection2, 0, collection2.Count).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new DiffElement<int>(1, Option<int>.None, DiffOperation.Delete),
                new DiffElement<int>(2, Option<int>.None, DiffOperation.Delete),
                new DiffElement<int>(3, Option<int>.None, DiffOperation.Delete),
            }, elements);
        }

        [Test]
        public void Align_ItemsOnlyInCollection2_OutputsInserts()
        {
            var aligner = new BasicInsertDeleteDiffElementAligner<int>();

            IList<int> collection1 = new List<int>();
            IList<int> collection2 = new List<int>
            {
                1,
                2,
                3
            };

            var elements = aligner.Align(collection1, 0, collection1.Count, collection2, 0, collection2.Count).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new DiffElement<int>(Option<int>.None, 1, DiffOperation.Insert),
                new DiffElement<int>(Option<int>.None, 2, DiffOperation.Insert),
                new DiffElement<int>(Option<int>.None, 3, DiffOperation.Insert),
            }, elements);
        }
    }
}
using System;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace DiffLib.Tests
{
    [TestFixture]
    public class ElementSimilarityDiffElementAlignerTests
    {
        [Test]
        public void Constructor_NullSimilarityFunc_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ElementSimilarityDiffElementAligner<int>(null));
        }
    }
}

using System;

using DiffLib.Alignment;

using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace DiffLib.Tests;

public class ElementSimilarityDiffElementAlignerTests
{
    [Test]
    public void Constructor_NullSimilarityFunc_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ElementSimilarityDiffElementAligner<int>(null));
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

using DiffLib.Alignment;

using NUnit.Framework;
using NUnit.Framework.Legacy;

// ReSharper disable AssignNullToNotNullAttribute

namespace DiffLib.Tests;

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
    public void AlignElements_NullCollection1_ThrowsArgumentNullException()
    {
        IList<int> collection1 = null;
        IList<int> collection2 = new int[0];
        IEnumerable<DiffSection> diffSections = new DiffSection[0];
        IDiffElementAligner<int> aligner = new BasicInsertDeleteDiffElementAligner<int>();
        Assert.Throws<ArgumentNullException>(() => Diff.AlignElements(collection1, collection2, diffSections, aligner));
    }

    [Test]
    public void AlignElements_NullCollection2_ThrowsArgumentNullException()
    {
        IList<int> collection1 = new int[0];
        IList<int> collection2 = null;
        IEnumerable<DiffSection> diffSections = new DiffSection[0];
        IDiffElementAligner<int> aligner = new BasicInsertDeleteDiffElementAligner<int>();
        Assert.Throws<ArgumentNullException>(() => Diff.AlignElements(collection1, collection2, diffSections, aligner));
    }

    [Test]
    public void AlignElements_NullDiffSections_ThrowsArgumentNullException()
    {
        IList<int> collection1 = new int[0];
        IList<int> collection2 = new int[0];
        IEnumerable<DiffSection> diffSections = null;
        IDiffElementAligner<int> aligner = new BasicInsertDeleteDiffElementAligner<int>();
        Assert.Throws<ArgumentNullException>(() => Diff.AlignElements(collection1, collection2, diffSections, aligner));
    }

    [Test]
    public void AlignElements_NullAligner_ThrowsArgumentNullException()
    {
        IList<int> collection1 = new int[0];
        IList<int> collection2 = new int[0];
        IEnumerable<DiffSection> diffSections = new DiffSection[0];
        IDiffElementAligner<int> aligner = null;
        Assert.Throws<ArgumentNullException>(() => Diff.AlignElements(collection1, collection2, diffSections, aligner));
    }

    [Test]
    public void SimpleDiff_ProducesCorrectResults()
    {
        const string text1 = "This is a test of the diff implementation, with some text that is deleted.";
        const string text2 = "This is another test of the same implementation, with some more text.";

        DiffSection[] diff = Diff.CalculateSections(text1.ToCharArray(), text2.ToCharArray()).ToArray();

        Assert.That(new[]
        {
            new DiffSection(true, 9, 9),   // same        "This is a"
            new DiffSection(false, 0, 6),  // add        "nother"
            new DiffSection(true, 13, 13), // same      " test of the "
            new DiffSection(false, 4, 4),  // replace    "same" with "diff"
            new DiffSection(true, 27, 27), // same      " implementation, with some "
            new DiffSection(false, 0, 5),  // add        "more "
            new DiffSection(true, 4, 4),   // same        "text"
            new DiffSection(false, 16, 0), // delete    " that is deleted"
            new DiffSection(true, 1, 1),   // same        "."
        }, Is.EqualTo(diff).AsCollection);
    }

    [Test]
    public void Diff_WithNullElements()
    {
        string[] collection1 =
        [
            "Line 1", "Line 2", null, "Line 3", "Line 4"
        ];

        string[] collection2 =
        [
            "Line 1", null, "Line 2", "Line 4"
        ];

        DiffSection[] sections = Diff.CalculateSections(collection1, collection2).ToArray();

        Assert.That(new[]
        {
            new DiffSection(true, 1, 1),
            new DiffSection(false, 0, 1),
            new DiffSection(true, 1, 1),
            new DiffSection(false, 2, 0),
            new DiffSection(true, 1, 1),
        }, Is.EqualTo(sections).AsCollection);
    }

    [Test]
    public void Align_WithNullElements()
    {
        string[] collection1 =
        [
            "Line 1", "Line 2", null, "Line 3", "Line 4"
        ];

        string[] collection2 =
        [
            "Line 1", null, "Line 2", "Line 4"
        ];

        DiffSection[] sections = Diff.CalculateSections(collection1, collection2).ToArray();
        IEnumerable<DiffElement<string>> elements = Diff.AlignElements(collection1, collection2, sections, new StringSimilarityDiffElementAligner());

        Assert.That(elements, Is.EqualTo(new[]
        {
            new DiffElement<string>(0, "Line 1", 0, "Line 1", DiffOperation.Match),
            new DiffElement<string>(null, Option<string>.None, 1, null, DiffOperation.Insert),
            new DiffElement<string>(1, "Line 2", 2, "Line 2", DiffOperation.Match),
            new DiffElement<string>(2, null, null, Option<string>.None, DiffOperation.Delete),
            new DiffElement<string>(3, "Line 3", null, Option<string>.None, DiffOperation.Delete),
            new DiffElement<string>(4, "Line 4", 3, "Line 4", DiffOperation.Match),
        }).AsCollection);
    }
}
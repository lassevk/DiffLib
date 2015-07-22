using System;
using System.Collections.Generic;
using System.Linq;
using DiffLib;
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
        public void SimpleDiff_ProducesCorrectResults()
        {
            const string text1 = "  123  ";
            const string text2 = "  1x2  ";

            DiffElement<char>[] diff = Diff.Calculate(text1, text2).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                new DiffElement<char>(' ', ' ', DiffOperation.None),
                new DiffElement<char>(' ', ' ', DiffOperation.None),
                new DiffElement<char>('1', '1', DiffOperation.None),
                new DiffElement<char>(Option<char>.None, 'x', DiffOperation.Insert),
                new DiffElement<char>('2', '2', DiffOperation.None),
                new DiffElement<char>('3', Option<char>.None, DiffOperation.Delete),
                new DiffElement<char>(' ', ' ', DiffOperation.None),
                new DiffElement<char>(' ', ' ', DiffOperation.None),
            }, diff);
        }
    }
}
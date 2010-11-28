using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace DiffLib.Tests
{
    [TestFixture]
    public class LongestCommonSubstringTests
    {
        [Test]
        public void Find_EqualStrings_ReturnsWholeString()
        {
            const string collection = "This is a test collection";
            LongestCommonSubstringResult lcsr = LongestCommonSubstring.Find(collection, collection);

            Assert.That(lcsr.PositionInCollection1, Is.EqualTo(0));
            Assert.That(lcsr.PositionInCollection2, Is.EqualTo(0));
            Assert.That(lcsr.Length, Is.EqualTo(collection.Length));
        }

        [Test]
        public void Find_EqualStringsButNotSameInstance_ReturnsWholeString()
        {
            const string collection1 = "This is a test collection";
            string collection2 = "This is a test collectio";
            collection2 += "n";

            Assert.That(collection1, Is.Not.SameAs(collection2));
            LongestCommonSubstringResult lcsr = LongestCommonSubstring.Find(collection1, collection2);

            Assert.That(lcsr.PositionInCollection1, Is.EqualTo(0));
            Assert.That(lcsr.PositionInCollection2, Is.EqualTo(0));
            Assert.That(lcsr.Length, Is.EqualTo(collection1.Length));
        }

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

        [TestCase(
            "This is a[ test of Longest Common Substring]",
            "This is another[ test of Longest Common Substring]")]
        [TestCase(
            "xxxx[YYYYYYYYYYYYYYYYYYYYYY]xxxxxxxx",
            "aaaaaaaa[YYYYYYYYYYYYYYYYYYYYYY]bbbb")]
        [TestCase(
            "[xxxxYYYYYYYYYYYYYYYYYYYYYYxxxx]xxxx",
            "xxxx[xxxxYYYYYYYYYYYYYYYYYYYYYYxxxx]")]
        public void Find_SomeSimplePatterns_ReturnsCorrectLocations(string input1, string input2)
        {
            int index1 = input1.IndexOf('[');
            int length1 = input1.IndexOf(']') - index1 - 1;
            Assert.That(index1, Is.GreaterThanOrEqualTo(0));
            Assert.That(length1, Is.GreaterThan(0));

            int index2 = input2.IndexOf('[');
            int length2 = input2.IndexOf(']') - index2 - 1;
            Assert.That(index2, Is.GreaterThanOrEqualTo(0));
            Assert.That(length2, Is.GreaterThan(0));

            Assert.That(length1, Is.EqualTo(length2));

            input1 = input1.Replace("[", "").Replace("]", "");
            input2 = input2.Replace("[", "").Replace("]", "");

            var lcsr = LongestCommonSubstring.Find(input1, input2);

            Assert.That(lcsr.PositionInCollection1, Is.EqualTo(index1));
            Assert.That(lcsr.PositionInCollection2, Is.EqualTo(index2));
            Assert.That(lcsr.Length, Is.EqualTo(length1));
        }
    }
}
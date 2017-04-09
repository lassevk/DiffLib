using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace DiffLib.Tests
{
    [TestFixture]
    public class MergeTests
    {
        //[Test]
        //public void Perform_AllEmptyCollections_ReturnsEmptyResults()
        //{
        //    var output = Merge.Perform(new int[0], new int[0], new int[0], new BasicInsertDeleteDiffElementAligner<int>(), new TakeLeftThenRightMergeConflictResolver<int>());
        //    CollectionAssert.IsEmpty(output);
        //}

        //[Test]
        //public void Perform_EmptyCommonBaseLeftSideInserts_ReturnsLeftSideContent()
        //{
        //    var commonBase = new int[0];
        //    var left = new[] { 1, 2, 3 };
        //    var right = new int[0];

        //    var output = Merge.Perform(commonBase, left, right, new BasicInsertDeleteDiffElementAligner<int>(), new TakeLeftThenRightMergeConflictResolver<int>());
        //    CollectionAssert.AreEqual(output, left);

        //}

        //[Test]
        //public void Perform_EmptyCommonBaseRightSideInserts_ReturnsRightSideContent()
        //{
        //    var commonBase = new int[0];
        //    var left = new int[0];
        //    var right = new[] { 1, 2, 3 };

        //    var output = Merge.Perform(commonBase, left, right, new BasicInsertDeleteDiffElementAligner<int>(), new TakeLeftThenRightMergeConflictResolver<int>());
        //    CollectionAssert.AreEqual(output, right);
        //}

        [Test]
        [TestCase("1234567890", "1234567890", "1234567890", "1234567890")]
        [TestCase("1234567890", "12a4567890", "123456b890", "12a456b890")]
        [TestCase("1234567890", "123abc4567890", "1234567klm890", "123abc4567klm890")]
        [TestCase("1234567890", "1234890", "1234567890", "1234890")]
        [TestCase("1234567890", "1234567890", "1234890", "1234890")]
        [TestCase("1234567890", "12abc34567890", "1234567890", "12abc34567890")]
        [TestCase("1234567890", "1234567890", "12abc34567890", "12abc34567890")]
        [TestCase("1234567890", "12abc34567890", "12klm34567890", "12abcklm34567890")]
        [TestCase("1234567890", "1234890", "1234890", "1234890")]
        [TestCase("1234567890", "123abc7890", "1237890", "123abc7890")]
        [TestCase("1234567890", "123a567890", "123b567890", "123ab567890")]
        public void Perform_TestCases(string commonBase, string left, string right, string expected)
        {
            var output = new string(Merge.Perform(commonBase.ToCharArray(), left.ToCharArray(), right.ToCharArray(), new BasicReplaceInsertDeleteDiffElementAligner<char>(), new TakeLeftThenRightMergeConflictResolver<char>()).ToArray());
            Assert.That(output, Is.EqualTo(expected));
        }
    }
}

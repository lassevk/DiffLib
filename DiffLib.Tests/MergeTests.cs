using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

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
        [TestCase("1234567890", "1234567890", "1234567890", "1234567890", TestName = "Nothing changed")]
        [TestCase("1234567890", "12a4567890", "123456b890", "12a456b890", TestName = "Both sides replaced to same")]
        [TestCase("1234567890", "123abc4567890", "1234567klm890", "123abc4567klm890", TestName = "Both sides inserted in separate places")]
        [TestCase("1234567890", "1234890", "1234567890", "1234890", TestName = "Left side deleted")]
        [TestCase("1234567890", "1234567890", "1234890", "1234890", TestName = "Right side deleted")]
        [TestCase("1234567890", "1234890", "1234890", "1234890", TestName = "Both sides deleted")]
        [TestCase("1234567890", "12abc34567890", "1234567890", "12abc34567890", TestName = "Left side inserted")]
        [TestCase("1234567890", "1234567890", "12abc34567890", "12abc34567890", TestName = "Right side inserted")]
        [TestCase("1234567890", "12abc34567890", "12klm34567890", "12abcklm34567890", TestName = "Both sides inserted at the same place, take left then right")]
        [TestCase("1234567890", "123abc7890", "1237890", "123abc7890", TestName = "Left side modified, right side deleted, take left side")]
        [TestCase("1234567890" ,"123567890", "123x567890", "123x567890", TestName = "Left side deleted, right side modified, take left then right")]
        [TestCase("1234567890", "123a567890", "123b567890", "123ab567890", TestName = "Both side modified, take left then right")]
        public void Perform_TestCases(string commonBase, string left, string right, string expected)
        {
            var output = new string(Merge.Perform(commonBase.ToCharArray(), left.ToCharArray(), right.ToCharArray(), new BasicReplaceInsertDeleteDiffElementAligner<char>(), new TakeLeftThenRightMergeConflictResolver<char>()).ToArray());
            Assert.That(output, Is.EqualTo(expected));
        }

        [NotNull]
        private List<string> StringToLines(string input)
        {
            var reader = new StringReader(input);
            var result = new List<string>();

            string line;
            while ((line = reader.ReadLine()) != null)
                result.Add(line);

            return result;
        }

        private class AbortIfConflictResolver<T> : IMergeConflictResolver<T>
        {
            public IEnumerable<T> Resolve(IList<T> commonBase, IList<T> left, IList<T> right)
            {
                throw new NotSupportedException();
            }
        }

        [Test]
        public void Perform_DistinctAdditions_ShouldNotProduceAConflict()
        {
            var common = "{}".ToCharArray();
            var left = "{a}".ToCharArray();
            var right = "{} {b}".ToCharArray();
            var expected = "{a} {b}".ToCharArray();

            var result = Merge.Perform(common, left, right, new DiffOptions { EnablePatienceOptimization = false }, new BasicReplaceInsertDeleteDiffElementAligner<char>(), new AbortIfConflictResolver<char>()).ToList();

            CollectionAssert.AreEqual(expected, result);
        }
    }
}

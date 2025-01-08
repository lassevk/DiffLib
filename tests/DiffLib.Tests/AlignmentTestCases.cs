using System;
using System.Collections.Generic;
using System.Linq;

using DiffLib.Alignment;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

namespace DiffLib.Tests
{
    [TestFixture]
    public class AlignmentTestCases
    {
        private readonly Dictionary<DiffOperation, char> _OperationCharacters = new Dictionary<DiffOperation, char>
        {
            [DiffOperation.Match] = '-',
            [DiffOperation.Insert] = 'I',
            [DiffOperation.Delete] = 'D',
            [DiffOperation.Replace] = 'R',
            [DiffOperation.Modify] = 'M',
        };

        private string GetElementOperationsAsAString(IEnumerable<DiffElement<Char>> elements)
        {
            return new string(elements.Select(element => _OperationCharacters[element.Operation]).ToArray());
        }

        [Test]
        [TestCase("", "", "", TestName = "Degenerate testcase, nothing to diff")]
        [TestCase("TEST123456XYZ", "TEST456123XYZ", "----III---DDD---")]
        [TestCase("ABC", "XbY", "DDDIII", TestName = "Basic alignment test")]
        [TestCase("123", "124", "--DI", TestName = "Replace last character")]
        [TestCase("123", "1123" ,"-I--", TestName = "Add new character (insert should not be the first operation, prefix patience match)")]
        [TestCase("123", "0123", "I---", TestName = "Prefix with new character")]
        [TestCase("123", "1233", "---I", TestName = "Add new character (insert should be the last operation, prefix patience match)")]
        [TestCase("123", "1234" ,"---I", TestName = "Append with new character")]
        [TestCase("123", "23", "D--", TestName = "Delete first character")]
        [TestCase("123", "12", "--D", TestName = "Delete last character")]
        [TestCase("123", "13", "-D-", TestName = "Delete middle character")]
        [TestCase("123", "456", "DDDIII", TestName = "Complete replacement")]
        [TestCase("123", "1X3", "-DI-", TestName = "Replace middle character")]
        [TestCase("123", "X23", "DI--", TestName = "Replace first character")]
        [TestCase("123", "12X", "--DI", TestName = "Replace last character")]
        [TestCase("ABCDEF", "ABxcyDEF", "--DIII---", TestName = "Modify one character in the middle + insert on both sides")]
        [TestCase("ABCDEFGH", "A1B2C3D4E5F6G7H" ,"-I-I-I-I-I-I-I-", TestName = "Degenerate, multiple insertions")]
        public void BasicInsertDeleteDiffElementAlignerTestCases(string s1, string s2, string expected)
        {
            IEnumerable<DiffSection> sections = Diff.CalculateSections(s1.ToCharArray(), s2.ToCharArray());
            IEnumerable<DiffElement<char>> elements = Diff.AlignElements(s1.ToCharArray(), s2.ToCharArray(), sections, new BasicInsertDeleteDiffElementAligner<char>());
            string output = GetElementOperationsAsAString(elements);

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("ABC", "XbY", "RRR", TestName = "Basic alignment test")]
        [TestCase("123", "124", "--R", TestName = "Replace last character")]
        [TestCase("123", "1245", "--RI", TestName = "Replace + insert")]
        [TestCase("123", "1123", "-I--", TestName = "Add new character (insert should not be the first operation, prefix patience match)")]
        [TestCase("123", "0123", "I---", TestName = "Prefix with new character")]
        [TestCase("123", "1233", "---I", TestName = "Add new character (insert should be the last operation, prefix patience match)")]
        [TestCase("123", "1234", "---I", TestName = "Append with new character")]
        [TestCase("123", "23", "D--", TestName = "Delete first character")]
        [TestCase("123", "12", "--D", TestName = "Delete last character")]
        [TestCase("123", "13", "-D-", TestName = "Delete middle character")]
        [TestCase("123", "456", "RRR", TestName = "Complete replacement")]
        [TestCase("123", "456789", "RRRIII", TestName = "Complete replacement + insert")]
        [TestCase("123", "1X3", "-R-", TestName = "Replace middle character")]
        [TestCase("123", "1XY3", "-RI-", TestName = "Replace middle character + insert")]
        [TestCase("123", "X23", "R--", TestName = "Replace first character")]
        [TestCase("123", "XY23", "RI--", TestName = "Replace first character + insert")]
        [TestCase("123", "12X", "--R", TestName = "Replace last character")]
        [TestCase("123", "12XY", "--RI", TestName = "Replace last character + insert")]
        [TestCase("ABCDEF", "ABxcyDEF", "--RII---", TestName = "Modify one character in the middle + insert on both sides")]
        public void BasicReplaceInsertDeleteDiffElementAlignerTestCases(string s1, string s2, string expected)
        {
            IEnumerable<DiffSection> sections = Diff.CalculateSections(s1.ToCharArray(), s2.ToCharArray());
            IEnumerable<DiffElement<char>> elements = Diff.AlignElements(s1.ToCharArray(), s2.ToCharArray(), sections, new BasicReplaceInsertDeleteDiffElementAligner<char>());
            string output = GetElementOperationsAsAString(elements);

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("ABC", "XbY", "DIMDI", TestName = "Basic alignment test")]
        [TestCase("123", "124", "--DI", TestName = "Replace last character")]
        [TestCase("123", "1245", "--DII", TestName = "Replace + insert")]
        [TestCase("123", "1123", "-I--", TestName = "Add new character (insert should not be the first operation, prefix patience match)")]
        [TestCase("123", "0123", "I---", TestName = "Prefix with new character")]
        [TestCase("123", "1233", "---I", TestName = "Add new character (insert should be the last operation, prefix patience match)")]
        [TestCase("123", "1234", "---I", TestName = "Append with new character")]
        [TestCase("123", "23", "D--", TestName = "Delete first character")]
        [TestCase("123", "12", "--D", TestName = "Delete last character")]
        [TestCase("123", "13", "-D-", TestName = "Delete middle character")]
        [TestCase("123", "456", "DDDIII", TestName = "Complete replacement")]
        [TestCase("123", "456789", "DDDIIIIII", TestName = "Complete replacement + insert")]
        [TestCase("123", "1X3", "-DI-", TestName = "Replace middle character")]
        [TestCase("123", "1XY3", "-DII-", TestName = "Replace middle character + insert")]
        [TestCase("123", "X23", "DI--", TestName = "Replace first character")]
        [TestCase("123", "XY23", "DII--", TestName = "Replace first character + insert")]
        [TestCase("123", "12X", "--DI", TestName = "Replace last character")]
        [TestCase("123", "12XY", "--DII", TestName = "Replace last character + insert")]
        [TestCase("ABCDEF", "ABxcyDEF", "--IMI---", TestName = "Modify one character in the middle + insert on both sides")]
        [TestCase("ABC12345678XYZ", "ABCklmnopqrXYZ", "---DDDDDDDDIIIIIIII---", TestName = "Degenerate, bigger match window than limit")]
        public void ElementSimilarityDiffElementAlignerTestCases(string s1, string s2, string expected)
        {
            double aligner(char element1, char element2)
            {
                if (element1 == element2)
                    return 1.0;

                if (char.ToUpper(element1) == char.ToUpper(element2))
                    return 0.75;

                return 0.0;
            }

            IEnumerable<DiffSection> sections = Diff.CalculateSections(s1.ToCharArray(), s2.ToCharArray());
            IEnumerable<DiffElement<char>> elements = Diff.AlignElements(s1.ToCharArray(), s2.ToCharArray(), sections, new ElementSimilarityDiffElementAligner<char>(aligner));
            string output = GetElementOperationsAsAString(elements);

            Assert.That(output, Is.EqualTo(expected));
        }
    }
}

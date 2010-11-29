using NUnit.Framework;

namespace DiffLib.Tests
{
    [TestFixture]
    public class AlignedDiffChangeTests
    {
        [TestCase(ChangeType.Added, "", "")]
        [TestCase(ChangeType.Deleted, "", "")]
        [TestCase(ChangeType.Changed, "", "")]
        [TestCase(ChangeType.Same, "", "")]
        [TestCase(ChangeType.Added, "xx", "")]
        [TestCase(ChangeType.Added, "yy", "")]
        [TestCase(ChangeType.Added, "", "xx")]
        [TestCase(ChangeType.Added, "", "yy")]
        public void Constructor_InitializesPropertiesCorrectly(ChangeType type, string element1, string element2)
        {
            var adc = new AlignedDiffChange<string>(type, element1, element2);
            Assert.That(adc.Type, Is.EqualTo(type));
            Assert.That(adc.Element1, Is.EqualTo(element1));
            Assert.That(adc.Element2, Is.EqualTo(element2));
        }
    }
}
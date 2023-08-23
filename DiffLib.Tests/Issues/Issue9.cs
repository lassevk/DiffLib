using System.Linq;
using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace DiffLib.Tests.Issues
{
    [TestFixture]
    public class Issue9
    {
        [Test]
        public void Repro()
        {
            char[] s1 = "904".ToCharArray();
            char[] s2 = "448".ToCharArray();

            Assert.DoesNotThrow(() => Diff.CalculateSections(s1, s2).ToList());
        }
    }
}

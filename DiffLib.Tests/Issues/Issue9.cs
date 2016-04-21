using System;
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
            var s1 = "904".ToCharArray();
            var s2 = "448".ToCharArray();

            Assert.DoesNotThrow(() => Diff.CalculateSections(s1, s2).ToList());
        }
    }
}

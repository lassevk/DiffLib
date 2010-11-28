using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DiffLib.Tests
{
    [TestFixture]
    public class AllClassesShouldBeSealedTests
    {
        public IEnumerable<Type> AllTypesInDiffLib()
        {
            return
                from type in typeof (Diff<string>).Assembly.GetTypes()
                where type.IsPublic
                select type;
        }

        [TestCaseSource("AllTypesInDiffLib")]
        public void TypeShouldBeSealed(Type type)
        {
            Assert.That(type.IsSealed, Is.True);
        }
    }
}
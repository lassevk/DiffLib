using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace DiffLib.Tests.CodeQuality
{
    [TestFixture]
    public class PublicApiTests : QaTestBase
    {
        [Test]
        [TestCaseSource(nameof(AllPublicTypes))]
        public void PublicType_MustHavePublicApiAttribute([NotNull] Type publicType)
        {
            if (publicType.IsDefined(typeof(PublicAPIAttribute), true))
                return;

            Assert.Fail("Type '{0}' is not tagged with [PublicAPI]", publicType.Name);
        }

        [Test]
        [TestCaseSource(nameof(AllPublicMethods))]
        public void PublicMethod_MustHavePublicApiAttribute([NotNull] MethodInfo method)
        {
            if (method.IsDefined(typeof(PublicAPIAttribute), true))
                return;

            Assert.Fail("Method '{0}' of type '{1}' is not tagged with [PublicAPI]", method.Name, method.DeclaringType.FullName);
        }

        [Test]
        [TestCaseSource(nameof(AllPublicProperties))]
        public void PublicProperties_MustHavePublicApiAttribute([NotNull] PropertyInfo property)
        {
            if (property.IsDefined(typeof(PublicAPIAttribute), true))
                return;

            Assert.Fail("Property '{0}' of type '{1}' is not tagged with [PublicAPI]", property.Name, property.DeclaringType.FullName);
        }

        [Test]
        [TestCaseSource(nameof(AllPublicConstructors))]
        public void PublicConstructors_MustHavePublicApiAttribute([NotNull] ConstructorInfo ctor)
        {
            if (ctor.IsDefined(typeof(PublicAPIAttribute), true))
                return;

            Assert.Fail("Constructor '{1}({0})' of type '{1}' is not tagged with [PublicAPI]", string.Join(", ", ctor.GetParameters().Select(p => p.ParameterType.FullName).ToArray()), ctor.DeclaringType.FullName);
        }

        public override IEnumerable<Assembly> AllAssemblies()
        {
            yield return typeof(LongestCommonSubsectionDiff).Assembly;
        }
    }
}

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace DiffLib.Tests.CodeQuality
{
#if DEBUG
    [TestFixture(Category = "QA")]
    public class ReferenceTests : QaTestBase
    {
        public override IEnumerable<Assembly> AllAssemblies()
        {
            yield return typeof(LongestCommonSubsectionDiff).Assembly;
        }

        public IEnumerable<MethodInfo> AllPublicMethodsReturningReference()
        {
            return from method in AllPublicMethods()
                   where method.ReturnType != typeof(void)
                   where !method.ReturnType.IsValueType
                   select method;
        }

        public IEnumerable<ParameterInfo> AllParametersTakingReferences()
        {
            return from parameter in AllPublicParameters()
                   where !parameter.ParameterType.IsValueType
                   select parameter;
        }

        public IEnumerable<PropertyInfo> AllPropertiesHoldingReferences()
        {
            return from property in AllPublicProperties()
                   where !property.PropertyType.IsValueType
                   select property;
        }

        [Test]
        [TestCaseSource(nameof(AllPublicMethodsReturningReference))]
        public void Method_ReturningReference_MustBeTaggedWithNotNullOrCanBeNull(MethodInfo method)
        {
            if (method.IsDefined(typeof(CanBeNullAttribute), true))
                return;
            if (method.IsDefined(typeof(NotNullAttribute), true))
                return;
            if (method.IsDefined(typeof(ContractAnnotationAttribute), true))
                return;

            Assert.Fail("Method '{0}' of type '{1}' is not tagged with [CanBeNull] or [NotNull]", method.Name, method.DeclaringType.Name);
        }

        [Test]
        [TestCaseSource(nameof(AllParametersTakingReferences))]
        public void Parameter_TakingReference_MustBeTaggedWithNotNullOrCanBeNull(ParameterInfo parameter)
        {
            if (parameter.IsDefined(typeof(CanBeNullAttribute), true))
                return;
            if (parameter.IsDefined(typeof(NotNullAttribute), true))
                return;
            if (parameter.Member.IsDefined(typeof(ContractAnnotationAttribute), true))
                return;
            if (parameter.ParameterType.IsByRef && parameter.ParameterType.GetElementType().IsValueType)
                return;

            Assert.Fail("Parameter '{0}' of method '{1}' of type '{2}' is not tagged with [CanBeNull] or [NotNull]", parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.Name);
        }

        [Test]
        [TestCaseSource(nameof(AllPropertiesHoldingReferences))]
        public void Property_HoldingReference_MustBeTaggedWithNotNullOrCanBeNull(PropertyInfo property)
        {
            if (property.IsDefined(typeof(CanBeNullAttribute), true))
                return;
            if (property.IsDefined(typeof(NotNullAttribute), true))
                return;

            Assert.Fail("Property '{0}' of type '{1}' is not tagged with [CanBeNull] or [NotNull]", property.Name, property.DeclaringType.Name);
        }
    }
#endif
}
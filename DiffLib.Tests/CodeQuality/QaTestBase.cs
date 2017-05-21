using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

// ReSharper disable PossibleNullReferenceException

namespace DiffLib.Tests.CodeQuality
{
    [PublicAPI]
    public abstract class QaTestBase
    {
        [NotNull, ItemNotNull]
        public abstract IEnumerable<Assembly> AllAssemblies();

        [NotNull, ItemNotNull]
        public IEnumerable<Type> AllTypes()
        {
            return from assembly in AllAssemblies()
                   from type in assembly.GetTypes()
                   where !type.Namespace.StartsWith("JetBrains.")
                   select type;
        }

        [NotNull, ItemNotNull]
        public IEnumerable<Type> AllPublicTypes()
        {
            return from type in AllTypes()
                   where type.IsPublic
                   where !type.IsNestedFamANDAssem
                   select type;
        }

        [NotNull, ItemNotNull]
        public IEnumerable<Type> AllPublicClasses()
        {
            return from type in AllPublicTypes()
                   where type.IsClass || type.IsInterface
                   where !typeof(Delegate).IsAssignableFrom(type)
                   select type;
        }

        [NotNull, ItemNotNull]
        public IEnumerable<PropertyInfo> AllPublicProperties()
        {
            return from type in AllPublicClasses()
                   from property in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
                   let getter = property.GetGetMethod()
                   let setter = property.GetSetMethod()
                   where (getter == null || !getter.IsFinal && (getter.IsPublic || getter.IsFamily || getter.IsFamilyOrAssembly)) && (setter == null || !setter.IsFinal && (setter.IsPublic || setter.IsFamily || setter.IsFamilyOrAssembly))
                   where property.DeclaringType == type
                   select property;
        }

        [NotNull, ItemNotNull]
        public IEnumerable<MethodInfo> AllPublicMethods()
        {
            return from type in AllPublicClasses()
                   from method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                   where method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly
                   where !method.Name.StartsWith("get_")
                   where !method.Name.StartsWith("set_")
                   where !method.Name.StartsWith("add_")
                   where !method.Name.StartsWith("remove_")
                   select method;
        }

        [NotNull, ItemNotNull]
        public IEnumerable<ConstructorInfo> AllPublicConstructors()
        {
            return from type in AllPublicClasses()
                   from ctor in type.GetConstructors()
                   select ctor;
        }

        [NotNull, ItemNotNull]
        public IEnumerable<ParameterInfo> AllPublicParametersOfMethods()
        {
            return from method in AllPublicMethods()
                   from parameter in method.GetParameters()
                   select parameter;
        }

        [NotNull, ItemNotNull]
        public IEnumerable<ParameterInfo> AllPublicParametersOfConstructors()
        {
            return from ctor in AllPublicConstructors()
                   from parameter in ctor.GetParameters()
                   select parameter;
        }

        [NotNull, ItemNotNull]
        public IEnumerable<ParameterInfo> AllPublicParameters()
        {
            return AllPublicParametersOfMethods().Concat(AllPublicParametersOfConstructors());
        }
    }
}
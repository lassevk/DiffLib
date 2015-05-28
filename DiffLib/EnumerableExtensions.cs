using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    internal static class EnumerableExtensions
    {
        [NotNull]
        internal static IList<T> ToRandomAccess<T>([NotNull] this IEnumerable<T> collection)
        {
            var result = collection as IList<T>;
            if (result != null)
                return result;

            return new List<T>(collection);
        }
    }
}
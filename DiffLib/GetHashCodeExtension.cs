using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace DiffLib
{
    internal static class GetHashCodeExtension
    {
        internal static int GetHashCode<T>([CanBeNull] this T instance, [NotNull] IEqualityComparer<T> equalityComparer)
        {
            if (instance == null)
                return 0;

            return equalityComparer.GetHashCode(instance);
        }
    }
}

using System.Collections.Generic;

namespace DiffLib;

internal static class GetHashCodeExtension
{
    internal static int GetHashCode<T>(this T? instance, IEqualityComparer<T> equalityComparer)
        => instance == null ? 0 : equalityComparer.GetHashCode(instance);
}
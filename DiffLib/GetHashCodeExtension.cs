using System.Collections.Generic;

namespace DiffLib
{
    internal static class GetHashCodeExtension
    {
        internal static int GetHashCode<T>(this T? instance, IEqualityComparer<T> equalityComparer)
        {
            if (instance == null)
                return 0;

            return equalityComparer.GetHashCode(instance);
        }
    }
}

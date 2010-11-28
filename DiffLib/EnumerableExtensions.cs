using System.Collections.Generic;

namespace DiffLib
{
    internal static class EnumerableExtensions
    {
        internal static IList<T> ToRandomAccess<T>(this IEnumerable<T> collection)
        {
            var result = collection as IList<T>;
            if (result != null)
                return result;

            return new List<T>(collection);
        }
    }
}
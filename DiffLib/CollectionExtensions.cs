using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace DiffLib
{
    internal static class CollectionExtensions
    {
        [NotNull]
        internal static IList<T> AsList<T>([NotNull] this IEnumerable<T> collection)
        {
            return (collection as IList<T>) ?? collection.ToList();
        }
    }
}
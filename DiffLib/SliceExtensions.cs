using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    public static class SliceExtensions
    {
        [NotNull]
        public static Slice<T> Slice<T>([NotNull] this IList<T> collection, int lower, int upper)
        {
            return new Slice<T>(collection, lower, upper);
        }
    }
}
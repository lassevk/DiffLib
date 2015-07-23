using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    [PublicAPI]
    public interface IDiffElementAligner<T>
    {
        [PublicAPI, NotNull]
        IEnumerable<DiffElement<T>> Align([NotNull] IList<T> collection1, int start1, int length1, [NotNull] IList<T> collection2, int start2, int length2);
    }
}
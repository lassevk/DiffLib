using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    public interface IDiffMatchAligner<T>
    {
        [NotNull]
        IEnumerable<DiffElement<T>> Align(Slice<T> slice1, Slice<T> slice2);
    }
}
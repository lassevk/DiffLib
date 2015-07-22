using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace DiffLib
{
    public class BasicInsertDeleteDiffMatchAligner<T> : IDiffMatchAligner<T>
    {
        [NotNull]
        public virtual IEnumerable<DiffElement<T>> Align(Slice<T> slice1, Slice<T> slice2) =>
            slice1.Select(element => new DiffElement<T>(element, new Option<T>(), DiffOperation.Delete))
            .Concat(slice2.Select(element => new DiffElement<T>(new Option<T>(), element, DiffOperation.Insert)));
    }
}
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    public class BasicInsertDeleteDiffElementAligner<T> : IDiffElementAligner<T>
    {
        [NotNull]
        public virtual IEnumerable<DiffElement<T>> Align([NotNull] IList<T> collection1, int start1, int length1, [NotNull] IList<T> collection2, int start2, int length2)
        {
            for (int index = 0; index < length1; index++)
                yield return new DiffElement<T>(collection1[start1 + index], Option<T>.None, DiffOperation.Delete);

            for (int index = 0; index < length2; index++)
                yield return new DiffElement<T>(collection2[start2 + index], Option<T>.None, DiffOperation.Insert);
        }
    }
}
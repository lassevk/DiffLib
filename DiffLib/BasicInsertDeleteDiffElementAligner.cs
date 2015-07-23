using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    [PublicAPI]
    public class BasicInsertDeleteDiffElementAligner<T> : IDiffElementAligner<T>
    {
        [PublicAPI]
        public BasicInsertDeleteDiffElementAligner()
        {
        }

        [PublicAPI, NotNull]
        public virtual IEnumerable<DiffElement<T>> Align([NotNull] IList<T> collection1, int start1, int length1, [NotNull] IList<T> collection2, int start2, int length2)
        {
            if (collection1 == null)
                throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null)
                throw new ArgumentNullException(nameof(collection2));

            for (int index = 0; index < length1; index++)
                yield return new DiffElement<T>(collection1[start1 + index], Option<T>.None, DiffOperation.Delete);

            for (int index = 0; index < length2; index++)
                yield return new DiffElement<T>(Option<T>.None, collection2[start2 + index], DiffOperation.Insert);
        }
    }
}
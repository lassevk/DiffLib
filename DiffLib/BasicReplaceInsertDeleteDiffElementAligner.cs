using System.Collections.Generic;
using JetBrains.Annotations;
using static System.Math;

namespace DiffLib
{
    [PublicAPI]
    public class BasicReplaceInsertDeleteDiffElementAligner<T> : BasicInsertDeleteDiffElementAligner<T>
    {
        [PublicAPI]
        public BasicReplaceInsertDeleteDiffElementAligner()
        {
        }

        [PublicAPI, NotNull]
        public override IEnumerable<DiffElement<T>> Align([NotNull] IList<T> collection1, int start1, int length1, [NotNull] IList<T> collection2, int start2, int length2)
        {
            int replaceCount = Min(length1, length2);
            for (int index = 0; index < replaceCount; index++)
                yield return new DiffElement<T>(collection1[start1 + index], collection2[start2 + index], DiffOperation.Replace);

            foreach (var element in base.Align(collection1, start1 + replaceCount, length1 - replaceCount, collection2, start2 + replaceCount, length2 - replaceCount))
                yield return element;
        }
    }
}
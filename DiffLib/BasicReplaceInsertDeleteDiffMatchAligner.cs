using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    public class BasicReplaceInsertDeleteDiffMatchAligner<T> : BasicInsertDeleteDiffMatchAligner<T>
    {
        [NotNull]
        public override IEnumerable<DiffElement<T>> Align(Slice<T> slice1, Slice<T> slice2)
        {
            int replaceCount = Math.Min(slice1.Count, slice2.Count);
            for (int index = 0; index < replaceCount; index++)
                yield return new DiffElement<T>(slice1[index], slice2[index], DiffOperation.Replace);

            slice1 = slice1.Constrain(replaceCount, 0);
            slice2 = slice2.Constrain(replaceCount, 0);

            foreach (var element in base.Align(slice1, slice2))
                yield return element;
        }
    }
}
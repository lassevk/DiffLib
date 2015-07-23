using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    [PublicAPI]
    public static class Diff
    {
        [PublicAPI, NotNull]
        public static IEnumerable<DiffSection> CalculateSections<T>([NotNull] IList<T> collection1, [NotNull] IList<T> collection2, [CanBeNull] IEqualityComparer<T> comparer = null)
        {
            if (collection1 == null)
                throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null)
                throw new ArgumentNullException(nameof(collection2));

            return LongestCommonSubsectionDiff.Calculate(collection1, collection2, comparer ?? EqualityComparer<T>.Default);
        }

        [PublicAPI, NotNull]
        public static IEnumerable<DiffElement<T>> AlignElements<T>([NotNull] IList<T> collection1, [NotNull] IList<T> collection2, [NotNull] IEnumerable<DiffSection> diffSections, [NotNull] IDiffElementAligner<T> aligner)
        {
            if (collection1 == null)
                throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null)
                throw new ArgumentNullException(nameof(collection2));
            if (diffSections == null)
                throw new ArgumentNullException(nameof(diffSections));
            if (aligner == null)
                throw new ArgumentNullException(nameof(aligner));

            int start1 = 0;
            int start2 = 0;

            foreach (var section in diffSections)
            {
                if (section.IsMatch)
                {
                    for (int index = 0; index < section.LengthInCollection1; index++)
                    {
                        yield return new DiffElement<T>(collection1[start1], collection2[start2], DiffOperation.None);
                        start1++;
                        start2++;
                    }
                }
                else
                {
                    foreach (var element in aligner.Align(collection1, start1, section.LengthInCollection1, collection2, start2, section.LengthInCollection2))
                        yield return element;

                    start1 += section.LengthInCollection1;
                    start2 += section.LengthInCollection2;
                }
            }
        }
    }
}
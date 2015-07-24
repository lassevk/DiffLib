using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    internal static class LongestCommonSubsectionDiff
    {
        [NotNull]
        public static IEnumerable<DiffSection> Calculate<T>([NotNull] IList<T> collection1, [NotNull] IList<T> collection2, [CanBeNull] IEqualityComparer<T> comparer)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            return Calculate(collection1, 0, collection1.Count, collection2, 0, collection2.Count, comparer, new LongestCommonSubsequence<T>(collection1, collection2, comparer));
        }

        [NotNull]
        private static IEnumerable<DiffSection> Calculate<T>([NotNull] IList<T> collection1, int lower1, int upper1, [NotNull] IList<T> collection2, int lower2, int upper2, [NotNull] IEqualityComparer<T> comparer, [NotNull] LongestCommonSubsequence<T> lcs)
        {
            // Short-circuit recursive call when nothing left (usually because match was found at the very start or end of a subsection
            if (lower1 == upper1 && lower2 == upper2)
                yield break;

            // Patience modification, let's find matching elements at both ends and remove those from LCS consideration
            int matchStart = MatchStart(collection1, lower1, upper1, collection2, lower2, upper2, comparer);

            if (matchStart > 0)
            {
                yield return new DiffSection(true, matchStart, matchStart);
                lower1 += matchStart;
                lower2 += matchStart;
            }

            int matchEnd = MatchEnd(collection1, lower1, upper1, collection2, lower2, upper2, comparer);
            if (matchEnd > 0)
            {
                upper1 -= matchEnd;
                upper2 -= matchEnd;
            }

            if (lower1 < upper1 || lower2 < upper2)
            {
                if (lower1 == upper1 || lower2 == upper2)
                {
                    // Degenerate case, only one of the collections still have elements
                    yield return new DiffSection(false, upper1 - lower1, upper2 - lower2);
                }
                else
                {
                    int position1;
                    int position2;
                    int length;

                    if (lcs.Find(lower1, upper1, lower2, upper2, out position1, out position2, out length))
                    {
                        // Recursively apply calculation to portion before common subsequence
                        foreach (var section in Calculate(collection1, lower1, position1, collection2, lower2, position2, comparer, lcs))
                            yield return section;

                        // Output match
                        yield return new DiffSection(true, length, length);

                        // Recursively apply calculation to portion after common subsequence
                        foreach (var section in Calculate(collection1, position1 + length, upper1, collection2, position2 + length, upper2, comparer, lcs))
                            yield return section;
                    }
                    else
                    {
                        // Unable to find a match, so just return section as unmatched
                        yield return new DiffSection(false, upper1 - lower1, upper2 - lower2);
                    }
                }
            }

            if (matchEnd > 0)
                yield return new DiffSection(true, matchEnd, matchEnd);
        }

        private static int MatchStart<T>([NotNull, ItemCanBeNull] IList<T> collection1, int lower1, int upper1, [NotNull, ItemCanBeNull] IList<T> collection2, int lower2, int upper2, [NotNull] IEqualityComparer<T> comparer)
        {
            int count = 0;

            // ReSharper disable AssignNullToNotNullAttribute
            while (lower1 < upper1 && lower2 < upper2 && comparer.Equals(collection1[lower1], collection2[lower2]))
            {
                count++;
                lower1++;
                lower2++;
            }
            // ReSharper restore AssignNullToNotNullAttribute

            return count;
        }

        private static int MatchEnd<T>([NotNull, ItemCanBeNull] IList<T> collection1, int lower1, int upper1, [NotNull, ItemCanBeNull] IList<T> collection2, int lower2, int upper2, [NotNull] IEqualityComparer<T> comparer)
        {
            int count = 0;

            // ReSharper disable AssignNullToNotNullAttribute
            while (upper1 > lower1 && upper2 > lower2 && comparer.Equals(collection1[upper1 - 1], collection2[upper2 - 1]))
            {
                count++;
                upper1--;
                upper2--;
            }
            // ReSharper restore AssignNullToNotNullAttribute

            return count;
        }
    }
}
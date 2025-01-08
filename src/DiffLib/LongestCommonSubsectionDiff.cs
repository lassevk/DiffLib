using System.Collections.Generic;

namespace DiffLib;

internal static class LongestCommonSubsectionDiff
{
    public static IEnumerable<DiffSection> Calculate<T>(IList<T?> collection1, IList<T?> collection2, DiffOptions options, IEqualityComparer<T?> comparer)
        => Calculate(collection1, 0, collection1.Count, collection2, 0, collection2.Count, comparer, new LongestCommonSubsequence<T>(collection1, collection2, comparer), options);

    private static IEnumerable<DiffSection> Calculate<T>(IList<T?> collection1, int lower1, int upper1, IList<T?> collection2, int lower2, int upper2, IEqualityComparer<T?> comparer, LongestCommonSubsequence<T> lcs, DiffOptions options)
    {
        // Short-circuit recursive call when nothing left (usually because match was found at the very start or end of a subsection
        if (lower1 == upper1 && lower2 == upper2)
            yield break;

        // Patience modification, let's find matching elements at both ends and remove those from LCS consideration
        int matchEnd = 0;

        if (options.EnablePatienceOptimization)
        {
            int matchStart = MatchStart(collection1, lower1, upper1, collection2, lower2, upper2, comparer);
            if (matchStart > 0)
            {
                yield return new DiffSection(true, matchStart, matchStart);

                lower1 += matchStart;
                lower2 += matchStart;
            }

            matchEnd = MatchEnd(collection1, lower1, upper1, collection2, lower2, upper2, comparer);
            if (matchEnd > 0)
            {
                upper1 -= matchEnd;
                upper2 -= matchEnd;
            }
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
                if (lcs.Find(lower1, upper1, lower2, upper2, out int position1, out int position2, out int length, options.ContextSize))
                {
                    // Recursively apply calculation to portion before common subsequence
                    foreach (DiffSection section in Calculate(collection1, lower1, position1, collection2, lower2, position2, comparer, lcs, options))
                        yield return section;

                    // Output match
                    yield return new DiffSection(true, length, length);

                    // Recursively apply calculation to portion after common subsequence
                    foreach (DiffSection section in Calculate(collection1, position1 + length, upper1, collection2, position2 + length, upper2, comparer, lcs, options))
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

    private static int MatchStart<T>(IList<T?> collection1, int lower1, int upper1, IList<T?> collection2, int lower2, int upper2, IEqualityComparer<T?> comparer)
    {
        int count = 0;

        while (lower1 < upper1 && lower2 < upper2 && comparer.Equals(collection1[lower1], collection2[lower2]))
        {
            count++;
            lower1++;
            lower2++;
        }

        return count;
    }

    private static int MatchEnd<T>(IList<T?> collection1, int lower1, int upper1, IList<T?> collection2, int lower2, int upper2, IEqualityComparer<T?> comparer)
    {
        int count = 0;

        while (upper1 > lower1 && upper2 > lower2 && comparer.Equals(collection1[upper1 - 1], collection2[upper2 - 1]))
        {
            count++;
            upper1--;
            upper2--;
        }

        return count;
    }
}
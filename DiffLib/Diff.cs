using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using static System.Math;

namespace DiffLib
{
    public static class Diff
    {
        [NotNull]
        public static IEnumerable<DiffElement<T>> Calculate<T>([NotNull] IEnumerable<T> collection1, [NotNull] IEnumerable<T> collection2, [CanBeNull] IEqualityComparer<T> comparer = null, [CanBeNull] IDiffMatchAligner<T> aligner = null)
        {
            var diff = new Diff<T>(collection1, collection2, comparer, aligner);
            return diff.Calculate();
        }
    }

    /// <summary>
    /// This class is responsible for calculating the difference between two collections.
    /// </summary>
    /// <typeparam name="T">
    /// The type of element in the collections.
    /// </typeparam>
    public sealed class Diff<T>
    {
        [NotNull]
        private readonly LongestCommonSubsequence<T> _LongestCommonSubsequence;

        public Diff([NotNull] IEnumerable<T> collection1, [NotNull] IEnumerable<T> collection2, [CanBeNull] IEqualityComparer<T> comparer = null, [CanBeNull] IDiffMatchAligner<T> aligner = null)
        {
            if (collection1 == null)
                throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null)
                throw new ArgumentNullException(nameof(collection2));

            Collection1 = collection1.AsList();
            Collection2 = collection2.AsList();
            Comparer = comparer ?? EqualityComparer<T>.Default;
            Aligner = aligner ?? new BasicInsertDeleteDiffMatchAligner<T>();
            _LongestCommonSubsequence = new LongestCommonSubsequence<T>(Collection1, Collection2, Comparer);
        }

        [NotNull]
        public IList<T> Collection2
        {
            get;
        }

        [NotNull]
        public IList<T> Collection1
        {
            get;
        }

        [NotNull]
        public IEqualityComparer<T> Comparer
        {
            get;
        }

        [NotNull]
        public IDiffMatchAligner<T> Aligner
        {
            get;
        }

        [NotNull]
        public IEnumerable<DiffElement<T>> Calculate() => Calculate(Collection1.Slice(0, Collection1.Count), Collection2.Slice(0, Collection2.Count));

        [NotNull]
        private IEnumerable<DiffElement<T>> Calculate(Slice<T> slice1, Slice<T> slice2)
        {
            // Patience modification, let's find matching elements at both ends and remove those from LCS consideration
            int matchStart = MatchStart(slice1, slice2);

            var subslice1 = slice1.Constrain(matchStart, 0);
            var subslice2 = slice2.Constrain(matchStart, 0);

            int matchEnd = MatchEnd(subslice1, subslice2);

            subslice1 = subslice1.Constrain(0, matchEnd);
            subslice2 = subslice2.Constrain(0, matchEnd);

            if (matchStart > 0)
                for (int index = 0; index < matchStart; index++)
                    yield return new DiffElement<T>(slice1[index], slice2[index], DiffOperation.None);

            if (subslice1.Count > 0 || subslice2.Count > 0)
            {
                if (subslice1.Count == 0 || subslice2.Count == 0)
                {
                    // Degenerate case, only one of the collections still have elements
                    foreach (var element in Align(subslice1.LowerBounds, subslice1.UpperBounds, subslice2.LowerBounds, subslice2.UpperBounds))
                        yield return element;
                }
                else
                {
                    int position1;
                    int position2;
                    int length;

                    if (_LongestCommonSubsequence.Find(subslice1, subslice2, out position1, out position2, out length))
                    {
                        // Recursively apply calculation to portion before common subsequence
                        foreach (var element in Calculate(subslice1.ConstrainFromStart(position1), subslice1.ConstrainFromStart(position2)))
                            yield return element;

                        // Output match
                        for (int index = 0; index < length; index++)
                            yield return new DiffElement<T>(subslice1[position1 + index], subslice2[position2 + index], DiffOperation.None);

                        // Recursively apply calculation to portion after common subsequence
                        foreach (var element in Calculate(subslice1.ConstrainFrom(position1 + length), subslice2.ConstrainFrom(position2 + length)))
                            yield return element;
                    }
                    else
                    {
                        // Unable to find a match, so just align as best as we can and output
                        foreach (var element in Align(subslice1.LowerBounds, subslice1.UpperBounds, subslice2.LowerBounds, subslice2.UpperBounds))
                            yield return element;
                    }
                }
            }

            if (matchEnd > 0)
                for (int index = 0; index < matchEnd; index++)
                    yield return new DiffElement<T>(slice1[slice1.UpperBounds - matchEnd + index], slice2[slice2.UpperBounds - matchEnd + index], DiffOperation.None);
        }

        [NotNull]
        private IEnumerable<DiffElement<T>> Align(int lower1, int upper1, int lower2, int upper2) => Aligner.Align(Collection1.Slice(lower1, upper1), Collection2.Slice(lower2, upper2));

        private int MatchStart(Slice<T> slice1, Slice<T> slice2)
        {
            int count = 0;

            for (int index = 0; index < Min(slice1.Count, slice2.Count); index++)
                if (Comparer.Equals(slice1[index], slice2[index]))
                    count++;
                else
                    break;

            return count;
        }

        private int MatchEnd(Slice<T> slice1, Slice<T> slice2)
        {
            int count = 0;
            for (int index = Min(slice1.Count, slice2.Count) - 1; index >= 0; index--)
                if (Comparer.Equals(slice1[index], slice2[index]))
                    count++;
                else
                    break;

            return count;
        }
    }
}
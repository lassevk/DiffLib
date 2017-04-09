using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// Static API class for the merge portion of DiffLib.
    /// </summary>
    public static class Merge
    {
        /// <summary>
        /// Performs a merge using a 3-way merge, returning the final merged output.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collections being merged.
        /// </typeparam>
        /// <param name="commonBase">
        /// The common base/ancestor of both <paramref name="left"/> and <paramref name="right"/>.
        /// </param>
        /// <param name="left">
        /// The left side being merged with the <paramref name="right"/>.
        /// </param>
        /// <param name="right">
        /// The right side being merged with the <paramref name="left"/>.
        /// </param>
        /// <param name="aligner">
        /// A <see cref="IDiffElementAligner{T}"/> implementation that will be responsible for lining up common vs. left and common vs. right as well as left vs. right
        /// during the merge.
        /// </param>
        /// <param name="conflictResolver">
        /// A <see cref="IMergeConflictResolver{T}"/> implementation that will be used to resolve conflicting modifications between left and right.
        /// </param>
        /// <param name="comparer">
        /// A <see cref="IEqualityComparer{T}"/> implementation that will be used to compare elements of all the collections. If <c>null</c> is specified then
        /// <see cref="EqualityComparer{T}.Default"/> will be used.
        /// </param>
        /// <returns>
        /// The final merged collection of elements from <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        /// <exception cref="MergeConflictException">
        /// The <paramref name="conflictResolver"/> threw a <see cref="MergeConflictException"/> to indicate a failure to resolve a conflict.
        /// </exception>
        [NotNull, ItemCanBeNull]
        public static IEnumerable<T> Perform<T>([NotNull] IList<T> commonBase, [NotNull] IList<T> left, [NotNull] IList<T> right, [NotNull] IDiffElementAligner<T> aligner, [NotNull] IMergeConflictResolver<T> conflictResolver, [CanBeNull] IEqualityComparer<T> comparer = null)
        {
            if (commonBase == null)
                throw new ArgumentNullException(nameof(commonBase));
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (aligner == null)
                throw new ArgumentNullException(nameof(aligner));
            if (conflictResolver == null)
                throw new ArgumentNullException(nameof(conflictResolver));

            comparer = comparer ?? EqualityComparer<T>.Default;

            List<DiffElement<T>> diffCommonBaseToLeft = Diff.AlignElements(commonBase, left, Diff.CalculateSections(commonBase, left, comparer), aligner).ToList();
            List<DiffElement<T>> diffCommonBaseToRight = Diff.AlignElements(commonBase, right, Diff.CalculateSections(commonBase, right, comparer), aligner).ToList();

            var sections = Diff.CalculateSections(diffCommonBaseToLeft, diffCommonBaseToRight, new DiffSectionMergeComparer<T>(comparer));

            int leftIndex = 0;
            int rightIndex = 0;
            foreach (var section in sections)
            {
                if (section.IsMatch)
                {
                    for (int index = 0; index < section.LengthInCollection1; index++)
                    {
                        var leftOp = diffCommonBaseToLeft[leftIndex].Operation;
                        if (leftOp == DiffOperation.Replace)
                            leftOp = DiffOperation.Modify;
                        var leftSide = diffCommonBaseToLeft[leftIndex].ElementFromCollection2.GetValueOrDefault();

                        var rightOp = diffCommonBaseToRight[rightIndex].Operation;
                        if (rightOp == DiffOperation.Replace)
                            rightOp = DiffOperation.Modify;
                        var rightSide = diffCommonBaseToRight[rightIndex].ElementFromCollection2.GetValueOrDefault();

                        if (leftOp == DiffOperation.Match && rightOp == DiffOperation.Match)
                        {
                            // both sides unmodified, either side wins
                            yield return leftSide;
                        }
                        else if (leftOp == DiffOperation.Modify && rightOp == DiffOperation.Modify)
                        {
                            // both sides modified, ask resolver
                            foreach (var item in conflictResolver.Resolve(new[] { diffCommonBaseToLeft[leftIndex].ElementFromCollection1.GetValueOrDefault() }, new[] { leftSide }, new[] { rightSide }))
                                yield return item;
                        }
                        else if (leftOp == DiffOperation.Match && rightOp == DiffOperation.Modify)
                        {
                            // left unmnodified, right modified, right wins
                            yield return rightSide;
                        }
                        else if (leftOp == DiffOperation.Modify && rightOp == DiffOperation.Match)
                        {
                            // left mnodified, right unmodified, left wins
                            yield return leftSide;
                        }
                        else if (leftOp == DiffOperation.Match && rightOp == DiffOperation.Delete)
                        {
                            // left unmodified, right deleted, right wins
                        }
                        else if (leftOp == DiffOperation.Delete && rightOp == DiffOperation.Match)
                        {
                            // left deleted, right unmodified, left wins
                        }
                        else if (leftOp == DiffOperation.Delete && rightOp == DiffOperation.Delete)
                        {
                            // both sides deleted, both win
                        }
                        else if (leftOp == DiffOperation.Modify && rightOp == DiffOperation.Delete)
                        {
                            // left modified, right deleted, ask resolver
                            foreach (var item in conflictResolver.Resolve(new[] { diffCommonBaseToLeft[leftIndex].ElementFromCollection1.GetValueOrDefault() }, new[] { leftSide }, new T[0]))
                                yield return item;
                        }
                        else if (leftOp == DiffOperation.Delete && rightOp == DiffOperation.Modify)
                        {
                            // left deleted, right modified, ask resolver
                            foreach (var item in conflictResolver.Resolve(new[] { diffCommonBaseToLeft[leftIndex].ElementFromCollection1.GetValueOrDefault() }, new T[0], new[] { rightSide }))
                                yield return item;
                        }
                        else
                        {
                            throw new MergeConflictException($"Unable to process {leftOp} vs. {rightOp}", new object[] { diffCommonBaseToLeft[leftIndex].ElementFromCollection1.GetValueOrDefault() }, new object[] { leftSide }, new object[] { rightSide });
                        }

                        leftIndex++;
                        rightIndex++;
                    }
                }
                else
                {
                    if (section.LengthInCollection1 == 0)
                    {
                        // right side inserted, right side wins
                        for (int index = 0; index < section.LengthInCollection2; index++)
                            yield return diffCommonBaseToRight[rightIndex + index].ElementFromCollection2.Value;
                    }
                    else if (section.LengthInCollection2 == 0)
                    {
                        // left side inserted, left side wins
                        for (int index = 0; index < section.LengthInCollection1; index++)
                            yield return diffCommonBaseToLeft[leftIndex + index].ElementFromCollection2.Value;
                    }
                    else
                    {
                        var leftSide = new List<T>();
                        for (int index = 0; index < section.LengthInCollection1; index++)
                            leftSide.Add(diffCommonBaseToLeft[leftIndex + index].ElementFromCollection2.Value);

                        var rightSide = new List<T>();
                        for (int index = 0; index < section.LengthInCollection2; index++)
                            rightSide.Add(diffCommonBaseToRight[rightIndex + index].ElementFromCollection2.Value);

                        foreach (var item in conflictResolver.Resolve(new List<T>(), leftSide, rightSide))
                            yield return item;
                    }

                    leftIndex += section.LengthInCollection1;
                    rightIndex += section.LengthInCollection2;
                }
            }
        }
    }
}
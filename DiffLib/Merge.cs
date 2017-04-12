using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace DiffLib
{
    internal class Merge<T> : IEnumerable<T>
    {
        [NotNull]
        private readonly IList<T> _CommonBase;

        [NotNull]
        private readonly IList<T> _Left;

        [NotNull]
        private readonly IList<T> _Right;

        [NotNull]
        private readonly IDiffElementAligner<T> _Aligner;

        [NotNull]
        private readonly IMergeConflictResolver<T> _ConflictResolver;

        [NotNull]
        private readonly IEqualityComparer<T> _Comparer;

        public Merge([NotNull] IList<T> commonBase, [NotNull] IList<T> left, [NotNull] IList<T> right, [NotNull] IDiffElementAligner<T> aligner, [NotNull] IMergeConflictResolver<T> conflictResolver, [NotNull] IEqualityComparer<T> comparer)
        {
            _CommonBase = commonBase ?? throw new ArgumentNullException(nameof(commonBase));
            _Left = left ?? throw new ArgumentNullException(nameof(left));
            _Right = right ?? throw new ArgumentNullException(nameof(right));
            _Aligner = aligner ?? throw new ArgumentNullException(nameof(aligner));
            _ConflictResolver = conflictResolver ?? throw new ArgumentNullException(nameof(conflictResolver));
            _Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public IEnumerator<T> GetEnumerator()
        {
            List<DiffElement<T>> diffCommonBaseToLeft = Diff.AlignElements(_CommonBase, _Left, Diff.CalculateSections(_CommonBase, _Left, _Comparer), _Aligner).ToList();
            Assume.That(diffCommonBaseToLeft != null);

            List<DiffElement<T>> diffCommonBaseToRight = Diff.AlignElements(_CommonBase, _Right, Diff.CalculateSections(_CommonBase, _Right, _Comparer), _Aligner).ToList();
            Assume.That(diffCommonBaseToRight != null);

            var sections = Diff.CalculateSections(diffCommonBaseToLeft, diffCommonBaseToRight, new DiffSectionMergeComparer<T>(_Comparer));

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
                            foreach (var item in _ConflictResolver.Resolve(new[] { diffCommonBaseToLeft[leftIndex].ElementFromCollection1.GetValueOrDefault() }, new[] { leftSide }, new[] { rightSide }))
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
                            foreach (var item in _ConflictResolver.Resolve(new[] { diffCommonBaseToLeft[leftIndex].ElementFromCollection1.GetValueOrDefault() }, new[] { leftSide }, new T[0]))
                                yield return item;
                        }
                        else if (leftOp == DiffOperation.Delete && rightOp == DiffOperation.Modify)
                        {
                            // left deleted, right modified, ask resolver
                            foreach (var item in _ConflictResolver.Resolve(new[] { diffCommonBaseToLeft[leftIndex].ElementFromCollection1.GetValueOrDefault() }, new T[0], new[] { rightSide }))
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

                        foreach (var item in _ConflictResolver.Resolve(new List<T>(), leftSide, rightSide))
                            yield return item;
                    }

                    leftIndex += section.LengthInCollection1;
                    rightIndex += section.LengthInCollection2;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiffLib;

internal class Merge<T> : IEnumerable<T?>
{
    private readonly IMergeConflictResolver<T?> _ConflictResolver;
    private readonly List<DiffSection> _MergeSections;
    private readonly List<DiffElement<T?>> _DiffCommonBaseToLeft;
    private readonly List<DiffElement<T?>> _DiffCommonBaseToRight;

    public Merge(IList<T?> commonBase, IList<T?> left, IList<T?> right, IDiffElementAligner<T?> aligner, IMergeConflictResolver<T?> conflictResolver, IEqualityComparer<T?> comparer, DiffOptions diffOptions)
    {
        _ = commonBase ?? throw new ArgumentNullException(nameof(commonBase));
        _ = left ?? throw new ArgumentNullException(nameof(left));
        _ = right ?? throw new ArgumentNullException(nameof(right));
        _ = aligner ?? throw new ArgumentNullException(nameof(aligner));
        _ = comparer ?? throw new ArgumentNullException(nameof(comparer));
        _ = diffOptions ?? throw new ArgumentNullException(nameof(diffOptions));

        _ConflictResolver = conflictResolver ?? throw new ArgumentNullException(nameof(conflictResolver));

        var diffCommonBaseToLeft = Diff.AlignElements(commonBase, left, Diff.CalculateSections(commonBase, left, diffOptions, comparer), aligner).ToList();
        _DiffCommonBaseToLeft = diffCommonBaseToLeft;

        var diffCommonBaseToRight = Diff.AlignElements(commonBase, right, Diff.CalculateSections(commonBase, right, diffOptions, comparer), aligner).ToList();
        _DiffCommonBaseToRight = diffCommonBaseToRight;

        var mergeSections = Diff.CalculateSections(diffCommonBaseToLeft!, diffCommonBaseToRight!, diffOptions, new DiffSectionMergeComparer<T>(comparer)).ToList();
        _MergeSections = mergeSections;
    }

    public IEnumerator<T?> GetEnumerator()
    {
        int leftIndex = 0;
        int rightIndex = 0;
        foreach (DiffSection section in _MergeSections)
        {
            if (section.IsMatch)
            {
                for (int index = 0; index < section.LengthInCollection1; index++)
                    foreach (T? item in ResolveMatchingElementFromBothSides(leftIndex++, rightIndex++))
                        yield return item;
            }
            else
            {
                foreach (T? item in ProcessNonMatchingElementsFromBothSides(section, rightIndex, leftIndex))
                    yield return item;

                leftIndex += section.LengthInCollection1;
                rightIndex += section.LengthInCollection2;
            }
        }
    }

    private IEnumerable<T?> ProcessNonMatchingElementsFromBothSides(DiffSection section, int rightIndex, int leftIndex)
    {
        if (section.LengthInCollection1 == 0)
        {
            // right side inserted, right side wins
            for (int index = 0; index < section.LengthInCollection2; index++)
                yield return _DiffCommonBaseToRight[rightIndex + index].ElementFromCollection2.Value;
        }
        else if (section.LengthInCollection2 == 0)
        {
            // left side inserted, left side wins
            for (int index = 0; index < section.LengthInCollection1; index++)
                yield return _DiffCommonBaseToLeft[leftIndex + index].ElementFromCollection2.Value;
        }
        else
        {
            var leftSide = new List<T?>();
            for (int index = 0; index < section.LengthInCollection1; index++)
                leftSide.Add(_DiffCommonBaseToLeft[leftIndex + index].ElementFromCollection2.Value);

            var rightSide = new List<T?>();
            for (int index = 0; index < section.LengthInCollection2; index++)
                rightSide.Add(_DiffCommonBaseToRight[rightIndex + index].ElementFromCollection2.Value);

            foreach (T? item in _ConflictResolver.Resolve(new List<T?>(), leftSide, rightSide))
                yield return item;
        }
    }

    private IEnumerable<T?> ResolveMatchingElementFromBothSides(int leftIndex, int rightIndex)
    {
        T? commonBase = _DiffCommonBaseToLeft[leftIndex].ElementFromCollection1.GetValueOrDefault();

        DiffOperation leftOp = _DiffCommonBaseToLeft[leftIndex].Operation;
        if (leftOp == DiffOperation.Replace)
            leftOp = DiffOperation.Modify;
        T? leftSide = _DiffCommonBaseToLeft[leftIndex].ElementFromCollection2.GetValueOrDefault();

        DiffOperation rightOp = _DiffCommonBaseToRight[rightIndex].Operation;
        if (rightOp == DiffOperation.Replace)
            rightOp = DiffOperation.Modify;
        T? rightSide = _DiffCommonBaseToRight[rightIndex].ElementFromCollection2.GetValueOrDefault();

        IEnumerable<T?> resolution = GetResolution(commonBase, leftOp, leftSide, rightOp, rightSide);
        return resolution;
    }

    private IEnumerable<T?> GetResolution(T? commonBase, DiffOperation leftOp, T? leftSide, DiffOperation rightOp, T? rightSide)
    {
        switch (leftOp)
        {
            case DiffOperation.Match:
                switch (rightOp)
                {
                    case DiffOperation.Match:
                        return [leftSide];
                    case DiffOperation.Insert:
                        break;
                    case DiffOperation.Delete:
                        return [];
                    case DiffOperation.Replace:
                    case DiffOperation.Modify:
                        return [rightSide];
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rightOp), rightOp, null);
                }

                break;

            case DiffOperation.Insert:
                break;

            case DiffOperation.Delete:
                switch (rightOp)
                {
                    case DiffOperation.Match:
                    case DiffOperation.Delete:
                        return [];
                    case DiffOperation.Insert:
                        break;
                    case DiffOperation.Replace:
                    case DiffOperation.Modify:
                        return _ConflictResolver.Resolve([commonBase], [], [rightSide]);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rightOp), rightOp, null);
                }

                break;

            case DiffOperation.Replace:
            case DiffOperation.Modify:
                switch (rightOp)
                {
                    case DiffOperation.Match:
                        return [leftSide];
                    case DiffOperation.Insert:
                        break;
                    case DiffOperation.Delete:
                        return _ConflictResolver.Resolve([commonBase], [leftSide], []);
                    case DiffOperation.Replace:
                    case DiffOperation.Modify:
                        return _ConflictResolver.Resolve([commonBase], [leftSide], [rightSide]);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(rightOp), rightOp, null);
                }

                break;
        }

        throw new MergeConflictException($"Unable to process {leftOp} vs. {rightOp}", [commonBase], [leftSide], [rightSide]);
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
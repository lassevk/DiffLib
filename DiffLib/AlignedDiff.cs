using System;
using System.Collections;
using System.Collections.Generic;

namespace DiffLib
{
    /// <summary>
    /// This class implements a slightly more advanced diff algorithm than <see cref="Diff{T}"/> by
    /// taking the output from <see cref="Diff{T}"/> and attempting to align individual elements inside
    /// replace-blocks. This is mostly suitable for text file diffs.
    /// </summary>
    /// <typeparam name="T">
    /// The types of elements in the collections being compared.
    /// </typeparam>
    public sealed class AlignedDiff<T> : IEnumerable<AlignedDiffChange<T>>
    {
        // If the combined lengths of the two change-sections is more than this number of
        // elements, punt to a delete + add for the entire change. The alignment code
        // is a recursive piece of code that can quickly balloon out of control, so
        // too big sections will take a long time to process. I will experiment more
        // with this number to see what is feasible.
        private const int MaximumChangedSectionSizeBeforePuntingToDeletePlusAdd = 15;

        private readonly Dictionary<AlignmentKey, ChangeNode> _BestAlignmentNodes =
            new Dictionary<AlignmentKey, ChangeNode>();

        private readonly IList<T> _Collection1;
        private readonly IList<T> _Collection2;
        private readonly Diff<T> _Diff;
        private readonly ISimilarityComparer<T> _SimilarityComparer;
        private readonly ISimilarityFilter<T> _SimilarityFilter;

        private int _Upper1;
        private int _Upper2;

        /// <summary>
        /// Initializes a new instance of <see cref="AlignedDiff{T}"/>.
        /// </summary>
        /// <param name="collection1">
        /// The first collection of items.
        /// </param>
        /// <param name="collection2">
        /// The second collection of items.
        /// </param>
        /// <param name="equalityComparer">
        /// The <see cref="IEqualityComparer{T}"/> that will be used to compare elements from
        /// <paramref name="collection1"/> with elements from <paramref name="collection2"/>.
        /// </param>
        /// <param name="similarityComparer">
        /// The <see cref="ISimilarityComparer{T}"/> that will be used to attempt to align elements
        /// inside blocks that consists of elements from the first collection being replaced
        /// with elements from the second collection.
        /// </param>
        /// <param name="similarityFilter">
        /// The <see cref="ISimilarityComparer{T}"/> that will be used to determine if
        /// two aligned elements are similar enough to be report them as a change from
        /// one to another, or to report them as one being deleted and the other added in
        /// its place.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection2"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="equalityComparer"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="similarityFilter"/> is <c>null</c>.</para>
        /// </exception>
        public AlignedDiff(IEnumerable<T> collection1, IEnumerable<T> collection2, IEqualityComparer<T> equalityComparer,
            ISimilarityComparer<T> similarityComparer, ISimilarityFilter<T> similarityFilter)
        {
            if (collection1 == null)
                throw new ArgumentNullException("collection1");
            if (collection2 == null)
                throw new ArgumentNullException("collection2");
            if (equalityComparer == null)
                throw new ArgumentNullException("equalityComparer");
            if (similarityComparer == null)
                throw new ArgumentNullException("similarityComparer");
            if (similarityFilter == null)
                throw new ArgumentNullException("similarityFilter");

            _Collection1 = collection1.ToRandomAccess();
            _Collection2 = collection2.ToRandomAccess();

            _Diff = new Diff<T>(_Collection1, _Collection2, equalityComparer);
            _SimilarityComparer = similarityComparer;
            _SimilarityFilter = similarityFilter;
        }

        #region IEnumerable<AlignedDiffChange<T>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<AlignedDiffChange<T>> GetEnumerator()
        {
            return Generate().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Generates the diff, one line of output at a time.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="AlignedDiffChange{T}"/> objects, one for
        /// each line in the first or second collection (sometimes one instance for a line
        /// from both, when lines are equal or similar.)
        /// </returns>
        public IEnumerable<AlignedDiffChange<T>> Generate()
        {
            int i1 = 0;
            int i2 = 0;
            foreach (DiffChange section in _Diff)
            {
                if (section.Equal)
                {
                    for (int index = 0; index < section.Length1; index++)
                    {
                        yield return new AlignedDiffChange<T>(ChangeType.Same, _Collection1[i1], _Collection2[i2]);
                        i1++;
                        i2++;
                    }
                }
                else
                {
                    bool deletePlusAdd = true;
                    if (section.Length1 > 0 && section.Length2 > 0)
                    {
                        AlignedDiffChange<T>[] alignedChanges = TryAlignChanges(section, i1, i2);
                        if (alignedChanges.Length > 0)
                        {
                            deletePlusAdd = false;
                            foreach (var change in alignedChanges)
                                yield return change;
                            i1 += section.Length1;
                            i2 += section.Length2;
                        }
                    }

                    if (deletePlusAdd)
                    {
                        for (int index = 0; index < section.Length1; index++)
                        {
                            yield return new AlignedDiffChange<T>(ChangeType.Deleted, _Collection1[i1], default(T));
                            i1++;
                        }
                        for (int index = 0; index < section.Length2; index++)
                        {
                            yield return new AlignedDiffChange<T>(ChangeType.Added, default(T), _Collection2[i2]);
                            i2++;
                        }
                    }
                }
            }
        }

        private AlignedDiffChange<T>[] TryAlignChanges(DiffChange change, int i1, int i2)
        {
            // "Optimization", too big input-sets will have to be dropped for now, will revisit this
            // number in the future to see if I can bring it up, or possible that I don't need it,
            // but since this is a recursive solution the combinations could get big fast.
            if (change.Length1 + change.Length2 > MaximumChangedSectionSizeBeforePuntingToDeletePlusAdd)
                return new AlignedDiffChange<T>[0];

            _BestAlignmentNodes.Clear();
            _Upper1 = i1 + change.Length1;
            _Upper2 = i2 + change.Length2;

            ChangeNode alignmentNodes = CalculateAlignmentNodes(i1, i2);
            if (alignmentNodes != null)
            {
                var result = new List<AlignedDiffChange<T>>();
                while (alignmentNodes != null)
                {
                    switch (alignmentNodes.Type)
                    {
                        case ChangeType.Added:
                            result.Add(new AlignedDiffChange<T>(ChangeType.Added, default(T), _Collection2[i2]));
                            i2++;
                            break;

                        case ChangeType.Deleted:
                            result.Add(new AlignedDiffChange<T>(ChangeType.Deleted, _Collection1[i1], default(T)));
                            i1++;
                            break;

                        case ChangeType.Changed:
                            if (_SimilarityFilter.IsSimilarEnough(_Collection1[i1], _Collection2[i2]))
                                result.Add(new AlignedDiffChange<T>(ChangeType.Changed, _Collection1[i1],
                                    _Collection2[i2]));
                            else
                            {
                                result.Add(new AlignedDiffChange<T>(ChangeType.Deleted, _Collection1[i1], default(T)));
                                result.Add(new AlignedDiffChange<T>(ChangeType.Added, default(T), _Collection2[i2]));
                            }
                            i1++;
                            i2++;
                            break;
                    }

                    alignmentNodes = alignmentNodes.Next;
                }
                return result.ToArray();
            }

            return new AlignedDiffChange<T>[0];
        }

        private ChangeNode CalculateAlignmentNodes(int i1, int i2)
        {
            ChangeNode result;
            if (_BestAlignmentNodes.TryGetValue(new AlignmentKey(i1, i2), out result))
                return result;

            if (i1 == _Upper1 && i2 == _Upper2)
                result = new ChangeNode(ChangeType.Same, 0.0, 0);
            else if (i1 == _Upper1)
            {
                ChangeNode restAfterAddition = CalculateAlignmentNodes(i1, i2 + 1);
                result = new ChangeNode(ChangeType.Added, restAfterAddition.Score, restAfterAddition.NodeCount + 1,
                    restAfterAddition);
            }
            else if (i2 == _Upper2)
            {
                ChangeNode restAfterDeletion = CalculateAlignmentNodes(i1 + 1, i2);
                result = new ChangeNode(ChangeType.Deleted, restAfterDeletion.Score, restAfterDeletion.NodeCount + 1,
                    restAfterDeletion);
            }
            else
            {
                ChangeNode restAfterAddition = CalculateAlignmentNodes(i1, i2 + 1);
                var resultAdded = new ChangeNode(ChangeType.Added,
                    restAfterAddition.Score,
                    restAfterAddition.NodeCount + 1, restAfterAddition);

                ChangeNode restAfterDeletion = CalculateAlignmentNodes(i1 + 1, i2);
                var resultDeleted = new ChangeNode(ChangeType.Deleted,
                    restAfterDeletion.Score,
                    restAfterDeletion.NodeCount + 1, restAfterDeletion);

                double similarity = _SimilarityComparer.Compare(_Collection1[i1], _Collection2[i2]);
                ChangeNode restAfterChange = CalculateAlignmentNodes(i1 + 1, i2 + 1);
                var resultChanged = new ChangeNode(ChangeType.Changed,
                    similarity + restAfterChange.Score,
                    restAfterChange.NodeCount + 1, restAfterChange);

                if (resultChanged.AverageScore >= resultAdded.AverageScore &&
                    resultChanged.AverageScore >= resultDeleted.AverageScore)
                    result = resultChanged;
                else if (resultAdded.AverageScore >= resultChanged.AverageScore &&
                         resultAdded.AverageScore >= resultDeleted.AverageScore)
                    result = resultAdded;
                else
                    result = resultDeleted;
            }

            _BestAlignmentNodes[new AlignmentKey(i1, i2)] = result;
            return result;
        }

        #region Nested type: AlignmentKey

        private struct AlignmentKey : IEquatable<AlignmentKey>
        {
            private readonly int _Position1;
            private readonly int _Position2;

            public AlignmentKey(int position1, int position2)
            {
                _Position1 = position1;
                _Position2 = position2;
            }

            #region IEquatable<AlignedDiff<T>.AlignmentKey> Members

            public bool Equals(AlignmentKey other)
            {
                return other._Position1 == _Position1 && other._Position2 == _Position2;
            }

            #endregion

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (obj.GetType() != typeof (AlignmentKey)) return false;
                return Equals((AlignmentKey) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_Position1*397) ^ _Position2;
                }
            }
        }

        #endregion

        #region Nested type: ChangeNode

        private class ChangeNode
        {
            public readonly ChangeNode Next;
            public readonly int NodeCount;
            public readonly double Score;
            public readonly ChangeType Type;

            public ChangeNode(ChangeType type, double score, int nodeCount, ChangeNode next = null)
            {
                Type = type;
                Score = score;
                Next = next;
                NodeCount = nodeCount;
            }

            public double AverageScore
            {
                get
                {
                    if (NodeCount == 0)
                        return 0.0;
                    else
                        return Score/NodeCount;
                }
            }
        }

        #endregion
    }
}
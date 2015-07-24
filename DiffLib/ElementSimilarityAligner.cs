using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This class implements a <see cref="IDiffElementAligner{T}"/> strategy that will try
    /// to work out the best way to align two portions, depending on individual element
    /// similarity.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the two collections to align.
    /// </typeparam>
    [PublicAPI]
    public class ElementSimilarityDiffElementAligner<T> : IDiffElementAligner<T>
    {
        // If the combined lengths of the two change-sections is more than this number of
        // elements, punt to a delete + add for the entire change. The alignment code
        // is a recursive piece of code that can quickly balloon out of control, so
        // too big sections will take a long time to process. I will experiment more
        // with this number to see what is feasible.
        private const int MaximumChangedSectionSizeBeforePuntingToDeletePlusAdd = 15;

        [NotNull]
        private readonly ElementSimilarity<T> _SimilarityFunc;

        private readonly double _ModificationThreshold;

        [NotNull]
        private readonly IDiffElementAligner<T> _BasicAligner = new BasicInsertDeleteDiffElementAligner<T>();

        /// <summary>
        /// Constructs a new <see cref="ElementSimilarityDiffElementAligner{T}"/>.
        /// </summary>
        /// <param name="similarityFunc">
        /// A <see cref="ElementSimilarity{T}"/> delegate that is used to work out how similar two elements are.
        /// </param>
        /// <param name="modificationThreshold">
        /// A threshold value used to determine if aligned elements are considered replacements or modifications. If
        /// two items are more similar than the threshold specifies (similarity > threshold), then it results in
        /// a <see cref="DiffOperation.Modify"/>, otherwise it results in a <see cref="DiffOperation.Replace"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="similarityFunc"/> is <c>null</c>.</para>
        /// </exception>
        [PublicAPI]
        public ElementSimilarityDiffElementAligner([NotNull] ElementSimilarity<T> similarityFunc, double modificationThreshold = 0.3333)
        {
            if (similarityFunc == null)
                throw new ArgumentNullException(nameof(similarityFunc));

            _SimilarityFunc = similarityFunc;
            _ModificationThreshold = modificationThreshold;
        }

        /// <summary>
        /// Align the specified portions of the two collections and output element-by-element operations for the aligned elements.
        /// </summary>
        /// <param name="collection1">
        /// The first collection.
        /// </param>
        /// <param name="start1">
        /// The start of the portion to look at in the first collection, <paramref name="collection1"/>.
        /// </param>
        /// <param name="length1">
        /// The length of the portion to look at in the first collection, <paramref name="collection1"/>.
        /// </param>
        /// <param name="collection2">
        /// The second collection.
        /// </param>
        /// <param name="start2">
        /// The start of the portion to look at in the second collection, <paramref name="collection2"/>.
        /// </param>
        /// <param name="length2">
        /// The length of the portion to look at in the second collection, <paramref name="collection2"/>.
        /// </param>
        /// <returns>
        /// A collection of <see cref="DiffElement{T}"/> values, one for each aligned element.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection2"/> is <c>null</c>.</para>
        /// </exception>
        [PublicAPI, NotNull]
        public IEnumerable<DiffElement<T>> Align([NotNull] IList<T> collection1, int start1, int length1, [NotNull] IList<T> collection2, int start2, int length2)
        {
            if (collection1 == null)
                throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null)
                throw new ArgumentNullException(nameof(collection2));

            if (length1 > 0 && length2 > 0)
            {
                var elements = TryAlignSections(collection1, start1, length1, collection2, start2, length2);
                if (elements.Count > 0)
                    return elements;
            }

            return _BasicAligner.Align(collection1, start1, length1, collection2, start2, length2);
        }

        [NotNull]
        private List<DiffElement<T>> TryAlignSections([NotNull] IList<T> collection1, int start1, int length1, [NotNull] IList<T> collection2, int start2, int length2)
        {
            // "Optimization", too big input-sets will have to be dropped for now, will revisit this
            // number in the future to see if I can bring it up, or possible that I don't need it,
            // but since this is a recursive solution the combinations could get big fast.
            if (length1 + length2 > MaximumChangedSectionSizeBeforePuntingToDeletePlusAdd)
                return new List<DiffElement<T>>();

            var nodes = new Dictionary<AlignmentKey, AlignmentNode>();
            var bestNode = CalculateBestAlignment(nodes, collection1, start1, start1 + length1, collection2, start2, start2 + length2);

            var result = new List<DiffElement<T>>();
            while (bestNode != null)
            {
                if (bestNode.NodeCount > 0)
                {
                    switch (bestNode.Operation)
                    {
                        case DiffOperation.Match:
                        case DiffOperation.Replace:
                        case DiffOperation.Modify:
                            result.Add(new DiffElement<T>(collection1[start1], collection2[start2], bestNode.Operation));
                            start1++;
                            start2++;
                            break;

                        case DiffOperation.Insert:
                            result.Add(new DiffElement<T>(Option<T>.None, collection2[start2], bestNode.Operation));
                            start2++;
                            break;

                        case DiffOperation.Delete:
                            result.Add(new DiffElement<T>(collection1[start1], Option<T>.None, bestNode.Operation));
                            start1++;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                bestNode = bestNode.Next;
            }
            return result;
        }

        [NotNull]
        private AlignmentNode CalculateBestAlignment([NotNull] Dictionary<AlignmentKey, AlignmentNode> nodes, [NotNull] IList<T> collection1, int lower1, int upper1, [NotNull] IList<T> collection2, int lower2, int upper2)
        {
            var key = new AlignmentKey(lower1, lower2);
            AlignmentNode result;
            if (nodes.TryGetValue(key, out result))
            {
                Assume.That(result != null);
                return result;
            }

            if (lower1 == upper1 && lower2 == upper2)
                result = new AlignmentNode(DiffOperation.Match, 0.0, 0, null);
            else if (lower1 == upper1)
            {
                var restAfterInsert = CalculateBestAlignment(nodes, collection1, lower1, upper1, collection2, lower2 + 1, upper2);
                result = new AlignmentNode(DiffOperation.Insert, restAfterInsert.Similarity, restAfterInsert.NodeCount + 1, restAfterInsert);
            }
            else if (lower2 == upper2)
            {
                var restAfterDelete = CalculateBestAlignment(nodes, collection1, lower1 + 1, upper1, collection2, lower2, upper2);
                result = new AlignmentNode(DiffOperation.Delete, restAfterDelete.Similarity, restAfterDelete.NodeCount + 1, restAfterDelete);
            }
            else
            {
                // Calculate how the results will be if we insert a new element
                var restAfterInsert = CalculateBestAlignment(nodes, collection1, lower1, upper1, collection2, lower2 + 1, upper2);
                var resultInsert = new AlignmentNode(DiffOperation.Insert, restAfterInsert.Similarity, restAfterInsert.NodeCount + 1, restAfterInsert);

                // Calculate how the results will be if we delete an element
                var restAfterDelete = CalculateBestAlignment(nodes, collection1, lower1 + 1, upper1, collection2, lower2, upper2);
                var resultDelete = new AlignmentNode(DiffOperation.Delete, restAfterDelete.Similarity, restAfterDelete.NodeCount + 1, restAfterDelete);

                // Calculate how the results will be if we replace or modify an element
                var restAfterChange = CalculateBestAlignment(nodes, collection1, lower1 + 1, upper1, collection2, lower2 + 1, upper2);
                double similarity = _SimilarityFunc(collection1[lower1], collection2[lower2]);
                AlignmentNode resultChange = null;
                if (similarity >= _ModificationThreshold)
                    resultChange = new AlignmentNode(DiffOperation.Modify, similarity + restAfterInsert.Similarity, restAfterChange.NodeCount + 1, restAfterChange);

                // Then pick the operation that resulted in the best average similarity
                result = resultDelete;
                if (resultInsert.AverageSimilarity > result.AverageSimilarity)
                    result = resultInsert;
                if (resultChange?.AverageSimilarity > result.AverageSimilarity)
                    result = resultChange;
            }

            nodes[key] = result;
            return result;
        }
    }
}
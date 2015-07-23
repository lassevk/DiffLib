using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace DiffLib
{
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

        private readonly double _AlignmentThreshold;

        [NotNull]
        private readonly IDiffElementAligner<T> _BasicAligner = new BasicInsertDeleteDiffElementAligner<T>(); 

        public ElementSimilarityDiffElementAligner([NotNull] ElementSimilarity<T> similarityFunc, double alignmentThreshold = 0.3333)
        {
            if (similarityFunc == null)
                throw new ArgumentNullException(nameof(similarityFunc));

            _SimilarityFunc = similarityFunc;
            _AlignmentThreshold = alignmentThreshold;
        }

        public IEnumerable<DiffElement<T>> Align([NotNull] IList<T> collection1, int start1, int length1, [NotNull] IList<T> collection2, int start2, int length2)
        {
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
                        case DiffOperation.None:
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
                result = new AlignmentNode(DiffOperation.None, 0.0, 0, null);
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
                double similarity = _SimilarityFunc(collection1[lower2], collection2[lower2]);
                AlignmentNode resultChange = null;
                if (similarity >= _AlignmentThreshold)
                    resultChange = new AlignmentNode(DiffOperation.Modify, similarity, restAfterChange.NodeCount + 1, restAfterChange);

                // Then pick the operation that resulted in the best average similarity
                Debug.WriteLine("insert: " + resultInsert.AverageSimilarity);
                Debug.WriteLine("delete: " + resultDelete.AverageSimilarity);
                Debug.WriteLine("change: " + resultChange?.AverageSimilarity);
                result = resultInsert;
                if (resultDelete.AverageSimilarity > result.AverageSimilarity)
                    result = resultDelete;
                if (resultChange?.AverageSimilarity > result.AverageSimilarity)
                    result = resultChange;
            }

            nodes[key] = result;
            return result;
        }
    }
}
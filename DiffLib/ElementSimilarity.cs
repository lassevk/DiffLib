using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This delegate is used by <see cref="ElementSimilarityDiffElementAligner{T}"/> to work out just how similar
    /// two elements are.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements to work out the similarity between.
    /// </typeparam>
    /// <param name="element1">
    /// The first element.
    /// </param>
    /// <param name="element2">
    /// The second element.
    /// </param>
    /// <returns>
    /// A <see cref="double"/> value between 0.0 and 1.0 specifying how similar the two items are. 0.0 means
    /// not similar at all, and 1.0 means the equivalent of equality. Values below zero or above one
    /// will result in undefined behavior.
    /// </returns>
    [PublicAPI]
    public delegate double ElementSimilarity<in T>([CanBeNull] T element1, [CanBeNull] T element2);
}
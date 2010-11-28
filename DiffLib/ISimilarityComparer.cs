namespace DiffLib
{
    /// <summary>
    /// This interface must be implemented by classes that will do similarity-calculation
    /// for use with the <see cref="AlignedDiff{T}"/> class.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements being compared.
    /// </typeparam>
    public interface ISimilarityComparer<in T>
    {
        /// <summary>
        /// Does a similarity comparison between the two values and returns their
        /// similarity, a value ranging from 0.0 to 1.0, where 0.0 means they're
        /// completely different and 1.0 means they have the same value.
        /// </summary>
        /// <param name="x">
        /// The first value to compare.
        /// </param>
        /// <param name="y">
        /// The second value to compare.
        /// </param>
        /// <returns>
        /// A value ranging from 0.0 to 1.0, where 0.0 means they're
        /// completely different and 1.0 means they have the same value.
        /// </returns>
        double Compare(T x, T y);
    }
}
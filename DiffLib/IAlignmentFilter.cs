namespace DiffLib
{
    /// <summary>
    /// This interface must be implemented by classes that will do similarity-filtering
    /// during alignment (<see cref="AlignedDiff{T}"/>) to determine
    /// if two aligned elements are similar enough to report
    /// them as a change, instead of as a delete plus an add.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements being compared.
    /// </typeparam>
    public interface IAlignmentFilter<in T>
    {
        /// <summary>
        /// Determines if the two values are similar enough to align them
        /// as a change, instead of not aligning them but reporting them
        /// as a delete plus an add instead.
        /// </summary>
        /// <param name="value1">
        /// The first value to compare against <paramref name="value2"/>.
        /// </param>
        /// <param name="value2">
        /// The second value to compare against <paramref name="value1"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the two values are similar enough to report
        /// them as a change; false if the two values aren't similar enough
        /// but needs to be reported as a delete plus an add.
        /// </returns>
        bool CanAlign(T value1, T value2);
    }
}
namespace DiffLib
{
    /// <summary>
    /// This enum is used by <see cref="AlignedDiffChange{T}"/> to specify how
    /// the two elements from the two collections relate.
    /// </summary>
    public enum ChangeType
    {
        /// <summary>
        /// The two elements are the same.
        /// </summary>
        Same,

        /// <summary>
        /// The second element was added in the second collection.
        /// </summary>
        Added,

        /// <summary>
        /// The first element was removed from the second collection.
        /// </summary>
        Deleted,

        /// <summary>
        /// The first element was replaced with the second element in the second collection.
        /// </summary>
        Replaced,
    }
}
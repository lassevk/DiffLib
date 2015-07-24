namespace DiffLib
{
    /// <summary>
    /// This enum is returned as part of <see cref="DiffElement{T}"/>, specifying how
    /// elements from the two collections were aligned.
    /// </summary>
    public enum DiffOperation
    {
        /// <summary>
        /// The two elements, <see cref="DiffElement{T}.ElementFromCollection1"/> and <see cref="DiffElement{T}.ElementFromCollection2"/> was aligned because they match.
        /// </summary>
        Match,

        /// <summary>
        /// There was no matching element in the first collection. <see cref="DiffElement{T}.ElementFromCollection2"/> contains the element to insert.
        /// </summary>
        Insert,

        /// <summary>
        /// There was no matching element in the second collection. <see cref="DiffElement{T}.ElementFromCollection1"/> contains the element to delete.
        /// </summary>
        Delete,

        /// <summary>
        /// The two elements, <see cref="DiffElement{T}.ElementFromCollection1"/> and <see cref="DiffElement{T}.ElementFromCollection2"/> was aligned
        /// but only in terms of the element from collection 2 should replace the element from collection 1. They were not deemed "similar enough" to
        /// warrant a <see cref="Modify"/> operation.
        /// </summary>
        /// <remarks>
        /// Typical example would be two lines of text where most or all of the text has been changed.
        /// </remarks>
        Replace,

        /// <summary>
        /// The two elements, <see cref="DiffElement{T}.ElementFromCollection1"/> and <see cref="DiffElement{T}.ElementFromCollection2"/> was aligned
        /// but only in terms of the element from collection 2 should be used to modify the element from collection 1. The alignment strategy implementation
        /// deemed the two elements unequal, but similar enough to warrant alignment.
        /// </summary>
        /// <remarks>
        /// Typical example would be two lines of text where only bits and pieces have been altered.
        /// </remarks>
        Modify
    }
}
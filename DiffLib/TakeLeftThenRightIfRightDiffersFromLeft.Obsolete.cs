using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This implementation of <see cref="IMergeConflictResolver{T}"/> resolves a conflict by taking the left side and then taking the right side. In the case
    /// where both cases are identical, only the left side is taken.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collections being merged.
    /// </typeparam>
    /// <remarks>
    /// This type has the wrong name and is missing "MergeConflictResolver" suffix, please use <see cref="TakeLeftThenRightIfRightDiffersFromLeftMergeConflictResolver{T}"/>
    /// instead.
    /// </remarks>
    [Obsolete("Use TakeLeftThenRightIfRightDiffersFromLeftMergeConflictResolver<T> instead")]
    [PublicAPI]
    public class TakeLeftThenRightIfRightDiffersFromLeft<T> : TakeLeftThenRightIfRightDiffersFromLeftMergeConflictResolver<T>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="TakeLeftThenRightIfRightDiffersFromLeft{T}"/> using the specified <paramref name="equalityComparer"/>.
        /// </summary>
        /// <param name="equalityComparer">
        /// The <see cref="IEqualityComparer{T}"/> to use when determining if elements of the left side of a conflict matches those on the right side. If
        /// <c>null</c> then <see cref="EqualityComparer{T}.Default"/> is used.
        /// </param>
        public TakeLeftThenRightIfRightDiffersFromLeft([CanBeNull] IEqualityComparer<T> equalityComparer = null)
            : base(equalityComparer)
        {
        }
    }
}
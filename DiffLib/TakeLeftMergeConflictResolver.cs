using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This implementation of <see cref="IMergeConflictResolver{T}"/> always takes the left side.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collections being merged.
    /// </typeparam>
    public class TakeLeftMergeConflictResolver<T> : IMergeConflictResolver<T>
    {
        /// <inheritdoc />
        [NotNull, ItemCanBeNull]
        public IEnumerable<T> Resolve([NotNull] IList<T> commonBase, [NotNull] IList<T> left, [NotNull] IList<T> right)
        {
            return left;
        }
    }
}
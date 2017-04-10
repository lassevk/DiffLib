using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This implementation of <see cref="IMergeConflictResolver{T}"/> takes the left side then takes the right side.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collections being merged.
    /// </typeparam>
    public class TakeLeftThenRightMergeConflictResolver<T> : IMergeConflictResolver<T>
    {
        /// <inheritdoc />
        [NotNull, ItemCanBeNull]
        public IEnumerable<T> Resolve([NotNull] IList<T> commonBase, [NotNull] IList<T> left, [NotNull] IList<T> right)
        {
            foreach (var item in left)
                yield return item;

            foreach (var item in right)
                yield return item;
        }
    }
}
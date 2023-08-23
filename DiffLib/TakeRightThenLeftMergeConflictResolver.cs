using System.Collections.Generic;

namespace DiffLib
{
    /// <summary>
    /// This implementation of <see cref="IMergeConflictResolver{T}"/> takes the left side then takes the right side.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collections being merged.
    /// </typeparam>
    public class TakeRightThenLeftMergeConflictResolver<T> : IMergeConflictResolver<T>
    {
        /// <inheritdoc />
        public IEnumerable<T?> Resolve(IList<T?> commonBase, IList<T?> left, IList<T?> right)
        {
            foreach (T? item in right)
                yield return item;

            foreach (T? item in left)
                yield return item;
        }
    }
}
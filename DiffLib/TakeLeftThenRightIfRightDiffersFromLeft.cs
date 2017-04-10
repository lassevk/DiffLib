using System;
using System.Collections.Generic;
using System.Linq;

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
    public class TakeLeftThenRightIfRightDiffersFromLeft<T> : IMergeConflictResolver<T>
    {
        private readonly IEqualityComparer<T> _EqualityComparer;

        /// <summary>
        /// Constructs a new instance of <see cref="TakeLeftThenRightIfRightDiffersFromLeft{T}"/> using the specified <paramref name="equalityComparer"/>.
        /// </summary>
        /// <param name="equalityComparer">
        /// The <see cref="IEqualityComparer{T}"/> to use when determining if elements of the left side of a conflict matches those on the right side. If
        /// <c>null</c> then <see cref="EqualityComparer{T}.Default"/> is used.
        /// </param>
        public TakeLeftThenRightIfRightDiffersFromLeft([CanBeNull] IEqualityComparer<T> equalityComparer = null)
        {
            _EqualityComparer = equalityComparer ?? EqualityComparer<T>.Default;
        }

        /// <inheritdoc />
        [NotNull, ItemCanBeNull]
        public IEnumerable<T> Resolve([NotNull] IList<T> commonBase, [NotNull] IList<T> left, [NotNull] IList<T> right)
        {
            foreach (var item in left)
                yield return item;

            if (left.SequenceEqual(right, _EqualityComparer))
                yield break;

            foreach (var item in right)
                yield return item;
        }
    }
}
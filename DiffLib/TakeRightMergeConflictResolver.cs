using System.Collections.Generic;

namespace DiffLib;

/// <summary>
/// This implementation of <see cref="IMergeConflictResolver{T}"/> always takes the right side.
/// </summary>
/// <typeparam name="T">
/// The type of elements in the collections being merged.
/// </typeparam>
public class TakeRightMergeConflictResolver<T> : IMergeConflictResolver<T>
{
    /// <inheritdoc />
    public IEnumerable<T?> Resolve(IList<T?> commonBase, IList<T?> left, IList<T?> right)
        => right;
}
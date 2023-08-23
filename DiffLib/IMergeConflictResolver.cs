using System.Collections.Generic;

namespace DiffLib
{
    /// <summary>
    /// This interface must be implemented by concrete types that should be used during a merge conflict resolution and will be reponsible for working out how to
    /// resolve each individual conflict.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collections being merged.
    /// </typeparam>
    public interface IMergeConflictResolver<T>
    {
        /// <summary>
        /// Must resolve one conflict, given the common base items, the left items and the right items.
        /// </summary>
        /// <param name="commonBase">
        /// The matching elements from the common base. Note that this can be empty in the case of inserts.
        /// </param>
        /// <param name="left">
        /// The left modification or insert, as compared to the <paramref name="commonBase"/>.
        /// </param>
        /// <param name="right">
        /// The right modification or insert, as compared to the <paramref name="commonBase"/>.
        /// </param>
        /// <returns>
        /// The final items that should be used to resolve the conflict. These items will be used instead
        /// of whatever was found from the left and right side.
        /// </returns>
        /// <exception cref="MergeConflictException">
        /// The conflict resolver implementation is unable to determine how to resolve the conflict. Throwing this exception will abort the merge.
        /// </exception>
        /// <remarks>
        /// <para>Note that the conflict resolver mechanism will only be invoked when there is actually a conflict. If this method is called with
        /// a common base, and one of the sides are empty, the other is not, then this actually means that one side deleted the content and the
        /// other modified it. If during the merge one side leaves the content unmodified and the other deletes it, then this is not a conflict and the
        /// conflict resolver will not be invoked for this case.</para>
        /// <para>The cases the resolved will be asked to handle are modify vs. modify (both sides modified, both common, left and right will have content),
        /// modify vs. delete (one side modified, the other deleted, common and the side that modified will have data, the other side will be empty).</para>
        /// </remarks>
        IEnumerable<T?> Resolve(IList<T?> commonBase, IList<T?> left, IList<T?> right);
    }
}
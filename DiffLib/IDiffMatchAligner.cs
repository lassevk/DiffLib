using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This interface is used by <see cref="Diff.AlignElements{T}"/> to specify the alignment strategy/implementation to use.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the two collections to align portions of.
    /// </typeparam>
    [PublicAPI]
    public interface IDiffElementAligner<T>
    {
        /// <summary>
        /// Align the specified portions of the two collections and output element-by-element operations for the aligned elements.
        /// </summary>
        /// <param name="collection1">
        /// The first collection.
        /// </param>
        /// <param name="start1">
        /// The start of the portion to look at in the first collection, <paramref name="collection1"/>.
        /// </param>
        /// <param name="length1">
        /// The length of the portion to look at in the first collection, <paramref name="collection1"/>.
        /// </param>
        /// <param name="collection2">
        /// The second collection.
        /// </param>
        /// <param name="start2">
        /// The start of the portion to look at in the second collection, <paramref name="collection2"/>.
        /// </param>
        /// <param name="length2">
        /// The length of the portion to look at in the second collection, <paramref name="collection2"/>.
        /// </param>
        /// <returns>
        /// A collection of <see cref="DiffElement{T}"/> values, one for each aligned element.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection2"/> is <c>null</c>.</para>
        /// </exception>
        [PublicAPI, NotNull]
        IEnumerable<DiffElement<T>> Align([NotNull] IList<T> collection1, int start1, int length1, [NotNull] IList<T> collection2, int start2, int length2);
    }
}
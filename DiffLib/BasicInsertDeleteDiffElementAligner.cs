﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This class can be used as a parameter to <see cref="Diff.AlignElements{T}"/>. It will basically output anything present in the first collection
    /// as a sequence of delete operations, and anything present in the second collection as a sequence of insert operations.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the two collections.
    /// </typeparam>
    [PublicAPI]
    public class BasicInsertDeleteDiffElementAligner<T> : IDiffElementAligner<T>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="BasicInsertDeleteDiffElementAligner{T}"/>.
        /// </summary>
        [PublicAPI]
        public BasicInsertDeleteDiffElementAligner()
        {
        }

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
        public virtual IEnumerable<DiffElement<T>> Align([NotNull] IList<T> collection1, int start1, int length1, [NotNull] IList<T> collection2, int start2, int length2)
        {
            if (collection1 == null)
                throw new ArgumentNullException(nameof(collection1));
            if (collection2 == null)
                throw new ArgumentNullException(nameof(collection2));

            for (int index = 0; index < length1; index++)
                yield return new DiffElement<T>(collection1[start1 + index], Option<T>.None, DiffOperation.Delete);

            for (int index = 0; index < length2; index++)
                yield return new DiffElement<T>(Option<T>.None, collection2[start2 + index], DiffOperation.Insert);
        }
    }
}
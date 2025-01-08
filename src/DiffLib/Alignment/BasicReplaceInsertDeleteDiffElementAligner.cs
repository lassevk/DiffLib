using System;
using System.Collections.Generic;

using static System.Math;

namespace DiffLib.Alignment;

/// <summary>
/// This class can be used as a parameter to <see cref="Diff.AlignElements{T}"/>.
/// It will output a number of replace operations, depending on overlap, and then anything leftover that is present in the first collection
/// as a sequence of delete operations, and in the second collection as a sequence of insert operations.
/// </summary>
/// <typeparam name="T">
/// The type of elements in the two collections.
/// </typeparam>
public class BasicReplaceInsertDeleteDiffElementAligner<T> : BasicInsertDeleteDiffElementAligner<T>
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
    public override IEnumerable<DiffElement<T?>> Align(IList<T?> collection1, int start1, int length1, IList<T?> collection2, int start2, int length2)
    {
        int replaceCount = Min(length1, length2);
        for (int index = 0; index < replaceCount; index++)
            yield return new DiffElement<T?>(start1 + index, collection1[start1 + index], start2 + index, collection2[start2 + index], DiffOperation.Replace);

        foreach (DiffElement<T?> element in base.Align(collection1, start1 + replaceCount, length1 - replaceCount, collection2, start2 + replaceCount, length2 - replaceCount))
            yield return element;
    }
}
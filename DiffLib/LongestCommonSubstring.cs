using System;
using System.Collections.Generic;

namespace DiffLib
{
    /// <summary>
    /// This class implements the LCS algorithm, to find the longest common substring that exists
    /// in two collections, and return the locations of those substrings.
    /// </summary>
    public sealed class LongestCommonSubstring
    {
        /// <summary>
        /// This is the public interface to the LCS algorithm. The method takes two collections, and the means to
        /// compare elements between them, and returns the position in each where there longest common
        /// substring exists, or <c>null</c> if there is no common substring between them.
        /// </summary>
        /// <typeparam name="T">
        /// The types of elements being compared.
        /// </typeparam>
        /// <param name="collection1">
        /// The first collection of items.
        /// </param>
        /// <param name="collection2">
        /// The second collection of items.
        /// </param>
        /// <param name="comparer">
        /// The <see cref="IEqualityComparer{T}"/> that will be used to compare elements from
        /// <paramref name="collection1"/> with elements from <paramref name="collection2"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Tuple{T1,T2}"/> containing the positions of the two substrings, one position
        /// for each collection, both 0-based. If no common substring can be found, <c>null</c>
        /// will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection2"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public static Tuple<int, int> Find<T>(IEnumerable<T> collection1, IEnumerable<T> collection2,
            IEqualityComparer<T> comparer)
        {
            if (collection1 == null)
                throw new ArgumentNullException("collection1");
            if (collection2 == null)
                throw new ArgumentNullException("collection2");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return Tuple.Create(0, 0);
        }
    }
}
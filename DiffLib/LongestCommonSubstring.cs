using System;
using System.Collections.Generic;
using System.Linq;

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
        /// compare elements between them, and returns a <see cref="LongestCommonSubstringResult"/> containing
        /// information about the location, or <c>null</c> if there is no common substring between them.
        /// This overload uses the default <see cref="EqualityComparer{T}"/> implementation
        /// for <typeparamref name="T"/>.
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
        /// <returns>
        /// A <see cref="LongestCommonSubstringResult"/> containing the positions of the two substrings, one position
        /// for each collection, both 0-based, and the length of the substring. If no common substring can be found, <c>null</c>
        /// will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection2"/> is <c>null</c>.</para>
        /// </exception>
        public static LongestCommonSubstringResult Find<T>(IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            return Find(collection1, collection2, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// This is the public interface to the LCS algorithm. The method takes two collections, and the means to
        /// compare elements between them, and returns a <see cref="LongestCommonSubstringResult"/> containing
        /// information about the location, or <c>null</c> if there is no common substring between them.
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
        /// A <see cref="LongestCommonSubstringResult"/> containing the positions of the two substrings, one position
        /// for each collection, both 0-based, and the length of the substring. If no common substring can be found, <c>null</c>
        /// will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection2"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public static LongestCommonSubstringResult Find<T>(IEnumerable<T> collection1, IEnumerable<T> collection2,
            IEqualityComparer<T> comparer)
        {
            if (collection1 == null)
                throw new ArgumentNullException("collection1");
            if (collection2 == null)
                throw new ArgumentNullException("collection2");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            if (ReferenceEquals(collection1, collection2))
                return new LongestCommonSubstringResult(0, 0, collection1.Count());

            var list1 = collection1.Select(e => Tuple.Create(comparer.GetHashCode(e), e)).ToArray();
            var list2 = collection2.Select(e => Tuple.Create(comparer.GetHashCode(e), e)).ToArray();

            Dictionary<int, List<int>> lookupTable = GetLookupTableForSecondCollection(comparer, list2);

            int maxMatchingLength = -1;
            int maxMatchingPosition1 = -1;
            int maxMatchingPosition2 = -1;
            for (int index1 = 0; index1 < list1.Length; index1++)
            {
                List<int> occurances;
                if (lookupTable.TryGetValue(list1[index1].Item1, out occurances))
                {
                    foreach (int index2 in occurances)
                    {
                        int length = CountMatchingElements(list1, index1, list2, index2, comparer);
                        if (length > 0 && length > maxMatchingLength)
                        {
                            maxMatchingLength = length;
                            maxMatchingPosition1 = index1;
                            maxMatchingPosition2 = index2;
                        }
                    }
                }
            }

            if (maxMatchingLength < 0)
                return null;

            return new LongestCommonSubstringResult(maxMatchingPosition1, maxMatchingPosition2, maxMatchingLength);
        }

        private static int CountMatchingElements<T>(Tuple<int, T>[] collection1, int index1, Tuple<int, T>[] collection2, int index2, IEqualityComparer<T> comparer)
        {
            int startIndex = index1;
            while (index1 < collection1.Length && index2 < collection2.Length && collection1[index1].Item1 == collection2[index2].Item1)
            {
                if (!comparer.Equals(collection1[index1].Item2, collection2[index2].Item2))
                    break;

                index1++;
                index2++;
            }
            return index1 - startIndex;
        }

        private static Dictionary<int, List<int>> GetLookupTableForSecondCollection<T>(IEqualityComparer<T> comparer, Tuple<int, T>[] list2)
        {
            var lookupTable = new Dictionary<int, List<int>>();
            for (int index = 0; index < list2.Length; index++)
            {
                List<int> occurances;
                int hc = list2[index].Item1;
                if (!lookupTable.TryGetValue(hc, out occurances))
                {
                    occurances = new List<int>();
                    lookupTable[hc] = occurances;
                }
                occurances.Add(index);
            }
            return lookupTable;
        }
    }
}
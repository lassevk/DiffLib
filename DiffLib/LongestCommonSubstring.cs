using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffLib
{
    /// <summary>
    /// This class implements the LCS algorithm, to find the longest common substring that exists
    /// in two collections, and return the locations of those substrings.
    /// </summary>
    /// <typeparam name="T">
    /// The types of elements in the collections being compared.
    /// </typeparam>
    public sealed class LongestCommonSubstring<T>
    {
        private readonly Element[] _Collection1;
        private readonly Element[] _Collection2;
        private readonly IEqualityComparer<T> _Comparer;
        private readonly Dictionary<int, Occurance> _LookupTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="LongestCommonSubstring{T}"/> class
        /// using the default <see cref="IEqualityComparer{T}"/> instance for the
        /// <typeparamref name="T"/> type.
        /// </summary>
        /// <param name="collection1">
        /// The first collection of items.
        /// </param>
        /// <param name="collection2">
        /// The second collection of items.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection2"/> is <c>null</c>.</para>
        /// </exception>
        public LongestCommonSubstring(IEnumerable<T> collection1, IEnumerable<T> collection2)
            : this(collection1, collection2, EqualityComparer<T>.Default)
        {
            // Nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongestCommonSubstring{T}"/> class.
        /// </summary>
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
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection2"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comparer"/> is <c>null</c>.</para>
        /// </exception>
        public LongestCommonSubstring(IEnumerable<T> collection1, IEnumerable<T> collection2,
            IEqualityComparer<T> comparer)
        {
            if (collection1 == null)
                throw new ArgumentNullException("collection1");
            if (collection2 == null)
                throw new ArgumentNullException("collection2");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _Collection1 = collection1.Select(e => new Element(comparer.GetHashCode(e), e)).ToArray();
            _Collection2 = collection2.Select(e => new Element(comparer.GetHashCode(e), e)).ToArray();
            _Comparer = comparer;
            _LookupTable = new Dictionary<int, Occurance>();
            PopulateLookupTable();
        }

        /// <summary>
        /// Finds the longest common substring and returns its position in the two collections, and
        /// its length, or <c>null</c> if no such common substring can be located.
        /// </summary>
        /// <returns>
        /// A <see cref="LongestCommonSubstringResult"/> containing the positions of the two substrings, one position
        /// for each collection, both 0-based, and the length of the substring. If no common substring can be found, <c>null</c>
        /// will be returned.
        /// </returns>
        public LongestCommonSubstringResult Find()
        {
            return Find(0, _Collection1.Length, 0, _Collection2.Length);
        }

        /// <summary>
        /// Finds the longest common substring and returns its position in the two collections, and
        /// its length, or <c>null</c> if no such common substring can be located.
        /// </summary>
        /// <param name="lower1">
        /// The starting position in the first collection, 0-based. Included in the search.
        /// </param>
        /// <param name="upper1">
        /// The ending position in the first collection, 0-based. <b>Not</b> included in the search.
        /// </param>
        /// <param name="lower2">
        /// The starting position in the second collection, 0-based. Included in the search.
        /// </param>
        /// <param name="upper2">
        /// The ending position in the second collection, 0-based. <b>Not</b> included in the search.
        /// </param>
        /// <returns>
        /// A <see cref="LongestCommonSubstringResult"/> containing the positions of the two substrings, one position
        /// for each collection, both 0-based, and the length of the substring. If no common substring can be found, <c>null</c>
        /// will be returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="lower1"/> is less than 0.</para>
        /// <para>- or -</para>
        /// <para><paramref name="lower1"/> is greater than <paramref name="upper1"/>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="upper1"/> is greater than the length of the first collection.</para>
        /// <para>- or -</para>
        /// <para><paramref name="lower2"/> is less than 0.</para>
        /// <para>- or -</para>
        /// <para><paramref name="lower2"/> is greater than <paramref name="upper2"/>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="upper2"/> is greater than the length of the second collection.</para>
        /// </exception>
        public LongestCommonSubstringResult Find(int lower1, int upper1, int lower2, int upper2)
        {
            if (lower1 < 0)
                throw new ArgumentOutOfRangeException("lower1", lower1, "lower1 must be 0 or greater");
            if (lower1 > upper1)
                throw new ArgumentOutOfRangeException("lower1", lower1,
                    string.Format("lower1 must be equal to or less than upper1 ({0})", upper1));
            if (upper1 > _Collection1.Length)
                throw new ArgumentOutOfRangeException("upper1", upper1,
                    "upper1 must be equal to or less than the length of the first collection");
            if (lower2 < 0)
                throw new ArgumentOutOfRangeException("lower2", lower2, "lower2 must be 0 or greater");
            if (lower2 > upper2)
                throw new ArgumentOutOfRangeException("lower2", lower2,
                    string.Format("lower2 must be equal to or less than upper2 ({0})", upper2));
            if (upper2 > _Collection2.Length)
                throw new ArgumentOutOfRangeException("upper2", upper2,
                    "upper2 must be equal to or less than the length of the first collection");

            // Pathological cases
            if (lower1 == upper1 || lower2 == upper2)
                return null;

            int maxMatchingLength = -1;
            int maxMatchingPosition1 = -1;
            int maxMatchingPosition2 = -1;
            for (int index1 = lower1; index1 < upper1; index1++)
            {
                Occurance occurance;
                if (_LookupTable.TryGetValue(_Collection1[index1].HashCode, out occurance))
                {
                    while (occurance != null)
                    {
                        int index2 = occurance.Position;
                        occurance = occurance.Next;

                        if (index2 < lower2 || index2 >= upper2)
                            continue;

                        int length = CountMatchingElements(index1, upper1, index2, upper2);
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

        private void PopulateLookupTable()
        {
            for (int index = 0; index < _Collection2.Length; index++)
                AddOccuranceOfHashCodeToLookupTable(index, _Collection2[index].HashCode);
        }

        private void AddOccuranceOfHashCodeToLookupTable(int index, int hc)
        {
            Occurance firstOccurance;
            if (_LookupTable.TryGetValue(hc, out firstOccurance))
                firstOccurance.Next = new Occurance(index, firstOccurance.Next);
            else
                _LookupTable[hc] = new Occurance(index, null);
        }

        private int CountMatchingElements(int index1, int upper1, int index2, int upper2)
        {
            int startIndex = index1;
            while (index1 < upper1 && index2 < upper2 &&
                   _Collection1[index1].HashCode == _Collection2[index2].HashCode)
            {
                if (!_Comparer.Equals(_Collection1[index1].Item, _Collection2[index2].Item))
                    break;

                index1++;
                index2++;
            }
            return index1 - startIndex;
        }

        #region Nested type: Element

        private class Element
        {
            public readonly int HashCode;
            public readonly T Item;

            public Element(int hashCode, T item)
            {
                HashCode = hashCode;
                Item = item;
            }
        }

        #endregion

        #region Nested type: Occurance

        private class Occurance
        {
            public readonly int Position;
            public Occurance Next;

            public Occurance(int position, Occurance next)
            {
                Position = position;
                Next = next;
            }
        }

        #endregion
    }
}
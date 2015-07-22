using System;
using System.Collections.Generic;
using DiffLib.OldImplementation;
using JetBrains.Annotations;

namespace DiffLib
{
    public class LongestCommonSubsequence<T>
    {
        [NotNull]
        private readonly IList<T> _Collection1;

        [NotNull]
        private readonly IList<T> _Collection2;

        [NotNull]
        private IEqualityComparer<T> _Comparer;

        [NotNull]
        private readonly int[] _HashCodes1;

        [NotNull]
        private readonly Dictionary<int, List<int>> _HashCodes2 = new Dictionary<int, List<int>>(); 

        private int _HashCodes1Lower;
        private int _HashCodes1Upper;

        private int _HashCodes2Lower;
        private int _HashCodes2Upper;

        public LongestCommonSubsequence([NotNull] IList<T> collection1, [NotNull] IList<T> collection2, [NotNull] IEqualityComparer<T> comparer)
        {
            _Collection1 = collection1;
            _Collection2 = collection2;
            _HashCodes1 = new int[collection1.Count];

            _HashCodes1Lower = collection1.Count;
            _HashCodes1Upper = 0;

            _HashCodes2Lower = collection2.Count;
            _HashCodes2Upper = 0;

            _Comparer = comparer;
        }

        public bool Find(Slice<T> slice1, Slice<T> slice2, out int position1, out int position2, out int length)
        {
            position1 = 0;
            position2 = 0;
            length = 0;

            EnsureHashCodes1(position1, position1 + length);
            EnsureHashCodes2(position2, position2 + length);

            for (int index = 0; index < slice1.Count; index++)
            {
                var hashcode = _HashCodes1[slice1.LowerBounds + index];

                List<int> positions;
                if (!_HashCodes2.TryGetValue(hashcode, out positions))
                    continue;

                foreach (var originalCollection2Index in positions)
                {
                    if (originalCollection2Index < slice2.LowerBounds || originalCollection2Index >= slice2.UpperBounds)
                        continue;

                    int index2 = originalCollection2Index - slice2.LowerBounds;
                    int matchLength = CountSimilarElements(slice1, index, slice2, index2);
                    if (matchLength > length)
                    {
                        position1 = index;
                        position2 = index2;
                        length = matchLength;
                    }

                    // Break early if there is no way we can find a better match
                    if (length >= slice2.Count)
                        break;
                }

                // Break early if there is no way we can find a better match
                if (length >= slice2.Count)
                    break;
            }

            return length > 0;
        }

        private int CountSimilarElements(Slice<T> slice1, int index1, Slice<T> slice2, int index2)
        {
            int count = 0;

            while (index1 < slice1.Count && index2 < slice2.Count)
            {
                if (!_Comparer.Equals(slice1[index1], slice2[index2]))
                    break;

                count++;
                index1++;
                index2++;
            }

            return count;
        }

        private void EnsureHashCodes1(int lower, int upper)
        {
            while (_HashCodes1Lower > lower)
            {
                _HashCodes1Lower--;
                _HashCodes1[_HashCodes1Lower] = _Collection1[_HashCodes1Lower]?.GetHashCode() ?? 0;
            }

            while (_HashCodes1Upper < upper)
            {
                _HashCodes1[_HashCodes1Upper] = _Collection1[_HashCodes1Upper]?.GetHashCode() ?? 0;
                _HashCodes1Upper++;
            }
        }

        private void EnsureHashCodes2(int lower, int upper)
        {
            while (_HashCodes2Lower > lower)
            {
                _HashCodes2Lower--;
                var hashcode = _Collection2[_HashCodes2Lower]?.GetHashCode() ?? 0;
                AddHashCode2(_HashCodes2Lower, hashcode);
            }

            while (_HashCodes2Upper < upper)
            {
                var hashcode = _Collection2[_HashCodes2Upper]?.GetHashCode() ?? 0;
                AddHashCode2(_HashCodes2Upper, hashcode);
                _HashCodes2Upper++;
            }
        }

        private void AddHashCode2(int position, int hashcode)
        {
            List<int> positions;
            if (_HashCodes2.TryGetValue(hashcode, out positions))
                Assume.That(positions != null);
            else
            {
                positions = new List<int>();
                _HashCodes2[hashcode] = positions;
            }

            positions.Add(position);
        }
    }
}
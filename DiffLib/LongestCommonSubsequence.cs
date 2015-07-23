using System.Collections.Generic;
using JetBrains.Annotations;
using static System.Math;

namespace DiffLib
{
    public class LongestCommonSubsequence<T>
    {
        [NotNull]
        private readonly IList<T> _Collection1;

        [NotNull]
        private readonly IList<T> _Collection2;

        [NotNull]
        private readonly IEqualityComparer<T> _Comparer;

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

        public bool Find(int lower1, int upper1, int lower2, int upper2, out int position1, out int position2, out int length)
        {
            position1 = 0;
            position2 = 0;
            length = 0;

            EnsureHashCodes1(lower1, upper1);
            EnsureHashCodes2(lower2, upper2);

            int maxLengthPossible = Min(upper1 - lower1, upper2 - lower2);

            for (int index1 = lower1; index1 < upper1; index1++)
            {
                var hashcode = _HashCodes1[index1];

                List<int> positions;
                if (!_HashCodes2.TryGetValue(hashcode, out positions))
                    continue;

                Assume.That(positions != null);
                foreach (var index2 in positions)
                {
                    if (index2 < lower2 || index2 >= upper2)
                        continue;

                    int matchLength = CountSimilarElements(index1, upper1, index2, upper2);
                    if (matchLength > length)
                    {
                        position1 = index1;
                        position2 = index2;
                        length = matchLength;
                    }

                    // Break early if there is no way we can find a better match
                    if (length >= maxLengthPossible)
                        break;
                }

                // Break early if there is no way we can find a better match
                if (length >= maxLengthPossible)
                    break;
            }

            return length > 0;
        }

        private int CountSimilarElements(int index1, int upper1, int index2, int upper2)
        {
            int count = 0;

            while (index1 < upper1 && index2 < upper2 && _Comparer.Equals(_Collection1[index1], _Collection2[index2]))
            {
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
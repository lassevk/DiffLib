using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    internal class LongestCommonSubsequence<T>
    {
        [NotNull]
        private readonly IList<T> _Collection1;

        [NotNull]
        private readonly IList<T> _Collection2;

        [NotNull]
        private readonly IEqualityComparer<T> _Comparer;

        [NotNull]
        private readonly Dictionary<int, HashcodeOccurance> _HashCodes2 = new Dictionary<int, HashcodeOccurance>();

        private bool _FirstHashCodes2 = true;
        private int _HashCodes2Lower;
        private int _HashCodes2Upper;

        public LongestCommonSubsequence([NotNull] IList<T> collection1, [NotNull] IList<T> collection2, [NotNull] IEqualityComparer<T> comparer)
        {
            _Collection1 = collection1;
            _Collection2 = collection2;

            _Comparer = comparer;
        }

        public bool Find(int lower1, int upper1, int lower2, int upper2, out int position1, out int position2, out int length)
        {
            position1 = 0;
            position2 = 0;
            length = 0;

            EnsureHashCodes2(lower2, upper2);

            for (int index1 = lower1; index1 < upper1; index1++)
            {
                // Break early if there is no way we can find a better match
                if (index1 + length >= upper1)
                    break;

                var hashcode = _Collection1[index1]?.GetHashCode() ?? 0;
                if (hashcode == 0)
                    continue;

                HashcodeOccurance occurance;
                if (!_HashCodes2.TryGetValue(hashcode, out occurance))
                    continue;

                Assume.That(occurance != null);
                while (occurance != null)
                {
                    int index2 = occurance.Position;
                    occurance = occurance.Next;

                    if (index2 < lower2 || index2 + length >= upper2)
                        continue;

                    // Don't bother with this if it doesn't match at the Nth element
                    if (!_Comparer.Equals(_Collection1[index1 + length], _Collection2[index2 + length]))
                        continue;

                    int matchLength = CountSimilarElements(index1, upper1, index2, upper2);
                    if (matchLength > length)
                    {
                        position1 = index1;
                        position2 = index2;
                        length = matchLength;
                    }
                }
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

        private void EnsureHashCodes2(int lower, int upper)
        {
            if (_FirstHashCodes2)
            {
                _FirstHashCodes2 = false;
                _HashCodes2Lower = lower;
                _HashCodes2Upper = upper;

                for (int index = lower; index < upper; index++)
                    AddHashCode2(index, _Collection2[index]?.GetHashCode() ?? 0);

                return;
            }

            while (_HashCodes2Lower > lower)
            {
                _HashCodes2Lower--;
                AddHashCode2(_HashCodes2Lower, _Collection2[_HashCodes2Lower]?.GetHashCode() ?? 0);
            }

            while (_HashCodes2Upper < upper)
            {
                AddHashCode2(_HashCodes2Upper, _Collection2[_HashCodes2Upper]?.GetHashCode() ?? 0);
                _HashCodes2Upper++;
            }
        }

        private void AddHashCode2(int position, int hashcode)
        {
            HashcodeOccurance occurance;
            if (_HashCodes2.TryGetValue(hashcode, out occurance))
            {
                Assume.That(occurance != null);
                occurance.Next = new HashcodeOccurance(position, occurance.Next);
            }
            else
            {
                _HashCodes2[hashcode] = new HashcodeOccurance(position, null);
            }
        }
    }
}
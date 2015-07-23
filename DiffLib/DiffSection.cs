using System;

namespace DiffLib
{
    public struct DiffSection : IEquatable<DiffSection>
    {
        public DiffSection(bool isMatch, int lengthInCollection1, int lengthInCollection2)
        {
            IsMatch = isMatch;
            LengthInCollection1 = lengthInCollection1;
            LengthInCollection2 = lengthInCollection2;
        }

        public bool IsMatch
        {
            get;
        }

        public int LengthInCollection1
        {
            get;
        }

        public int LengthInCollection2
        {
            get;
        }

        public bool Equals(DiffSection other)
        {
            return IsMatch == other.IsMatch && LengthInCollection1 == other.LengthInCollection1 && LengthInCollection2 == other.LengthInCollection2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is DiffSection && Equals((DiffSection)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsMatch.GetHashCode();
                hashCode = (hashCode * 397) ^ LengthInCollection1;
                hashCode = (hashCode * 397) ^ LengthInCollection2;
                return hashCode;
            }
        }

        public static bool operator ==(DiffSection section1, DiffSection section2)
        {
            return section1.Equals(section2);
        }

        public static bool operator !=(DiffSection section1, DiffSection section2)
        {
            return !section1.Equals(section2);
        }

        public override string ToString()
        {
            if (IsMatch)
                return $"{LengthInCollection1} matched";

            if (LengthInCollection1 == LengthInCollection2)
                return $"{LengthInCollection1} did not match";

            if (LengthInCollection1 == 0)
                return $"{LengthInCollection2} was present in collection2, but not in collection1";

            if (LengthInCollection2 == 0)
                return $"{LengthInCollection1} was present in collection1, but not in collection2";

            return $"{LengthInCollection1} did not match with {LengthInCollection2}";
        }
    }
}
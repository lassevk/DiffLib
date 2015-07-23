using System;
using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This struct holds a section of matched or unmatch element portions from the two collectoins.
    /// </summary>
    [PublicAPI]
    public struct DiffSection : IEquatable<DiffSection>
    {
        /// <summary>
        /// Construct a new instance of <see cref="DiffSection"/>.
        /// </summary>
        /// <param name="isMatch">
        /// <c>true</c> if a match was found between the two collections;
        /// otherwise, <c>false</c>.
        /// </param>
        /// <param name="lengthInCollection1">
        /// How many elements from the first collection this section contains.
        /// </param>
        /// <param name="lengthInCollection2">
        /// How many elements from the second collection this section contains.
        /// </param>
        [PublicAPI]
        public DiffSection(bool isMatch, int lengthInCollection1, int lengthInCollection2)
        {
            IsMatch = isMatch;
            LengthInCollection1 = lengthInCollection1;
            LengthInCollection2 = lengthInCollection2;
        }

        /// <summary>
        /// Gets a value indicating whether there the section specifies a match between the two collections or
        /// portions that could not be matched.
        /// </summary>
        /// <value>
        /// <c>true</c> if a match was found between the two collections;
        /// otherwise, <c>false</c>.
        /// </value>
        [PublicAPI]
        public bool IsMatch
        {
            get;
        }

        /// <summary>
        /// How many elements from the first collection this section contains.
        /// </summary>
        [PublicAPI]
        public int LengthInCollection1
        {
            get;
        }

        /// <summary>
        /// How many elements from the second collection this section contains.
        /// </summary>
        [PublicAPI]
        public int LengthInCollection2
        {
            get;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        [PublicAPI]
        public bool Equals(DiffSection other)
        {
            return IsMatch == other.IsMatch && LengthInCollection1 == other.LengthInCollection1 && LengthInCollection2 == other.LengthInCollection2;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        [PublicAPI]
        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is DiffSection && Equals((DiffSection)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [PublicAPI]
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

        /// <summary>
        /// Implements the equality operator.
        /// </summary>
        /// <param name="section1"></param>
        /// <param name="section2"></param>
        /// <returns></returns>
        public static bool operator ==(DiffSection section1, DiffSection section2)
        {
            return section1.Equals(section2);
        }

        /// <summary>
        /// Implements the inequality operator.
        /// </summary>
        /// <param name="section1"></param>
        /// <param name="section2"></param>
        /// <returns></returns>
        public static bool operator !=(DiffSection section1, DiffSection section2)
        {
            return !section1.Equals(section2);
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [PublicAPI, NotNull]
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
using System;
using System.Globalization;

namespace DiffLib
{
    /// <summary>
    /// This class holds the result of calling <see cref="LongestCommonSubstring{T}.Find()"/>.
    /// </summary>
    public class LongestCommonSubstringResult : IEquatable<LongestCommonSubstringResult>
    {
        private readonly int _Length;
        private readonly int _PositionInCollection1;
        private readonly int _PositionInCollection2;

        /// <summary>
        /// Initializes a new instance of <see cref="LongestCommonSubstringResult"/>.
        /// </summary>
        /// <param name="positionInCollection1">
        /// The position in the first collection, 0-based.
        /// </param>
        /// <param name="positionInCollection2">
        /// The position in the second collection, 0-based.
        /// </param>
        /// <param name="length">
        /// The length of the common substring.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="positionInCollection1"/> is negative.</para>
        /// <para>- or -</para>
        /// <para><paramref name="positionInCollection2"/> is negative.</para>
        /// <para>- or -</para>
        /// <para><paramref name="length"/> is zero or negative.</para>
        /// </exception>
        public LongestCommonSubstringResult(int positionInCollection1, int positionInCollection2, int length)
        {
            if (positionInCollection1 < 0)
                throw new ArgumentOutOfRangeException("positionInCollection1", positionInCollection1,
                    "positionInCollection1 must be zero or greater");
            if (positionInCollection2 < 0)
                throw new ArgumentOutOfRangeException("positionInCollection2", positionInCollection2,
                    "positionInCollection2 must be zero or greater");
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", length, "length must be greater than zero");

            _PositionInCollection1 = positionInCollection1;
            _PositionInCollection2 = positionInCollection2;
            _Length = length;
        }

        /// <summary>
        /// The position in the first collection, 0-based.
        /// </summary>
        public int PositionInCollection1
        {
            get
            {
                return _PositionInCollection1;
            }
        }

        /// <summary>
        /// The position in the second collection, 0-based.
        /// </summary>
        public int PositionInCollection2
        {
            get
            {
                return _PositionInCollection2;
            }
        }

        /// <summary>
        /// The length of the common substring.
        /// </summary>
        public int Length
        {
            get
            {
                return _Length;
            }
        }

        #region IEquatable<LongestCommonSubstringResult> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(LongestCommonSubstringResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._Length == _Length && other._PositionInCollection1 == _PositionInCollection1 &&
                   other._PositionInCollection2 == _PositionInCollection2;
        }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (LongestCommonSubstringResult)) return false;
            return Equals((LongestCommonSubstringResult) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = _Length;
                result = (result*397) ^ _PositionInCollection1;
                result = (result*397) ^ _PositionInCollection2;
                return result;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "Length: {0}, PositionInCollection1: {1}, PositionInCollection2: {2}", _Length,
                _PositionInCollection1, _PositionInCollection2);
        }
    }
}
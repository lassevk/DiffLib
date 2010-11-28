using System;
using System.Collections.Generic;

namespace DiffLib
{
    /// <summary>
    /// This class holds the result of calling <see cref="LongestCommonSubstring.Find{T}(IEnumerable{T},IEnumerable{T},IEqualityComparer{T})"/>.
    /// </summary>
    public class LongestCommonSubstringResult
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
    }
}
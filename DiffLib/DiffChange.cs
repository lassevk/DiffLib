using System;
using System.Globalization;

namespace DiffLib
{
    /// <summary>
    /// This class contains a single section of diff output from the <see cref="Diff{T}.Generate"/>
    /// method.
    /// </summary>
    public sealed class DiffChange : IEquatable<DiffChange>
    {
        private readonly bool _Equal;
        private readonly int _Length1;
        private readonly int _Length2;

        /// <summary>
        /// Initializes a new instance of <see cref="DiffChange"/>.
        /// </summary>
        /// <param name="equal">
        /// If <c>true</c>, then the section specifies a section from the first
        /// collection that is equal to a section from the second collection;
        /// otherwise, if <c>false</c>, then the section from the first
        /// collection was replaced with the section from the second collection.
        /// </param>
        /// <param name="length1">
        /// The length of the section in the first collection. Can be 0 if
        /// the section specifies that new content was added in the second
        /// collection.
        /// </param>
        /// <param name="length2">
        /// The length of the section in the second collection. Can be 0 if
        /// the section specifies that old content was deleted in the second
        /// collection.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="length1"/> is negative.</para>
        /// <para>- or -</para>
        /// <para><paramref name="length2"/> is negative.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="equal"/> is <c>true</c> but <paramref name="length1"/> is not equal to <paramref name="length2"/>.</para>
        /// </exception>
        public DiffChange(bool equal, int length1, int length2)
        {
            if (length1 < 0)
                throw new ArgumentOutOfRangeException("length1", length1, "length1 must be 0 or greater");
            if (length2 < 0)
                throw new ArgumentOutOfRangeException("length2", length2, "length2 must be 0 or greater");
            if (equal && length1 != length2)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "length1 ({0}) must be equal to length2 ({1}) when the equal parameter is true", length1, length2));

            _Equal = equal;
            _Length1 = length1;
            _Length2 = length2;
        }

        /// <summary>
        /// Gets whether the <see cref="DiffChange"/> specifies equal sections in the two
        /// collections, or differing sections.
        /// </summary>
        /// <value>
        /// If <c>true</c>, then the section specifies a section from the first
        /// collection that is equal to a section from the second collection;
        /// otherwise, if <c>false</c>, then the section from the first
        /// collection was replaced with the section from the second collection.
        /// </value>
        public bool Equal
        {
            get
            {
                return _Equal;
            }
        }

        /// <summary>
        /// The length of the section in the first collection.
        /// </summary>
        public int Length1
        {
            get
            {
                return _Length1;
            }
        }

        /// <summary>
        /// The length of the section in the second collection.
        /// </summary>
        public int Length2
        {
            get
            {
                return _Length2;
            }
        }

        #region IEquatable<DiffChange> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(DiffChange other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other._Equal.Equals(_Equal) && other._Length1 == _Length1 && other._Length2 == _Length2;
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
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof (DiffChange))
                return false;
            return Equals((DiffChange) obj);
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
                int result = _Equal.GetHashCode();
                result = (result*397) ^ _Length1;
                result = (result*397) ^ _Length2;
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
            if (Equal)
                return string.Format(CultureInfo.InvariantCulture, "equal {0}", _Length1);

            if (_Length1 == _Length2)
                return string.Format(CultureInfo.InvariantCulture, "replace {0}", _Length1);

            return string.Format(CultureInfo.InvariantCulture, "replace {0} with {1}", _Length1, _Length2);
        }
    }
}
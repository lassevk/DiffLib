using System;

namespace DiffLib
{
    /// <summary>
    /// This class holds a single collection from either the first or the second, or both,
    /// collections given to the <see cref="AlignedDiff{T}"/> class, along
    /// with the type of change that the elements produce.
    /// </summary>
    public sealed class AlignedDiffChange<T> : IEquatable<AlignedDiffChange<T>>
    {
        private readonly ChangeType _Change;
        private readonly T _Element1;
        private readonly T _Element2;

        /// <summary>
        /// Initializes a new instance of <see cref="AlignedDiffChange{T}"/>.
        /// </summary>
        /// <param name="change">
        /// The <see cref="Change">type</see> of change this <see cref="AlignedDiffChange{T}"/> details.
        /// </param>
        /// <param name="element1">
        /// The element from the first collection. If <paramref name="change"/> is <see cref="ChangeType.Added"/>, then
        /// this parameter has no meaning.
        /// </param>
        /// <param name="element2">
        /// The element from the second collection. If <paramref name="change"/> is <see cref="ChangeType.Deleted"/>, then
        /// this parameter has no meaning.
        /// </param>
        public AlignedDiffChange(ChangeType change, T element1, T element2)
        {
            _Change = change;
            _Element1 = element1;
            _Element2 = element2;
        }

        /// <summary>
        /// The <see cref="Change">type</see> of change this <see cref="AlignedDiffChange{T}"/> details.
        /// </summary>
        public ChangeType Change
        {
            get
            {
                return _Change;
            }
        }

        /// <summary>
        /// The element from the first collection. If <see cref="System.Type"/> is <see cref="ChangeType.Added"/>, then
        /// the value of this property has no meaning.
        /// </summary>
        public T Element1
        {
            get
            {
                return _Element1;
            }
        }

        /// <summary>
        /// The element from the second collection. If <see cref="System.Type"/> is <see cref="ChangeType.Deleted"/>, then
        /// the value of this property has no meaning.
        /// </summary>
        public T Element2
        {
            get
            {
                return _Element2;
            }
        }

        #region IEquatable<AlignedDiffChange<T>> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(AlignedDiffChange<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._Element1, _Element1) && Equals(other._Element2, _Element2) &&
                   Equals(other._Change, _Change);
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
            if (obj.GetType() != typeof (AlignedDiffChange<T>)) return false;
            return Equals((AlignedDiffChange<T>) obj);
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
                int result = _Element1.GetHashCode();
                result = (result*397) ^ _Element2.GetHashCode();
                result = (result*397) ^ _Change.GetHashCode();
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
            switch (Change)
            {
                case ChangeType.Same:
                    return "  " + _Element1;

                case ChangeType.Added:
                    return "+ " + _Element2;

                case ChangeType.Deleted:
                    return "- " + _Element1;

                case ChangeType.Changed:
                    return "* " + _Element1 + " --> " + _Element2;

                default:
                    return _Change + ": " + _Element1 + " --> " + _Element2;
            }
        }
    }
}
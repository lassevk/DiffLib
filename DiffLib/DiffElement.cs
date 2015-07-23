using System;
using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This struct holds a single aligned element from the two collections given to <see cref="Diff.AlignElements{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements from the two collections compared.
    /// </typeparam>
    [PublicAPI]
    public struct DiffElement<T> : IEquatable<DiffElement<T>> 
    {
        /// <summary>
        /// Constructs a new instance of <see cref="DiffElement{T}"/>.
        /// </summary>
        /// <param name="elementFromCollection1">
        /// The aligned element from the first collection, or <see cref="Option{T}.None"/> if an element from the second collection could
        /// not be aligned with anything from the first.
        /// </param>
        /// <param name="elementFromCollection2">
        /// The aligned element from the second collection, or <see cref="Option{T}.None"/> if an element from the first collection could
        /// not be aligned with anything from the second.
        /// </param>
        /// <param name="operation">
        /// A <see cref="DiffOperation"/> specifying how <paramref name="elementFromCollection1"/> corresponds to <paramref name="elementFromCollection2"/>.
        /// </param>
        [PublicAPI]
        public DiffElement(Option<T> elementFromCollection1, Option<T> elementFromCollection2, DiffOperation operation)
        {
            ElementFromCollection1 = elementFromCollection1;
            ElementFromCollection2 = elementFromCollection2;
            Operation = operation;
        }

        /// <summary>
        /// The aligned element from the first collection, or <see cref="Option{T}.None"/> if an element from the second collection could
        /// not be aligned with anything from the first.
        /// </summary>
        [PublicAPI]
        public Option<T> ElementFromCollection1
        {
            get;
        }

        /// <summary>
        /// The aligned element from the second collection, or <see cref="Option{T}.None"/> if an element from the first collection could
        /// not be aligned with anything from the second.
        /// </summary>
        [PublicAPI]
        public Option<T> ElementFromCollection2
        {
            get;
        }

        /// <summary>
        /// A <see cref="DiffOperation"/> specifying how <see cref="ElementFromCollection1"/> corresponds to <see cref="ElementFromCollection2"/>.
        /// </summary>
        [PublicAPI]
        public DiffOperation Operation
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
        public bool Equals(DiffElement<T> other)
        {
            return ElementFromCollection1.Equals(other.ElementFromCollection1) && ElementFromCollection2.Equals(other.ElementFromCollection2) && Operation == other.Operation;
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
            return obj is DiffElement<T> && Equals((DiffElement<T>)obj);
        }

        /// <summary>
        /// Implements the equality operator.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator ==(DiffElement<T> element, DiffElement<T> other)
        {
            return element.Equals(other);
        }

        /// <summary>
        /// Implements the inequality operator.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator !=(DiffElement<T> element, DiffElement<T> other)
        {
            return !element.Equals(other);
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
                var hashCode = ElementFromCollection1.GetHashCode();
                hashCode = (hashCode * 397) ^ ElementFromCollection2.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Operation;
                return hashCode;
            }
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
            switch (Operation)
            {
                case DiffOperation.Match:
                    return $"same: {ElementFromCollection1}";

                case DiffOperation.Insert:
                    return $"insert: {ElementFromCollection2}";

                case DiffOperation.Delete:
                    return $"delete: {ElementFromCollection1}";

                case DiffOperation.Replace:
                    return $"replace: {ElementFromCollection1} with: {ElementFromCollection2}";

                case DiffOperation.Modify:
                    return $"modify: {ElementFromCollection1} to: {ElementFromCollection2}";

                default:
                    return $"? {Operation}: {ElementFromCollection1}, {ElementFromCollection2}";
            }
        }
    }
}
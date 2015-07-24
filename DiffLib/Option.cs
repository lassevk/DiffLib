using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This type functions similar to <see cref="Nullable{T}"/> except that it can hold any type of value
    /// and is used for situations where you may or may not have a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [PublicAPI]
    public struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
    {
        private readonly T _Value;

        /// <summary>
        /// Constructs a new instance of <see cref="Option{T}"/> with the specified value.
        /// </summary>
        /// <param name="value">
        /// The value of this <see cref="Option{T}"/>.
        /// </param>
        [PublicAPI]
        public Option([CanBeNull] T value)
        {
            _Value = value;
            HasValue = true;
        }

        /// <summary>
        /// Gets the value of this <see cref="Option{T}"/>.
        /// </summary>
        [PublicAPI, CanBeNull]
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("This Option<T> does not have a value");

                return _Value;
            }
        }

        /// <summary>
        /// Gets whether this <see cref="Option{T}"/> has a value.
        /// </summary>
        [PublicAPI]
        public bool HasValue
        {
            get;
        }

        /// <summary>
        /// Implements equality operator.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Option<T>(T value)
        {
            return new Option<T>(value);
        }

        /// <summary>
        /// Implements inequality operator.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static explicit operator T(Option<T> option)
        {
            return option.Value;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Option<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_Value, other._Value) && HasValue == other.HasValue;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(T other)
        {
            if (!HasValue)
                return false;

            return EqualityComparer<T>.Default.Equals(_Value, other);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Option<T> && Equals((Option<T>)obj);
        }

        /// <summary>
        /// Implements equality operator.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator ==(Option<T> option, Option<T> other)
        {
            return option.Equals(other);
        }

        /// <summary>
        /// Implements inequality operator.
        /// </summary>
        /// <param name="option"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool operator !=(Option<T> option, Option<T> other)
        {
            return !option.Equals(other);
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
                return (EqualityComparer<T>.Default.GetHashCode(_Value) * 397) ^ HasValue.GetHashCode();
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
            if (!HasValue)
                return string.Empty;

            return _Value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Returns an <see cref="Option{T}"/> that has no value.
        /// </summary>
        [PublicAPI]
        public static Option<T> None => new Option<T>();
    }
}
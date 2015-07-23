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

        public static implicit operator Option<T>(T value)
        {
            return new Option<T>(value);
        }

        public static explicit operator T(Option<T> option)
        {
            return option.Value;
        }

        public bool Equals(Option<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_Value, other._Value) && HasValue == other.HasValue;
        }

        public bool Equals(T other)
        {
            if (!HasValue)
                return false;

            return EqualityComparer<T>.Default.Equals(_Value, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Option<T> && Equals((Option<T>)obj);
        }

        public static bool operator ==(Option<T> option, Option<T> other)
        {
            return option.Equals(other);
        }

        public static bool operator !=(Option<T> option, Option<T> other)
        {
            return !option.Equals(other);
        }

        [PublicAPI]
        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(_Value) * 397) ^ HasValue.GetHashCode();
            }
        }

        [PublicAPI, NotNull]
        public override string ToString()
        {
            if (!HasValue)
                return "<no value>";

            return _Value?.ToString() ?? "<null>";
        }

        [PublicAPI]
        public static Option<T> None => new Option<T>();
    }
}
using System;

namespace DiffLib
{
    /// <summary>
    /// This type functions similar to <see cref="Nullable{T}"/> except that it can hold any type of value
    /// and is used for situations where you may or may not have a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Option<T>
    {
        private readonly T _Value;
        
        /// <summary>
        /// Constructs a new instance of <see cref="Option{T}"/> with the specified value.
        /// </summary>
        /// <param name="value">
        /// The value of this <see cref="Option{T}"/>.
        /// </param>
        public Option(T value)
        {
            _Value = value;
            HasValue = true;
        }

        /// <summary>
        /// Gets the value of this <see cref="Option{T}"/>.
        /// </summary>
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
    }
}
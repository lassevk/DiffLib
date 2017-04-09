using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace DiffLib
{
    /// <summary>
    /// This exception should be thrown by implementations of <see cref="IMergeConflictResolver{T}"/> to indicate a failure to resolve a conflict.
    /// </summary>
    public class MergeConflictException : Exception
    {
        /// <summary>
        /// Constructs a new instance of <see cref="MergeConflictException"/> given the specific <paramref name="message"/>.
        /// </summary>
        /// <param name="message">
        /// The message indicating what the reason for the failure was.
        /// </param>
        /// <param name="commonBase">
        /// The common base of the elements involved in the conflict.
        /// </param>
        /// <param name="left">
        /// The left side of the conflict.
        /// </param>
        /// <param name="right">
        /// The right side of the conflict.
        /// </param>
        public MergeConflictException([CanBeNull] string message, [NotNull] IEnumerable<object> commonBase, [NotNull] IEnumerable<object> left, [NotNull] IEnumerable<object> right)
            : base(message)
        {
            CommonBase = commonBase.ToArray();
            Left = left.ToArray();
            Right = right.ToArray();
        }

        /// <summary>
        /// The common base of the elements involved in the conflict.
        /// </summary>
        [NotNull, ItemCanBeNull]
        public object[] CommonBase { get; }

        /// <summary>
        /// The left side of the conflict.
        /// </summary>
        [NotNull, ItemCanBeNull]
        public object[] Left { get; }

        /// <summary>
        /// The right side of the conflict.
        /// </summary>
        [NotNull, ItemCanBeNull]
        public object[] Right { get; }
    }
}
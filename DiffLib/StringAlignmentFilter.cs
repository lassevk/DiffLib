using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffLib
{
    /// <summary>
    /// This class implements <see cref="ISimilarityComparer{T}"/> for strings, doing a very basic "diff" between the two,
    /// and calculating how much of the text occurs in both.
    /// </summary>
    public sealed class StringAlignmentFilter : IAlignmentFilter<string>
    {
        private readonly StringSimilarityFilterPredicate _DiffPredicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringAlignmentFilter"/> class.
        /// </summary>
        public StringAlignmentFilter()
        {
            _DiffPredicate = delegate(string value1, string value2, IEnumerable<DiffChange> diff)
            {
                int same = diff.Where(s => s.Equal).Sum(s => s.Length1);
                return ((same*2.0)/(value1.Length + value2.Length + 0.0)) >= 0.1;
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringAlignmentFilter"/> class.
        /// </summary>
        /// <param name="diffPredicate">
        /// The diff predicate used to determine if the strings are
        /// similar enough (see <see cref="StringSimilarityFilterPredicate"/> for details.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="diffPredicate" /> is <c>null</c>.</exception>
        public StringAlignmentFilter(StringSimilarityFilterPredicate diffPredicate)
        {
            if (diffPredicate == null)
                throw new ArgumentNullException("diffPredicate");

            _DiffPredicate = diffPredicate;
        }

        #region IAlignmentFilter<string> Members

        /// <summary>
        /// Determines if the two values are similar enough to align them
        /// as a change, instead of not aligning them but reporting them
        /// as a delete plus an add instead.
        /// </summary>
        /// <param name="value1">
        /// The first value to compare against <paramref name="value2"/>.
        /// </param>
        /// <param name="value2">
        /// The second value to compare against <paramref name="value1"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the two values are similar enough to report
        /// them as a change; false if the two values aren't similar enough
        /// but needs to be reported as a delete plus an add.
        /// </returns>
        public bool CanAlign(string value1, string value2)
        {
            if (ReferenceEquals(value1, value2))
                return true;

            value1 = (value1 ?? String.Empty);
            value2 = (value2 ?? String.Empty);

            if (value1.Length == 0 && value2.Length == 0)
                return true;
            if (value1.Length == 0 || value2.Length == 0)
                return false;

            var diff = new Diff<char>(value1, value2);
            return _DiffPredicate(value1, value2, diff);
        }

        #endregion
    }
}
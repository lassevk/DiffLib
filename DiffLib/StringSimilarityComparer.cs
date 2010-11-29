using System;
using System.Linq;

namespace DiffLib
{
    /// <summary>
    /// This class implements <see cref="ISimilarityComparer{T}"/> for strings, doing a very basic "diff" between the two,
    /// and calculating how much of the text occurs in both.
    /// </summary>
    public sealed class StringSimilarityComparer : ISimilarityComparer<string>
    {
        #region ISimilarityComparer<string> Members

        /// <summary>
        /// Does a similarity comparison between the two values and returns their
        /// similarity, a value ranging from 0.0 to 1.0, where 0.0 means they're
        /// completely different and 1.0 means they have the same value.
        /// </summary>
        /// <param name="value1">
        /// The first value to compare.
        /// </param>
        /// <param name="value2">
        /// The second value to compare.
        /// </param>
        /// <returns>
        /// A value ranging from 0.0 to 1.0, where 0.0 means they're
        /// completely different and 1.0 means they have the same value.
        /// </returns>
        public double Compare(string value1, string value2)
        {
            if (ReferenceEquals(value1, value2))
                return 1.0;

            value1 = (value1 ?? String.Empty);
            value2 = (value2 ?? String.Empty);

            if (value1.Length == 0 && value2.Length == 0)
                return 1.0;
            if (value1.Length == 0 || value2.Length == 0)
                return 0.0;

            int same = new Diff<char>(value1, value2).Where(s => s.Equal).Sum(s => s.Length1);
            return (same*2.0)/(value1.Length + value2.Length + 0.0);
        }

        #endregion
    }
}
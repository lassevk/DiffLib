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
        /// <param name="x">
        /// The first value to compare.
        /// </param>
        /// <param name="y">
        /// The second value to compare.
        /// </param>
        /// <returns>
        /// A value ranging from 0.0 to 1.0, where 0.0 means they're
        /// completely different and 1.0 means they have the same value.
        /// </returns>
        public double Similarity(string x, string y)
        {
            if (ReferenceEquals(x, y))
                return 1.0;

            x = (x ?? String.Empty);
            y = (y ?? String.Empty);

            if (x == String.Empty && y == String.Empty)
                return 1.0;
            if (x == String.Empty || y == String.Empty)
                return 0.0;

            DiffSection[] diff = new Diff<char>(x, y).Generate().ToArray();
            int same = diff.Where(s => s.Equal).Sum(s => s.Length1);
            return (same*2.0)/(x.Length + y.Length + 0.0);
        }

        #endregion
    }
}
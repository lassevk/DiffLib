using System.Collections.Generic;

namespace DiffLib
{
    /// <summary>
    /// This delegate is used by <see cref="StringSimilarityFilter"/> to
    /// determine if the two strings are similar enough to report them
    /// as a change, instead of as a delete plus and add.
    /// </summary>
    /// <param name="value1">
    /// The first string to compare.
    /// </param>
    /// <param name="value2">
    /// The second string to compare.
    /// </param>
    /// <param name="diff">
    /// The diff between <paramref name="value1"/> and <paramref name="value2"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the strings are similar enough (reported as a change);
    /// otherwise, <c>false</c> (reported as a delete plus an add.)
    /// </returns>
    public delegate bool StringSimilarityFilterPredicate(string value1, string value2, IEnumerable<DiffChange> diff);
}
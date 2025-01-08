using System;
using System.Diagnostics.CodeAnalysis;

namespace DiffLib;

/// <summary>
/// This struct holds a section of matched or unmatch element portions from the two collectoins.
/// </summary>
[ExcludeFromCodeCoverage]
public readonly record struct DiffSection(bool IsMatch, int LengthInCollection1, int LengthInCollection2)
{
    /// <summary>
    /// Returns the fully qualified type name of this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"/> containing a fully qualified type name.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    public override string ToString()
    {
        if (IsMatch)
            return $"{LengthInCollection1} matched";

        if (LengthInCollection1 == LengthInCollection2)
            return $"{LengthInCollection1} did not match";

        if (LengthInCollection1 == 0)
            return $"{LengthInCollection2} was present in collection2, but not in collection1";

        if (LengthInCollection2 == 0)
            return $"{LengthInCollection1} was present in collection1, but not in collection2";

        return $"{LengthInCollection1} did not match with {LengthInCollection2}";
    }
}
using System;
using System.Collections.Generic;

namespace DiffLib;

internal class DiffSectionMergeComparer<T> : IEqualityComparer<DiffElement<T>>
{
    private readonly IEqualityComparer<T?> _Comparer;

    public DiffSectionMergeComparer(IEqualityComparer<T?> comparer)
    {
        _Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
    }

    public bool Equals(DiffElement<T> x, DiffElement<T> y)
        => _Comparer.Equals(GetElement(x), GetElement(y));

    public int GetHashCode(DiffElement<T> obj)
    {
        T? element = this.GetElement(obj);
        return element is null ? 0 : _Comparer.GetHashCode(element);
    }

    private T? GetElement(DiffElement<T> diffElement)
    {
        if (diffElement.ElementFromCollection1.HasValue)
            return diffElement.ElementFromCollection1.Value;

        if (diffElement.ElementFromCollection2.HasValue)
            return diffElement.ElementFromCollection2.Value;

        return default(T);
    }
}
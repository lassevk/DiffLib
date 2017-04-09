using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace DiffLib
{
    internal class DiffSectionMergeComparer<T> : IEqualityComparer<DiffElement<T>>
    {
        [NotNull]
        private readonly IEqualityComparer<T> _Comparer;

        public DiffSectionMergeComparer([NotNull] IEqualityComparer<T> comparer)
        {
            _Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public bool Equals(DiffElement<T> x, DiffElement<T> y)
        {
            return _Comparer.Equals(GetElement(x), GetElement(y));
        }

        public int GetHashCode(DiffElement<T> obj)
        {
            return _Comparer.GetHashCode(GetElement(obj));
        }

        private T GetElement(DiffElement<T> diffElement)
        {
            if (diffElement.ElementFromCollection1.HasValue)
                return diffElement.ElementFromCollection1.Value;

            if (diffElement.ElementFromCollection2.HasValue)
                return diffElement.ElementFromCollection2.Value;

            return default(T);
        }
    }
}
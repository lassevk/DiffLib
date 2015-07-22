using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DiffLib
{
    public struct Slice<T> : IList<T>
    {
        public Slice([NotNull] IList<T> collection, int lower, int upper)
        {
            Collection = collection;
            LowerBounds = lower;
            UpperBounds = upper;
        }

        [NotNull]
        public IList<T> Collection
        {
            get;
        }

        public int LowerBounds
        {
            get;
        }

        public int UpperBounds
        {
            get;
        }

        public Slice<T> ConstrainFromStart(int count)
        {
            return new Slice<T>(Collection, LowerBounds, LowerBounds + count);
        }

        public Slice<T> ConstrainFromEnd(int count)
        {
            return new Slice<T>(Collection, UpperBounds - count, UpperBounds);
        }

        public Slice<T> ConstrainAbsolute(int position, int length)
        {
            return new Slice<T>(Collection, LowerBounds + position, LowerBounds + position + length);
        }

        public Slice<T> ConstrainFrom(int position)
        {
            return new Slice<T>(Collection, LowerBounds + position, UpperBounds);
        }

        public Slice<T> Constrain(int removeStart, int removeEnd)
        {
            int lower = LowerBounds + removeStart;
            int upper = UpperBounds - removeEnd;
            if (upper < lower)
                throw new ArgumentOutOfRangeException(nameof(removeEnd));

            return new Slice<T>(Collection, lower, upper);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int index = LowerBounds; index < UpperBounds; index++)
                yield return Collection[index];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public int Count => UpperBounds - LowerBounds;

        public bool IsReadOnly => true;

        public int IndexOf(T item)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public T this[int index]
        {
            get
            {
                return Collection[LowerBounds + index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}
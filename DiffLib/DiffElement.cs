using System;

namespace DiffLib
{
    public struct DiffElement<T>
    {
        public DiffElement(Option<T> elementFromCollection1, Option<T> elementFromCollection2, DiffOperation operation)
        {
            ElementFromCollection1 = elementFromCollection1;
            ElementFromCollection2 = elementFromCollection2;
            Operation = operation;
        }

        public Option<T> ElementFromCollection1
        {
            get;
        }

        public Option<T> ElementFromCollection2
        {
            get;
        }

        public DiffOperation Operation
        {
            get;
        }

        public bool Equals(DiffElement<T> other)
        {
            return ElementFromCollection1.Equals(other.ElementFromCollection1) && ElementFromCollection2.Equals(other.ElementFromCollection2) && Operation == other.Operation;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is DiffElement<T> && Equals((DiffElement<T>)obj);
        }

        public static bool operator ==(DiffElement<T> element, DiffElement<T> other)
        {
            return element.Equals(other);
        }

        public static bool operator !=(DiffElement<T> element, DiffElement<T> other)
        {
            return !element.Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ElementFromCollection1.GetHashCode();
                hashCode = (hashCode * 397) ^ ElementFromCollection2.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Operation;
                return hashCode;
            }
        }

        public override string ToString()
        {
            switch (Operation)
            {
                case DiffOperation.None:
                    return $"same: {ElementFromCollection1}";

                case DiffOperation.Insert:
                    return $"insert: {ElementFromCollection2}";

                case DiffOperation.Delete:
                    return $"delete: {ElementFromCollection1}";

                case DiffOperation.Replace:
                    return $"replace: {ElementFromCollection1} with: {ElementFromCollection2}";

                case DiffOperation.Modify:
                    return $"modify: {ElementFromCollection1} to: {ElementFromCollection2}";

                default:
                    return $"? {Operation}: {ElementFromCollection1}, {ElementFromCollection2}";
            }
        }
    }
}
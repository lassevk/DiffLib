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
    }
}
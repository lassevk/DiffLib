namespace DiffLib
{
    internal class HashcodeOccurance
    {
        public HashcodeOccurance(int position, HashcodeOccurance? next)
        {
            Position = position;
            Next = next;
        }

        public int Position
        {
            get;
        }

        public HashcodeOccurance? Next
        {
            get;
            set;
        }
    }
}

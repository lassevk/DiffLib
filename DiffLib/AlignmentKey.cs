using System;

namespace DiffLib
{
    internal struct AlignmentKey : IEquatable<AlignmentKey>
    {
        public AlignmentKey(int position1, int position2)
        {
            Position1 = position1;
            Position2 = position2;
        }

        public int Position1
        {
            get;
        }

        public int Position2
        {
            get;
        }

        public bool Equals(AlignmentKey other) => Position1 == other.Position1 && Position2 == other.Position2;

        public override bool Equals(object? obj) => obj is AlignmentKey key && this.Equals(key);

        public override int GetHashCode() => unchecked((Position1 * 397) ^ Position2);
    }
}
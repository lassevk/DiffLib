using System;

namespace DiffLib
{
    internal struct AlignmentKey : IEquatable<AlignmentKey>
    {
        private readonly int _Position1;
        private readonly int _Position2;

        public AlignmentKey(int position1, int position2)
        {
            _Position1 = position1;
            _Position2 = position2;
        }

        public bool Equals(AlignmentKey other) => _Position1 == other._Position1 && _Position2 == other._Position2;

        public override bool Equals(object obj) => !ReferenceEquals(null, obj) && (obj is AlignmentKey && Equals((AlignmentKey)obj));

        public override int GetHashCode() => unchecked((_Position1 * 397) ^ _Position2);
    }
}
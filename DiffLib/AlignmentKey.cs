namespace DiffLib;

#if NETSTANDARD2_0
internal record struct AlignmentKey(int Position1, int Position2);
#endif
#if NETSTANDARD2_1
internal record struct AlignmentKey(int Position1, int Position2);
#endif
#if !NETSTANDARD2_0 && !NETSTANDARD2_1
internal record struct AlignmentKey(int Position1, int Position2);
#endif
using JetBrains.Annotations;

namespace DiffLib
{
    [PublicAPI]
    public delegate double ElementSimilarity<in T>([CanBeNull] T element1, [CanBeNull] T element2);
}
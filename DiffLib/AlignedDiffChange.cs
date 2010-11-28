namespace DiffLib
{
    /// <summary>
    /// This class holds a single collection from either the first or the second, or both,
    /// collections given to the <see cref="AlignedDiff{T}"/> class, along
    /// with the type of change that the elements produce.
    /// </summary>
    public sealed class AlignedDiffChange<T>
    {
        private readonly T _Element1;
        private readonly T _Element2;
        private readonly ChangeType _Type;

        /// <summary>
        /// Initializes a new instance of <see cref="AlignedDiffChange{T}"/>.
        /// </summary>
        /// <param name="type">
        /// The <see cref="ChangeType">type</see> of change this <see cref="AlignedDiffChange{T}"/> details.
        /// </param>
        /// <param name="element1">
        /// The element from the first collection. If <paramref name="type"/> is <see cref="ChangeType.Added"/>, then
        /// this parameter has no meaning.
        /// </param>
        /// <param name="element2">
        /// The element from the second collection. If <paramref name="type"/> is <see cref="ChangeType.Deleted"/>, then
        /// this parameter has no meaning.
        /// </param>
        public AlignedDiffChange(ChangeType type, T element1, T element2)
        {
            _Type = type;
            _Element1 = element1;
            _Element2 = element2;
        }

        /// <summary>
        /// The <see cref="ChangeType">type</see> of change this <see cref="AlignedDiffChange{T}"/> details.
        /// </summary>
        public ChangeType Type
        {
            get
            {
                return _Type;
            }
        }

        /// <summary>
        /// The element from the first collection. If <see cref="Type"/> is <see cref="ChangeType.Added"/>, then
        /// the value of this property has no meaning.
        /// </summary>
        public T Element1
        {
            get
            {
                return _Element1;
            }
        }

        /// <summary>
        /// The element from the second collection. If <see cref="Type"/> is <see cref="ChangeType.Deleted"/>, then
        /// the value of this property has no meaning.
        /// </summary>
        public T Element2
        {
            get
            {
                return _Element2;
            }
        }
    }
}
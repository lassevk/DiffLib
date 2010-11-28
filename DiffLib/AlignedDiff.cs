using System;
using System.Collections.Generic;

namespace DiffLib
{
    /// <summary>
    /// This class implements a slightly more advanced diff algorithm than <see cref="Diff{T}"/> by
    /// taking the output from <see cref="Diff{T}"/> and attempting to align individual elements inside
    /// replace-blocks. This is mostly suitable for text file diffs.
    /// </summary>
    /// <typeparam name="T">
    /// The types of elements in the collections being compared.
    /// </typeparam>
    public sealed class AlignedDiff<T>
    {
        private readonly IList<T> _Collection1;
        private readonly IList<T> _Collection2;
        private readonly Diff<T> _Diff;
        private readonly ISimilarityComparer<T> _SimilarityComparer;

        /// <summary>
        /// Initializes a new instance of <see cref="AlignedDiff{T}"/>.
        /// </summary>
        /// <param name="collection1">
        /// The first collection of items.
        /// </param>
        /// <param name="collection2">
        /// The second collection of items.
        /// </param>
        /// <param name="equalityComparer">
        /// The <see cref="IEqualityComparer{T}"/> that will be used to compare elements from
        /// <paramref name="collection1"/> with elements from <paramref name="collection2"/>.
        /// </param>
        /// <param name="similarityComparer">
        /// The <see cref="ISimilarityComparer{T}"/> that will be used to attempt to align elements
        /// inside blocks that consists of elements from the first collection being replaced
        /// with elements from the second collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection1"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="collection2"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="equalityComparer"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="similarityComparer"/> is <c>null</c>.</para>
        /// </exception>
        public AlignedDiff(IEnumerable<T> collection1, IEnumerable<T> collection2, IEqualityComparer<T> equalityComparer,
            ISimilarityComparer<T> similarityComparer)
        {
            if (collection1 == null)
                throw new ArgumentNullException("collection1");
            if (collection2 == null)
                throw new ArgumentNullException("collection2");
            if (equalityComparer == null)
                throw new ArgumentNullException("equalityComparer");
            if (similarityComparer == null)
                throw new ArgumentNullException("similarityComparer");

            _Collection1 = collection1.ToRandomAccess();
            _Collection2 = collection2.ToRandomAccess();

            _Diff = new Diff<T>(_Collection1, _Collection2, equalityComparer);
            _SimilarityComparer = similarityComparer;
        }

        /// <summary>
        /// Generates the diff, one line of output at a time.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="AlignedDiffChange{T}"/> objects, one for
        /// each line in the first or second collection (sometimes one instance for a line
        /// from both, when lines are equal or similar.)
        /// </returns>
        public IEnumerable<AlignedDiffChange<T>> Generate()
        {
            int i1 = 0;
            int i2 = 0;
            foreach (DiffSection section in _Diff.Generate())
            {
                if (section.Equal)
                {
                    for (int index = 0; index < section.Length1; index++)
                    {
                        yield return new AlignedDiffChange<T>(ChangeType.Same, _Collection1[i1], _Collection2[i2]);
                        i1++;
                        i2++;
                    }
                }
                else
                {
                    for (int index = 0; index < section.Length1; index++)
                    {
                        yield return new AlignedDiffChange<T>(ChangeType.Deleted, _Collection1[i1], default(T));
                        i1++;
                    }
                    for (int index = 0; index < section.Length2; index++)
                    {
                        yield return new AlignedDiffChange<T>(ChangeType.Added, default(T), _Collection2[i2]);
                        i2++;
                    }
                }
            }
        }
    }
}
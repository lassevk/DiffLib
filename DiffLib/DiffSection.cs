using System;
using System.Globalization;

namespace DiffLib
{
    /// <summary>
    /// This class contains a single section of diff output from the <see cref="Diff{T}.Generate"/>
    /// method.
    /// </summary>
    public class DiffSection
    {
        private readonly bool _Equal;
        private readonly int _Length1;
        private readonly int _Length2;

        /// <summary>
        /// Initializes a new instance of <see cref="DiffSection"/>.
        /// </summary>
        /// <param name="equal">
        /// If <c>true</c>, then the section specifies a section from the first
        /// collection that is equal to a section from the second collection;
        /// otherwise, if <c>false</c>, then the section from the first
        /// collection was replaced with the section from the second collection.
        /// </param>
        /// <param name="length1">
        /// The length of the section in the first collection. Can be 0 if
        /// the section specifies that new content was added in the second
        /// collection.
        /// </param>
        /// <param name="length2">
        /// The length of the section in the second collection. Can be 0 if
        /// the section specifies that old content was deleted in the second
        /// collection.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="length1"/> is negative.</para>
        /// <para>- or -</para>
        /// <para><paramref name="length2"/> is negative.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="equal"/> is <c>true</c> but <paramref name="length1"/> is not equal to <paramref name="length2"/>.</para>
        /// </exception>
        public DiffSection(bool equal, int length1, int length2)
        {
            if (length1 < 0)
                throw new ArgumentOutOfRangeException("length1", length1, "length1 must be 0 or greater");
            if (length2 < 0)
                throw new ArgumentOutOfRangeException("length2", length2, "length2 must be 0 or greater");
            if (equal && length1 != length2)
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    "length1 ({0}) must be equal to length2 ({1}) when the equal parameter is true", length1, length2));

            _Equal = equal;
            _Length1 = length1;
            _Length2 = length2;
        }

        /// <summary>
        /// Gets whether the <see cref="DiffSection"/> specifies equal sections in the two
        /// collections, or differing sections.
        /// </summary>
        /// <value>
        /// If <c>true</c>, then the section specifies a section from the first
        /// collection that is equal to a section from the second collection;
        /// otherwise, if <c>false</c>, then the section from the first
        /// collection was replaced with the section from the second collection.
        /// </value>
        public bool Equal
        {
            get
            {
                return _Equal;
            }
        }

        /// <summary>
        /// The length of the section in the first collection.
        /// </summary>
        public int Length1
        {
            get
            {
                return _Length1;
            }
        }

        /// <summary>
        /// The length of the section in the second collection.
        /// </summary>
        public int Length2
        {
            get
            {
                return _Length2;
            }
        }
    }
}
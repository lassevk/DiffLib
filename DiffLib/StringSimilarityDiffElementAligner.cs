using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace DiffLib
{
    public class StringSimilarityDiffElementAligner : IDiffElementAligner<string>
    {
        [NotNull]
        private readonly IDiffElementAligner<string> _Aligner;

        public StringSimilarityDiffElementAligner(double modificationThreshold = 0.3333)
        {
            _Aligner = new ElementSimilarityDiffElementAligner<string>(StringSimilarity, modificationThreshold);
        }

        private static double StringSimilarity(string element1, string element2)
        {
            element1 = (element1 ?? String.Empty);
            element2 = (element2 ?? String.Empty);

            if (ReferenceEquals(element1, element2))
                return 1.0;

            Debug.WriteLine($"Comparing '{element1}' with '{element2}'");

            if (element1.Length == 0 && element2.Length == 0)
                return 1.0;
            if (element1.Length == 0 || element2.Length == 0)
                return 0.0;

            var diffSections = Diff.CalculateSections(element1.ToCharArray(), element2.ToCharArray()).ToArray();
            int matchLength = diffSections.Where(section => section.IsMatch).Sum(section => section.LengthInCollection1);
            return (matchLength * 2.0) / (element1.Length + element2.Length + 0.0);
        }

        public IEnumerable<DiffElement<string>> Align(IList<string> collection1, int start1, int length1, IList<string> collection2, int start2, int length2)
        {
            return _Aligner.Align(collection1, start1, length1, collection2, start2, length2);
        }
    }
}
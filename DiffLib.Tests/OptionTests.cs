using System;
using NUnit.Framework;

// ReSharper disable UnusedVariable
// ReSharper disable PossibleNullReferenceException
// ReSharper disable EqualExpressionComparison
// ReSharper disable SuspiciousTypeConversion.Global

namespace DiffLib.Tests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void Value_WithDefaultValue_ThrowsInvalidOperationException()
        {
            var o = default(Option<int>);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var x = o.Value;
            });
        }

        [Test]
        public void Value_ConstructedOption_ReturnsValue()
        {
            var o = new Option<int>(10);

            Assert.That(o.Value, Is.EqualTo(10));
        }

        [Test]
        public void HasValue_WithDefaultValue_IsFalse()
        {
            var o = default(Option<int>);

            Assert.That(o.HasValue, Is.False);
        }

        [Test]
        public void HasValue_ConstructedOption_IsTrue()
        {
            var o = new Option<int>(10);

            Assert.That(o.HasValue, Is.True);
        }

        [Test]
        public void CastFromOptionToValue_WithDefaultValue_ThrowsInvalidOperationException()
        {
            var o = default(Option<int>);

            Assert.Throws<InvalidOperationException>(() =>
            {
                var x = (int)o;
            });
        }

        [Test]
        public void CastFromOptionToValue_ConstructedOption_ReturnsValue()
        {
            var o = new Option<int>(10);

            Assert.That((int)o, Is.EqualTo(10));
        }

        [Test]
        public void Equals_DefaultOptionToItself_ReturnsTrue()
        {
            var o = default(Option<int>);

            var output = o.Equals(o);

            Assert.That(output, Is.True);
        }

        [Test]
        public void Equals_ConstructedOptionToItself_ReturnsTrue()
        {
            var o = new Option<int>(10);

            var output = o.Equals(o);

            Assert.That(output, Is.True);
        }

        [Test]
        public void Equals_DefaultOptionToOtherWithSameValue_ReturnsTrue()
        {
            var o1 = default(Option<int>);
            var o2 = default(Option<int>);

            var output = o1.Equals(o2);

            Assert.That(output, Is.True);
        }

        [Test]
        public void Equals_ConstructedOptionToOtherWithSameValue_ReturnsTrue()
        {
            var o1 = new Option<int>(10);
            var o2 = new Option<int>(10);

            var output = o1.Equals(o2);

            Assert.That(output, Is.True);
        }

        [Test]
        public void Equals_ConstructedOptionToDefaultOption_ReturnsFalse()
        {
            var o1 = new Option<int>(10);
            var o2 = default(Option<int>);

            var output = o1.Equals(o2);

            Assert.That(output, Is.False);
        }

        [Test]
        public void Equals_DefaultOptionToConstructedOption_ReturnsFalse()
        {
            var o1 = default(Option<int>);
            var o2 = new Option<int>(10);

            var output = o1.Equals(o2);

            Assert.That(output, Is.False);
        }

        [Test]
        public void Equals_ConstructedOptionToOtherWithDifferentValue_ReturnsFalse()
        {
            var o1 = new Option<int>(10);
            var o2 = new Option<int>(15);

            var output = o1.Equals(o2);

            Assert.That(output, Is.False);
        }

        [Test]
        public void Equals_ConstructedOptionToSameValue_ReturnsTrue()
        {
            var o1 = new Option<int>(10);
            var o2 = 10;

            var output = o1.Equals(o2);

            Assert.That(output, Is.True);
        }

        [Test]
        public void Equals_ConstructedOptionToDifferentValue_ReturnsFalse()
        {
            var o1 = new Option<int>(10);
            var o2 = 15;

            var output = o1.Equals(o2);

            Assert.That(output, Is.False);
        }

        [Test]
        public void Equals_DefaultdOptionToValue_ReturnsFalse()
        {
            var o1 = default(Option<int>);
            var o2 = 10;

            var output = o1.Equals(o2);

            Assert.That(output, Is.False);
        }

        [Test]
        public void Equals_ConstructedOptionToString_ReturnsFalse()
        {
            var o1 = new Option<int>(10);

            var output = o1.Equals("string");

            Assert.That(output, Is.False);
        }

        [Test]
        public void Equals_ConstructedOptionToNull_ReturnsFalse()
        {
            var o1 = new Option<int>(10);

            var output = o1.Equals(null);

            Assert.That(output, Is.False);
        }

        [Test]
        public void Equals_ConstructedOptionToBoxedCopyOfItself_ReturnsTrue()
        {
            var o1 = new Option<int>(10);
            var o2 = (object)o1;

            var output = o1.Equals(o2);

            Assert.That(output, Is.True);
        }

        [Test]
        public void EqualityOperator_ConstructedOptionToItself_ReturnsTrue()
        {
            var o = new Option<int>(10);

#pragma warning disable 1718
            var output = o == o;
#pragma warning restore 1718
            Assert.That(output, Is.True);
        }

        [Test]
        public void InequalityOperators_ConstructedOptionToItself_ReturnsFalse()
        {
            var o = new Option<int>(10);

#pragma warning disable 1718
            var output = o != o;
#pragma warning restore 1718

            Assert.That(output, Is.False);
        }

        [Test]
        public void GetHashCode_OfTwoConstructedOptionsWithSameValue_ReturnsTheSameValue()
        {
            var o1 = new Option<int>(10);
            var o2 = new Option<int>(10);

            var h1 = o1.GetHashCode();
            var h2 = o2.GetHashCode();

            Assert.That(h1, Is.EqualTo(h2));
        }

        [Test]
        public void GetHashCode_OfTwoConstructedOptionsWithDifferentValues_ReturnsDifferentValues()
        {
            var o1 = new Option<int>(10);
            var o2 = new Option<int>(15);

            var h1 = o1.GetHashCode();
            var h2 = o2.GetHashCode();

            Assert.That(h1, Is.Not.EqualTo(h2));
        }

        [Test]
        public void GetHashCode_OfConstructedOptionWithValueZeroAndDefaultOption_ReturnsDifferentValues()
        {
            var o1 = new Option<int>(0);
            var o2 = default(Option<int>);

            var h1 = o1.GetHashCode();
            var h2 = o2.GetHashCode();

            Assert.That(h1, Is.Not.EqualTo(h2));
        }

        [Test]
        public void ToString_OfConstructedOption_ReturnsStringWithValue()
        {
            var o = new Option<int>(10);

            var s = o.ToString();

            Assert.That(s, Is.EqualTo("10"));
        }

        [Test]
        public void ToString_OfConstructedOptionHoldingNullReference_ReturnsEmptyString()
        {
            var o = new Option<string>(null);

            var s = o.ToString();

            Assert.That(s, Is.SameAs(string.Empty));
        }

        [Test]
        public void ToString_OfDefaultOption_ReturnsEmptyString()
        {
            var o = default(Option<string>);

            var s = o.ToString();

            Assert.That(s, Is.SameAs(string.Empty));
        }
    }
}
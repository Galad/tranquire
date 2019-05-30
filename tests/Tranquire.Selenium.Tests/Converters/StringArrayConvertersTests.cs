using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using Tranquire.Selenium.Questions.UIModels.Converters;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Converters
{
    public class StringArrayConvertersTests
    {
        [Theory, DomainAutoData]
        internal void FromBool(StringArrayConverters sut)
        {
            // arrange
            // act
            var actual = sut.Convert(true, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal((IEnumerable<string>)ImmutableArray.Create("True"), actual);
        }

        [Theory, DomainAutoData]
        internal void FromString(StringArrayConverters sut, string expected)
        {
            // arrange
            // act
            var actual = sut.Convert(expected, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal((IEnumerable<string>)ImmutableArray.Create(expected), actual);
        }

        [Theory, DomainAutoData]
        internal void FromStringArray(StringArrayConverters sut, ImmutableArray<string> expected)
        {
            // act
            var actual = sut.Convert(expected, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal((IEnumerable<string>)expected, actual);
        }
    }
    public class StringConvertersTests
    {
        [Theory, DomainAutoData]
        internal void FromBool(StringConverters sut)
        {
            // arrange
            // act
            var actual = sut.Convert(true, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal("True", actual);
        }

        [Theory, DomainAutoData]
        internal void FromString(StringConverters sut, string expected)
        {
            // arrange
            // act
            var actual = sut.Convert(expected, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        internal void FromStringArray(StringConverters sut, ImmutableArray<string> values)
        {
            // act
            var actual = sut.Convert(values, CultureInfo.InvariantCulture);
            // assert
            var expected = string.Join(", ", values);
            Assert.Equal(expected, actual);
        }
    }
}

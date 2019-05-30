using System;
using System.Collections.Immutable;
using System.Globalization;
using Tranquire.Selenium.Questions.UIModels.Converters;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Converters
{
    public class BooleanConvertersTests
    {
        [Theory]
        [DomainInlineAutoData(true)]
        [DomainInlineAutoData(false)]
        internal void FromBool(bool expected, BooleanConverters sut)
        {
            // act
            var actual = sut.Convert(expected, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        internal void FromString(BooleanConverters sut)
        {
            // act
            var actual = sut.Convert("true", CultureInfo.InvariantCulture);
            // assert
            Assert.True(actual);
        }

        [Theory, DomainAutoData]
        internal void FromStringArray(BooleanConverters sut)
        {
            Assert.Throws<NotSupportedException>(() => sut.Convert(ImmutableArray<string>.Empty, CultureInfo.InvariantCulture));
        }
    }
}

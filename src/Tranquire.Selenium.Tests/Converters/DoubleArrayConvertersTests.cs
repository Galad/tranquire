using System.Collections.Generic;
using Xunit;
using System.Linq;
using AutoFixture;
using System;
using Tranquire.Tests;
using Tranquire.Selenium.Questions.UIModels.Converters;
using System.Globalization;
using System.Collections.Immutable;

namespace Tranquire.Selenium.Tests.Converters
{
    public class DoubleArrayConvertersTests
    {
        [Theory, DomainAutoData]
        internal void FromBool(DoubleArrayConverters sut)
        {
            Assert.Throws<NotSupportedException>(() => sut.Convert(true, CultureInfo.InvariantCulture));
        }

        [Theory, DomainAutoData]
        internal void FromString(DoubleArrayConverters sut, IFixture fixture)
        {
            // arrange
            var expected = fixture.Create<double>();
            var doubleString = expected.ToString("r", CultureInfo.InvariantCulture);
            // act
            var actual = sut.Convert(doubleString, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal((IEnumerable<double>)ImmutableArray.Create(expected), actual);
        }

        [Theory, DomainAutoData]
        internal void FromStringArray(DoubleArrayConverters sut, IFixture fixture)
        {
            // arrange
            var expected = fixture.CreateMany<double>().ToArray();
            var doublesString = expected.Select(s => s.ToString("r", CultureInfo.InvariantCulture)).ToImmutableArray();
            // act
            var actual = sut.Convert(doublesString, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal(expected, actual);
        }
    }
}

using AutoFixture;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Tranquire.Selenium.Questions.UIModels.Converters;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Converters
{
    public class IntegerArrayConvertersTests
    {
        [Theory, DomainAutoData]
        internal void FromBool(IntegerArrayConverters sut)
        {
            Assert.Throws<NotSupportedException>(() => sut.Convert(true, CultureInfo.InvariantCulture));
        }

        [Theory, DomainAutoData]
        internal void FromString(IntegerArrayConverters sut, IFixture fixture)
        {
            // arrange
            var expected = fixture.Create<int>();
            var integerString = expected.ToString(CultureInfo.InvariantCulture);
            // act
            var actual = sut.Convert(integerString, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal((IEnumerable<int>)new[] { expected }, actual);
        }

        [Theory, DomainAutoData]
        internal void FromStringArray(IntegerArrayConverters sut, IFixture fixture)
        {
            // arrange
            var expected = fixture.CreateMany<int>().ToArray();
            var doublesString = expected.Select(s => s.ToString(CultureInfo.InvariantCulture)).ToImmutableArray();
            // act
            var actual = sut.Convert(doublesString, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal(expected, actual);
        }
    }
}

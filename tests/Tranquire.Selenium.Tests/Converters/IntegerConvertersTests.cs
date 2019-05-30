using System;
using System.Globalization;
using AutoFixture;
using Tranquire.Selenium.Questions.UIModels.Converters;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Converters
{
    public class IntegerConvertersTests
    {
        [Theory, DomainAutoData]
        internal void FromBool(IntegerConverters sut)
        {
            Assert.Throws<NotSupportedException>(() => sut.Convert(true, CultureInfo.InvariantCulture));
        }

        [Theory, DomainAutoData]
        internal void FromString(IntegerConverters sut, IFixture fixture)
        {
            // arrange
            var expected = fixture.Create<int>();
            var doubleString = expected.ToString(CultureInfo.InvariantCulture);
            // act
            var actual = sut.Convert(doubleString, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        internal void FromStringArray(IntegerConverters sut)
        {
            Assert.Throws<NotSupportedException>(() => sut.Convert(true, CultureInfo.InvariantCulture));
        }
    }
}

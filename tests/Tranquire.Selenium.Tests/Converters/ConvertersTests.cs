using System;
using System.Globalization;
using AutoFixture;
using Tranquire.Selenium.Questions.UIModels.Converters;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Converters
{
    public class DateTimeConvertersTests
    {
        [Theory, DomainAutoData]
        internal void FromBool(DateTimeConverters sut)
        {
            Assert.Throws<NotSupportedException>(() => sut.Convert(true, CultureInfo.InvariantCulture));
        }

        [Theory, DomainAutoData]
        internal void FromString(DateTimeConverters sut, IFixture fixture)
        {
            // arrange
            var expected = fixture.Create<DateTime>();
            var dateString = expected.ToString("o", CultureInfo.InvariantCulture);
            // act
            var actual = sut.Convert(dateString, CultureInfo.InvariantCulture);
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        internal void FromStringArray(DateTimeConverters sut)
        {
            Assert.Throws<NotSupportedException>(() => sut.Convert(true, CultureInfo.InvariantCulture));
        }
    }
}

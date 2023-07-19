using System;
using System.Globalization;
using AutoFixture;
using Tranquire.Selenium.Questions.UIModels.Converters;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Converters;

public class DoubleConvertersTests
{
    [Theory, DomainAutoData]
    internal void FromBool(DoubleConverters sut)
    {
        Assert.Throws<NotSupportedException>(() => sut.Convert(true, CultureInfo.InvariantCulture));
    }

    [Theory, DomainAutoData]
    internal void FromString(DoubleConverters sut, IFixture fixture)
    {
        // arrange
        var expected = fixture.Create<double>();
        var doubleString = expected.ToString("r", CultureInfo.InvariantCulture);
        // act
        var actual = sut.Convert(doubleString, CultureInfo.InvariantCulture);
        // assert
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    internal void FromStringArray(DoubleConverters sut)
    {
        Assert.Throws<NotSupportedException>(() => sut.Convert(true, CultureInfo.InvariantCulture));
    }
}

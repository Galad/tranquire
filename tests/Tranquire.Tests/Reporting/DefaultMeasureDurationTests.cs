using System;
using System.Threading;
using AutoFixture.Idioms;
using FluentAssertions;
using Tranquire.Reporting;
using Xunit;

namespace Tranquire.Tests.Reporting;

public class DefaultMeasureDurationTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(DefaultMeasureDuration));
    }

    [Theory, DomainAutoData]
    public void MeasureTime_ShouldReturnValue(
        DefaultMeasureDuration sut,
        object expected)
    {
        //arrange
        //act
        var (duration, actual, exception) = sut.Measure(() => expected);
        //assert
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void MeasureTime_ShouldReturnCorrectDuration(
        DefaultMeasureDuration sut,
        object value,
        TimeSpan wait)
    {
        //arrange
        //act
        var (actual, result, exception) = sut.Measure(() =>
        {
            Thread.Sleep((int)wait.TotalMilliseconds);
            return value;
        });
        //assert
        actual.Should().BeGreaterOrEqualTo(wait);
    }

    [Theory, DomainAutoData]
    public void MeasureTime_WhenFuncThrows_ShouldReturnCorrectDuration(
        DefaultMeasureDuration sut,
        Exception exception,
        TimeSpan wait)
    {
        //arrange

        //act
        var (actual, result, ex) = sut.Measure<object>(() =>
        {
            Thread.Sleep((int)wait.TotalMilliseconds);
            throw exception;
        });
        //assert
        actual.Should().BeGreaterThan(wait);
    }

    [Theory, DomainAutoData]
    public void MeasureTime_WhenFuncThrows_ShouldReturnCorrectException(
        DefaultMeasureDuration sut,
        Exception exception,
        TimeSpan wait)
    {
        //arrange

        //act
        var (duration, result, actual) = sut.Measure<object>(() =>
        {
            Thread.Sleep((int)wait.TotalMilliseconds);
            throw exception;
        });
        //assert
        actual.Should().Be(exception);
    }

    [Theory, DomainAutoData]
    public void Now_ShouldReturnCorrectValue(
        DefaultMeasureDuration sut)
    {
        //arrange
        var expected = DateTimeOffset.UtcNow;
        //act
        var actual = sut.Now;
        //assert
        actual.Should().BeCloseTo(expected, TimeSpan.FromSeconds(1));
        actual.Offset.Should().Be(TimeSpan.Zero);
    }
}

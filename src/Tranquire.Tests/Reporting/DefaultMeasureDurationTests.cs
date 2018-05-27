using AutoFixture.Idioms;
using FluentAssertions;
using System;
using System.Threading;
using Tranquire.Reporting;
using Xunit;

namespace Tranquire.Tests.Reporting
{
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
            var actual = sut.Measure(() => expected);
            //assert
            Assert.Equal(expected, actual.Item2);
        }

        [Theory, DomainAutoData]
        public void MeasureTime_ShouldReturnCorrectDuration(
            DefaultMeasureDuration sut,
            object value,
            TimeSpan wait)
        {
            //arrange
            //act
            var actual = sut.Measure(() =>
            {
                Thread.Sleep((int)wait.TotalMilliseconds);
                return value;
            });
            //assert
            actual.Item1.Should().BeGreaterOrEqualTo(wait);
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
}

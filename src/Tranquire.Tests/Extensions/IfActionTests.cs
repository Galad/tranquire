using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using System;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions
{
    public class IfActionTests : IfActionTestsBase<IfAction<object>, IAction<object>, IAction<object>>
    {
        [Theory]
        [DomainAutoData]
        public void ExecuteWhen_WhenPredicateIsTrue_ShouldReturnCorrectValue(
            IFixture fixture,
            Mock<IActor> actor,
            object expected)
        {
            //arrange
            fixture.Inject<Func<bool>>(() => true);
            var sut = fixture.Create<IfAction<object>>();
            actor.Setup(a => a.Execute(sut.Action)).Returns(expected);
            //act
            var actual = sut.ExecuteWhenAs(actor.Object);
            //assert
            actual.Should().Be(expected);
        }

        [Theory]
        [DomainAutoData]
        public void ExecuteWhen_WhenPredicateIsFalse_ShouldReturnCorrectValue(
           IFixture fixture,
           Mock<IActor> actor)
        {
            //arrange
            fixture.Inject<Func<bool>>(() => false);
            var expected = fixture.Freeze<object>();
            var sut = fixture.Create<IfAction<object>>();
            //act
            var actual = sut.ExecuteWhenAs(actor.Object);
            //assert
            actual.Should().Be(expected);
        }

        [Theory]
        [DomainAutoData]
        public void ExecuteGiven_WhenPredicateIsTrue_ShouldReturnCorrectValue(
         IFixture fixture,
         Mock<IActor> actor,
         object expected)
        {
            //arrange
            fixture.Inject<Func<bool>>(() => true);
            var sut = fixture.Create<IfAction<object>>();
            actor.Setup(a => a.Execute(sut.Action)).Returns(expected);
            //act
            var actual = sut.ExecuteGivenAs(actor.Object);
            //assert
            actual.Should().Be(expected);
        }

        [Theory]
        [DomainAutoData]
        public void ExecuteGiven_WhenPredicateIsFalse_ShouldReturnCorrectValue(
           IFixture fixture,
           Mock<IActor> actor)
        {
            //arrange
            fixture.Inject<Func<bool>>(() => false);
            var expected = fixture.Freeze<object>();
            var sut = fixture.Create<IfAction<object>>();
            //act
            var actual = sut.ExecuteGivenAs(actor.Object);
            //assert
            actual.Should().Be(expected);
        }
    }
}

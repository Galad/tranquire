using System;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions
{

    public class IfActionWithAbilityTests : IfActionTestsBase<IfActionWithAbility<Ability, object>, IAction<object>, IAction<Ability, Ability, object>>
    {
        [Theory]
        [DomainAutoData]
        public void ExecuteWhen_WhenPredicateIsTrue_ShouldReturnCorrectValue(
            IFixture fixture,
            Mock<IActor> actor,
            object expected)
        {
            //arrange
            Func<bool> func = () => true;
            fixture.Inject(func);
            var sut = fixture.Create<IfActionWithAbility<Ability, object>>();
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
            Func<bool> func = () => false;
            fixture.Inject(func);
            var expected = fixture.Freeze<object>();
            var sut = fixture.Create<IfActionWithAbility<Ability, object>>();
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
            Func<bool> func = () => true;
            fixture.Inject(func);
            var sut = fixture.Create<IfActionWithAbility<Ability, object>>();
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
            Func<bool> func = () => false;
            fixture.Inject(func);
            var expected = fixture.Freeze<object>();
            var sut = fixture.Create<IfActionWithAbility<Ability, object>>();
            //act
            var actual = sut.ExecuteGivenAs(actor.Object);
            //assert
            actual.Should().Be(expected);
        }
    }
}

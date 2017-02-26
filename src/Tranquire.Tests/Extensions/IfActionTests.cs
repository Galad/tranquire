using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
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
            Func<bool> func = () => true;
            fixture.Inject(func);
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
            Func<bool> func = () => false;
            fixture.Inject(func);
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
            Func<bool> func = () => true;
            fixture.Inject(func);
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
            Func<bool> func = () => false;
            fixture.Inject(func);
            var expected = fixture.Freeze<object>();
            var sut = fixture.Create<IfAction<object>>();
            //act
            var actual = sut.ExecuteGivenAs(actor.Object);
            //assert
            actual.Should().Be(expected);
        }
    }
}

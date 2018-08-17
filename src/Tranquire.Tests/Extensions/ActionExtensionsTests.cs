using FluentAssertions;
using Moq;
using AutoFixture;
using AutoFixture.Idioms;
using System;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions
{
    public class ActionExtensionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion) => assertion.Verify(typeof(ActionExtensions));

        private void TestIf<TAction, TFunc, TValue, TResult>(Func<TAction, TFunc, TValue, TResult> act)
        {
            //arrange
            var fixture = new Fixture().Customize(new DomainCustomization());
            var action = fixture.Freeze<TAction>();
            var predicate = fixture.Freeze<TFunc>();
            var defaultValue = fixture.Freeze<TValue>();
            var expected = fixture.Create<TResult>();
            //act
            var actual = act(action, predicate, defaultValue);
            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        #region If
        [Fact]
        public void If_ShouldReturnCorrectValue() => TestIf((IAction<object> a, Func<bool> p, object v) => ActionExtensions.If(a, p, v));

        [Fact]
        public void If_Unit_ShouldReturnCorrectValue() => TestIf((IAction<Unit> a, Func<bool> p, Unit v) => ActionExtensions.If(a, p));

        [Fact]
        public void If_PredicateAbility_ShouldReturnCorrectValue() => TestIf((IAction<object> a, Func<Ability, bool> p, object v) => ActionExtensions.If(a, p, v));

        [Fact]
        public void If_PredicateAbility_Unit_ShouldReturnCorrectValue() => TestIf((IAction<Unit> a, Func<Ability, bool> p, Unit v) => ActionExtensions.If(a, p));
        #endregion

        #region AsActionUnit
        [Theory, DomainAutoData]
        public void AsActionUnit_ExecuteWhen_ShouldCallExecuteWhenOnSourceAction(Mock<IAction<object>> action, IActor actor)
        {
            //act
            var actual = ActionExtensions.AsActionUnit(action.Object);
            actual.ExecuteWhenAs(actor);
            //assert            
            action.Verify(a => a.ExecuteWhenAs(actor));
        }

        [Theory, DomainAutoData]
        public void AsActionUnit_ExecuteGiven_ShouldCallExecuteGivenOnSourceAction(Mock<IAction<object>> action, IActor actor)
        {
            //act
            var actual = ActionExtensions.AsActionUnit(action.Object);
            actual.ExecuteGivenAs(actor);
            //assert            
            action.Verify(a => a.ExecuteGivenAs(actor));
        }

        [Theory, DomainAutoData]
        public void AsActionUnit_ShouldReturnCorrectValue(Mock<IAction<object>> action)
        {
            //act
            var actual = ActionExtensions.AsActionUnit(action.Object);
            //assert
            actual.Should().BeAssignableTo<IAction<Unit>>();
            actual.Name.Should().Be(action.Object.Name);
        }
        #endregion

        #region Using
        [Theory, DomainAutoData]
        public void Using_ShouldReturnCorrectValue(IAction<IDisposable> disposableAction, IAction<object> action)
        {
            // act
            var actual = ActionExtensions.Using(action, disposableAction);
            // assert
            var expected = new UsingAction<object>(disposableAction, action);
            actual.Should().BeEquivalentTo(expected);
        }
        #endregion

        #region SelectMany
        [Theory, DomainAutoData]
        public void SelectMany_ShouldReturnCorrectResult(IAction<string> action, Func<string, IAction<object>> selector)
        {
            // act
            var actual = action.SelectMany(selector);
            // assert
            var expected = new SelectManyAction<string, object>(action, selector);
            actual.Should().BeOfType<SelectManyAction<string, object>>();
            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
}

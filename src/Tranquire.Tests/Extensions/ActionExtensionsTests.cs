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
            actual.ShouldBeEquivalentTo(expected);
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

        [Fact]
        public void If_ActionAbility_ShouldReturnCorrectValue() => TestIf((IAction<Ability1, Ability2, object> a, Func<bool> p, object v) => ActionExtensions.If(a, p, v));

        [Fact]
        public void If_ActionAbility_Unit_ShouldReturnCorrectValue() => TestIf((IAction<Ability1, Ability2, Unit> a, Func<bool> p, Unit v) => ActionExtensions.If(a, p));

        [Fact]
        public void If_ActionAndPredicateAbility_ShouldReturnCorrectValue()
        {
            TestIf((IAction<AbilityAction, AbilityAction, object> a, Func<Ability, bool> p, object v) => ActionExtensions.If(a, p, v));
        }

        [Fact]
        public void If_ActionAndPredicateAbility_Unit_ShouldReturnCorrectValue()
        {
            TestIf((IAction<AbilityAction, AbilityAction, Unit> a, Func<Ability, bool> p, Unit v) => ActionExtensions.If(a, p));
        }
        #endregion

        [Theory, DomainAutoData]
        public void AsActionWithoutAbility_ShouldReturnCorrectValue(IAction<Ability, Ability2, object> sut)
        {            
            //act
            var actual = ActionExtensions.AsActionWithoutAbility(sut);
            //assert
            var expected = new ActionWithAbilityToActionAdapter<Ability, Ability2, object>(sut);
            actual.ShouldBeEquivalentTo(expected);
        }

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

        [Theory, DomainAutoData]
        public void AsActionUnit_WithAbilities_ExecuteWhen_ShouldCallExecuteWhenOnSourceAction(
            Mock<IAction<Ability1, Ability2, object>> action,
            Mock<IActor> actor,
            Ability2 ability)
        {
            //act
            var actual = ActionExtensions.AsActionUnit(action.Object);
            actual.ExecuteWhenAs(actor.Object);
            //assert            
            actor.Verify(a => a.ExecuteWithAbility(action.Object));
        }

        [Theory, DomainAutoData]
        public void AsActionUnit_WithAbilities_ExecuteGiven_ShouldCallExecuteGivenOnSourceAction(
            Mock<IAction<Ability1, Ability2, object>> action,
            Mock<IActor> actor,
            Ability1 ability)
        {
            //arrange            
            //act
            var actual = ActionExtensions.AsActionUnit(action.Object);
            actual.ExecuteGivenAs(actor.Object);
            //assert            
            actor.Verify(a => a.ExecuteWithAbility(action.Object));
        }

        [Theory, DomainAutoData]
        public void AsActionUnit_WithAbilities_ShouldReturnCorrectValue(Mock<IAction<Ability1, Ability2, object>> action)
        {
            //act
            var actual = ActionExtensions.AsActionUnit(action.Object);
            //assert
            actual.Should().BeAssignableTo<IAction<Unit>>();
            actual.Name.Should().Be(action.Object.Name);
        } 
        #endregion
    }
}

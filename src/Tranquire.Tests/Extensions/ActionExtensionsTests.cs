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
    }
}

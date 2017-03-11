using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionWithAbility2Tests
    {
        public class AbilityGiven
        {
        }

        public class AbilityWhen
        {
        }

        public class ActionExecuteWhen : Action<AbilityGiven, AbilityWhen, object>
        {
            public IActor Actor;
            private readonly IAction<object> _action;            
            public AbilityWhen Ability;
            public override string Name { get; }

            public ActionExecuteWhen(IAction<object> action, string name)
            {
                _action = action;
                Name = name;
            }

            protected override object ExecuteWhen(IActor actor, AbilityWhen ability)
            {
                Actor = actor;
                Ability = ability;
                return actor.Execute(_action);
            }

            protected override object ExecuteGiven(IActor actor, AbilityGiven ability)
            {
                return new object();
            }
        }

        public class ActionExecuteGiven : Action<AbilityGiven, AbilityWhen, object>
        {
            public IActor Actor;
            private readonly IAction<object> _action;
            public AbilityGiven Ability;
            public override string Name => "";

            public ActionExecuteGiven(IAction<object> action)
            {
                _action = action;
            }

            protected override object ExecuteWhen(IActor actor, AbilityWhen ability)
            {
                return new object();
            }

            protected override object ExecuteGiven(IActor actor, AbilityGiven ability)
            {
                Actor = actor;
                Ability = ability;
                return actor.Execute(_action);
            }
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(Action<AbilityGiven, AbilityWhen, object> sut)
        {
            Assert.IsAssignableFrom(typeof (IAction<AbilityGiven, AbilityWhen, object>), sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(IFixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof (ActionExecuteGiven).GetMethods());
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorExecute(
            [Frozen] IAction<object> action,
            ActionExecuteWhen sut,
            Mock<IActor> actor,
            AbilityWhen ability,
            object expected)
        {
            //arrange
            actor.Setup(a => a.Execute(action)).Returns(expected);
            //act
            var actual = sut.ExecuteWhenAs(actor.Object, ability);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorExecute(
            [Frozen] IAction<object> action,
            ActionExecuteGiven sut,
            Mock<IActor> actor,
            AbilityGiven ability,
            object expected)
        {
            //arrange
            actor.Setup(a => a.Execute(action)).Returns(expected);
            //act
            var actual = sut.ExecuteGivenAs(actor.Object, ability);
            //assert
            Assert.Equal(expected, actual);
        }
        
        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldUseCorrectActor(
            ActionExecuteWhen sut,
            Mock<IActor> actor,
            AbilityWhen ability)
        {
            //arrange
            var expected = actor.Object;
            //act
            sut.ExecuteWhenAs(actor.Object, ability);
            //assert
            Assert.Equal(expected, sut.Actor);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldUseCorrectActor(ActionExecuteGiven sut, Mock<IActor> actor, AbilityGiven ability)
        {
            //arrange
            var expected = actor.Object;
            //act
            sut.ExecuteGivenAs(actor.Object, ability);
            //assert
            Assert.Equal(expected, sut.Actor);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldUseCorrectAbility(ActionExecuteWhen sut, Mock<IActor> actor, AbilityWhen expected)
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object, expected);
            //assert
            Assert.Equal(expected, sut.Ability);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldUseCorrectAbility(ActionExecuteGiven sut, Mock<IActor> actor, AbilityGiven expected)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object, expected);
            //assert
            Assert.Equal(expected, sut.Ability);
        }

        [Theory, DomainAutoData]
        public void ToString_ShouldReturnCorrectValue(ActionExecuteWhen sut)
        {
            //arrange
            //act
            var actual = sut.ToString();
            //assert
            Assert.Equal(sut.Name, actual);
        }
    }
}
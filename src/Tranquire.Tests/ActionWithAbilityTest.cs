using Moq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionWithAbilityTests
    {
        public class TestAbility
        {
        }

        public class ActionExecuteWhen : Action<TestAbility, object>
        {
            public IActor Actor;
            private readonly IAction<object> _action;
            public TestAbility Ability;
            public override string Name { get; }

            public ActionExecuteWhen(IAction<object> action, string name)
            {
                _action = action;
                Name = name;
            }

            protected override object ExecuteWhen(IActor actor, TestAbility ability)
            {
                Actor = actor;
                Ability = ability;
                return actor.Execute(_action);
            }

            protected override object ExecuteGiven(IActor actor, TestAbility ability)
            {
                return new object();
            }
        }

        public class ActionExecuteGiven : Action<TestAbility, object>
        {
            public IActor Actor;
            private readonly IAction<object> _action;
            public TestAbility Ability;
            public override string Name => "";

            public ActionExecuteGiven(IAction<object> action)
            {
                _action = action;
            }

            protected override object ExecuteWhen(IActor actor, TestAbility ability)
            {
                return new object();
            }

            protected override object ExecuteGiven(IActor actor, TestAbility ability)
            {
                Actor = actor;
                Ability = ability;
                return actor.Execute(_action);
            }
        }

        public class ActionExecuteWhenAndGivenNotOverridden : Action<TestAbility, object>
        {
            private readonly IAction<object> _action;
            public override string Name => "";

            public ActionExecuteWhenAndGivenNotOverridden(IAction<object> action)
            {
                _action = action;
            }

            protected override object ExecuteWhen(IActor actor, TestAbility ability)
            {
                return actor.Execute(_action);
            }
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(Action<TestAbility, object> sut)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.IsAssignableFrom<IAction<TestAbility, object>>(sut);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(IFixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(ActionExecuteGiven).GetMethods());
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorExecute(
            [Frozen] IAction<object> action,
            ActionExecuteWhen sut,
            Mock<IActor> actor,
            TestAbility ability,
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
            TestAbility ability,
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
        public void ExecuteGivenAs_WhenExecuteGivenIsNotOverridden_ShouldCallActorExecute(
            [Frozen] IAction<object> action,
            ActionExecuteWhenAndGivenNotOverridden sut,
            Mock<IActor> actor,
            TestAbility ability,
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
            TestAbility ability)
        {
            //arrange
            var expected = actor.Object;
            //act
            sut.ExecuteWhenAs(actor.Object, ability);
            //assert
            Assert.Equal(expected, sut.Actor);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldUseCorrectActor(ActionExecuteGiven sut, Mock<IActor> actor, TestAbility ability)
        {
            //arrange
            var expected = actor.Object;
            //act
            sut.ExecuteGivenAs(actor.Object, ability);
            //assert
            Assert.Equal(expected, sut.Actor);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldUseCorrectAbility(ActionExecuteWhen sut, Mock<IActor> actor, TestAbility expected)
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object, expected);
            //assert
            Assert.Equal(expected, sut.Ability);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldUseCorrectAbility(ActionExecuteGiven sut, Mock<IActor> actor, TestAbility expected)
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
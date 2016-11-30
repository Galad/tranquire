using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionUnitWithAbilityTests
    {
        public class TestAbility
        {
        }

        public class ActionExecuteWhen : ActionUnit<TestAbility>
        {
            public IActor Actor;
            private readonly IAction<Unit> _action;
            public TestAbility Ability;
            public override string Name { get; }

            public ActionExecuteWhen(IAction<Unit> action, string name)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                _action = action;
                Name = name;
            }

            protected override void ExecuteWhen(IActor actor, TestAbility ability)
            {
                Actor = actor;
                Ability = ability;
                actor.Execute(_action);
            }

            protected override void ExecuteGiven(IActor actor, TestAbility ability)
            {
            }
        }

        public class ActionExecuteGiven : ActionUnit<TestAbility>
        {
            public IActor Actor;
            private readonly IAction<Unit> _action;
            public TestAbility Ability;
            public override string Name => "";

            public ActionExecuteGiven(IAction<Unit> action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                _action = action;
            }

            protected override void ExecuteWhen(IActor actor, TestAbility ability)
            {
            }

            protected override void ExecuteGiven(IActor actor, TestAbility ability)
            {
                Actor = actor;
                Ability = ability;
                actor.Execute(_action);
            }
        }

        public class ActionExecuteWhenAndGivenNotOverridden : ActionUnit<TestAbility>
        {
            private readonly IAction<Unit> _action;
            public override string Name => "";

            public ActionExecuteWhenAndGivenNotOverridden(IAction<Unit> action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                _action = action;
            }

            protected override void ExecuteWhen(IActor actor, TestAbility ability)
            {
                actor.Execute(_action);
            }
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(Action<TestAbility, object> sut)
        {
            Assert.IsAssignableFrom(typeof(IAction<TestAbility, TestAbility, object>), sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(IFixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(ActionExecuteGiven).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorExecute([Frozen] IAction<Unit> expected, ActionExecuteWhen sut, Mock<IActor> actor, TestAbility ability)
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object, ability);
            //assert
            actor.Verify(a => a.Execute(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorExecute([Frozen] IAction<Unit> expected, ActionExecuteGiven sut, Mock<IActor> actor, TestAbility ability)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object, ability);
            //assert
            actor.Verify(a => a.Execute(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_WhenExecuteGivenIsNotOverridden_ShouldCallActorExecute([Frozen] IAction<Unit> expected, ActionExecuteWhenAndGivenNotOverridden sut, Mock<IActor> actor, TestAbility ability)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object, ability);
            //assert
            actor.Verify(a => a.Execute(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldUseCorrectActor(ActionExecuteWhen sut, Mock<IActor> actor, TestAbility ability)
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
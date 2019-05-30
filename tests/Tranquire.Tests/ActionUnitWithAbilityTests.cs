using System;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Moq;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionUnitWithAbilityTests
    {
        public class TestAbility
        {
        }

        public class ActionExecuteWhen : ActionBaseUnit<TestAbility>
        {
            public IActor Actor;
            private readonly IAction<Unit> _action;
            public TestAbility Ability;
            public override string Name { get; }

            public ActionExecuteWhen(IAction<Unit> action, string name)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
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

        public class ActionExecuteGiven : ActionBaseUnit<TestAbility>
        {
            public IActor Actor;
            private readonly IAction<Unit> _action;
            public TestAbility Ability;
            public override string Name => "";

            public ActionExecuteGiven(IAction<Unit> action)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
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

        public class ActionExecuteWhenAndGivenNotOverridden : ActionBaseUnit<TestAbility>
        {
            private readonly IAction<Unit> _action;
            public override string Name => "";

            public ActionExecuteWhenAndGivenNotOverridden(IAction<Unit> action)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
            }

            protected override void ExecuteWhen(IActor actor, TestAbility ability)
            {
                actor.Execute(_action);
            }
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(ActionBase<TestAbility, object> sut)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.IsAssignableFrom<IAction<TestAbility, object>>(sut);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(IFixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(ActionExecuteGiven));
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

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_WhenSutIsActionUnit_ShouldCallActorExecute(ActionExecuteWhen sut, Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
#pragma warning disable CS0618 // Type or member is obsolete
            actor.Verify(a => a.ExecuteWithAbility(sut));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_WhenSutIsActionUnit_ShouldCallActorExecute(ActionExecuteGiven sut, Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
#pragma warning disable CS0618 // Type or member is obsolete
            actor.Verify(a => a.ExecuteWithAbility(sut));
#pragma warning restore CS0618 // Type or member is obsolete
        }

    }
}
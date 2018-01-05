using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionUnitTests
    {
        public class ActionExecuteWhen : ActionUnit
        {
            public IActor Actor;
            private readonly IAction<Unit> _action;
            public override string Name { get; }

            public ActionExecuteWhen(IAction<Unit> action, string name)
            {
                _action = action;
                Name = name;
            }

            protected override void ExecuteWhen(IActor actor)
            {
                Actor = actor;
                actor.Execute(_action);
            }

            protected override void ExecuteGiven(IActor actor)
            {
            }
        }

        public class ActionExecuteGiven : ActionUnit
        {
            public IActor Actor;
            private readonly IAction<Unit> _action;
            public override string Name => "";

            public ActionExecuteGiven(IAction<Unit> action)
            {
                _action = action;
            }

            protected override void ExecuteWhen(IActor actor)
            {
            }

            protected override void ExecuteGiven(IActor actor)
            {
                Actor = actor;
                actor.Execute(_action);
            }
        }

        public class ActionExecuteWhenAndGivenNotOverridden : ActionUnit
        {
            private readonly IAction<Unit> _action;
            public override string Name => "";

            public ActionExecuteWhenAndGivenNotOverridden(IAction<Unit> action)
            {
                _action = action;
            }

            protected override void ExecuteWhen(IActor actor)
            {
                actor.Execute(_action);
            }
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(Action<object> sut)
        {
            Assert.IsAssignableFrom(typeof(IAction<object>), sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(IFixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(ActionExecuteGiven).GetMethods());
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorExecute([Frozen] IAction<Unit> expected, ActionExecuteWhen sut, Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            actor.Verify(a => a.Execute(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorExecute([Frozen] IAction<Unit> expected, ActionExecuteGiven sut, Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            actor.Verify(a => a.Execute(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_WhenExecuteGivenIsNotOverridden_ShouldCallActorExecute([Frozen] IAction<Unit> expected, ActionExecuteWhenAndGivenNotOverridden sut, Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            actor.Verify(a => a.Execute(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldUseCorrectActor(ActionExecuteWhen sut, Mock<IActor> actor)
        {
            //arrange
            var expected = actor.Object;
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            Assert.Equal(expected, sut.Actor);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldUseCorrectActor(ActionExecuteGiven sut, Mock<IActor> actor)
        {
            //arrange
            var expected = actor.Object;
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            Assert.Equal(expected, sut.Actor);
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
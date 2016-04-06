using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionTests
    {
        public class TestAbility : IAbility<TestAbility>
        {
            public TestAbility AsActor(IActor actor)
            {
                throw new NotImplementedException();
            }
        }

        public class ActionExecuteWhen : Action
        {
            public IActor Actor;
            private readonly IAction _action;

            public ActionExecuteWhen(IAction action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }
                _action = action;
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

        public class ActionExecuteGiven : Action
        {
            public IActor Actor;
            private readonly IAction _action;

            public ActionExecuteGiven(IAction action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }
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

        public class ActionExecuteWhenAndGivenNotOverridden : Action
        {
            private readonly IAction _action;

            public ActionExecuteWhenAndGivenNotOverridden(IAction action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }
                _action = action;
            }

            protected override void ExecuteWhen(IActor actor)
            {
                actor.Execute(_action);
            }
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(Action sut)
        {
            Assert.IsAssignableFrom(typeof(IAction), sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(IFixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(ActionExecuteGiven).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorExecute(            
            [Frozen] IAction expected,
            ActionExecuteWhen sut,
            Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            actor.Verify(a => a.Execute(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorExecute(
           [Frozen] IAction expected,
           ActionExecuteGiven sut,
           Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            actor.Verify(a => a.Execute(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_WhenExecuteGivenIsNotOverridden_ShouldCallActorExecute(
           [Frozen] IAction expected,
           ActionExecuteWhenAndGivenNotOverridden sut,
           Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            actor.Verify(a => a.Execute(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldReturnCorrectValue(
           ActionExecuteWhenAndGivenNotOverridden sut,
           Mock<IActor> actor)
        {
            //arrange
            var expected = actor;
            //act
            var actual = sut.ExecuteGivenAs(expected.Object);
            //assert
            Assert.Equal(expected.Object, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldReturnCorrectValue(
           ActionExecuteWhen sut,
           Mock<IActor> actor)
        {
            //arrange
            var expected = actor;
            //act
            var actual = sut.ExecuteWhenAs(expected.Object);
            //assert
            Assert.Equal(expected.Object, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldUseCorrectActor(           
           ActionExecuteWhen sut,
           Mock<IActor> actor)
        {
            //arrange
            var expected = actor.Object;            
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            Assert.Equal(expected, sut.Actor);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldUseCorrectActor(           
           ActionExecuteGiven sut,
           Mock<IActor> actor)
        {
            //arrange
            var expected = actor.Object;            
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            Assert.Equal(expected, sut.Actor);
        }
    }
}

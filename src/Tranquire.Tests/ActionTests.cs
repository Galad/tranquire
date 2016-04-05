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

        public class ActionTest : Action<TestAbility>
        {
            private readonly IAction _action;

            public ActionTest(IAction action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }
                _action = action;
            }

            protected override void ExecuteWhen(IActionCommand command)
            {
                command.Execute(_action);
            }

            protected override void ExecuteGiven(IActionCommand command)
            {
                base.ExecuteGiven(command);
                command.Execute(_action);
            }
        }

        public class ActionTestExecuteGivenNotImplemented : Action<TestAbility>
        {
            private readonly IAction _action;

            public ActionTestExecuteGivenNotImplemented(IAction action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }
                _action = action;
            }

            protected override void ExecuteWhen(IActionCommand command)
            {
                command.Execute(_action);
            }
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(Action<TestAbility> sut)
        {
            Assert.IsAssignableFrom(typeof(IAction), sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(IFixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(ActionTest).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorAttemptsTo(            
            [Frozen] IAction expected,
            ActionTest sut,
            Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            actor.Verify(a => a.AttemptsTo(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorWasAbleTo(
           [Frozen] IAction expected,
           ActionTest sut,
           Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            actor.Verify(a => a.WasAbleTo(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_WhenExecuteGivenIsNotOverridden_ShouldCallActorAttemptsTo(
           [Frozen] IAction expected,
           ActionTestExecuteGivenNotImplemented sut,
           Mock<IActor> actor)
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            actor.Verify(a => a.WasAbleTo(expected));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldReturnCorrectValue(           
           ActionTestExecuteGivenNotImplemented sut,
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
           ActionTestExecuteGivenNotImplemented sut,
           Mock<IActor> actor)
        {
            //arrange
            var expected = actor;
            //act
            var actual = sut.ExecuteWhenAs(expected.Object);
            //assert
            Assert.Equal(expected.Object, actual);
        }
    }
}

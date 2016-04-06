using Moq;
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
    public class ActorTests
    {
        public class AbilityTest : IAbility<AbilityTest>
        {
            public AbilityTest AsActor(IActor actor)
            {
                return this;
            }
        }

        public class AbilityTest2 : IAbility<AbilityTest2>
        {
            public AbilityTest2 AsActor(IActor actor)
            {
                return this;
            }
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeActor(Actor sut)
        {
            Assert.IsAssignableFrom(typeof(IActor), sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorParameters(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(Actor));
        }

        [Theory, DomainAutoData]
        public void AbilityTo_WhenAddingAbility_ShouldReturnCorrectValue(
            Actor sut,
            AbilityTest expected)
        {
            //arrange
            sut.Can(expected);
            //act
            var actual = sut.AbilityTo<AbilityTest>();
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AbilityTo_WhenAddingSameAbilityTwice_ShouldReturnCorrectValue(
            Actor sut,
            AbilityTest ability,
            AbilityTest expected)
        {
            //arrange
            sut.Can(ability);
            sut.Can(expected);
            //act
            var actual = sut.AbilityTo<AbilityTest>();
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AbilityTo_WhenAddingMultipleAbilities_ShouldReturnCorrectValue(
          Actor sut,
          AbilityTest2 ability,
          AbilityTest expected)
        {
            //arrange
            sut.Can(expected);
            sut.Can(ability);
            //act
            var actual = sut.AbilityTo<AbilityTest>();
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AbilityTo_WhenAbilityDoesNotExists_ShouldThrow(
        Actor sut)
        {
            Assert.Throws<InvalidOperationException>(() => sut.AbilityTo<AbilityTest>());
        }

        [Theory, DomainAutoData]
        public void AttemptsTo_ShouldReturnCorrectValue(
            Actor sut,
            Mock<IWhenCommand> command,
            Actor expected)
        {
            command.Setup(p => p.ExecuteWhenAs(It.IsAny<IActor>())).Returns(expected);
            //act
            var actual = sut.AttemptsTo(command.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void WasAbleTo_ShouldReturnCorrectValue(
            Actor sut,
            Mock<IGivenCommand> performable,
            Actor expected)
        {
            performable.Setup(p => p.ExecuteGivenAs(It.IsAny<IActor>())).Returns(expected);
            //act
            var actual = sut.WasAbleTo(performable.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AsksFor_ShouldReturnCorrectValue(
            Actor sut,
            Mock<IQuestion<object>> question,
            object expected)
        {
            question.Setup(q => q.AnsweredBy(sut)).Returns(expected);
            //act
            var actual = sut.AsksFor(question.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Execute_ShouldReturnCorrectValue(
           Actor sut,
           Mock<IAction> action,
           Actor expected)
        {
            action.Setup(p => p.ExecuteWhenAs(It.IsAny<IActor>())).Returns(expected);
            //act
            var actual = sut.Execute(action.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Execute_CalledAfterCallingAttempsTo_ShouldCallExecuteWhen(
           Actor sut,
           Mock<IAction> action)
        {
            var secondAction = new Mock<IAction>(MockBehavior.Loose);
            IActor actual = null;
            action.Setup(p => p.ExecuteWhenAs(It.IsAny<IActor>())).Callback((IActor actor) => actual = actor);            
            //act
            sut.AttemptsTo(action.Object);
            actual.Execute(secondAction.Object);
            //assert
            secondAction.Verify(a => a.ExecuteWhenAs(It.IsAny<IActor>()));
        }

        [Theory, DomainAutoData]
        public void Execute_CalledAfterCallingWasAbleTo_ShouldCallExecuteGive(
           Actor sut,
           Mock<IAction> action)
        {
            var secondAction = new Mock<IAction>(MockBehavior.Loose);
            IActor actual = null;
            action.Setup(p => p.ExecuteGivenAs(It.IsAny<IActor>())).Callback((IActor actor) => actual = actor);
            //act
            sut.WasAbleTo(action.Object);
            actual.Execute(secondAction.Object);
            //assert
            secondAction.Verify(a => a.ExecuteGivenAs(It.IsAny<IActor>()));
        }
    }
}

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
            Mock<IPerformable> performable,
            Actor expected)
        {
            performable.Setup(p => p.PerformAs(sut)).Returns(expected);
            //act
            var actual = sut.AttemptsTo(performable.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void WasAbleTo_ShouldReturnCorrectValue(
            Actor sut,
            Mock<IPerformable> performable,
            Actor expected)
        {
            performable.Setup(p => p.PerformAs(sut)).Returns(expected);
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
    }
}

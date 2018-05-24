using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using System;
using Tranquire;
using Xunit;

namespace Tranquire.Tests
{
    public class QuestionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Questions));
        }

        [Theory, DomainAutoData]
        public void Create_ShouldReturnCorrectValue(IActor actor, object expected, string name)
        {
            // arrange
            var func = new Mock<Func<IActor, object>>();
            func.Setup(f => f(actor)).Returns(expected);
            // act
            var question = Questions.Create(name, func.Object);
            var actual = question.AnsweredBy(actor);
            // assert
            actual.Should().Be(expected);
        }
        
        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(string expected)
        {
            // arrange
            // act
            var question = Questions.Create(expected, _ => default(object));
            var actual = question.Name;
            // assert
            actual.Should().Be(expected);
        }
        
        [Theory, DomainAutoData]
        public void CreateWithAbility_ShouldReturnCorrectValue(
            IActor actor, 
            object ability,
            object expected, 
            string name)
        {
            // arrange
            var func = new Mock<Func<IActor, object, object>>();
            func.Setup(f => f(actor, ability)).Returns(expected);
            // act
            var question = Questions.Create(name, func.Object);
            var actual = question.AnsweredBy(actor, ability);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void NameWithAbility_ShouldReturnCorrectValue(string expected)
        {
            // arrange
            // act
            var question = Questions.Create(expected, (IActor _, object __) => default(object));
            var actual = question.Name;
            // assert
            actual.Should().Be(expected);
        }
    }
}

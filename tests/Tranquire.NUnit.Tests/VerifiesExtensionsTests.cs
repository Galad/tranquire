using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Moq;
using System;
using Tranquire;
using Tranquire.Tests;
using Xunit;
using NUnitF = NUnit.Framework;
using NUnitC = NUnit.Framework.Constraints;
using FluentAssertions;
using System.Linq;

namespace Tranquire.NUnit.Tests
{
    public class VerifiesExtensionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(VerifiesExtensions));
        }

        private static IQuestion<T> Question<T>(T value) => Questions.Create("Question", _ => value);

        [Theory, DomainAutoData]
        public void Then_WithConstraints_ShouldReturnCorrectValue(
            [Modest]Actor actor,
            object expected)
        {
            // arrange
            var question = Question(expected);
            var constraint = NUnitF.Is.EqualTo(expected);
            // act                        
            var actual = actor.Then(question, constraint);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Then_WithConstraints_WhenFailing_ShouldThrow(
            [Modest]Actor actor)
        {
            // arrange
            var question = Question(new object());
            var constraint = NUnitF.Is.EqualTo(new object());
            // act and assert
            Assert.Throws<NUnitF.AssertionException>(() => actor.Then(question, constraint));
        }

        [Theory, DomainAutoData]
        public void Then_WithConstraints_AndFuncExceptionMessage_ShouldReturnCorrectValue(
            [Modest]Actor actor,
            object expected,
            string message)
        {
            // arrange
            var question = Question(expected);
            var constraint = NUnitF.Is.EqualTo(expected);
            // act                        
            var actual = actor.Then(question, constraint, () => message);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Then_WithConstraints_AndFuncExceptionMessage_WhenFailing_ShouldThrow(
            [Modest]Actor actor,
            string message)
        {
            // arrange
            var question = Question(new object());
            var constraint = NUnitF.Is.EqualTo(new object());
            // act and assert
            new System.Action(() => actor.Then(question, constraint, () => message))
                .Should().Throw<NUnitF.AssertionException>()
                .Where(ex => ex.Message.Contains(message));
        }

        [Theory, DomainAutoData]
        public void Then_WithConstraints_AndExceptionMessage_ShouldReturnCorrectValue(
            [Modest]Actor actor,
            object expected,
            string message,
            object[] args)
        {
            // arrange
            var question = Question(expected);
            var constraint = NUnitF.Is.EqualTo(expected);
            // act                        
            var actual = actor.Then(question, constraint, message, args);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Then_WithConstraints_AndExceptionMessage_WhenFailing_ShouldThrow(
            [Modest]Actor actor,
            string[] args)
        {
            // arrange
            var message = string.Join("", args.Select((_, i) => $"{{{i}}}"));
            var question = Question(new object());
            var constraint = NUnitF.Is.EqualTo(new object());
            // act and assert
            var expectedMessage = string.Format(message, args);
            new System.Action(() => actor.Then(question, constraint, message, args))
                .Should().Throw<NUnitF.AssertionException>()
                .Where(ex => ex.Message.Contains(expectedMessage));
        }
    }
}

using FluentAssertions;
using AutoFixture.Idioms;
using System;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests
{
    public class QuestionExtensionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion) => assertion.Verify(typeof(QuestionExtensions));

        [Theory, DomainAutoData]
        public void Select_ShouldReturnCorrectResult(IQuestion<string> question, Func<string, object> selector)
        {
            // act
            var actual = QuestionExtensions.Select(question, selector);
            // assert
            var expected = new SelectQuestion<string, object>(question, selector);
            actual.Should().BeOfType<SelectQuestion<string, object>>();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}

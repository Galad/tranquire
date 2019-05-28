using FluentAssertions;
using AutoFixture.Idioms;
using System;
using Tranquire.Extensions;
using Xunit;
using Moq;
using System.Threading.Tasks;

namespace Tranquire.Tests
{
    public class QuestionExtensionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion) => assertion.Verify(typeof(QuestionExtensions));
        
        #region Select
        [Theory, DomainAutoData]
        public void Select_ShouldReturnCorrectResult(IQuestion<string> question, Func<string, object> selector)
        {
            // act
            var actual = question.Select(selector);
            // assert
            var expected = new SelectQuestion<string, object>(question, selector);
            actual.Should().BeOfType<SelectQuestion<string, object>>();
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory, DomainAutoData]
        public async Task Select_Async_ShouldReturnCorrectResult(IQuestion<Task<string>> question, string value, object expected)
        {
            // arrange
            var selector = new Mock<Func<string, object>>();
            selector.Setup(s => s(value)).Returns(expected);
            // act
            var actual = question.Select(selector.Object);
            // assert            
            var selectQuestion = actual.Should().BeOfType<SelectQuestionAsync<string, object>>().Which;
            selectQuestion.Question.Should().Be(question);
            var actualSelected = await selectQuestion.Selector(value);
            Assert.Equal(expected, actualSelected);
        }

        [Theory, DomainAutoData]
        public void Select_Async_WithAsyncFunc_ShouldReturnCorrectResult(IQuestion<Task<string>> question, Func<string, Task<object>> selector)
        {
            // act
            var actual = question.Select(selector);
            // assert
            var expected = new SelectQuestionAsync<string, object>(question, selector);
            actual.Should().BeOfType<SelectQuestionAsync<string, object>>();
            actual.Should().BeEquivalentTo(expected);
        }
        #endregion

        [Theory, DomainAutoData]
        public void SelectMany_ShouldReturnCorrectResult(IQuestion<string> question, Func<string, IQuestion<object>> selector)
        {
            // act
            var actual = question.SelectMany(selector);
            // assert
            var expected = new SelectManyQuestion<string, object>(question, selector);
            actual.Should().BeOfType<SelectManyQuestion<string, object>>();
            actual.Should().BeEquivalentTo(expected);
        }

        [Theory, DomainAutoData]
        public void SelectMany_ReturnAction_ShouldReturnCorrectResult(IQuestion<string> question, Func<string, IAction<object>> selector)
        {
            // act
            var actual = question.SelectMany(selector);
            // assert
            var expected = new SelectManyQuestionToAction<string, object>(question, selector);
            actual.Should().BeOfType<SelectManyQuestionToAction<string, object>>();
            actual.Should().BeEquivalentTo(expected);
        }

        #region Tagged
        [Theory, DomainAutoData]
        public void Tagged_ShouldReturnCorrectValue(IQuestion<object> question, string tag)
        {
            // arrange
            // act
            var actual = question.Tagged(tag);
            // assert
            var expected = Questions.CreateTagged(question.Name, (tag, question));
            actual.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }
        #endregion
    }
}

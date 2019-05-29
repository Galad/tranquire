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

        #region SelectMany - question
        [Theory, DomainAutoData]
        public void SelectMany_ShouldReturnCorrectResult(
            int value1,
            int value2)
        {
            // arrange
            var question = Questions.FromResult(value1)
                                    .SelectMany(v => Questions.FromResult(v + value2));
            var actor = new Actor("john");
            // act
            var actual = actor.AsksFor(question);
            // assert
            var expected = value1 + value2;
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void SelectMany_2_ShouldReturnCorrectResult(
           int value1,
           int value2,
           int value3)
        {
            // arrange
            var question = Questions.FromResult(value1)
                                    .SelectMany(v => Questions.FromResult(v + value2))
                                    .SelectMany(v => Questions.FromResult(v + value3));
            var actor = new Actor("john");
            // act
            var actual = actor.AsksFor(question);
            // assert
            var expected = value1 + value2 + value3;
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void SelectMany_Name_ShouldReturnCorrectResult(
            IQuestion<int> question1,
            IQuestion<int> question2)
        {
            // arrange
            var question = question1.SelectMany(_ => question2);
            // act
            var actual = question.Name;
            // assert
            var expected = "[SelectMany] " + question1.Name;
            Assert.Equal(expected, actual);
        }
        #endregion

        #region SelectMany - question to action
        [Theory, DomainAutoData]
        public void SelectMany_ReturningAction_ShouldReturnCorrectResult(
            int value1,
            int value2)
        {
            // arrange
            var action = Questions.FromResult(value1)
                                    .SelectMany(v => Actions.FromResult(v + value2));
            var actor = new Actor("john");
            // act
            var actual = actor.When(action);
            // assert
            var expected = value1 + value2;
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void SelectMany_ReturningQuestion_Name_ShouldReturnCorrectResult(
            IQuestion<int> question1,
            IAction<int> action2)
        {
            // arrange
            var action = question1.SelectMany(_ => action2);
            // act
            var actual = action.Name;
            // assert
            var expected = "[SelectMany] " + question1.Name;
            Assert.Equal(expected, actual);
        }
        #endregion

        #region SelectMany - question async
        [Theory, DomainAutoData]
        public async Task SelectMany_Async_ShouldReturnCorrectResult(
            int value1,
            int value2)
        {
            // arrange
            var question = Questions.FromResult(Task.FromResult(value1))
                                    .SelectMany(v => Questions.FromResult(v + value2));
            var actor = new Actor("john");
            // act
            var actual = await actor.AsksFor(question);
            // assert
            var expected = value1 + value2;
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public async Task SelectMany_Async_2_ShouldReturnCorrectResult(
           int value1,
           int value2,
           int value3)
        {
            // arrange
            var question = Questions.FromResult(Task.FromResult(value1))
                                    .SelectMany(v => Questions.FromResult(v + value2))
                                    .SelectMany(v => Questions.FromResult(v + value3));
            var actor = new Actor("john");
            // act
            var actual = await actor.AsksFor(question);
            // assert
            var expected = value1 + value2 + value3;
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void SelectMany_Async_Name_ShouldReturnCorrectResult(
            IQuestion<Task<int>> question1,
            IQuestion<int> question2)
        {
            // arrange
            var question = question1.SelectMany((int _) => question2);
            // act
            var actual = question.Name;
            // assert
            var expected = "[SelectMany] " + question1.Name;
            Assert.Equal(expected, actual);
        }
        #endregion

        #region SelectMany - question async returning async question
        [Theory, DomainAutoData]
        public async Task SelectMany_Async_ReturningAsyncQuestion_ShouldReturnCorrectResult(
           int value1,
           int value2)
        {
            // arrange
            var question = Questions.FromResult(Task.FromResult(value1))
                                    .SelectMany(v => Questions.FromResult(Task.FromResult(v + value2)));
            var actor = new Actor("john");
            // act
            var actual = await actor.AsksFor(question);
            // assert
            var expected = value1 + value2;
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public async Task SelectMany_Async_2_ReturningAsyncQuestion_ShouldReturnCorrectResult(
           int value1,
           int value2,
           int value3)
        {
            // arrange
            var question = Questions.FromResult(Task.FromResult(value1))
                                    .SelectMany(v => Questions.FromResult(Task.FromResult(v + value2)))
                                    .SelectMany(v => Questions.FromResult(Task.FromResult(v + value3)));
            var actor = new Actor("john");
            // act
            var actual = await actor.AsksFor(question);
            // assert
            var expected = value1 + value2 + value3;
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void SelectMany_Async_Name_ReturningAsyncQuestion_ShouldReturnCorrectResult(
            IQuestion<Task<int>> question1,
            IQuestion<Task<int>> question2)
        {
            // arrange
            var question = question1.SelectMany((int _) => question2);
            // act
            var actual = question.Name;
            // assert
            var expected = "[SelectMany] " + question1.Name;
            Assert.Equal(expected, actual);
        }
        #endregion

        #region SelectMany - question to action async
        [Theory, DomainAutoData]
        public async Task SelectMany_Async_ReturningAction_ShouldReturnCorrectResult(
            int value1,
            int value2)
        {
            // arrange
            var action = Questions.FromResult(Task.FromResult(value1))
                                    .SelectMany(v => Actions.FromResult(v + value2));
            var actor = new Actor("john");
            // act
            var actual = await actor.When(action);
            // assert
            var expected = value1 + value2;
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void SelectMany_Async_Name_ReturningAction_ShouldReturnCorrectResult(
            IQuestion<Task<int>> question1,
            IAction<int> action2)
        {
            // arrange
            var action = question1.SelectMany((int _) => action2);
            // act
            var actual = action.Name;
            // assert
            var expected = "[SelectMany] " + question1.Name;
            Assert.Equal(expected, actual);
        }
        #endregion

        #region SelectMany - question async returning async action 
        [Theory, DomainAutoData]
        public async Task SelectMany_Async_ReturningAsyncAction_ShouldReturnCorrectResult(
            int value1,
            int value2)
        {
            // arrange
            var action = Questions.FromResult(Task.FromResult(value1))
                                  .SelectMany(v => Actions.FromResult(Task.FromResult(v + value2)));
            var actor = new Actor("john");
            // act
            var actual = await actor.When(action);
            // assert
            var expected = value1 + value2;
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void SelectMany_Async_Name_ReturningAsyncAction_ShouldReturnCorrectResult(
            IQuestion<Task<int>> question1,
            IAction<Task<int>> action2)
        {
            // arrange
            var action = question1.SelectMany((int _) => action2);
            // act
            var actual = action.Name;
            // assert
            var expected = "[SelectMany] " + question1.Name;
            Assert.Equal(expected, actual);
        }
        #endregion

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

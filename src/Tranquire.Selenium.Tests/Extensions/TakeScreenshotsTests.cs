using System;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace Tranquire.Selenium.Tests.Extensions
{
    public class TakeScreenshotsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(TakeScreenshot).GetConstructors());
        }

        #region Execute
        [Theory, DomainAutoData]
        public void Execute_CallingGiven_ShouldCallTakeScreenshotStrategy(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            [Greedy]TakeScreenshot sut,
            IAction<object> action,
            IActor otherActor,
            object expected
            )
        {
            // arrange
            Mock.Get(action).Setup(q => q.ExecuteGivenAs(otherActor)).Returns(expected);            
            Mock.Get(sut.Actor).Setup(a => a.Execute(It.IsAny<IAction<object>>())).Returns((IAction<object> a) => a.ExecuteGivenAs(otherActor));
            Mock.Get(sut.TakeScreenshotStrategy).Setup(t => t.ExecuteTakeScreenshot(Unit.Default,
                                                                                    otherActor,
                                                                                    It.IsAny<Func<object>>(),
                                                                                    sut.NextScreenshotName,
                                                                                    observer.Object))
                                                .Returns<Unit, IActor, Func<object>, Func<string>, IObserver<ScreenshotInfo>>((a, b, execute, c, d) => execute());
            // act
            var actual = sut.Execute(action);
            // assert                        
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Execute_CallingWhen_ShouldCallTakeScreenshotStrategy(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            [Greedy]TakeScreenshot sut,
            IAction<object> action,
            IActor otherActor,
            object expected
            )
        {
            // arrange
            Mock.Get(action).Setup(q => q.ExecuteWhenAs(otherActor)).Returns(expected);
            Mock.Get(sut.Actor).Setup(a => a.Execute(It.IsAny<IAction<object>>())).Returns((IAction<object> a) => a.ExecuteWhenAs(otherActor));
            Mock.Get(sut.TakeScreenshotStrategy).Setup(t => t.ExecuteTakeScreenshot(Unit.Default,
                                                                                    otherActor,
                                                                                    It.IsAny<Func<object>>(),
                                                                                    sut.NextScreenshotName,
                                                                                    observer.Object))
                                                .Returns<Unit, IActor, Func<object>, Func<string>, IObserver<ScreenshotInfo>>((a, b, execute, c, d) => execute());
            // act
            var actual = sut.Execute(action);
            // assert                        
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteWithAbility_CallingGiven_ShouldCallTakeScreenshotStrategy(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            [Greedy]TakeScreenshot sut,
            IAction<object, object> action,
            IActor otherActor,
            object expected,
            object ability
            )
        {
            // arrange
            Mock.Get(action).Setup(q => q.ExecuteGivenAs(otherActor, ability)).Returns(expected);
            Mock.Get(sut.Actor).Setup(a => a.ExecuteWithAbility(It.IsAny<IAction<object, object>>())).Returns((IAction<object, object> a) => a.ExecuteGivenAs(otherActor, ability));
            Mock.Get(sut.TakeScreenshotStrategy).Setup(t => t.ExecuteTakeScreenshot(ability,
                                                                                    otherActor,
                                                                                    It.IsAny<Func<object>>(),
                                                                                    sut.NextScreenshotName,
                                                                                    observer.Object))
                                                .Returns<object, IActor, Func<object>, Func<string>, IObserver<ScreenshotInfo>>((a, b, execute, c, d) => execute());
            // act
            var actual = sut.ExecuteWithAbility(action);
            // assert                        
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteWithAbility_CallingWhen_ShouldCallTakeScreenshotStrategy(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            [Greedy]TakeScreenshot sut,
            IAction<object, object> action,
            IActor otherActor,
            object expected,
            object ability
            )
        {
            // arrange
            Mock.Get(action).Setup(q => q.ExecuteWhenAs(otherActor, ability)).Returns(expected);
            Mock.Get(sut.Actor).Setup(a => a.ExecuteWithAbility(It.IsAny<IAction<object, object>>())).Returns((IAction<object, object> a) => a.ExecuteWhenAs(otherActor, ability));
            Mock.Get(sut.TakeScreenshotStrategy).Setup(t => t.ExecuteTakeScreenshot(ability,
                                                                                    otherActor,
                                                                                    It.IsAny<Func<object>>(),
                                                                                    sut.NextScreenshotName,
                                                                                    observer.Object))
                                                .Returns<object, IActor, Func<object>, Func<string>, IObserver<ScreenshotInfo>>((a, b, execute, c, d) => execute());
            // act
            var actual = sut.ExecuteWithAbility(action);
            // assert                        
            Assert.Equal(expected, actual);
        }
        #endregion

        #region AskFor
        [Theory, DomainAutoData]
        public void AsksFor_ShouldCallTakeScreenshotStrategy(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            [Greedy]TakeScreenshot sut,
            IQuestion<object> question,
            IActor otherActor,
            object expected
            )
        {
            // arrange
            Mock.Get(question).Setup(q => q.AnsweredBy(otherActor)).Returns(expected);
            Mock.Get(sut.Actor).Setup(a => a.AsksFor(It.IsAny<IQuestion<object>>())).Returns((IQuestion<object> a) => a.AnsweredBy(otherActor));
            Mock.Get(sut.TakeScreenshotStrategy).Setup(t => t.ExecuteTakeScreenshot(Unit.Default,
                                                                                    otherActor,
                                                                                    It.IsAny<Func<object>>(),
                                                                                    sut.NextScreenshotName,
                                                                                    observer.Object))
                                                .Returns<Unit, IActor, Func<object>, Func<string>, IObserver<ScreenshotInfo>>((a, b, execute, c, d) => execute());
            // act
            var actual = sut.AsksFor(question);
            // assert                        
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AsksFor_WhenQuestionNameIsWebBrowserQuestion_ShouldNotCallTakeScreenshotStrategy(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            [Greedy]TakeScreenshot sut,
            IQuestion<object> question,
            IActor otherActor,
            object expected,
            object notExpected
            )
        {
            // arrange
            Mock.Get(question).Setup(q => q.AnsweredBy(otherActor)).Returns(expected);
            Mock.Get(question).Setup(q => q.Name).Returns(TakeScreenshotOnErrorStrategy.GetWebBrowserQuestionName);
            Mock.Get(sut.Actor).Setup(a => a.AsksFor(It.IsAny<IQuestion<object>>())).Returns((IQuestion<object> a) => a.AnsweredBy(otherActor));
            Mock.Get(sut.TakeScreenshotStrategy).Setup(t => t.ExecuteTakeScreenshot(Unit.Default,
                                                                                    otherActor,
                                                                                    It.IsAny<Func<object>>(),
                                                                                    sut.NextScreenshotName,
                                                                                    observer.Object))
                                                .Returns<Unit, IActor, Func<object>, Func<string>, IObserver<ScreenshotInfo>>((a, b, execute, c, d) => notExpected);
            // act
            var actual = sut.AsksFor(question);
            // assert                        
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AsksForWithAbility_ShouldCallTakeScreenshotStrategy(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            [Greedy]TakeScreenshot sut,
            IQuestion<object, object> question,
            IActor otherActor,
            object ability,
            object expected
            )
        {
            // arrange
            Mock.Get(question).Setup(q => q.AnsweredBy(otherActor, ability)).Returns(expected);
            Mock.Get(sut.Actor).Setup(a => a.AsksForWithAbility(It.IsAny<IQuestion<object, object>>())).Returns((IQuestion<object, object> a) => a.AnsweredBy(otherActor, ability));
            Mock.Get(sut.TakeScreenshotStrategy).Setup(t => t.ExecuteTakeScreenshot(ability,
                                                                                    otherActor,
                                                                                    It.IsAny<Func<object>>(),
                                                                                    sut.NextScreenshotName,
                                                                                    observer.Object))
                                                .Returns<object, IActor, Func<object>, Func<string>, IObserver<ScreenshotInfo>>((a, b, execute, c, d) => execute());
            // act
            var actual = sut.AsksForWithAbility(question);
            // assert                        
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AsksForWithAbility_WhenQuestionNameIsWebBrowserQuestion_ShouldNotCallTakeScreenshotStrategy(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            [Greedy]TakeScreenshot sut,
            IQuestion<object, object> question,
            IActor otherActor,
            object ability,
            object expected,
            object notExpected
            )
        {
            // arrange
            Mock.Get(question).Setup(q => q.AnsweredBy(otherActor, ability)).Returns(expected);
            Mock.Get(question).Setup(q => q.Name).Returns(TakeScreenshotOnErrorStrategy.GetWebBrowserQuestionName);
            Mock.Get(sut.Actor).Setup(a => a.AsksForWithAbility(It.IsAny<IQuestion<object, object>>())).Returns((IQuestion<object, object> a) => a.AnsweredBy(otherActor, ability));
            Mock.Get(sut.TakeScreenshotStrategy).Setup(t => t.ExecuteTakeScreenshot(ability,
                                                                                    otherActor,
                                                                                    It.IsAny<Func<object>>(),
                                                                                    sut.NextScreenshotName,
                                                                                    observer.Object))
                                                .Returns<object, IActor, Func<object>, Func<string>, IObserver<ScreenshotInfo>>((a, b, execute, c, d) => notExpected);
            // act
            var actual = sut.AsksForWithAbility(question);
            // assert                        
            Assert.Equal(expected, actual);
        }
        #endregion

        [Theory, DomainAutoData]
        public void QuestionName_ShouldReturnCorrectValue(
            TakeScreenshot sut,
            IQuestion<object> question
            )
        {
            // arrange
            string actual = "";
            Mock.Get(sut.Actor).Setup(a => a.AsksFor(It.IsAny<IQuestion<object>>()))
                .Callback((IQuestion<object> q) => actual = q.Name);
            // act
            sut.AsksFor(question);
            // assert
            var expected = question.Name;
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void QuestionName_WithAbility_ShouldReturnCorrectValue(
            TakeScreenshot sut,
            IQuestion<object, object> question
            )
        {
            // arrange
            string actual = "";
            Mock.Get(sut.Actor).Setup(a => a.AsksForWithAbility(It.IsAny<IQuestion<object, object>>()))
                .Callback((IQuestion<object, object> q) => actual = q.Name);
            // act
            sut.AsksForWithAbility(question);
            // assert
            var expected = "[Take screenshot] " + question.Name;
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ActionName_ShouldReturnCorrectValue(
            TakeScreenshot sut,
            IAction<object> action
            )
        {
            // arrange
            string actual = "";
            Mock.Get(sut.Actor).Setup(a => a.Execute(It.IsAny<IAction<object>>()))
                .Callback((IAction<object> q) => actual = q.Name);
            // act
            sut.Execute(action);
            // assert
            var expected = action.Name;
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ActionName_WithAbility_ShouldReturnCorrectValue(
            TakeScreenshot sut,
            IAction<object, object> action
            )
        {
            // arrange
            string actual = "";
            Mock.Get(sut.Actor).Setup(a => a.ExecuteWithAbility(It.IsAny<IAction<object, object>>()))
                .Callback((IAction<object, object> q) => actual = q.Name);
            // act
            sut.ExecuteWithAbility(action);
            // assert
            var expected = "[Take screenshot] " + action.Name;
            actual.Should().Be(expected);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete

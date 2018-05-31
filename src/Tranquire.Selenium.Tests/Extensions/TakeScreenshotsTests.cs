using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
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
        public void Execute_WithActionUnit_CallingGiven_ShouldNotCallObserver(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            TakeScreenshot sut,
            IAction<object> action,
            IActor otherActor
            )
        {
            // arrange
            Mock.Get(sut.Actor).Setup(a => a.Execute(action)).Callback((IAction<object> a) => a.ExecuteGivenAs(otherActor));
            // act
            sut.Execute(action);
            // assert            
            observer.Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
            Mock.Get(action).Verify(a => a.ExecuteGivenAs(otherActor));
        }

        [Theory, DomainAutoData]
        public void Execute_WithActionUnit_CallingWhen_ShouldNotCallObserver(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            TakeScreenshot sut,
            IAction<object> action,
            IActor otherActor
            )
        {
            // arrange
            Mock.Get(sut.Actor).Setup(a => a.Execute(action)).Callback((IAction<object> a) => a.ExecuteWhenAs(otherActor));
            // act
            sut.Execute(action);
            // assert            
            observer.Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
            Mock.Get(action).Verify(a => a.ExecuteWhenAs(otherActor));
        }

        [Theory, DomainAutoData]
        public void Execute_WithActionWithoutWebBrowserAbility_CallingGiven_ShouldNotCallObserver(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            TakeScreenshot sut,
            IAction<object, object> action,
            IActor otherActor,
            object ability
            )
        {
            // arrange
            Mock.Get(sut.Actor).Setup(a => a.ExecuteWithAbility(It.IsAny<IAction<object, object>>()))
                               .Callback((IAction<object, object> a) => a.ExecuteGivenAs(otherActor, ability));
            // act
            sut.ExecuteWithAbility(action);
            // assert
            observer.Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
            Mock.Get(action).Verify(a => a.ExecuteGivenAs(otherActor, ability));
        }

        [Theory, DomainAutoData]
        public void Execute_WithActionWithoutWebBrowserAbility_CallingWhen_ShouldNotCallObserver(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            TakeScreenshot sut,
            IAction<object, object> action,
            IActor otherActor,
            object ability
            )
        {
            // arrange
            Mock.Get(sut.Actor).Setup(a => a.ExecuteWithAbility(It.IsAny<IAction<object, object>>()))
                               .Callback((IAction<object, object> a) => a.ExecuteWhenAs(otherActor, ability));
            // act
            sut.ExecuteWithAbility(action);
            // assert
            observer.Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
            Mock.Get(action).Verify(a => a.ExecuteWhenAs(otherActor, ability));
        }

        [Theory, DomainAutoData]
        public void Execute_WithActionWithWebBrowserAbility_CallingGiven_ShouldCallObserver(
            [Frozen] string expectedName,
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            TakeScreenshot sut,
            IAction<WebBrowser, object> action,
            IActor otherActor
            )
        {
            // arrange            
            var screenshot = new Screenshot("abcdefgskdjf");
            var expected = new ScreenshotInfo(screenshot, expectedName);
            var webDriver = new Mock<IWebDriver>();
            webDriver.As<ITakesScreenshot>().Setup(t => t.GetScreenshot()).Returns(screenshot);
            var ability = WebBrowser.With(webDriver.Object);
            Mock.Get(sut.Actor).Setup(a => a.ExecuteWithAbility(It.IsAny<IAction<WebBrowser, object>>()))
                .Callback((IAction<WebBrowser, object> a) => a.ExecuteGivenAs(otherActor, ability));
            // act
            sut.ExecuteWithAbility(action);
            // assert
            observer.Verify(CallOnNext(expected), Times.Once());
            Mock.Get(action).Verify(a => a.ExecuteGivenAs(otherActor, ability));
        }

        [Theory, DomainAutoData]
        public void Execute_WithActionWithWebBrowserAbility_CallingWhen_ShouldCallObserver(
             [Frozen] string expectedName,
             [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
             TakeScreenshot sut,
             IAction<WebBrowser, object> action,
             IActor otherActor
            )
        {
            // arrange            
            var screenshot = new Screenshot("abcdefgskdjf");
            var expected = new ScreenshotInfo(screenshot, expectedName);
            var webDriver = new Mock<IWebDriver>();
            webDriver.As<ITakesScreenshot>().Setup(t => t.GetScreenshot()).Returns(screenshot);
            var ability = WebBrowser.With(webDriver.Object);
            Mock.Get(sut.Actor).Setup(a => a.ExecuteWithAbility(It.IsAny<IAction<WebBrowser, object>>()))
                               .Callback((IAction<WebBrowser, object> a) => a.ExecuteWhenAs(otherActor, ability));
            // act
            sut.ExecuteWithAbility(action);
            // assert
            observer.Verify(CallOnNext(expected), Times.Once());
            Mock.Get(action).Verify(a => a.ExecuteWhenAs(otherActor, ability));
        }
        #endregion

        #region AskFor
        [Theory, DomainAutoData]
        public void AsksFor_WithQuestion_CallingGiven_ShouldNotCallObserver(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            TakeScreenshot sut,
            IQuestion<object> action,
            IActor otherActor
            )
        {
            // arrange
            Mock.Get(sut.Actor).Setup(a => a.AsksFor(action)).Callback((IQuestion<object> a) => a.AnsweredBy(otherActor));
            // act
            sut.AsksFor(action);
            // assert            
            observer.Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
            Mock.Get(action).Verify(a => a.AnsweredBy(otherActor));
        }

        [Theory, DomainAutoData]
        public void AsksFor_WithQuestionWithoutWebBrowserAbility_ShouldNotCallObserver(
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            TakeScreenshot sut,
            IQuestion<object, object> action,
            IActor otherActor,
            object ability
            )
        {
            // arrange
            Mock.Get(sut.Actor).Setup(a => a.AsksForWithAbility(It.IsAny<IQuestion<object, object>>()))
                .Callback((IQuestion<object, object> a) => a.AnsweredBy(otherActor, ability));
            // act
            sut.AsksForWithAbility(action);
            // assert            
            observer.Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
            Mock.Get(action).Verify(a => a.AnsweredBy(otherActor, ability));
        }

        [Theory, DomainAutoData]
        public void AsksFor_WithQuestionWithWebBrowserAbility_ShouldNotCallObserver(
            [Frozen] string expectedName,
            [Frozen]Mock<IObserver<ScreenshotInfo>> observer,
            TakeScreenshot sut,
            IQuestion<object, WebBrowser> action,
            IActor otherActor
            )
        {
            // arrange
            var screenshot = new Screenshot("abcdefgskdjf");
            var expected = new ScreenshotInfo(screenshot, expectedName);
            var webDriver = new Mock<IWebDriver>();
            webDriver.As<ITakesScreenshot>().Setup(t => t.GetScreenshot()).Returns(screenshot);
            var ability = WebBrowser.With(webDriver.Object);
            Mock.Get(sut.Actor).Setup(a => a.AsksForWithAbility(It.IsAny<IQuestion<object, WebBrowser>>()))
                .Callback((IQuestion<object, WebBrowser> a) => a.AnsweredBy(otherActor, ability));
            // act
            sut.AsksForWithAbility(action);
            // assert            
            observer.Verify(CallOnNext(expected), Times.Once());
            Mock.Get(action).Verify(a => a.AnsweredBy(otherActor, ability));
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

        private Expression<System.Action<IObserver<ScreenshotInfo>>> CallOnNext(ScreenshotInfo expected)
        {
            return o => o.OnNext(It.Is<ScreenshotInfo>(s => s.FileName == expected.FileName &&
                                                            s.Screenshot.AsBase64EncodedString == expected.Screenshot.AsBase64EncodedString));
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete

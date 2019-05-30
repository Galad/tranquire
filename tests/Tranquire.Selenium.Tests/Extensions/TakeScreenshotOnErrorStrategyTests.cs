using System;
using System.Linq.Expressions;
using AutoFixture.Idioms;
using Moq;
using OpenQA.Selenium;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Extensions
{
    public class TakeScreenshotOnErrorStrategyTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AlwaysTakeScreenshotStrategy));
        }

        [Theory, DomainAutoData]
        public void ExecuteTakeScreenshot_ShouldReturnCorrectValue(
            TakeScreenshotOnErrorStrategy sut,
            IObserver<ScreenshotInfo> screenshotObserver,
            IActor actor,
            object expected,
            Func<string> nextScreenshotName)
        {
            // Act
            var actual = sut.ExecuteTakeScreenshot(
                Unit.Default,
                actor,
                () => expected,
                nextScreenshotName,
                screenshotObserver);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteTakeScreenshot_WhenAbilityIsWebBrowser_ShouldNotCallObserver(
            TakeScreenshotOnErrorStrategy sut,
            IActor actor,
            string expectedName,
            IObserver<ScreenshotInfo> observer)
        {
            // arrange
            var screenshot = new Screenshot("abcdefgskdjf");
            var screenshotInfo = new ScreenshotInfo(screenshot, expectedName);
            var webDriver = new Mock<IWebDriver>();
            webDriver.As<ITakesScreenshot>().Setup(t => t.GetScreenshot()).Returns(screenshot);
            var webBrowser = WebBrowser.With(webDriver.Object);
            // act
            sut.ExecuteTakeScreenshot(webBrowser, actor, () => new object(), () => expectedName, observer);
            // assert
            Mock.Get(observer).Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
        }

        [Theory, DomainAutoData]
        public void ExecuteTakeScreenshot_WhenAbilityIsWebBrowser_AndExecuteThrows_ShouldCallObserver(
            TakeScreenshotOnErrorStrategy sut,
            IActor actor,
            string expectedName,
            IObserver<ScreenshotInfo> observer)
        {
            // arrange
            var screenshot = new Screenshot("abcdefgskdjf");
            var expected = new ScreenshotInfo(screenshot, expectedName);
            var webDriver = new Mock<IWebDriver>();
            webDriver.As<ITakesScreenshot>().Setup(t => t.GetScreenshot()).Returns(screenshot);
            var webBrowser = WebBrowser.With(webDriver.Object);
            // act
            Assert.Throws<Exception>(() => sut.ExecuteTakeScreenshot(webBrowser, actor, Throws, () => expectedName, observer));
            // assert
            Mock.Get(observer).Verify(CallOnNext(expected), Times.Once());

            object Throws() => throw new Exception();
        }

        [Theory, DomainAutoData]
        public void ExecuteTakeScreenshot_WhenAbilityIsNotWebBrowser_AndExecuteThrows_ShouldCallObserver(
            TakeScreenshotOnErrorStrategy sut,
            IActor actor,
            string expectedName,
            IObserver<ScreenshotInfo> observer)
        {
            // arrange
            var screenshot = new Screenshot("abcdefgskdjf");
            var expected = new ScreenshotInfo(screenshot, expectedName);
            var webDriver = new Mock<IWebDriver>();
            webDriver.As<ITakesScreenshot>().Setup(t => t.GetScreenshot()).Returns(screenshot);
            var webBrowser = WebBrowser.With(webDriver.Object);
            Mock.Get(actor).Setup(a => a.AsksFor(It.IsAny<IQuestion<WebBrowser>>()))
                           .Returns<IQuestion<WebBrowser>>(q => q.AnsweredBy(actor));
#pragma warning disable CS0618 // Type or member is obsolete
            Mock.Get(actor).Setup(a => a.AsksFor(It.IsAny<IQuestion<WebBrowser, WebBrowser>>()))
                           .Returns<IQuestion<WebBrowser, WebBrowser>>(q => q.AnsweredBy(actor, webBrowser));
#pragma warning restore CS0618 // Type or member is obsolete
            // act
            Assert.Throws<Exception>(() => sut.ExecuteTakeScreenshot(new object(), actor, Throws, () => expectedName, observer));
            // assert
            Mock.Get(observer).Verify(CallOnNext(expected), Times.Once());

            object Throws() => throw new Exception();
        }

        private Expression<System.Action<IObserver<ScreenshotInfo>>> CallOnNext(ScreenshotInfo expected)
        {
            return o => o.OnNext(It.Is<ScreenshotInfo>(s => s.FileName == expected.FileName &&
                                                            s.Screenshot.AsBase64EncodedString == expected.Screenshot.AsBase64EncodedString));
        }
    }
}

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
    public class AlwaysTakeScreenshotStrategyTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AlwaysTakeScreenshotStrategy));
        }

        [Theory, DomainAutoData]
        public void ExecuteTakeScreenshot_ShouldReturnCorrectValue(
            AlwaysTakeScreenshotStrategy sut,
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
        public void ExecuteTakeScreenshot_WhenAbilityIsWebBrowser_ShouldCallObserver(
            AlwaysTakeScreenshotStrategy sut,
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
            sut.ExecuteTakeScreenshot(webBrowser, actor, () => new object(), () => expectedName, observer);
            // assert
            Mock.Get(observer).Verify(CallOnNext(expected), Times.Once());
        }

        [Theory, DomainAutoData]
        public void ExecuteTakeScreenshot_WhenAbilityIsWebBrowser_AndExecuteThrows_ShouldCallObserver(
            AlwaysTakeScreenshotStrategy sut,
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
        public void ExecuteTakeScreenshot_WhenAbilityIsNotWebBrowser_ShouldNotCallObserver(
            AlwaysTakeScreenshotStrategy sut,
            IActor actor,
            string name,
            IObserver<ScreenshotInfo> observer)
        {
            // arrange            
            // act
            sut.ExecuteTakeScreenshot(new object(), actor, () => new object(), () => name, observer);
            // assert
            Mock.Get(observer).Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
        }

        private Expression<System.Action<IObserver<ScreenshotInfo>>> CallOnNext(ScreenshotInfo expected)
        {
            return o => o.OnNext(It.Is<ScreenshotInfo>(s => s.FileName == expected.FileName &&
                                                            s.Screenshot.AsBase64EncodedString == expected.Screenshot.AsBase64EncodedString));
        }
    }
}

using System;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture.Idioms;
using Moq;
using OpenQA.Selenium;
using Tranquire;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Extensions
{
    public class TakeSampleScreenshotsStrategyTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(AlwaysTakeScreenshotStrategy));
        }

        [Theory, DomainAutoData]
        public void ExecuteTakeScreenshot_ShouldReturnCorrectValue(
            TakeSampleScreenshotsStrategy sut,
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

        [Theory]
        [DomainInlineAutoData(1, 3, new int[] { })]
        [DomainInlineAutoData(2, 3, new int[] { })]
        [DomainInlineAutoData(3, 3, new int[] { 3 })]
        [DomainInlineAutoData(4, 3, new int[] { 3 })]
        [DomainInlineAutoData(5, 3, new int[] { 3 })]
        [DomainInlineAutoData(6, 3, new int[] { 3, 6 })]
        [DomainInlineAutoData(11, 5, new int[] { 5, 10 })]
        public void ExecuteTakeScreenshot_WhenAbilityIsWebBrowser_ShouldCallObserver(
            int callCount,
            int sampleSize,
            int[] expectedCalls,
            IActor actor,
            string expectedName,
            IObserver<ScreenshotInfo> observer)
        {
            // arrange
            var sut = new TakeSampleScreenshotsStrategy(sampleSize);
            var screenshot = new Screenshot("abcdefgskdjf");
            var webDriver = new Mock<IWebDriver>();
            webDriver.As<ITakesScreenshot>().Setup(t => t.GetScreenshot()).Returns(screenshot);
            var webBrowser = WebBrowser.With(webDriver.Object);
            // act
            for (var i = 1; i <= callCount; i++)
            {
                sut.ExecuteTakeScreenshot(webBrowser, actor, () => new object(), () => expectedName + i.ToString(), observer);
            }
            // assert
            if (expectedCalls.Length == 0)
            {
                Mock.Get(observer).Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
            }
            else
            {
                foreach (var i in expectedCalls)
                {
                    var expected = new ScreenshotInfo(screenshot, expectedName + i.ToString());
                    Mock.Get(observer).Verify(CallOnNext(expected), Times.Once());
                }
            }
        }

        [Theory]
        [DomainInlineAutoData(new bool[] { true, false, true }, 3, new int[] { })]
        [DomainInlineAutoData(new bool[] { true, false, true, true }, 3, new int[] { 4 })]
        [DomainInlineAutoData(new bool[] { false, false, false }, 3, new int[] { })]
        public void ExecuteTakeScreenshot_WhenAbilityIsNotWebBrowser_ShouldNotCallObserver(
            bool[] useWebBrowsers,
            int sampleSize,
            int[] expectedCalls,
            IActor actor,
            string expectedName,
            IObserver<ScreenshotInfo> observer)
        {
            // arrange
            var sut = new TakeSampleScreenshotsStrategy(sampleSize);
            var screenshot = new Screenshot("abcdefgskdjf");
            var webDriver = new Mock<IWebDriver>();
            webDriver.As<ITakesScreenshot>().Setup(t => t.GetScreenshot()).Returns(screenshot);
            var webBrowser = WebBrowser.With(webDriver.Object);
            // act
            foreach (var (useWebBrowser, i) in useWebBrowsers.Select((useWebBrowser, i) => (useWebBrowser, i + 1)))
            {
                var ability = useWebBrowser ? webBrowser : new object();
                sut.ExecuteTakeScreenshot(ability, actor, () => new object(), () => expectedName + i.ToString(), observer);
            }
            // assert
            if (expectedCalls.Length == 0)
            {
                Mock.Get(observer).Verify(o => o.OnNext(It.IsAny<ScreenshotInfo>()), Times.Never());
            }
            else
            {
                foreach (var i in expectedCalls)
                {
                    var expected = new ScreenshotInfo(screenshot, expectedName + i.ToString());
                    Mock.Get(observer).Verify(CallOnNext(expected), Times.Once());
                }
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteTakeScreenshot_WhenAbilityIsWebBrowser_AndExecuteThrows_ShouldCallObserver(
            TakeSampleScreenshotsStrategy sut,
            IActor actor,
            string expectedName,
            IObserver<ScreenshotInfo> observer)
        {
            // arrange
            var screenshot = new Screenshot("abcdefgskdjf");
            var webDriver = new Mock<IWebDriver>();
            webDriver.As<ITakesScreenshot>().Setup(t => t.GetScreenshot()).Returns(screenshot);
            var webBrowser = WebBrowser.With(webDriver.Object);
            // act
            Assert.Throws<Exception>(() => sut.ExecuteTakeScreenshot(webBrowser, actor, Throws, () => expectedName, observer));
            // assert            
            var expected = new ScreenshotInfo(screenshot, expectedName);
            Mock.Get(observer).Verify(CallOnNext(expected), Times.Once());

            object Throws()
            {
                throw new Exception();
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteTakeScreenshot_WhenAbilityIsNotWebBrowser_AndExecuteThrows_ShouldCallObserver(
            TakeSampleScreenshotsStrategy sut,
            IActor actor,
            string expectedName,
            IObserver<ScreenshotInfo> observer)
        {
            // arrange
            var screenshot = new Screenshot("abcdefgskdjf");
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
            var expected = new ScreenshotInfo(screenshot, expectedName);
            Mock.Get(observer).Verify(CallOnNext(expected), Times.Once());

            object Throws()
            {
                throw new Exception();
            }
        }

        private Expression<System.Action<IObserver<ScreenshotInfo>>> CallOnNext(ScreenshotInfo expected)
        {
            return o => o.OnNext(It.Is<ScreenshotInfo>(s => s.FileName == expected.FileName &&
                                                            s.Screenshot.AsBase64EncodedString == expected.Screenshot.AsBase64EncodedString));
        }
    }
}

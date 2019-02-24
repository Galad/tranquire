using System;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// Take a screenshot by sampling based on the number of actions that were executed
    /// </summary>
    public sealed class TakeSampleScreenshotsStrategy : ITakeScreenshotStrategy
    {
        private readonly int _sampleSize;
        private int _count = 0;

        /// <summary>
        /// Creates a new instance of <see cref="TakeSampleScreenshotsStrategy"/>
        /// </summary>
        /// <param name="sampleSize">The number of executed actions in the sample before taking a screenshot</param>
        public TakeSampleScreenshotsStrategy(int sampleSize = 5)
        {
            _sampleSize = sampleSize;
        }

        /// <inheritdoc />
        public TResult ExecuteTakeScreenshot<TResult, TAbility>(TAbility actionAbility,
                                                                IActor actor,
                                                                Func<TResult> execute,
                                                                Func<string> nextScreenshotName,
                                                                IObserver<ScreenshotInfo> screenshotObserver)
        {
            bool canTakeScreenshot = true;
            if (!(actionAbility is WebBrowser webBrowser))
            {
                webBrowser = actor.AsksFor(Tranquire.Questions.Create(TakeScreenshotOnErrorStrategy.GetWebBrowserQuestionName, (IActor a, WebBrowser w) => w));
                canTakeScreenshot = false;
            }

            try
            {
                if (canTakeScreenshot)
                {
                    _count++;
                }
                var result = execute();
                if (canTakeScreenshot && _count == _sampleSize)
                {
                    TakeScreenshot();
                }
                return result;
            }
            catch (Exception)
            {
                TakeScreenshot();
                throw;
            }

            void TakeScreenshot()
            {
                var name = nextScreenshotName();
                var screenshot = ((ITakesScreenshot)webBrowser.Driver).GetScreenshot();
                screenshotObserver.OnNext(new ScreenshotInfo(screenshot, name));
                _count = 0;
            }
        }
    }
}

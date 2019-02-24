using System;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// Take a screenshot when every action is executed
    /// </summary>
    public sealed class AlwaysTakeScreenshotStrategy : ITakeScreenshotStrategy
    {
        /// <inheritdoc />
        public TResult ExecuteTakeScreenshot<TResult, TAbility>(TAbility actionAbility,
                                                                IActor actor,
                                                                Func<TResult> execute,
                                                                Func<string> nextScreenshotName,
                                                                IObserver<ScreenshotInfo> screenshotObserver)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            if (nextScreenshotName == null)
            {
                throw new ArgumentNullException(nameof(nextScreenshotName));
            }
            if (screenshotObserver == null)
            {
                throw new ArgumentNullException(nameof(screenshotObserver));
            }
            
            if (!(actionAbility is WebBrowser webBrowser))
            {
                return execute();
            }

            try
            {
                return execute();    
            }
            finally
            {
                var name = nextScreenshotName();
                var screenshot = ((ITakesScreenshot)webBrowser.Driver).GetScreenshot();
                screenshotObserver.OnNext(new ScreenshotInfo(screenshot, name));
            }
        }
    }
}

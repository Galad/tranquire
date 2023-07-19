using System;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Extensions;

/// <summary>
/// Take a screenshot only when an error occurs on an action
/// </summary>
public sealed class TakeScreenshotOnErrorStrategy : ITakeScreenshotStrategy
{
    /// <summary>
    /// The name of the question that returns the web browser ability
    /// </summary>
    public const string GetWebBrowserQuestionName = "A13D76CB-5102-4482-958B-2A6D81CE4AF9";

    /// <inheritdoc />
    public TResult ExecuteTakeScreenshot<TResult, TAbility>(TAbility actionAbility,
                                                            IActor actor,
                                                            Func<TResult> execute,
                                                            Func<string> nextScreenshotName,
                                                            IObserver<ScreenshotInfo> screenshotObserver)
    {
        if (!(actionAbility is WebBrowser webBrowser))
        {
            webBrowser = actor.AsksFor(Tranquire.Questions.Create(GetWebBrowserQuestionName, (IActor a, WebBrowser w) => w));
        }

        try
        {
            return execute();
        }
        catch (Exception)
        {
            var name = nextScreenshotName();
            var screenshot = ((ITakesScreenshot)webBrowser.Driver).GetScreenshot();
            screenshotObserver.OnNext(new ScreenshotInfo(screenshot, name));
            throw;
        }
    }
}

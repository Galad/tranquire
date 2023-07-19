using System;
using Tranquire.Selenium.Extensions;

namespace Tranquire.Selenium;

/// <summary>
/// Define the behavior of taking a screnshot
/// </summary>
public interface ITakeScreenshotStrategy
{
    /// <summary>
    /// Execute the action and take a screenshot.
    /// </summary>
    /// <typeparam name="TResult">The result of the action</typeparam>
    /// <typeparam name="TAbility">The ability type of the current action</typeparam>
    /// <param name="actionAbility">The ability of the current action</param>
    /// <param name="actor">The actor</param>
    /// <param name="execute">A function that execute the action</param>
    /// <param name="nextScreenshotName">A function that returns the next screenshot name</param>
    /// <param name="screenshotObserver">An observer that is used to notify that a screenshot has been taken</param>
    /// <returns></returns>
    TResult ExecuteTakeScreenshot<TResult, TAbility>(TAbility actionAbility,
                                                     IActor actor,
                                                     Func<TResult> execute,
                                                     Func<string> nextScreenshotName,
                                                     IObserver<ScreenshotInfo> screenshotObserver);
}

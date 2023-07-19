using System;
using System.Collections.Immutable;
using Tranquire.Reporting;
using Tranquire.Selenium.Extensions;

namespace Tranquire.Selenium;

/// <summary>
/// Represents the configuration of the selenium reporting
/// </summary>
public sealed class SeleniumReportingConfiguration
{
    private readonly ICanNotify _canNotify;
    /// <summary>
    /// Get the <see cref="ICanNotify"/> depe
    /// </summary>
    public static ICanNotify DefaultCanNotify { get; } = new CannotNotifyGetWebDriver();

    private sealed class CannotNotifyGetWebDriver : ICanNotify
    {
        public bool Action<TResult>(IAction<TResult> action) => true;

        public bool Question<TResult>(IQuestion<TResult> question)
        {
            return question == null ||
                   question.Name != TakeScreenshotOnErrorStrategy.GetWebBrowserQuestionName;
        }
    }

    /// <summary>
    /// Creates a new instance of <see cref="SeleniumReportingConfiguration"/>
    /// </summary>
    /// <param name="screenshotDirectory">The directory where the screenshots are saved</param>
    /// <param name="screenshotNameOrFormat">A string containing a format for 0 or 1 format item, used to generate the screenshot names. If no format item is provided, the default format is "<paramref name="screenshotNameOrFormat"/>_{0:00}".</param>
    public SeleniumReportingConfiguration(string screenshotDirectory,
                                          string screenshotNameOrFormat) : this(screenshotDirectory,
                                                                                screenshotNameOrFormat,
                                                                                null,
                                                                                ImmutableArray<IObserver<string>>.Empty,
                                                                                new AlwaysTakeScreenshotStrategy(),
                                                                                ScreenshotFormat.Jpeg)
    {
    }

    private SeleniumReportingConfiguration(string screenshotDirectory,
                                           string screenshotNameOrFormat,
                                           ICanNotify canNotify,
                                           ImmutableArray<IObserver<string>> textOutputObservers,
                                           ITakeScreenshotStrategy takeScreenshotStrategy,
                                           ScreenshotFormat screenshotFormat)
    {
        if (string.IsNullOrEmpty(screenshotDirectory))
        {
            throw new ArgumentNullException(nameof(screenshotDirectory));
        }
        if (string.IsNullOrEmpty(screenshotNameOrFormat))
        {
            throw new ArgumentNullException(nameof(screenshotNameOrFormat));
        }
        ScreenshotDirectory = screenshotDirectory;
        ScreenshotNameOrFormat = screenshotNameOrFormat;
        _canNotify = canNotify;
        TextOutputObservers = textOutputObservers;
        TakeScreenshotStrategy = takeScreenshotStrategy;
        ScreenshotFormat = screenshotFormat ?? throw new ArgumentNullException(nameof(screenshotFormat));
    }

    /// <summary>
    /// Gets the directory when the screenshots are saved
    /// </summary>
    public string ScreenshotDirectory { get; }
    /// <summary>
    /// Gets the screenshot name or format
    /// </summary>
    public string ScreenshotNameOrFormat { get; }
    internal ImmutableArray<IObserver<string>> TextOutputObservers { get; }
    internal ITakeScreenshotStrategy TakeScreenshotStrategy { get; }
    internal ScreenshotFormat ScreenshotFormat { get; }

    internal Actor ApplyWithReporting(Actor actor, IObserver<ActionNotification> observer)
    {
        var canNotify = _canNotify == null ? DefaultCanNotify : new CompositeCanNotify(_canNotify, DefaultCanNotify);
        return actor.WithReporting(observer, canNotify);
    }

    /// <summary>
    /// Use the give <see cref="ICanNotify"/> instance to determine if an action can trigger a notification
    /// </summary>
    /// <param name="canNotify">A <see cref="ICanNotify"/> instance that allows to control what actions and questions can send a notification</param>
    /// <returns></returns>
    public SeleniumReportingConfiguration WithCanNotify(ICanNotify canNotify)
    {
        if (canNotify == null)
        {
            throw new ArgumentNullException(nameof(canNotify));
        }

        return new SeleniumReportingConfiguration(ScreenshotDirectory,
                                                  ScreenshotNameOrFormat,
                                                  canNotify,
                                                  TextOutputObservers,
                                                  TakeScreenshotStrategy,
                                                  ScreenshotFormat);
    }

    /// <summary>
    /// Adds the given text observers to the configuration
    /// </summary>
    /// <param name="textOutputObservers">Additional observer that can be used to display the text report</param>
    /// <returns></returns>
    public SeleniumReportingConfiguration AddTextObservers(params IObserver<string>[] textOutputObservers)
    {
        if (textOutputObservers == null)
        {
            throw new ArgumentNullException(nameof(textOutputObservers));
        }

        return new SeleniumReportingConfiguration(ScreenshotDirectory,
                                                  ScreenshotNameOrFormat,
                                                  _canNotify,
                                                  TextOutputObservers.AddRange(textOutputObservers),
                                                  TakeScreenshotStrategy,
                                                  ScreenshotFormat);
    }

    /// <summary>
    /// Use the given strategy to take screenshots
    /// </summary>        
    public SeleniumReportingConfiguration WithTakeScreenshotStrategy(ITakeScreenshotStrategy takeScreenshotStrategy)
    {
        return new SeleniumReportingConfiguration(ScreenshotDirectory,
                                                  ScreenshotNameOrFormat,
                                                  _canNotify,
                                                  TextOutputObservers,
                                                  takeScreenshotStrategy,
                                                  ScreenshotFormat);
    }

    /// <summary>
    /// Save the screeshots in the specified format
    /// </summary>
    /// <param name="screenshotFormat">The format</param>
    /// <returns></returns>
    public SeleniumReportingConfiguration WithScreenshotFormat(ScreenshotFormat screenshotFormat)
    {
        return new SeleniumReportingConfiguration(ScreenshotDirectory,
                                                  ScreenshotNameOrFormat,
                                                  _canNotify,
                                                  TextOutputObservers,
                                                  TakeScreenshotStrategy,
                                                  screenshotFormat);
    }
}
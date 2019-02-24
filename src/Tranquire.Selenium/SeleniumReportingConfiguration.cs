using System;
using System.Collections.Immutable;
using Tranquire.Reporting;
using Tranquire.Selenium.Extensions;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Represents the configuration of the selenium reporting
    /// </summary>
    public sealed class SeleniumReportingConfiguration
    {
        private readonly ICanNotify _canNotify;

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
                                                                                    new AlwaysTakeScreenshotStrategy())
        {            
        }

        private SeleniumReportingConfiguration(string screenshotDirectory,
                                               string screenshotNameOrFormat,
                                               ICanNotify canNotify,
                                               ImmutableArray<IObserver<string>> textOutputObservers,
                                               ITakeScreenshotStrategy takeScreenshotStrategy)
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

        internal Actor ApplyWithReporting(Actor actor, IObserver<ActionNotification> observer)
        {
            if (_canNotify == null)
            {
                return actor.WithReporting(observer);
            }
            return actor.WithReporting(observer, _canNotify);
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
                                                      TakeScreenshotStrategy);
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
                                                      TakeScreenshotStrategy);
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
                                                      takeScreenshotStrategy);
        }
    }
}

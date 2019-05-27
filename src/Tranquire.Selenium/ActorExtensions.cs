using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Tranquire.Reporting;
using Tranquire.Selenium.Extensions;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Extension methods for the <see cref="Actor"/> class
    /// </summary>
    public static class ActorExtensions
    {
        /// <summary>
        /// Add the ability to highligh a target in the web browser on each action
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <returns>An new actor</returns>
        public static Actor HighlightTargets(this Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            return new Actor(actor.Name, actor.Abilities, a => new HighlightTarget(actor.InnerActorBuilder(a)));
        }

        /// <summary>
        /// Add the ability to slow down the execution of actions using Selenium
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="delay">The wait time. A delay is observerd before and after the action is performed.</param>
        /// <returns>An new actor</returns>
        public static Actor SlowSelenium(this Actor actor, TimeSpan delay)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            return new Actor(actor.Name, actor.Abilities, a => new SlowSelenium(actor.InnerActorBuilder(a), delay));
        }

        /// <summary>
        /// Add the ability to take screenshot on each action
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="directory">The directory where the screenshots will be saved</param>
        /// <param name="screenshotNameOrFormat">A string containing a format for 0 or 1 format item, used to generate the screenshot names. If no format item is provided, the default format is "<paramref name="screenshotNameOrFormat"/>_{0:00}".</param>
        /// <example>
        /// actor.TakeScreenshots(@"C:\tests\screeshots", "Screenhshot_{0:000}")
        /// </example>
        /// <returns>An new actor taking screenshots</returns>
        public static Actor TakeScreenshots(this Actor actor, string directory, string screenshotNameOrFormat)
        {
            return actor.TakeScreenshots(screenshotNameOrFormat, new SaveScreenshotsToFileOnNext(directory, ScreenshotFormat.Jpeg));
        }

        /// <summary>
        /// Add the ability to take screenshot on each action. The screenshots are saved using the given <see cref="ScreenshotInfo"/> observer.
        /// </summary>
        /// <param name="actor">The actor</param>        
        /// <param name="screenshotNameOrFormat">A string containing a format for 0 or 1 format item, used to generate the screenshot names. If no format item is provided, the default format is "<paramref name="screenshotNameOrFormat"/>_{0:00}".</param>
        /// <param name="screenshotInfoObserver">An observer that is notify that screenshots were taken, and can be used to save them on the disk.
        /// Use <see cref="SaveScreenshotsToFileOnComplete"/> or <see cref="SaveScreenshotsToFileOnNext"/></param>        
        /// <example>
        /// actor.TakeScreenshots("Screenhshot_{0:000}", new SaveScreenshotsToFileOnComplete(@"C:\tests\screeshots"));
        /// </example>
        /// <returns>An new actor taking screenshots</returns>
        public static Actor TakeScreenshots(
            this Actor actor,
            string screenshotNameOrFormat,
            IObserver<ScreenshotInfo> screenshotInfoObserver)
        {
            return actor.TakeScreenshots(screenshotNameOrFormat, screenshotInfoObserver, new AlwaysTakeScreenshotStrategy());
        }


        /// <summary>
        /// Add the ability to take screenshot on each action. The screenshots are saved using the given <see cref="ScreenshotInfo"/> observer.
        /// </summary>
        /// <param name="actor">The actor</param>        
        /// <param name="screenshotNameOrFormat">A string containing a format for 0 or 1 format item, used to generate the screenshot names. If no format item is provided, the default format is "<paramref name="screenshotNameOrFormat"/>_{0:00}".</param>
        /// <param name="screenshotInfoObserver">An observer that is notify that screenshots were taken, and can be used to save them on the disk.
        /// Use <see cref="SaveScreenshotsToFileOnComplete"/> or <see cref="SaveScreenshotsToFileOnNext"/></param>
        /// <param name="takeScreenshotStrategy">The strategy used to create a screenshot</param>
        /// <example>
        /// actor.TakeScreenshots("Screenhshot_{0:000}", new SaveScreenshotsToFileOnComplete(@"C:\tests\screeshots"));
        /// </example>
        /// <returns>An new actor taking screenshots</returns>
        public static Actor TakeScreenshots(
            this Actor actor,
            string screenshotNameOrFormat,
            IObserver<ScreenshotInfo> screenshotInfoObserver,
            ITakeScreenshotStrategy takeScreenshotStrategy)
        {
            if (screenshotInfoObserver == null)
            {
                throw new ArgumentNullException(nameof(screenshotInfoObserver));
            }
            if (takeScreenshotStrategy == null)
            {
                throw new ArgumentNullException(nameof(takeScreenshotStrategy));
            }
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (screenshotNameOrFormat == null)
            {
                throw new ArgumentNullException(nameof(screenshotNameOrFormat));
            }
            
            var id = 0;

            string screenshotFormat;
            if (string.Format(screenshotNameOrFormat, 1) == screenshotNameOrFormat)
            {
                screenshotFormat = screenshotNameOrFormat + "_" + "{0:00}";
            }
            else
            {
                screenshotFormat = screenshotNameOrFormat;
            }

            string nextScreenshotName()
            {
                return string.Format(CultureInfo.InvariantCulture, screenshotFormat, Interlocked.Increment(ref id));
            }
            return new Actor(actor.Name, actor.Abilities, a => new TakeScreenshot(actor.InnerActorBuilder(a),
                                                                                  nextScreenshotName,
                                                                                  screenshotInfoObserver,
                                                                                  takeScreenshotStrategy));
        }

        /// <summary>
        /// Configure the actor for Selenium reporting and returns an object that allows to retrieve the report. This is similar to using:
        /// - <see cref="Tranquire.ActorExtensions.WithReporting(Actor, IObserver{Reporting.ActionNotification}, ICanNotify)"/>
        /// - <see cref="TakeScreenshots(Actor, string, IObserver{ScreenshotInfo}, ITakeScreenshotStrategy)"/>
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="configuration">The reporting configuration</param>
        /// <param name="seleniumReporter">A <see cref="ISeleniumReporter"/> object that can be used to save the screenshots and retrieve the report at the end of the run</param>        
        /// <returns></returns>
        public static Actor WithSeleniumReporting(
            this Actor actor,
            SeleniumReportingConfiguration configuration,
            out ISeleniumReporter seleniumReporter)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var xmlDocumentObserver = new XmlDocumentObserver();
            var textObservers = new CompositeObserver<string>(configuration.TextOutputObservers.ToArray());
            var reportingObserver = new CompositeObserver<ActionNotification>(
                xmlDocumentObserver,
                new RenderedReportingObserver(textObservers, RenderedReportingObserver.DefaultRenderer)
                );
            var saveScreenshotObserver = new SaveScreenshotsToFileOnComplete(configuration.ScreenshotDirectory, configuration.ScreenshotFormat);
            var screenshotObserver = new CompositeObserver<ScreenshotInfo>(
                saveScreenshotObserver,
                new ScreenshotInfoToActionAttachmentObserverAdapter(xmlDocumentObserver, configuration.ScreenshotFormat),
                new RenderedScreenshotInfoObserver(textObservers, configuration.ScreenshotFormat)
                );
            seleniumReporter = new SeleniumReporter(xmlDocumentObserver, saveScreenshotObserver);
            return configuration.ApplyWithReporting(actor, reportingObserver)
                                .TakeScreenshots(configuration.ScreenshotNameOrFormat,
                                                 screenshotObserver,
                                                 configuration.TakeScreenshotStrategy);
        }
    }
}

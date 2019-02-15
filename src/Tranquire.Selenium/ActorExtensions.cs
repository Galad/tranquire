using System;
using System.Globalization;
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
            return actor.TakeScreenshots(screenshotNameOrFormat, new SaveScreenshotsToFileOnNext(directory));
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
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            if (screenshotNameOrFormat == null)
            {
                throw new ArgumentNullException(nameof(screenshotNameOrFormat));
            }
            if (screenshotInfoObserver == null)
            {
                throw new ArgumentNullException(nameof(screenshotInfoObserver));
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
            return new Actor(actor.Name, actor.Abilities, a => new TakeScreenshot(actor.InnerActorBuilder(a), nextScreenshotName, screenshotInfoObserver));
        }

        /// <summary>
        /// Configure the actor for Selenium reporting and returns an object that allows to retrieve the report. This is similar to using:
        /// - <see cref="Tranquire.ActorExtensions.WithReporting(Actor, IObserver{Reporting.ActionNotification})"/>
        /// - <see cref="TakeScreenshots(Actor, string, IObserver{ScreenshotInfo})"/>
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="screenshotDirectory">The directory where the screenshots are saved</param>
        /// <param name="screenshotNameOrFormat">A string containing a format for 0 or 1 format item, used to generate the screenshot names. If no format item is provided, the default format is "<paramref name="screenshotNameOrFormat"/>_{0:00}".</param>
        /// <param name="seleniumReporter">A <see cref="ISeleniumReporter"/> object that can be used to save the screenshots and retrieve the report at the end of the run</param>
        /// <param name="textOutputObservers">Additional observer that can be used to display the text report</param>
        /// <returns></returns>
        public static Actor WithSeleniumReporting(
            this Actor actor,
            string screenshotDirectory,
            string screenshotNameOrFormat,
            out ISeleniumReporter seleniumReporter,
            params IObserver<string>[] textOutputObservers)
        {
            return WithSeleniumReportingInternal(
                actor, 
                screenshotDirectory, 
                screenshotNameOrFormat, 
                out seleniumReporter, 
                textOutputObservers,
                (a, o) => a.WithReporting(o));
        }

        /// <summary>
        /// Configure the actor for Selenium reporting and returns an object that allows to retrieve the report. This is similar to using:
        /// - <see cref="Tranquire.ActorExtensions.WithReporting(Actor, IObserver{Reporting.ActionNotification}, ICanNotify)"/>
        /// - <see cref="TakeScreenshots(Actor, string, IObserver{ScreenshotInfo})"/>
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="screenshotDirectory">The directory where the screenshots are saved</param>
        /// <param name="screenshotNameOrFormat">A string containing a format for 0 or 1 format item, used to generate the screenshot names. If no format item is provided, the default format is "<paramref name="screenshotNameOrFormat"/>_{0:00}".</param>
        /// <param name="canNotify">A <see cref="ICanNotify"/> instance that allows to control what actions and questions can send a notification</param>
        /// <param name="seleniumReporter">A <see cref="ISeleniumReporter"/> object that can be used to save the screenshots and retrieve the report at the end of the run</param>
        /// <param name="textOutputObservers">Additional observer that can be used to display the text report</param>
        /// <returns></returns>
        public static Actor WithSeleniumReporting(
            this Actor actor,
            string screenshotDirectory,
            string screenshotNameOrFormat,
            ICanNotify canNotify,
            out ISeleniumReporter seleniumReporter,
            params IObserver<string>[] textOutputObservers)
        {
            return WithSeleniumReportingInternal(
                actor,
                screenshotDirectory,
                screenshotNameOrFormat,
                out seleniumReporter,
                textOutputObservers,
                (a, o) => a.WithReporting(o, canNotify));
        }

        private static Actor WithSeleniumReportingInternal(
            Actor actor, 
            string screenshotDirectory, 
            string screenshotNameOrFormat, 
            out ISeleniumReporter seleniumReporter, 
            IObserver<string>[] textOutputObservers,
            Func<Actor, IObserver<ActionNotification>, Actor> applyWithReporting)
        {
            if (string.IsNullOrEmpty(screenshotDirectory))
            {
                throw new ArgumentNullException(nameof(screenshotDirectory));
            }
            if (string.IsNullOrEmpty(screenshotNameOrFormat))
            {
                throw new ArgumentNullException(nameof(screenshotNameOrFormat));
            }
            if (textOutputObservers == null)
            {
                throw new ArgumentNullException(nameof(textOutputObservers));
            }

            var xmlDocumentObserver = new XmlDocumentObserver();
            var textObservers = new CompositeObserver<string>(textOutputObservers);
            var reportingObserver = new CompositeObserver<ActionNotification>(
                xmlDocumentObserver,
                new RenderedReportingObserver(textObservers, RenderedReportingObserver.DefaultRenderer)
                );
            var saveScreenshotObserver = new SaveScreenshotsToFileOnComplete(screenshotDirectory);
            var screenshotObserver = new CompositeObserver<ScreenshotInfo>(
                saveScreenshotObserver,
                new ScreenshotInfoToActionAttachmentObserverAdapter(xmlDocumentObserver),
                new RenderedScreenshotInfoObserver(textObservers)
                );
            seleniumReporter = new SeleniumReporter(xmlDocumentObserver, saveScreenshotObserver);
            return applyWithReporting(actor, reportingObserver)
                        .TakeScreenshots(screenshotNameOrFormat, screenshotObserver);
        }
    }
}

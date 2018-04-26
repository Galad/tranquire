using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
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
            Guard.ForNull(actor, nameof(actor));
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
            Guard.ForNull(actor, nameof(actor));
            return new Actor(actor.Name, actor.Abilities, a => new SlowSelenium(actor.InnerActorBuilder(a), delay));
        }

        //private static readonly Regex hasFormatItem = new Regex("\{")
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

        public static Actor TakeScreenshots(
            this Actor actor,
            string screenshotNameOrFormat,
            IObserver<ScreenshotInfo> screenshotInfoObserver)
        {
            if (screenshotInfoObserver == null)
            {
                throw new ArgumentNullException(nameof(screenshotInfoObserver));
            }

            Guard.ForNull(actor, nameof(actor));            
            Guard.ForNull(screenshotNameOrFormat, nameof(screenshotNameOrFormat));
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
    }
}

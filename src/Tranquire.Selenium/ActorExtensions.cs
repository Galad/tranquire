using System;
using System.Threading;

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

        /// <summary>
        /// Add the ability to take screenshot on each action
        /// </summary>
        /// <param name="actor">The actor</param>
        /// <param name="directory">The directory where the screenshots will be saved</param>
        /// <param name="screenshotName">A function that generate a new name of the screenshot</param>
        /// <returns>An new actor taking screenshots</returns>
        public static Actor TakeScreenshots(this Actor actor, string directory, string screenshotName)
        {
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(directory, nameof(directory));
            Guard.ForNull(screenshotName, nameof(screenshotName));
            var id = 0;
            return new Actor(actor.Name, actor.Abilities, a => new TakeScreenshot(actor.InnerActorBuilder(a), directory, () => NextScreenshotName(ref id, screenshotName)));
        }

        private static string NextScreenshotName(ref int i, string screenshotName)
        {
            return $"{screenshotName}_{Interlocked.Increment(ref i)}.jpg";
        }
    }
}

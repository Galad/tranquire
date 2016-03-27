using System;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{    
    /// <summary>
    /// Configures the targets
    /// </summary>
    public class TargetBuilder
    {
        /// <summary>
        /// Gets the name of the target
        /// </summary>
        public string FriendlyName { get; }

        public TargetBuilder(string friendlyName)
        {
            this.FriendlyName = friendlyName ?? string.Empty;
        }

        /// <summary>
        /// Returns the target located with the given <see cref="By"/> object
        /// </summary>
        /// <param name="by">The location of the target</param>
        /// <returns></returns>
        public ITarget LocatedBy(By by)
        {
            Guard.ForNull(by, nameof(by));
            return new TargetBy(by);
        }

        /// <summary>
        /// Returns a target taking formatting parameters
        /// </summary>
        /// <param name="formatValue">The format used to locate the target</param>
        /// <param name="createBy">A function taking the formatted located as input and returning the <see cref="By"/> object</param>
        /// <returns></returns>
        public ITargetWithParameters LocatedBy(string formatValue, Func<string, By> createBy)
        {
            Guard.ForNull(createBy, nameof(createBy));
            Guard.ForNull(formatValue, nameof(formatValue));
            return new TargetByParameterizable(createBy, formatValue);
        }

        /// <summary>
        /// Returns <see cref="ITarget"/> from a <see cref="IWebElement"/>
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public ITarget LocatedByWebElement(IWebElement webElement)
        {
            Guard.ForNull(webElement, nameof(webElement));
            return new TargetByWebElement(webElement);
        }
    }
}
using OpenQA.Selenium;
using System.Globalization;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to ask question about wether the target is enabled
    /// </summary>
    public class Enabled : SingleUIState<bool, Enabled>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Enabled"/>
        /// </summary>
        /// <param name="target"></param>
        public Enabled(ITarget target) : this(target, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Enabled"/> with a culture
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public Enabled(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Asks questions about wether the target is enabled
        /// </summary>
        /// <param name="target">The target on which the questions are asked</param>
        /// <returns></returns>
        public static Enabled Of(ITarget target)
        {
            return new Enabled(target);
        }

        /// <summary>
        /// Creates a new <see cref="Enabled"/> instance
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override Enabled CreateState(ITarget target, CultureInfo culture)
        {
            return new Enabled(target, culture);
        }

        /// <summary>
        /// Returns wether the element is enabled
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override bool ResolveFor(IWebElement element)
        {
            return element.Enabled;
        }
    }
}

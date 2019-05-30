using System.Globalization;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to ask questions about the value of a select element
    /// </summary>
    public class Value : SingleUIState<string, Value>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Value"/>
        /// </summary>
        /// <param name="target"></param>
        public Value(ITarget target) : this(target, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Value"/> with a culture
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public Value(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Ask questions about the value of a select element
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Value Of(ITarget target)
        {
            return new Value(target);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Value"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override Value CreateState(ITarget target, CultureInfo culture)
        {
            return new Value(target, culture);
        }

        /// <summary>
        /// Returns the value of the given element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override string ResolveFor(IWebElement element)
        {
            return element.GetAttribute("value");
        }
    }
}

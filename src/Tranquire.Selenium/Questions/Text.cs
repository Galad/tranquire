using OpenQA.Selenium;
using System.Globalization;
using System;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to ask questions about the text of the given target
    /// </summary>
    public class Text : SingleUIState<string, Text>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Text"/>
        /// </summary>
        /// <param name="target"></param>
        public Text(ITarget target): base (target)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Text"/> with a culture
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public Text(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Retrieves the text of the given target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Text Of(ITarget target)
        {
            return new Text(target);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Text"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override Text CreateState(ITarget target, CultureInfo culture)
        {
            return new Text(target, culture);
        }

        /// <summary>
        /// Returns the text value of the given element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override string ResolveFor(IWebElement element)
        {
            return element.Text;
        }
    }
}
using OpenQA.Selenium;
using System.Globalization;
using System;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Retrieves the text of the given target
    /// </summary>
    public class Text : SingleUIState<string, Text>
    {
        public Text(ITarget target): base (target)
        {
        }

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

        protected override Text CreateState(ITarget target, CultureInfo culture)
        {
            return new Text(target, culture);
        }

        protected override string ResolveFor(IWebElement element)
        {
            return element.Text;
        }
    }
}
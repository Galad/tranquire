using OpenQA.Selenium;
using System.Globalization;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to ask questions about the text of the given target
    /// </summary>
    public class TextContent : SingleUIState<string, TextContent>
    {
        /// <summary>
        /// Creates a new instance of <see cref="TextContent"/>
        /// </summary>
        /// <param name="target"></param>
        public TextContent(ITarget target) : this(target, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="TextContent"/> with a culture
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public TextContent(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Retrieves the text of the given target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static TextContent Of(ITarget target)
        {
            return new TextContent(target);
        }

        /// <summary>
        /// Creates a new instance of <see cref="TextContent"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override TextContent CreateState(ITarget target, CultureInfo culture)
        {
            return new TextContent(target, culture);
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
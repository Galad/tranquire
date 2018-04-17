using OpenQA.Selenium;
using System;
using System.Collections.Immutable;
using System.Globalization;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to ask question about the CSS classes of a target
    /// </summary>
    public class Classes : SingleUIState<ImmutableArray<string>, Classes>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Classes"/>
        /// </summary>
        /// <param name="target"></param>
        public Classes(ITarget target) : this(target, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Classes"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public Classes(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Asks questions about the CSS classes of the given target
        /// </summary>
        /// <param name="target">The target on which the questions are asked</param>
        /// <returns></returns>
        public static Classes Of(ITarget target)
        {
            return new Classes(target);
        }

        /// <summary>
        /// Create a new instance of <see cref="Classes"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override Classes CreateState(ITarget target, CultureInfo culture)
        {
            return new Classes(target, culture);
        }

        /// <summary>
        /// Returns the classes for the element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override ImmutableArray<string> ResolveFor(IWebElement element)
        {
            return ImmutableArray.Create(element.GetAttribute("class").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}

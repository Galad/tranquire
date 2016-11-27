using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to ask question about the state of a <see cref="IWebElement"/>
    /// </summary>
    public class Element : SingleUIState<IWebElement, Element>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Element"/>
        /// </summary>
        /// <param name="target"></param>
        public Element(ITarget target) : base(target)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Element"/> with a culture
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public Element(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Ask questions about the state of a <see cref="IWebElement"/>
        /// </summary>
        /// <param name="target">The <see cref="ITarget"/> from which the element can be found</param>
        /// <returns></returns>
        public static Element Of(ITarget target)
        {
            return new Element(target);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Element"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override Element CreateState(ITarget target, CultureInfo culture)
        {
            return new Element(target, culture);
        }

        /// <summary>
        /// Returns the element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override IWebElement ResolveFor(IWebElement element)
        {
            return element;
        }

        /// <summary>
        /// Returns the action's name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"What is the element identified by {Target.ToString()} ?";
    }
}

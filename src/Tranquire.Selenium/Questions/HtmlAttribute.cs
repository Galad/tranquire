using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to ask questions about the attribute value of an element
    /// </summary>
    public class HtmlAttribute : SingleUIState<string, HtmlAttribute>
    {
        /// <summary>
        /// The attribute to look for
        /// </summary>
        public string AttributeName { get; }

        /// <summary>
        /// Creates a new instance of <see cref="HtmlAttribute"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="attributeName"></param>
        public HtmlAttribute(ITarget target, string attributeName) : base(target)
        {
            Guard.ForNullOrEmpty(attributeName, nameof(attributeName));
            AttributeName = attributeName;
        }

        /// <summary>
        /// Creates a new instance of <see cref="HtmlAttribute"/> with a culture
        /// </summary>
        /// <param name="target"></param>
        /// <param name="attributeName"></param>
        /// <param name="culture"></param>
        public HtmlAttribute(ITarget target, string attributeName, CultureInfo culture) : base(target, culture)
        {
            Guard.ForNullOrEmpty(attributeName, nameof(attributeName));
            AttributeName = attributeName;
        }

        /// <summary>
        /// Ask questions about attributes values
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static HtmlAttributeBuilder Of(ITarget target)
        {
            return new HtmlAttributeBuilder(target);
        }

        /// <summary>
        /// Creates a new instance of <see cref="HtmlAttribute"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override HtmlAttribute CreateState(ITarget target, CultureInfo culture)
        {
            return new HtmlAttribute(target, AttributeName, culture);
        }

        /// <summary>
        /// Returns the element attribute value
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override string ResolveFor(IWebElement element)
        {
            return element.GetAttribute(AttributeName);
        }

        /// <summary>
        /// Build a <see cref="HtmlAttribute "/> object
        /// </summary>
        public class HtmlAttributeBuilder
        {
            private readonly ITarget _target;

            /// <summary>
            /// Creates a new instance of <see cref="HtmlAttributeBuilder"/>
            /// </summary>
            /// <param name="target"></param>
            public HtmlAttributeBuilder(ITarget target)
            {
                _target = target;
            }

            /// <summary>
            /// Returns a <see cref="HtmlAttribute"/> object with the given attribute name
            /// </summary>
            /// <param name="attributeName"></param>
            /// <returns></returns>
            public HtmlAttribute Named(string attributeName)
            {
                return new HtmlAttribute(_target, attributeName);
            }
        }
    }
}

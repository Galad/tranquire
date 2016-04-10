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
    /// Allow to ask question about the CSS values of a target
    /// </summary>
    public class CssValue : SingleUIState<string, CssValue>
    {
        /// <summary>
        /// Gets the CSS property to look for
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Creates a new instance of <see cref="CssValue"/>
        /// </summary>
        /// <param name="propertyName">The CSS property to look for</param>
        /// <param name="target">The target on which the CSS property should be looked for</param>
        public CssValue(string propertyName, ITarget target) : base(target)
        {
            Guard.ForNullOrEmpty(propertyName, nameof(propertyName));
            PropertyName = propertyName;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CssValue"/> with a <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="propertyName">The CSS property to look for</param>
        /// <param name="target">The target on which the CSS property should be looked for</param>
        /// <param name="culture">The CultureInfo of the value</param>
        public CssValue(string propertyName, ITarget target, CultureInfo culture) : base(target, culture)
        {
            Guard.ForNullOrEmpty(propertyName, nameof(propertyName));
        }

        /// <summary>
        /// Asks question about the CSS values of a target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static CssValueBuilder Of(ITarget target)
        {
            return new CssValueBuilder(target);
        }

        /// <summary>
        /// Creates a new instance of <see cref="CssValue"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override CssValue CreateState(ITarget target, CultureInfo culture)
        {
            return new CssValue(PropertyName, target, culture);
        }

        /// <summary>
        /// Returns the property value of the given element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override string ResolveFor(IWebElement element)
        {
            return element.GetCssValue(PropertyName);
        }
    }

    /// <summary>
    /// A builder used to configure the <see cref="CssValue"/> class
    /// </summary>
    public class CssValueBuilder
    {
        private readonly ITarget _target;

        /// <summary>
        /// Creates a new instance of <see cref="CssValueBuilder"/>
        /// </summary>
        /// <param name="target"></param>
        public CssValueBuilder(ITarget target)
        {
            _target = target;
        }

        /// <summary>
        /// Specifies the property name to look for
        /// </summary>
        /// <param name="propertyName">The CSS property name</param>
        /// <returns>A new instance of <see cref="CssValue"/></returns>
        public CssValue AndTheProperty(string propertyName)
        {
            return new CssValue(propertyName, _target);
        }
    }
}

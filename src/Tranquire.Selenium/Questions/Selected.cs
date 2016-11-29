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
    /// Allow to ask question about wether an element is selected
    /// </summary>
    public class Selected : SingleUIState<bool, Selected>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Selected"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public Selected(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Selected"/>
        /// </summary>
        /// <param name="target"></param>
        public Selected(ITarget target) : this(target, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Selected"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override Selected CreateState(ITarget target, CultureInfo culture)
        {
            return new Selected(target, culture);
        }

        /// <summary>
        /// Ask questions about wether an element is selected
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Selected Of(ITarget target)
        {
            return new Selected(target);
        }

        /// <summary>
        /// Returns wether the element is selected
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override bool ResolveFor(IWebElement element)
        {
            return element.Selected;
        }
    }
}

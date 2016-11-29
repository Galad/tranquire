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
    /// Allow to ask questions about the visibility of an element
    /// </summary>
    public class Visibility : SingleUIState<bool, Visibility>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Visibility"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public Visibility(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Visibility"/> with a culture
        /// </summary>
        /// <param name="target"></param>        
        public Visibility(ITarget target) : this(target, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Visibility"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override Visibility CreateState(ITarget target, CultureInfo culture)
        {
            return new Visibility(target, culture);
        }

        /// <summary>
        /// Ask questions about the visibility of a select element
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Visibility Of(ITarget target)
        {
            return new Visibility(target);
        }

        /// <summary>
        /// Returns wether the element is visible
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override bool ResolveFor(IWebElement element)
        {
            return element.Displayed;
        }
    }
}

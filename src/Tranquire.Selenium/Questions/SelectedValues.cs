using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to ask questions about the selected values of a select element
    /// </summary>
    public class SelectedValues : SingleUIState<ImmutableArray<string>, SelectedValues>
    {
        /// <summary>
        /// Creates a new instance of <see cref="SelectedValues"/>
        /// </summary>
        /// <param name="target"></param>
        public SelectedValues(ITarget target) : this(target, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="SelectedValues"/> with a culture
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public SelectedValues(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Ask questions about the selected values of a select element
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static SelectedValues Of(ITarget target)
        {
            return new SelectedValues(target);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SelectedValues"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override SelectedValues CreateState(ITarget target, CultureInfo culture)
        {
            return new SelectedValues(target, culture);
        }

        /// <summary>
        /// Returns the selected values of the given element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override ImmutableArray<string> ResolveFor(IWebElement element)
        {
            var selected = new SelectElement(element);
            var values = selected.AllSelectedOptions.Select(w => w.GetAttribute("value")).ToImmutableArray();
            return values;
        }
    }
}

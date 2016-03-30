using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Immutable;

namespace Tranquire.Selenium.Questions
{
    public class SelectedValues : SingleUIState<ImmutableArray<string>, SelectedValues>
    {
        public SelectedValues(ITarget target) : base(target)
        {
        }

        public SelectedValues(ITarget target, CultureInfo culture) : base(target, culture)
        {            
        }

        public static SelectedValues Of(ITarget target)
        {
            return new SelectedValues(target);
        }

        protected override SelectedValues CreateState(ITarget target, CultureInfo culture)
        {
            return new SelectedValues(target, culture);
        }

        protected override ImmutableArray<string> ResolveFor(IWebElement element)
        {
            var selected = new SelectElement(element);
            var values = selected.AllSelectedOptions.Select(w => w.GetAttribute("value")).ToImmutableArray();
            return values;
        }
    }
}

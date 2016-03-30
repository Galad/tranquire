using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tranquire.Selenium.Questions
{
    public class SelectedValue : SingleUIState<string, SelectedValue>
    {
        public SelectedValue(ITarget target) : base(target)
        {
        }

        public SelectedValue(ITarget target, CultureInfo culture) : base(target, culture)
        {            
        }

        public static SelectedValue Of(ITarget target)
        {
            return new SelectedValue(target);
        }

        protected override SelectedValue CreateState(ITarget target, CultureInfo culture)
        {
            return new SelectedValue(target, culture);
        }

        protected override string ResolveFor(IWebElement element)
        {
            var selected = new SelectElement(element);
            if(selected.AllSelectedOptions.Count == 0)
            {
                return string.Empty;
            }
            return selected.SelectedOption.GetAttribute("value");
        }
    }
}

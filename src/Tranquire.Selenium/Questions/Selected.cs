using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    public class Selected : SingleUIState<bool, Selected>
    {
        public Selected(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        public Selected(ITarget target) : base(target)
        {
        }

        protected override Selected CreateState(ITarget target, CultureInfo culture)
        {
            return new Selected(target, culture);
        }

        public static Selected Of(ITarget target)
        {
            return new Selected(target);
        }

        protected override bool ResolveFor(IWebElement element)
        {
            return element.Selected;
        }
    }
}

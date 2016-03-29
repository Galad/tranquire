using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    public class Visibility : SingleUIState<bool, Visibility>
    {
        public Visibility(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        public Visibility(ITarget target) : base(target)
        {
        }

        protected override Visibility CreateState(ITarget target, CultureInfo culture)
        {
            return new Visibility(target, culture);
        }

        public static Visibility Of(ITarget target)
        {
            return new Visibility(target);
        }

        protected override bool ResolveFor(IWebElement element)
        {
            return element.Displayed;
        }
    }
}

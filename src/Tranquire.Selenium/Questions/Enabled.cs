using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    public class Enabled : SingleUIState<bool, Enabled>
    {
        public Enabled(ITarget target) : base(target)
        {
        }

        public Enabled(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        public static Enabled Of(ITarget target)
        {
            return new Enabled(target);
        }

        protected override Enabled CreateState(ITarget target, CultureInfo culture)
        {
            return new Enabled(target, culture);
        }

        protected override bool ResolveFor(IWebElement element)
        {
            return element.Enabled;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    public class Value : SingleUIState<string, Value>
    {
        public Value(ITarget target) : base(target)
        {
        }

        public Value(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        public static Value Of(ITarget target)
        {
            return new Value(target);
        }

        protected override Value CreateState(ITarget target, CultureInfo culture)
        {
            return new Value(target, culture);
        }

        protected override string ResolveFor(IWebElement element)
        {
            return element.GetAttribute("value");
        }
    }
}

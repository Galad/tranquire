using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public class Element : SingleUIState<IWebElement, Element>
    {
        public Element(ITarget target) : base(target)
        {
        }

        public Element(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        public static Element Of(ITarget target)
        {
            return new Element(target);
        }

        public override Element CreateState(ITarget target, CultureInfo culture)
        {
            return new Element(target, culture);
        }

        protected override IWebElement ResolveFor(IWebElement element)
        {
            return element;
        }
    }
}

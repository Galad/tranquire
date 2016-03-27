using OpenQA.Selenium;
using System.Globalization;
using System;

namespace Tranquire.Selenium.Questions
{
    public class Text : SingleUIState<string, Text>
    {
        public Text(ITarget target): base (target)
        {
        }

        public Text(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        public static Text Of(ITarget target)
        {
            return new Text(target);
        }

        public override Text CreateState(ITarget target, CultureInfo culture)
        {
            return new Text(target, culture);
        }

        protected override string ResolveFor(IWebElement element)
        {
            return element.Text;
        }
    }
}
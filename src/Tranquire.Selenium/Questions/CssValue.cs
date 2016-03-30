using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    public class CssValue : SingleUIState<string, CssValue>
    {
        public string PropertyName { get; }

        public CssValue(string propertyName, ITarget target) : base(target)
        {
            Guard.ForNullOrEmpty(propertyName, nameof(propertyName));
            PropertyName = propertyName;
        }

        public CssValue(string propertyName, ITarget target, CultureInfo culture) : base(target, culture)
        {
            Guard.ForNullOrEmpty(propertyName, nameof(propertyName));
        }

        public static CssValueBuilder Of(ITarget target)
        {
            return new CssValueBuilder(target);
        }

        protected override CssValue CreateState(ITarget target, CultureInfo culture)
        {
            return new CssValue(PropertyName, target, culture);
        }

        protected override string ResolveFor(IWebElement element)
        {
            return element.GetCssValue(PropertyName);
        }
    }

    public class CssValueBuilder
    {
        private readonly ITarget _target;

        public CssValueBuilder(ITarget target)
        {
            _target = target;
        }

        public CssValue AndTheProperty(string propertyName)
        {
            return new CssValue(propertyName, _target);
        }
    }
}

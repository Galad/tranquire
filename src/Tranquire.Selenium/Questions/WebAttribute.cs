using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    public class WebAttribute : SingleUIState<string, WebAttribute>
    {
        public string AttributeName { get; }

        public WebAttribute(ITarget target, string attributeName) : base(target)
        {
            Guard.ForNullOrEmpty(attributeName, nameof(attributeName));
            AttributeName = attributeName;
        }

        public WebAttribute(ITarget target, string attributeName, CultureInfo culture) : base(target, culture)
        {
            Guard.ForNullOrEmpty(attributeName, nameof(attributeName));
            AttributeName = attributeName;
        }

        public static WebAttributeBuilder Of(ITarget target)
        {
            return new WebAttributeBuilder(target);
        }

        protected override WebAttribute CreateState(ITarget target, CultureInfo culture)
        {
            return new WebAttribute(target, AttributeName, culture);
        }

        protected override string ResolveFor(IWebElement element)
        {
            return element.GetAttribute(AttributeName);
        }

        public class WebAttributeBuilder
        {
            private readonly ITarget _target;

            public WebAttributeBuilder(ITarget target)
            {
                _target = target;
            }

            public WebAttribute Named(string attributeName)
            {
                return new WebAttribute(_target, attributeName);
            }
        }
    }
}

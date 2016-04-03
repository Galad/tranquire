using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    public class HtmlAttribute : SingleUIState<string, HtmlAttribute>
    {
        public string AttributeName { get; }

        public HtmlAttribute(ITarget target, string attributeName) : base(target)
        {
            Guard.ForNullOrEmpty(attributeName, nameof(attributeName));
            AttributeName = attributeName;
        }

        public HtmlAttribute(ITarget target, string attributeName, CultureInfo culture) : base(target, culture)
        {
            Guard.ForNullOrEmpty(attributeName, nameof(attributeName));
            AttributeName = attributeName;
        }

        public static HtmlAttributeBuilder Of(ITarget target)
        {
            return new HtmlAttributeBuilder(target);
        }

        protected override HtmlAttribute CreateState(ITarget target, CultureInfo culture)
        {
            return new HtmlAttribute(target, AttributeName, culture);
        }

        protected override string ResolveFor(IWebElement element)
        {
            return element.GetAttribute(AttributeName);
        }

        public class HtmlAttributeBuilder
        {
            private readonly ITarget _target;

            public HtmlAttributeBuilder(ITarget target)
            {
                _target = target;
            }

            public HtmlAttribute Named(string attributeName)
            {
                return new HtmlAttribute(_target, attributeName);
            }
        }
    }
}

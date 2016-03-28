using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    public class Classes : SingleUIState<ImmutableArray<string>, Classes>
    {
        public Classes(ITarget target):base(target)
        {
        }

        public Classes(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        public static Classes Of(ITarget target)
        {
            return new Classes(target);
        }

        protected override Classes CreateState(ITarget target, CultureInfo culture)
        {
            return new Classes(target, culture);
        }

        protected override ImmutableArray<string> ResolveFor(IWebElement element)
        {
            return ImmutableArray.Create(element.GetAttribute("class").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}

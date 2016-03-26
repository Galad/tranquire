using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions.Converters;

namespace Tranquire.Selenium.Questions
{
    public class Element : SingleUIState<IWebElement>
    {
        private readonly IEnumerable<ICanConvert<IWebElement>> _customConverters;

        public Element(ITarget target) : this(target, Enumerable.Empty<ICanConvert<IWebElement>>())
        {
        }

        public Element(ITarget target, IEnumerable<ICanConvert<IWebElement>> customConverters) : base(target, new WebElementConverter(customConverters.ToArray()))
        {
            _customConverters = customConverters;
        }

        public static Element Of(ITarget target)
        {
            return new Element(target);
        }

        public Element WithCustomConverter(ICanConvert<IWebElement> converter)
        {
            return new Element(Target, _customConverters.Concat(new[] { converter }));
        }

        protected override IWebElement ResolveFor(IWebElement element)
        {
            return element;
        }
    }
}

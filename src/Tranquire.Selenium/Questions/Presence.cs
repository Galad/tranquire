using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Tranquire.Selenium.Questions
{
    public class Presence : SingleUIState<bool, Presence>
    {
        private Presence(ITarget target) : base(target)
        {
        }

        private Presence(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        public static Presence Of(ITarget target)
        {
            return new Presence(new SafeResolvedTarget(target));
        }

        protected override Presence CreateState(ITarget target, CultureInfo culture)
        {
            return new Presence(target, culture);
        }

        protected override bool ResolveFor(IWebElement element)
        {
            return element != NotFoundElement.Instance;
        }

        private class SafeResolvedTarget : ITarget
        {
            private readonly ITarget _target;

            public SafeResolvedTarget(ITarget target)
            {
                _target = target;
            }

            public string Name => _target.Name;

            public ITarget RelativeTo(ITarget targetSource)
            {
                return new SafeResolvedTarget(_target.RelativeTo(targetSource));
            }

            public IWebElement ResolveFor(IActor actor)
            {
                try
                {
                    return _target.ResolveFor(actor);
                }
                catch (NoSuchElementException)
                {
                    return NotFoundElement.Instance;
                }
            }

            public ImmutableArray<IWebElement> ResoveAllFor(IActor actor)
            {
                try
                {
                    return _target.ResoveAllFor(actor);
                }
                catch (NoSuchElementException)
                {
                    return ImmutableArray<IWebElement>.Empty;
                }
            }

            
        }

        private class NotFoundElement : IWebElement
        {
            public readonly static IWebElement Instance = new NotFoundElement();
            private NotFoundElement() { }
            public bool Displayed => false;
            public bool Enabled => false;
            public Point Location => Point.Empty;
            public bool Selected => false;
            public Size Size => Size.Empty;
            public string TagName => string.Empty;
            public string Text => string.Empty;

            public void Clear()
            {
            }

            public void Click()
            {
            }

            public IWebElement FindElement(By by) => this;
            public ReadOnlyCollection<IWebElement> FindElements(By by) => new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            public string GetAttribute(string attributeName) => string.Empty;
            public string GetCssValue(string propertyName) => string.Empty;

            public void SendKeys(string text)
            {
            }

            public void Submit()
            {
            }
        }
    }
}

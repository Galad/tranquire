using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;

namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to asks a question about wether an element is present
    /// </summary>
    public class Presence : SingleUIState<bool, Presence>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Presence"/>
        /// </summary>
        /// <param name="target"> </param>
        public Presence(ITarget target) : this(target, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Presence"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        public Presence(ITarget target, CultureInfo culture) : base(target, culture)
        {
        }

        /// <summary>
        /// Asks a question about wether an element is present
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Presence Of(ITarget target)
        {
            return new Presence(new SafeResolvedTarget(target));
        }

        /// <summary>
        /// Create a new instance of <see cref="Presence"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected override Presence CreateState(ITarget target, CultureInfo culture)
        {
            return new Presence(target, culture);
        }

        /// <summary>
        /// Returns wether the element is present
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
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

            public IWebElement ResolveFor(ISearchContext searchContext)
            {
                try
                {
                    return _target.ResolveFor(searchContext);
                }
                catch (NoSuchElementException)
                {
                    return NotFoundElement.Instance;
                }
            }

            public ImmutableArray<IWebElement> ResolveAllFor(ISearchContext searchContext)
            {
                try
                {
                    return _target.ResolveAllFor(searchContext);
                }
                catch (NoSuchElementException)
                {
                    return ImmutableArray<IWebElement>.Empty;
                }
            }

            public override string ToString() => _target.ToString();
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
                // Does not do anything since it is not a real element in the DOM
            }

            public void Click()
            {
                // Does not do anything since it is not a real element in the DOM
            }

            public IWebElement FindElement(By by) => this;
            public ReadOnlyCollection<IWebElement> FindElements(By by) => new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            public string GetAttribute(string attributeName) => string.Empty;
            public string GetCssValue(string propertyName) => string.Empty;

            public string GetProperty(string propertyName)
            {
                return string.Empty;
            }

            public void SendKeys(string text)
            {
                // Does not do anything since it is not a real element in the DOM
            }

            public void Submit()
            {
                // Does not do anything since it is not a real element in the DOM
            }
        }
    }
}

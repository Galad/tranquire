using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Tranquire.Selenium.Questions.UIModels
{
    /// <summary>
    /// Identify a property that should be mapped to a UI element, and specify how the element is located
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TargetAttribute : Attribute
    {
        /// <summary>
        /// Initialize a new instance of <see cref="TargetAttribute"/>
        /// </summary>
        /// <param name="by">Define the method used to locate the element</param>
        /// <param name="value">The By value</param>
        public TargetAttribute(ByMethod by, string value)
        {
            if (by != ByMethod.Self && string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), ExceptionMessages.ArgumentCannotBeNullOrEmpty);
            }

            ByMethod = by;
            Value = value;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="TargetAttribute"/> that use the container as the element
        /// </summary>
        public TargetAttribute() : this(ByMethod.Self, null)
        {
        }

        /// <summary>
        /// Gets the By method
        /// </summary>
        public ByMethod ByMethod { get; }
        /// <summary>
        /// Gets the By value
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// Gets the target name
        /// </summary>
        public string Name { get; set; }

        private static readonly Dictionary<ByMethod, Func<string, By>> _byMethods = new Dictionary<ByMethod, Func<string, By>>()
        {
            {ByMethod.CssSelector, By.CssSelector },
            {ByMethod.Id, By.Id },
            {ByMethod.Name, By.Name },
            {ByMethod.ClassName, By.ClassName },
            {ByMethod.TagName, By.TagName },
            {ByMethod.XPath, By.XPath }
        };

        internal ITarget CreateTarget(string name)
        {
            if (ByMethod == ByMethod.Self)
            {
                return new SelfTarget(name);
            }

            return Target.The(name).LocatedBy(_byMethods[ByMethod](Value));
        }

        private class SelfTarget : ITarget
        {
            public SelfTarget(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public ITarget RelativeTo(ITarget targetSource)
            {
                return targetSource;
            }

            public ImmutableArray<IWebElement> ResolveAllFor(ISearchContext searchContext)
            {
                throw new InvalidOperationException();
            }

            public IWebElement ResolveFor(ISearchContext searchContext)
            {
                throw new InvalidOperationException();
            }
        }
    }
}

using OpenQA.Selenium;
using Tranquire;
using System;

namespace Tranquire.Selenium.Actions.Enters
{
    /// <summary>
    /// Enter a value in a target
    /// </summary>
    public class EnterValue : TargetedAction
    {
        /// <summary>
        /// Gets the value to enter
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EnterValue"/>
        /// </summary>
        /// <param name="value">The value to enter</param>
        /// <param name="target">The target on which the value is entered</param>
        public EnterValue(string value, ITarget target)
            : base(target)
        {
            Guard.ForNull(value, nameof(value));
            Value = value;
        }

        /// <summary>
        /// Enter the value
        /// </summary>        
        /// <param name="actor"></param>
        /// <param name="element"></param>
        protected override void ExecuteAction(IActor actor, IWebElement element)
        {
            element.SendKeys(Value);
        }
    }
}
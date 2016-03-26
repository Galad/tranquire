using OpenQA.Selenium;
using Tranquire;
using System;

namespace Tranquire.Selenium.Actions.Enters
{
    public class EnterValue : TargetedAction
    {
        private readonly string _value;

        public EnterValue(string value, ITarget target)
            : base(target)
        {
            Guard.ForNull(value, nameof(value));
            _value = value;
        }

        protected override void ExecuteAction<T>(T actor, IWebElement element)
        {
            element.SendKeys(_value);
        }
    }
}
using OpenQA.Selenium;
using System;

namespace Tranquire.Selenium.Actions.Clicks
{
    public class ClickOnAction : TargetedAction
    {
        public ClickOnAction(ITarget target) : base(target)
        {
        }

        protected override void ExecuteAction<T>(T actor, IWebElement element)
        {
            element.Click();
        }

        public ClickOnActionWithRetry AllowRetry()
        {
            return new ClickOnActionWithRetry(this);
        }
    }
}
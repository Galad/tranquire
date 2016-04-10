using OpenQA.Selenium;
using System;

namespace Tranquire.Selenium.Actions.Clicks
{
    /// <summary>
    /// Action representing a click on a target
    /// </summary>
    public class ClickOnAction : TargetedAction
    {
        /// <summary>
        /// Creates a new instance of <see cref="ClickOnAction"/>
        /// </summary>
        /// <param name="target">The target to click on</param>
        public ClickOnAction(ITarget target) : base(target)
        {
        }

        /// <summary>
        /// Click on the target
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actor"></param>
        /// <param name="element"></param>
        protected override void ExecuteAction<T>(T actor, IWebElement element)
        {
            element.Click();
        }

        /// <summary>
        /// Allow the click action to be retried if it fails
        /// </summary>
        /// <returns>A new instance of <see cref="ClickOnActionWithRetry"/></returns>
        public ClickOnActionWithRetry AllowRetry()
        {
            return new ClickOnActionWithRetry(this);
        }
    }
}
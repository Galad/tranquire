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
        /// Gets the action's name
        /// </summary>
        public override string Name => $"Click on {Target.Name}";
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
        /// <param name="actor"></param>
        /// <param name="element"></param>
        protected override void ExecuteAction(IActor actor, IWebElement element)
        {
            element.Click();
        }

        /// <summary>
        /// Allow the click action to be retried if it fails
        /// </summary>
        /// <returns>A new instance of <see cref="ClickOnActionWithRetry{T}"/></returns>
        public ClickOnActionWithRetry<BrowseTheWeb> AllowRetry()
        {
            return new ClickOnActionWithRetry<BrowseTheWeb>(this);
        }
    }
}
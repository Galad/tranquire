using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Actions.Clicks
{
    /// <summary>
    /// Action representing a double click on a target
    /// </summary>
    public sealed class DoubleClickOnAction : TargetedAction
    {
        /// <inheritdoc />
        public override string Name => $"DoubleClick on " + Target.Name;

        /// <summary>
        /// Creates a new instance of <see cref="DoubleClickOnAction"/>
        /// </summary>
        /// <param name="target">The target to double click on</param>
        public DoubleClickOnAction(ITarget target) : base(target)
        {
        }

        /// <inheritdoc />
        protected override void ExecuteAction(IActor actor, IWebElement element, IWebDriver webDriver)
        {
            new OpenQA.Selenium.Interactions.Actions(webDriver).DoubleClick(element).Build().Perform();
        }
    }
}

﻿using OpenQA.Selenium;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Clear the value of an element
    /// </summary>
    public sealed class Clear : ActionUnit<WebBrowser>
    {
        /// <summary>
        /// Gets the target to clear the value from
        /// </summary>
        public ITarget Target { get; }
        /// <summary>
        /// Gets the action's name
        /// </summary>
        public override string Name => "Click on " + Target.Name;

        /// <summary>
        /// Creates a new instance of <see cref="Clear"/>
        /// </summary>
        /// <param name="target">The target to clear the value from</param>
        public Clear(ITarget target)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Clear the value of an element
        /// </summary>
        /// <returns></returns>
        public static Clear TheValueOf(ITarget target)
        {
            return new Clear(target);
        }

        /// <summary>
        /// Clear the value
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            var element = Target.ResolveFor(ability);
            element.SendKeys(Keys.LeftControl + "a");
            element.SendKeys(Keys.Delete);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Opens the context menu on a target
    /// </summary>
    public sealed class OpenContextMenu : ActionUnit<WebBrowser>
    {
        private readonly ITarget _target;

        /// <summary>
        /// Gets the action name
        /// </summary>
        public override string Name => $"Right click on {_target.Name}";

        /// <summary>
        /// Creates a new instance of <see cref="OpenContextMenu"/>
        /// </summary>
        /// <param name="target">The element where the action should be performed</param>
        public OpenContextMenu(ITarget target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Creates a new <see cref="OpenContextMenu"/> action
        /// </summary>
        /// <param name="target">The element where the action should be performed</param>
        /// <returns></returns>
        public static IAction<Unit> On(ITarget target)
        {
            return new OpenContextMenu(target);
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            var seleniumActions = new OpenQA.Selenium.Interactions.Actions(ability.Driver);
            var element = _target.ResolveFor(ability.Driver);
            seleniumActions.ContextClick(element)
                           .Build()
                           .Perform();
        }
    }
}

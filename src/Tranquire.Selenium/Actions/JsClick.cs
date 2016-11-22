using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// Represent a click on a target with javascript
    /// </summary>
    public class JsClick : ActionUnit<BrowseTheWeb>
    {
        private ITarget target;

        /// <summary>
        /// Creates a new instance of <see cref="JsClick"/>
        /// </summary>
        /// <param name="target"></param>
        public JsClick(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            this.target = target;
        }

        /// <summary>
        /// Creates the click action
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static JsClick On(ITarget target)
        {
            return new JsClick(target);
        }
        
        /// <summary>
        /// Click on the target
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, BrowseTheWeb ability)
        {
            var element = target.ResolveFor(ability);
            ((IJavaScriptExecutor)ability.Driver).ExecuteScript("arguments[0].click();", element);            
        }
    }
}

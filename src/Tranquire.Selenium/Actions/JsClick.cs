using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions
{
    public class JsClick : IAction
    {
        private ITarget target;

        public JsClick(ITarget target)
        {
            Guard.ForNull(target, nameof(target));
            this.target = target;
        }

        public static IAction On(ITarget target)
        {
            return new JsClick(target);
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            var element = target.ResolveFor(actor);
            ((IJavaScriptExecutor)actor.AbilityTo<BrowseTheWeb>().Driver).ExecuteScript("arguments[0].click();", element);
            return actor;
        }
    }
}

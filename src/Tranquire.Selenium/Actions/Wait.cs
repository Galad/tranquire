using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions
{
    public class Wait : Action
    {
        private readonly TimeSpan _timeout;
        private readonly ITarget _target;

        public Wait(ITarget target, TimeSpan timeout)
        {
            Guard.ForNull(target, nameof(target));
            _target = target;
            _timeout = timeout;
        }

        public Wait(ITarget target) : this(target, TimeSpan.FromSeconds(5)) { }

        public Wait Timeout(TimeSpan timeout)
        {
            return new Wait(_target, timeout);
        }

        protected override void ExecuteWhen(IActor actor)
        {
            var wait = new WebDriverWait(actor.BrowseTheWeb(), _timeout);
            try
            {
                wait.Until(_ => _target.ResolveFor(actor));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"The target '{_target.Name}' was not present after waiting {_timeout.ToString()}", ex);
            }
        }

        public static Wait UntilTargetIsPresent(ITarget target)
        {
            return new Wait(target);
        }
    }
}

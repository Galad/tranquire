using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions.Clicks
{
    public sealed class ClickOnActionWithRetry : Action
    {
        private readonly IAction _innerAction;
        private readonly TimeSpan _timeout;

        public ClickOnActionWithRetry(IAction innerAction, TimeSpan timeout)
        {
            Guard.ForNull(innerAction, nameof(innerAction));
            _innerAction = innerAction;
            _timeout = timeout;
        }

        public ClickOnActionWithRetry(IAction innerAction) : this(innerAction, TimeSpan.FromSeconds(5))
        {
        }

        public ClickOnActionWithRetry During(TimeSpan time)
        {
            return new ClickOnActionWithRetry(_innerAction, time);
        }

        protected override void ExecuteGiven(IActor actor)
        {
            Execute(actor);
        }

        protected override void ExecuteWhen(IActor actor)
        {
            Execute(actor);
        }

        private void Execute(IActor actor)
        {
            var startTime = DateTimeOffset.Now;
            bool hasSucceeded = false;   
            while (!hasSucceeded)
            {
                try
                {
                    _innerAction.ExecuteWhenAs(actor);
                    hasSucceeded = true;
                }
                catch (WebDriverException)
                {
                    if(DateTimeOffset.Now.Subtract(startTime) > _timeout)
                    {
                        throw;
                    }
                    Thread.Sleep(500);
                }
            }
        }
    }
}

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions.Clicks
{
    /// <summary>
    /// Click on a target. If the click fails, it will be retried every 500ms until the timout expires
    /// </summary>
    public sealed class ClickOnActionWithRetry<T> : Action<T>
    {
        /// <summary>
        /// Gets the action used to click on the target
        /// </summary>
        public IAction<T> InnerAction { get; }
        /// <summary>
        /// Gets the duration during which the action will be retried
        /// </summary>
        public TimeSpan Timeout { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ClickOnActionWithRetry"/>
        /// </summary>
        /// <param name="innerAction">The action used to click on the target</param>
        /// <param name="timeout">The duration during which the action will be retried</param>
        public ClickOnActionWithRetry(IAction<T> innerAction, TimeSpan timeout)
        {
            Guard.ForNull(innerAction, nameof(innerAction));
            InnerAction = innerAction;
            Timeout = timeout;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClickOnActionWithRetry"/> with a default timeout of 5 seconds
        /// </summary>
        /// <param name="innerAction">The action used to click on the target</param>
        public ClickOnActionWithRetry(IAction<T> innerAction) : this(innerAction, TimeSpan.FromSeconds(5))
        {
        }

        /// <summary>
        /// Changes the timeout value
        /// </summary>
        /// <param name="time">The duration during which the action will be retried</param>
        /// <returns>A new instance of <see cref="ClickOnActionWithRetry"/> with the new timeout</returns>
        public ClickOnActionWithRetry<T> During(TimeSpan time)
        {
            return new ClickOnActionWithRetry<T>(InnerAction, time);
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        protected override void ExecuteGiven(IActor executor, T ability)
        {
            Execute(executor, ability);
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        protected override void ExecuteWhen(IActor executor, T ability)
        {
            Execute(executor, ability);
        }

        private void Execute(IActor executor, T ability)
        {
            var startTime = DateTimeOffset.Now;
            bool hasSucceeded = false;   
            while (!hasSucceeded)
            {
                try
                {
                    InnerAction.ExecuteWhenAs(executor, ability);
                    hasSucceeded = true;
                }
                catch (WebDriverException)
                {
                    if(DateTimeOffset.Now.Subtract(startTime) > Timeout)
                    {
                        throw;
                    }
                    Thread.Sleep(500);
                }
            }
        }
    }
}

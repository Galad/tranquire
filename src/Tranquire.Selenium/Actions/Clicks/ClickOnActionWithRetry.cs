using OpenQA.Selenium;
using System;
using System.Threading;

namespace Tranquire.Selenium.Actions.Clicks
{
    /// <summary>
    /// Click on a target. If the click fails, it will be retried every 500ms until the timout expires
    /// </summary>
    public sealed class ClickOnActionWithRetry : ActionBaseUnit
    {
        /// <summary>
        /// Gets the action used to click on the target
        /// </summary>
        public IAction<Unit> InnerAction { get; }
        /// <summary>
        /// Gets the duration during which the action will be retried
        /// </summary>
        public TimeSpan Timeout { get; }
        /// <summary>
        /// Gets the action's name
        /// </summary>
        public override string Name => $"[Retry during {Timeout}] " + InnerAction.Name;

        /// <summary>
        /// Creates a new instance of <see cref="ClickOnActionWithRetry"/>
        /// </summary>
        /// <param name="innerAction">The action used to click on the target</param>
        /// <param name="timeout">The duration during which the action will be retried</param>
        public ClickOnActionWithRetry(IAction<Unit> innerAction, TimeSpan timeout)
        {
            InnerAction = innerAction ?? throw new ArgumentNullException(nameof(innerAction));
            Timeout = timeout;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClickOnActionWithRetry"/> with a default timeout of 5 seconds
        /// </summary>
        /// <param name="innerAction">The action used to click on the target</param>
        public ClickOnActionWithRetry(IAction<Unit> innerAction) : this(innerAction, TimeSpan.FromSeconds(5))
        {
        }

        /// <summary>
        /// Changes the timeout value
        /// </summary>
        /// <param name="time">The duration during which the action will be retried</param>
        /// <returns>A new instance of <see cref="ClickOnActionWithRetry"/> with the new timeout</returns>
        public ClickOnActionWithRetry During(TimeSpan time)
        {
            return new ClickOnActionWithRetry(InnerAction, time);
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>        
        protected override void ExecuteGiven(IActor actor)
        {
            Execute(actor);
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
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
                    InnerAction.ExecuteWhenAs(actor);
                    hasSucceeded = true;
                }
                catch (WebDriverException)
                {
                    if (DateTimeOffset.Now.Subtract(startTime) > Timeout)
                    {
                        throw;
                    }
                    Thread.Sleep(500);
                }
                catch (InvalidOperationException)
                {
                    if (DateTimeOffset.Now.Subtract(startTime) > Timeout)
                    {
                        throw;
                    }
                    Thread.Sleep(500);
                }
            }
        }
    }
}

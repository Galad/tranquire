using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;
using Tranquire.Selenium.Actions.Waiters;
using Tranquire.Selenium.Questions;

namespace Tranquire.Selenium.Actions
{
    /// <summary>
    /// An action used to wait for a condition
    /// </summary>
    public class Wait : ActionUnit<WebBrowser>
    {
        private readonly TimeSpan _timeout;
        private readonly ITarget _target;
        /// <summary>
        /// Gets the action's name
        /// </summary>
        public override string Name => $"Wait until the target {_target.Name} is present during {_timeout.ToString("c", CultureInfo.CurrentCulture)}";

        /// <summary>
        /// Creates a new instance of <see cref="Wait"/>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timeout"></param>
        [Obsolete("This class will become static in a future version. Use Wait.Until(target).IsPresent instead")]
        public Wait(ITarget target, TimeSpan timeout)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _timeout = timeout;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Wait"/>
        /// </summary>
        /// <param name="target"></param>
        [Obsolete("This class will become static in a future version. Use Wait.Until(target).IsPresent instead")]
        public Wait(ITarget target) : this(target, TimeSpan.FromSeconds(5)) { }

        /// <summary>
        /// Change the maximum duration to wait
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        [Obsolete("This class will become static in a future version. Use Wait.Until(target).IsPresent instead")]
        public Wait Timeout(TimeSpan timeout)
        {
            return new Wait(_target, timeout);
        }

        /// <inheritdoc />
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            var wait = new WebDriverWait(ability.Driver, _timeout);
            try
            {
                wait.Until(_ => _target.ResolveFor(ability.Driver));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"The target '{_target.Name}' was not present after waiting {_timeout.ToString()}", ex);
            }
        }

        /// <summary>
        /// Wait until the given target is present in the DOM
        /// </summary>
        /// <param name="target">The target to wait for</param>
        /// <returns></returns>
        [Obsolete("This method will be removed in a future version. Use Wait.Until(target).IsPresent")]
        public static Wait UntilTargetIsPresent(ITarget target)
        {
            return new Wait(target);
        }

        /// <summary>
        /// Wait until a question is answered
        /// </summary>
        /// <typeparam name="TAnswer"></typeparam>
        /// <param name="question">The question to answer</param>
        /// <param name="isAnswered">A predicate returning wether the answer is satisfying</param>
        /// <returns>An action waiting until the question is answered</returns>
        public static WaitUntilQuestionIsAnswered<TAnswer> UntilQuestionIsAnswered<TAnswer>(IQuestion<TAnswer> question, Predicate<TAnswer> isAnswered)
        {
            return new WaitUntilQuestionIsAnswered<TAnswer>(question, isAnswered);
        }
                
        /// <summary>
        /// Wait until the given target is visible
        /// </summary>
        /// <param name="target">The target to wait for</param>
        public static WaitUntilTargetBuilder Until(ITarget target)
        {
            return new WaitUntilTargetBuilder(target);
        }


        /// <summary>
        /// Wait during the specified amount of time
        /// </summary>
        /// <param name="timeToWait">The time to wait</param>
        public static WaitDuring During(TimeSpan timeToWait) => new WaitDuring(timeToWait);
    }
}

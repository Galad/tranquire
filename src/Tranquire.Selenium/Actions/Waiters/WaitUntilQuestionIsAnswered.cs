using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;

namespace Tranquire.Selenium.Actions.Waiters
{
    /// <summary>
    /// Wait until a question is answered
    /// </summary>
    /// <typeparam name="TAnswer">The type of the answer</typeparam>
    public class WaitUntilQuestionIsAnswered<TAnswer> : ActionBaseUnit<WebBrowser>
    {
        private readonly Predicate<TAnswer> _isAnswered;
        private readonly IQuestion<TAnswer> _question;
        private readonly TimeSpan _timeout;
        /// <summary>
        /// Gets the question's name
        /// </summary>
        public override string Name => $"Wait until the question {_question.Name} is answered during { _timeout.ToString("c", CultureInfo.CurrentCulture)}";

        /// <summary>
        /// Creates a new instance of <see cref="WaitUntilQuestionIsAnswered{TAnswer}"/>
        /// </summary>
        /// <param name="question">The question to answer</param>
        /// <param name="isAnswered">A predicate returning wether the answer is satisfying</param>
        /// <param name="timeout">The duration during which the question must be asked</param>
        public WaitUntilQuestionIsAnswered(
            IQuestion<TAnswer> question,
            Predicate<TAnswer> isAnswered,
            TimeSpan timeout)
        {
            _question = question ?? throw new ArgumentNullException(nameof(question));
            _isAnswered = isAnswered ?? throw new ArgumentNullException(nameof(isAnswered));
            _timeout = timeout;
        }

        /// <summary>
        /// Creates a new instance of <see cref="WaitUntilQuestionIsAnswered{TAnswer}"/> with a default timeout of 5 seconds
        /// </summary>
        /// <param name="question">The question to answer</param>
        /// <param name="isAnswered">A predicate returning wether the answer is satisfying</param>        
        public WaitUntilQuestionIsAnswered(
            IQuestion<TAnswer> question,
            Predicate<TAnswer> isAnswered)
            : this(question, isAnswered, TimeSpan.FromSeconds(5))
        {
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            var wait = new WebDriverWait(ability.Driver, _timeout);
            try
            {
                wait.Until(_ => _isAnswered(actor.AsksFor(_question)));
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new TimeoutException($"The question '{_question}' was not answered after waiting {_timeout.ToString()}", ex);
            }
        }

        /// <summary>
        /// Change the timout of the action
        /// </summary>
        /// <param name="timout">The new timeout</param>
        /// <returns></returns>
        public WaitUntilQuestionIsAnswered<TAnswer> Timeout(TimeSpan timout)
        {
            return new WaitUntilQuestionIsAnswered<TAnswer>(_question, _isAnswered, timout);
        }
    }
}

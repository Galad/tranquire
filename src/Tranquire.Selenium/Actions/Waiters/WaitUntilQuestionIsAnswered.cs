using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions.Waiters
{
    /// <summary>
    /// Wait until a question is answered
    /// </summary>
    /// <typeparam name="TAnswer">The type of the answer</typeparam>
    public class WaitUntilQuestionIsAnswered<TAnswer> : ActionUnit<BrowseTheWeb>
    {
        private Predicate<TAnswer> _isAnswered;
        private IQuestion<TAnswer, BrowseTheWeb> _question;
        private readonly TimeSpan _timeout;
        /// <summary>
        /// Gets the question's name
        /// </summary>
        public override string Name=> $"Wait until the question {_question.Name} is answered during { _timeout.ToString("c", CultureInfo.CurrentCulture)}";

        /// <summary>
        /// Creates a new instance of <see cref="WaitUntilQuestionIsAnswered{TAnswer}"/>
        /// </summary>
        /// <param name="question">The question to answer</param>
        /// <param name="isAnswered">A predicate returning wether the answer is satisfying</param>
        /// <param name="timeout">The duration during which the question must be asked</param>
        public WaitUntilQuestionIsAnswered(
            IQuestion<TAnswer, BrowseTheWeb> question,
            Predicate<TAnswer> isAnswered,
            TimeSpan timeout)
        {
            Guard.ForNull(question, nameof(question));
            Guard.ForNull(isAnswered, nameof(isAnswered));
            _question = question;
            _isAnswered = isAnswered;
            _timeout = timeout;
        }

        /// <summary>
        /// Creates a new instance of <see cref="WaitUntilQuestionIsAnswered{TAnswer}"/> with a default timeout of 5 seconds
        /// </summary>
        /// <param name="question">The question to answer</param>
        /// <param name="isAnswered">A predicate returning wether the answer is satisfying</param>        
        public WaitUntilQuestionIsAnswered(
            IQuestion<TAnswer, BrowseTheWeb> question,
            Predicate<TAnswer> isAnswered)
            :this(question, isAnswered, TimeSpan.FromSeconds(5))
        {
        }

        /// <summary>
        /// Execute the action
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        protected override void ExecuteWhen(IActor actor, BrowseTheWeb ability)
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

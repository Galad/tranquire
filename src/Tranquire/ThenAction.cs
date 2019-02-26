using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquire
{
    /// <summary>
    /// Represent an action that verifies the answer of a question
    /// </summary>
    /// <typeparam name="T">The question answer type</typeparam>
    public sealed class ThenAction<T> : ActionBase<T>
    {
        /// <summary>
        /// Gets the question
        /// </summary>
        public IQuestion<T> Question { get; }
        /// <summary>
        /// Gets the action that verifies the outcome
        /// </summary>
        public System.Action<T> VerifyAction { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ThenAction{T}"/>
        /// </summary>
        /// <param name="question">The question</param>
        /// <param name="verifyAction">The action that verifies the outcome</param>
        public ThenAction(IQuestion<T> question, System.Action<T> verifyAction)
        {
            Question = question ?? throw new ArgumentNullException(nameof(question));
            VerifyAction = verifyAction ?? throw new ArgumentNullException(nameof(verifyAction));
        }

        /// <inheritdoc />
        public override string Name => "Then verifies the answer of " + Question.Name;

        /// <inheritdoc />
        protected override T ExecuteWhen(IActor actor)
        {
            var answer = actor.AsksFor(Question);
            VerifyAction(answer);
            return answer;
        }
    }
}

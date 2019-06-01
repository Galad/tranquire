using System;

namespace Tranquire
{
    /// <summary>
    /// Represent the base class for an action that verifies the answer of a question
    /// </summary>
    /// <typeparam name="TResult">The result type</typeparam>
    public abstract class ThenAction<TResult> : ActionBase<TResult>
    {
        /// <summary>
        /// Gets the question
        /// </summary>
        public abstract INamed Question { get; }
    }

    /// <summary>
    /// Represent an action that verifies the answer of a question
    /// </summary>
    /// <typeparam name="T">The question answer type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public sealed class ThenAction<T, TResult> : ThenAction<TResult>
    {        
        /// <summary>
        /// Gets the question
        /// </summary>
        public override INamed Question { get; }

        /// <summary>
        /// Gets the action that verifies the outcome
        /// </summary>
        public Func<T, TResult> VerifyAction { get; }

        private readonly IQuestion<T> _question;

        /// <summary>
        /// Creates a new instance of <see cref="ThenAction{T, TResult}"/>
        /// </summary>
        /// <param name="question">The question</param>
        /// <param name="verifyAction">The action that verifies the outcome</param>
        public ThenAction(IQuestion<T> question, Func<T, TResult> verifyAction)
        {
            Question = question ?? throw new ArgumentNullException(nameof(question));
            VerifyAction = verifyAction ?? throw new ArgumentNullException(nameof(verifyAction));
            _question = question;
        }

        /// <inheritdoc />
        public override string Name => "Then verifies the answer of " + Question.Name;

        /// <inheritdoc />
        protected override TResult ExecuteWhen(IActor actor)
        {
            var answer = actor.AsksFor(_question);
            return VerifyAction(answer);
        }
    }
}

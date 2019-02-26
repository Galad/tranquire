using System;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents a question which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source question</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    public sealed class SelectQuestion<TSource, TResult> : QuestionBase<TResult>
    {
        /// <summary>
        /// Gets the question injected in the constructor
        /// </summary>
        public Func<TSource, TResult> Selector { get; }
        /// <summary>
        /// Gets the question injected in the constructor
        /// </summary>
        public IQuestion<TSource> Question { get; }


        /// <summary>Record Constructor</summary>
        /// <param name="question">The question to get the result from</param>
        /// <param name="selector">The function to apply of the question result.</param>
        public SelectQuestion(IQuestion<TSource> question, Func<TSource, TResult> selector)
        {
            Selector = selector ?? throw new ArgumentNullException(nameof(selector));
            Question = question ?? throw new ArgumentNullException(nameof(question));
        }

        /// <inheritsdoc />
        public override string Name => "[Select] " + Question.Name;

        /// <inheritsdoc />
        protected override TResult Answer(IActor actor)
        {
            return Selector(actor.AsksFor(Question));
        }
    }
}

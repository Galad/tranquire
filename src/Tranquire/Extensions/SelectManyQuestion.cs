using System;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents a question which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source question</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    internal sealed class SelectManyQuestion<TSource, TResult> : QuestionBase<TResult>
    {
        private readonly ISelectMany<TResult> _selectMany;

        /// <summary>Record Constructor</summary>
        /// <param name="question">The question to get the result from</param>
        /// <param name="selector">The function to apply of the question result.</param>
        public SelectManyQuestion(IQuestion<TSource> question, Func<TSource, IQuestion<TResult>> selector)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _selectMany = SelectMany.Create(question, SelectMany.AsksFor<TSource>(), selector, SelectMany.AsksFor<TResult>());
        }

        /// <inheritsdoc />
        public override string Name => _selectMany.Name;

        /// <inheritsdoc />
        protected override TResult Answer(IActor actor) => _selectMany.Apply(actor);
    }
}

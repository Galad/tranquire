using System;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents a question which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source question</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    public sealed class SelectManyQuestionToAction<TSource, TResult> : Tranquire.ActionBase<TResult>
    {
        private readonly SelectMany<IQuestion<TSource>, TSource, IAction<TResult>, TResult> _selectMany;

        /// <summary>
        /// Gets the question injected in the constructor
        /// </summary>
        public Func<TSource, IAction<TResult>> Selector => _selectMany.Selector;
        /// <summary>
        /// Gets the question injected in the constructor
        /// </summary>
        public IQuestion<TSource> Question => _selectMany.Source;


        /// <summary>Record Constructor</summary>
        /// <param name="question">The question to get the result from</param>
        /// <param name="selector">The function to apply of the question result.</param>
        public SelectManyQuestionToAction(IQuestion<TSource> question, Func<TSource, IAction<TResult>> selector)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _selectMany = SelectMany.Create(question, SelectMany.AsksFor<TSource>(), selector, SelectMany.Execute<TResult>());
        }

        /// <inheritsdoc />
        public override string Name => _selectMany.Name;

        /// <inheritsdoc />
        protected override TResult ExecuteWhen(IActor actor) => _selectMany.Apply(actor);
    }
}

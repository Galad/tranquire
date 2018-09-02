using System;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents an action which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source action</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    public sealed class SelectManyActionToQuestion<TSource, TResult> : Question<TResult>
    {
        private readonly SelectMany<IAction<TSource>, TSource, IQuestion<TResult>, TResult> _selectMany;

        /// <summary>
        /// Gets the question injected in the constructor
        /// </summary>
        public Func<TSource, IQuestion<TResult>> Selector => _selectMany.Selector;
        /// <summary>
        /// Gets the question injected in the constructor
        /// </summary>
        public IAction<TSource> Action => _selectMany.Source;


        /// <summary>Creates a new instance of <see cref="SelectManyActionToQuestion{TSource, TResult}"/></summary>
        /// <param name="action">The action to get the result from</param>
        /// <param name="selector">The function to apply of the action result.</param>
        public SelectManyActionToQuestion(IAction<TSource> action, Func<TSource, IQuestion<TResult>> selector)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _selectMany = SelectMany.Create(action, SelectMany.Execute<TSource>(), selector, SelectMany.AsksFor<TResult>());
        }

        /// <inheritdoc />
        public override string Name => _selectMany.Name;

        /// <inheritdoc />
        protected override TResult Answer(IActor actor) => _selectMany.Apply(actor);
    }
}

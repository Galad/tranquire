using System;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents an action which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source question</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    internal sealed class SelectManyAction<TSource, TResult> : ActionBase<TResult>
    {
        private readonly SelectMany<IAction<TSource>, TSource, IAction<TResult>, TResult> _selectMany;

        /// <summary>Record Constructor</summary>
        /// <param name="action">The question to get the result from</param>
        /// <param name="selector">The function to apply of the question result.</param>
        public SelectManyAction(IAction<TSource> action, Func<TSource, IAction<TResult>> selector)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _selectMany = SelectMany.Create(action, SelectMany.Execute<TSource>(), selector, SelectMany.Execute<TResult>());
        }

        /// <inheritsdoc />
        public override string Name => _selectMany.Name;

        /// <inheritsdoc />
        protected override TResult ExecuteWhen(IActor actor) => _selectMany.Apply(actor);
    }
}

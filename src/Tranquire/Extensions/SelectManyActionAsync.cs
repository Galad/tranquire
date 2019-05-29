using System;
using System.Threading.Tasks;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents an asynchronous action which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source question</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    internal sealed class SelectManyActionAsync<TSource, TResult> : ActionBase<Task<TResult>>
    {
        private readonly SelectMany<IAction<Task<TSource>>, Task<TSource>, Task<IAction<TResult>>, Task<TResult>> _selectMany;

        /// <summary>Record Constructor</summary>
        /// <param name="action">The question to get the result from</param>
        /// <param name="selector">The function to apply of the question result.</param>
        public SelectManyActionAsync(IAction<Task<TSource>> action, Func<TSource, IAction<TResult>> selector)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            Func<Task<TSource>, Task<IAction<TResult>>> selectorAsync = async sourceTask =>
            {
                var source = await sourceTask;
                return selector(source);
            };
            _selectMany = SelectMany.Create(action, SelectMany.Execute<Task<TSource>>(), selectorAsync, SelectMany.ExecuteAsync<TResult>());
        }

        /// <inheritsdoc />
        public override string Name => _selectMany.Name;

        /// <inheritsdoc />
        protected override Task<TResult> ExecuteWhen(IActor actor) => _selectMany.Apply(actor);
    }
}

using System;
using System.Threading.Tasks;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents an asynchronous action which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source question</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    internal sealed class SelectManyActionAsyncReturningAsync<TSource, TResult> : ActionBase<Task<TResult>>
    {
        private readonly ISelectMany<Task<Task<TResult>>> _selectMany;

        /// <summary>Record Constructor</summary>
        /// <param name="action">The question to get the result from</param>
        /// <param name="selector">The function to apply of the question result.</param>
        public SelectManyActionAsyncReturningAsync(IAction<Task<TSource>> action, Func<TSource, IAction<Task<TResult>>> selector)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            _selectMany = SelectMany.Create(action, SelectMany.Execute<Task<TSource>>(), selector, SelectMany.ExecuteAsync<Task<TResult>>());
        }

        /// <inheritsdoc />
        public override string Name => _selectMany.Name;

        /// <inheritsdoc />
        protected override async Task<TResult> ExecuteWhen(IActor actor)
        {
            var resultTask = await _selectMany.Apply(actor);
            return await resultTask;
        }
    }
}

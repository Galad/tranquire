using System;
using System.Threading.Tasks;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents an action which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source action</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    internal sealed class SelectManyActionAsyncToQuestionAsync<TSource, TResult> : QuestionBase<Task<TResult>>
    {
        private readonly ISelectMany<Task<Task<TResult>>> _selectMany;

        /// <summary>Creates a new instance of <see cref="SelectManyActionToQuestion{TSource, TResult}"/></summary>
        /// <param name="action">The action to get the result from</param>
        /// <param name="selector">The function to apply of the action result.</param>
        public SelectManyActionAsyncToQuestionAsync(IAction<Task<TSource>> action, Func<TSource, IQuestion<Task<TResult>>> selector)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            _selectMany = SelectMany.Create(action, SelectMany.Execute<Task<TSource>>(), selector, SelectMany.AsksForAsync<Task<TResult>>());
        }

        /// <inheritdoc />
        public override string Name => _selectMany.Name;

        /// <inheritdoc />
        protected override async Task<TResult> Answer(IActor actor)
        {
            var questionTask = await _selectMany.Apply(actor);
            return await questionTask;
        }
    }
}

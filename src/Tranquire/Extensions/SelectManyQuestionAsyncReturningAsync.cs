﻿using System;
using System.Threading.Tasks;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents a question which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source question</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    internal sealed class SelectManyQuestionAsyncReturningAsync<TSource, TResult> : QuestionBase<Task<TResult>>
    {
        private readonly ISelectMany<Task<Task<TResult>>> _selectMany;

        /// <summary>Record Constructor</summary>
        /// <param name="question">The question to get the result from</param>
        /// <param name="selector">The function to apply of the question result.</param>
        public SelectManyQuestionAsyncReturningAsync(IQuestion<Task<TSource>> question, Func<TSource, IQuestion<Task<TResult>>> selector)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            _selectMany = SelectMany.Create(question, SelectMany.AsksFor<Task<TSource>>(), selector, SelectMany.AsksForAsync<Task<TResult>>());
        }

        /// <inheritsdoc />
        public override string Name => _selectMany.Name;

        /// <inheritsdoc />
        protected override async Task<TResult> Answer(IActor actor)
        {
            var resultTask = await _selectMany.Apply(actor);
            return await resultTask;
        }
    }
}
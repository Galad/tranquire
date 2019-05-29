using System;
using System.Threading.Tasks;
using Tranquire.Extensions;

namespace Tranquire
{
    /// <summary>
    /// Contains extension methods for <see cref="IQuestion{TAnswer}"/>
    /// </summary>
    public static class QuestionExtensions
    {
        #region Select
        /// <summary>
        /// Projects the result of a question into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question which result is transformed</param>
        /// <param name="selector">A transform function to the question answer.</param>
        /// <returns></returns>
        public static IQuestion<TResult> Select<TSource, TResult>(this IQuestion<TSource> question, Func<TSource, TResult> selector)
        {
            return new SelectQuestion<TSource, TResult>(question, selector);
        }

        /// <summary>
        /// Projects the result of a question into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question which result is transformed</param>
        /// <param name="selector">A transform function to the question answer.</param>
        /// <returns></returns>
        public static IQuestion<Task<TResult>> Select<TSource, TResult>(this IQuestion<Task<TSource>> question, Func<TSource, TResult> selector)
        {
            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new SelectQuestionAsync<TSource, TResult>(question, result => Task.FromResult(selector(result)));
        }

        /// <summary>
        /// Projects the result of a question into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question which result is transformed</param>
        /// <param name="selector">A transform function to the question answer.</param>
        /// <returns></returns>
        public static IQuestion<Task<TResult>> Select<TSource, TResult>(this IQuestion<Task<TSource>> question, Func<TSource, Task<TResult>> selector)
        {
            return new SelectQuestionAsync<TSource, TResult>(question, selector);
        }
        #endregion

        #region SelectMany
        /// <summary>
        /// Projects the result of a question into a new question.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question which result is transformed</param>
        /// <param name="selector">A transform function that returns a new question.</param>
        /// <returns></returns>
        public static IQuestion<TResult> SelectMany<TSource, TResult>(this IQuestion<TSource> question, Func<TSource, IQuestion<TResult>> selector)
        {
            return new SelectManyQuestion<TSource, TResult>(question, selector);
        }

        /// <summary>
        /// Projects the result of a question into a new action.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<TResult> SelectMany<TSource, TResult>(this IQuestion<TSource> question, Func<TSource, IAction<TResult>> selector)
        {
            return new SelectManyQuestionToAction<TSource, TResult>(question, selector);
        }

        /// <summary>
        /// Projects the result of an asynchronous question into a new question.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question which result is transformed</param>
        /// <param name="selector">A transform function that returns a new question.</param>
        /// <returns></returns>
        public static IQuestion<Task<TResult>> SelectMany<TSource, TResult>(this IQuestion<Task<TSource>> question, Func<TSource, IQuestion<TResult>> selector)
        {
            return new SelectManyQuestionAsync<TSource, TResult>(question, selector);
        }

        /// <summary>
        /// Projects the result of an asynchronous question into a new asynchronous question.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question which result is transformed</param>
        /// <param name="selector">A transform function that returns a new question.</param>
        /// <returns></returns>
        public static IQuestion<Task<TResult>> SelectMany<TSource, TResult>(this IQuestion<Task<TSource>> question, Func<TSource, IQuestion<Task<TResult>>> selector)
        {
            return new SelectManyQuestionAsyncReturningAsync<TSource, TResult>(question, selector);
        }

        /// <summary>
        /// Projects the result of an asynchronous question into a new action.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<Task<TResult>> SelectMany<TSource, TResult>(this IQuestion<Task<TSource>> question, Func<TSource, IAction<TResult>> selector)
        {
            return new SelectManyQuestionAsyncToAction<TSource, TResult>(question, selector);
        }

        /// <summary>
        /// Projects the result of an asynchronous question into a new asynchronous action.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question which result is transformed</param>
        /// <param name="selector">A transform function that returns a new action.</param>
        /// <returns></returns>
        public static IAction<Task<TResult>> SelectMany<TSource, TResult>(this IQuestion<Task<TSource>> question, Func<TSource, IAction<Task<TResult>>> selector)
        {
            return new SelectManyQuestionAsyncToActionAsync<TSource, TResult>(question, selector);
        }
        #endregion

        #region Tagged
        /// <summary>
        /// Create a tagged question from the current question.
        /// </summary>
        /// <typeparam name="T">The question type</typeparam>
        /// <typeparam name="TTag">The tag type</typeparam>
        /// <param name="question">The question to tag</param>
        /// <param name="tag">The tag to apply to the question</param>
        /// <returns>A question with a single tag <paramref name="tag"/></returns>
        public static IQuestion<T> Tagged<T, TTag>(this IQuestion<T> question, TTag tag)
        {
            if (question is null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            return Questions.CreateTagged(question.Name, (tag, question));
        }
        #endregion
    }
}

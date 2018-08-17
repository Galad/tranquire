﻿using System;
using Tranquire.Extensions;

namespace Tranquire
{
    /// <summary>
    /// Contains extension methods for <see cref="IQuestion{TAnswer}"/>
    /// </summary>
    public static class QuestionExtensions
    {
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
    }
}

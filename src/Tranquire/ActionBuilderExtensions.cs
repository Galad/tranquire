using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.ActionBuilders;

namespace Tranquire
{
    /// <summary>
    /// Contains extension methods for the action builders
    /// </summary>
    public static class ActionBuilderExtensions
    {
        /// <summary>
        /// Chain the given action returning <see cref="Unit"/>
        /// </summary>
        /// <typeparam name="TAction">The type of the next action</typeparam>        
        /// <param name="source">The builder source</param>        
        /// <param name="nextAction">The action to chain</param>
        /// <returns>A new builder</returns>
        public static IActionBuilder<TAction, Unit> Then<TAction>(this IActionBuilder source, TAction nextAction) where TAction : class, IAction<Unit>
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (nextAction == null)
            {
                throw new ArgumentNullException(nameof(nextAction));
            }

            return source.Then<TAction, Unit>(nextAction);
        }

        /// <summary>
        /// Get the result of the previous action returning <see cref="Unit"/> and use it to create the next action.
        /// </summary>
        /// <typeparam name="TCurrentAction">The type of the current action</typeparam>
        /// <typeparam name="TCurrentResult">The type of the current action result</typeparam>
        /// <typeparam name="TAction">The type of the next action</typeparam>
        /// <param name="source">The builder source</param>        
        /// <param name="nextAction">A function that creates the next action from the previous action result</param>
        /// <returns>A new builder</returns>
        public static IActionBuilderWithPreviousResult<TAction, Unit, TCurrentAction, TCurrentResult> Then<TCurrentAction, TCurrentResult, TAction>(
            this IActionBuilderWithCurrentAction<TCurrentAction, TCurrentResult> source,
            Func<ActionResult<TCurrentAction, TCurrentResult>, TAction> nextAction)
            where TAction : class, IAction<Unit>
            where TCurrentAction : class, IAction<TCurrentResult>
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (nextAction == null)
            {
                throw new ArgumentNullException(nameof(nextAction));
            }

            return source.Then<TAction, Unit>(nextAction);
        }

        /// <summary>
        /// Chain the given action
        /// </summary>
        /// <typeparam name="TAction">The type of the next action</typeparam>
        /// <typeparam name="TResult">The type fo the next action result</typeparam>        
        /// <param name="source">The builder source</param>        
        /// <param name="nextAction">The action to chain</param>
        /// <param name="dummy">An value used for infering the result type. This parameter is never used.</param>
        /// <returns>A new builder</returns>
        public static IActionBuilder<TAction, TResult> Then<TAction, TResult>(
            this IActionBuilder source,
            TAction nextAction,
#pragma warning disable RCS1163 // Used to inference
            TResult dummy)
#pragma warning restore RCS1163 // Unused parameter.
            where TAction : class, IAction<TResult>
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (nextAction == null)
            {
                throw new ArgumentNullException(nameof(nextAction));
            }

            return source.Then<TAction, TResult>(nextAction);
        }

        /// <summary>
        /// Get the result of the previous action and use it to create the next action.
        /// </summary>
        /// <typeparam name="TCurrentAction">The type of the current action</typeparam>
        /// <typeparam name="TCurrentResult">The type of the current action result</typeparam>
        /// <typeparam name="TAction">The type of the next action</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">The builder source</param>        
        /// <param name="nextAction">A function that creates the next action from the previous action result</param>
        /// <param name="dummy">An value used for infering the result type. This parameter is never used.</param>
        /// <returns>A new builder</returns>
        public static IActionBuilderWithPreviousResult<TAction, TResult, TCurrentAction, TCurrentResult> Then<TCurrentAction, TCurrentResult, TAction, TResult>(
           this IActionBuilderWithCurrentAction<TCurrentAction, TCurrentResult> source,
           Func<ActionResult<TCurrentAction, TCurrentResult>, TAction> nextAction,
#pragma warning disable RCS1163 // Used to inference
           TResult dummy)
#pragma warning restore RCS1163 // Unused parameter.
           where TAction : class, IAction<TResult>
           where TCurrentAction : class, IAction<TCurrentResult>
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (nextAction == null)
            {
                throw new ArgumentNullException(nameof(nextAction));
            }

            return source.Then<TAction, TResult>(nextAction);
        }

    }
}

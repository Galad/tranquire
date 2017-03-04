using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.ActionBuilders
{
    /// <summary>
    /// Provides an API to compose and build actions
    /// </summary>
    public interface IActionBuilder
    {
        /// <summary>
        /// Chain the given action
        /// </summary>
        /// <typeparam name="TNextAction">The type of the next action</typeparam>
        /// <typeparam name="TNextResult">The type of the next result</typeparam>
        /// <param name="nextAction">The action to chain</param>
        /// <returns>A new builder</returns>
        IActionBuilder<TNextAction, TNextResult> Then<TNextAction, TNextResult>(TNextAction nextAction)
            where TNextAction : class, IAction<TNextResult>;
    }

    /// <summary>
    /// Represent an action builder that is already composed with an action
    /// </summary>
    /// <typeparam name="TAction">The current action type</typeparam>
    /// <typeparam name="TResult">The current action result type</typeparam>
    public interface IActionBuilder<TAction, TResult> : IAction<TResult>, IActionBuilder, IActionContainer<TAction, TResult>
        where TAction : class, IAction<TResult>
    {
        /// <summary>
        /// Get the result of the previous action and use it to create the next action.
        /// </summary>
        /// <typeparam name="TNextAction">The type of the next action</typeparam>
        /// <typeparam name="TNextResult">The type of the next action result</typeparam>
        /// <param name="nextAction">A function that creates the next action from the previous action result</param>
        /// <returns>A new builder</returns>
        IActionBuilderWithPreviousResult<TNextAction, TNextResult, TAction, TResult> Then<TNextAction, TNextResult>(Func<ActionResult<TAction, TResult>, TNextAction> nextAction)
            where TNextAction : class, IAction<TNextResult>;
    }

    /// <summary>
    /// Represent an action builder that is already composed with an action factory
    /// </summary>
    /// <typeparam name="TAction">The current action type</typeparam>
    /// <typeparam name="TResult">The current action result type</typeparam>
    /// <typeparam name="TPreviousAction">The previous action type</typeparam>
    /// <typeparam name="TPreviousResult">The previous action result type</typeparam>
    public interface IActionBuilderWithPreviousResult<TAction, TResult, TPreviousAction, TPreviousResult>
        : IAction<TResult>,
          IActionBuilder,
          IActionFactoryContainer<TAction, TResult, TPreviousAction, TPreviousResult>
        where TAction : class, IAction<TResult>
        where TPreviousAction : class, IAction<TPreviousResult>
    {
        /// <summary>
        /// Get the result of the previous action and use it to create the next action.
        /// </summary>
        /// <typeparam name="TNextAction">The type of the next action</typeparam>
        /// <typeparam name="TNextResult">The type of the next action result</typeparam>
        /// <param name="nextAction">A function that creates the next action from the previous action result</param>
        /// <returns>A new builder</returns>
        IActionBuilderWithPreviousResult<TNextAction, TNextResult, TAction, TResult> Then<TNextAction, TNextResult>(Func<ActionResult<TAction, TResult>, TNextAction> nextAction)
            where TNextAction : class, IAction<TNextResult>;
    }
}

using System;

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
    /// Provides function to build actions based on an action builder having a current action
    /// </summary>
    /// <typeparam name="TAction"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IActionBuilderWithCurrentAction<TAction, TResult> where TAction : class, IAction<TResult>
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
    /// Represent an action builder that is already composed with an action
    /// </summary>
    /// <typeparam name="TAction">The current action type</typeparam>
    /// <typeparam name="TResult">The current action result type</typeparam>
    public interface IActionBuilder<TAction, TResult>
        : IAction<TResult>,
          IActionBuilder,
          IActionContainer<TAction, TResult>,
          IActionBuilderWithCurrentAction<TAction, TResult>
        where TAction : class, IAction<TResult>
    {
        /// <summary>
        /// Assign a new name to the current action
        /// </summary>
        /// <param name="name">The new name</param>
        /// <returns>An new action builder having the name of the <paramref name="name"/> parameter</returns>
        IActionBuilder<TAction, TResult> Named(string name);
    }

    /// <summary>
    /// Represent an action builder that is already composed with an action factory
    /// </summary>
    /// <typeparam name="TAction">The current action type</typeparam>
    /// <typeparam name="TResult">The current action result type</typeparam>
    /// <typeparam name="TPreviousAction">The previous action type</typeparam>
    /// <typeparam name="TPreviousResult">The previous action result type</typeparam>
    public interface IActionBuilderWithPreviousResult<TAction, TResult, TPreviousAction, TPreviousResult>
        : IActionBuilder,
          IActionBuilderWithCurrentAction<TAction, TResult>,
          IActionFactoryContainer<TAction, TResult, TPreviousAction, TPreviousResult>
        where TAction : class, IAction<TResult>
        where TPreviousAction : class, IAction<TPreviousResult>
    {
        /// <summary>
        /// Assign a new name to the current action
        /// </summary>
        /// <param name="name">The new name</param>
        /// <returns>An new action builder having the name of the <paramref name="name"/> parameter</returns>
        IActionBuilderWithPreviousResult<TAction, TResult, TPreviousAction, TPreviousResult> Named(string name);
    }
}

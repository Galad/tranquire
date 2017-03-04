using System;

namespace Tranquire.ActionBuilders
{
    /// <summary>
    /// Represent an action that contains an actor factory
    /// </summary>
    /// <typeparam name="TAction">The action type</typeparam>
    /// <typeparam name="TResult">The action result type</typeparam>
    /// <typeparam name="TPreviousAction">The previous action type</typeparam>
    /// <typeparam name="TPreviousResult">The previous action result type</typeparam>
    public interface IActionFactoryContainer<TAction, TResult, TPreviousAction, TPreviousResult> : IAction<TResult>
        where TAction : class, IAction<TResult>
        where TPreviousAction : class, IAction<TPreviousResult>
    {
        /// <summary>
        /// Gets the action factory that creates an action from a previous result
        /// </summary>
        Func<ActionResult<TPreviousAction, TPreviousResult>, TAction> ActionFactory { get; }
    }
}

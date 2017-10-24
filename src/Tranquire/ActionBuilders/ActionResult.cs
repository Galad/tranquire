using System;

namespace Tranquire.ActionBuilders
{
    /// <summary>
    /// Contains the result of a previous action
    /// </summary>
    /// <typeparam name="TAction">The action type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    public sealed class ActionResult<TAction, TResult> where TAction : class
    {
        /// <summary>
        /// Creates a new instance of <see cref="ActionResult{TAction, TResult}"/>
        /// </summary>
        /// <param name="action">The action that produced the result</param>
        /// <param name="result">The action result</param>
        public ActionResult(TAction action, TResult result)
        {
            Result = result;
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        /// Gets the action that produced the result
        /// </summary>
        public TAction Action { get; }
        /// <summary>
        /// Gets the action result
        /// </summary>
        public TResult Result { get; }
    }
}

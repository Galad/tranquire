using System;

namespace Tranquire
{
    /// <summary>
    /// Represent an actor who can execute actions or asks questions
    /// </summary>
    public interface IActor : IAsksQuestions
    {
        /// <summary>
        /// Execute an action with an ability
        /// </summary>
        /// <typeparam name = "TGiven">The ability required for the Given context</typeparam>
        /// <typeparam name = "TWhen">The ability required for the When context</typeparam>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <param name = "action">The action to execute</param>
#pragma warning disable CS0618 // Type or member is obsolete
        [Obsolete("Prefer using Execute<TResult>(IAction<TResult>). Implementors can ignore this warning.", false)]
        TResult ExecuteWithAbility<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action);
#pragma warning restore CS0618 // Type or member is obsolete

        /// <summary>
        /// Execute an action
        /// </summary>
        /// <param name = "action">The action to execute</param>
        TResult Execute<TResult>(IAction<TResult> action);

        /// <summary>
        /// Gets the actor name
        /// </summary>
        string Name { get; }
    }
}
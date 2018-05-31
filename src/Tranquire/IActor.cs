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
        /// <typeparam name = "TAbility">The ability type required for the action</typeparam>        
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <param name = "action">The action to execute</param>
#pragma warning disable CS0618 // Type or member is obsolete
        [Obsolete("Prefer using Execute<TResult>(IAction<TResult>). Implementors can ignore this warning.", false)]
        TResult ExecuteWithAbility<TAbility, TResult>(IAction<TAbility, TResult> action);
#pragma warning restore CS0618 // Type or member is obsolete

        /// <summary>
        /// Execute an action
        /// </summary>
        /// <param name = "action">The action to execute</param>
        TResult Execute<TResult>(IAction<TResult> action);

        /// <summary>
        /// Ask a question about the current state of the system with an ability
        /// </summary>
        /// <typeparam name = "TAnswer">Type answer's type</typeparam>
        /// <typeparam name = "TAbility">Type of the required ability</typeparam>
        /// <param name = "question">A <see cref = "IQuestion{TAnswer}"/> instance representing the question to ask</param>
        /// <returns>The answer to the question.</returns>
        [Obsolete("Prefer using AsksFor<TResult>(IQuestion<TResult>). Implementors can ignore this warning.", false)]
        TAnswer AsksForWithAbility<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question);

        /// <summary>
        /// Gets the actor name
        /// </summary>
        string Name { get; }
    }
}
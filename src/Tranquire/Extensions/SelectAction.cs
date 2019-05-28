using System;

namespace Tranquire.Extensions
{
    /// <summary>
    /// Represents an action which result is transform by the given selector function
    /// </summary>
    /// <typeparam name="TSource">The result type of the source action</typeparam>
    /// <typeparam name="TResult">The result type of the selector function</typeparam>
    public sealed class SelectAction<TSource, TResult> : ActionBase<TResult>
    {
        /// <summary>
        /// Gets the question injected in the constructor
        /// </summary>
        public Func<TSource, TResult> Selector { get; }
        /// <summary>
        /// Gets the question injected in the constructor
        /// </summary>
        public IAction<TSource> Action { get; }


        /// <summary>Record Constructor</summary>
        /// <param name="action">The question to get the result from</param>
        /// <param name="selector">The function to apply of the question result.</param>
        public SelectAction(IAction<TSource> action, Func<TSource, TResult> selector)
        {
            Selector = selector ?? throw new ArgumentNullException(nameof(selector));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <inheritsdoc />
        public override string Name => "[Select] " + Action.Name;

        /// <inheritsdoc />
        protected override TResult ExecuteWhen(IActor actor)
        {
            return Selector(actor.Execute(Action));
        }
    }
}

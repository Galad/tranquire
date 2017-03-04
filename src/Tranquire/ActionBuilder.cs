using Tranquire.ActionBuilders;

namespace Tranquire
{
    /// <summary>
    /// Provides an API to compose and build actions
    /// </summary>
    public sealed class ActionBuilder : IActionBuilder
    {
        /// <summary>
        /// Gets a default instance of <see cref="IActionBuilder"/>
        /// </summary>
        public static readonly IActionBuilder Default = new ActionBuilder();

        /// <summary>
        /// Chain the given action
        /// </summary>
        /// <typeparam name="TNextAction">The type of the next action</typeparam>
        /// <typeparam name="TNextResult">The type of the next result</typeparam>
        /// <param name="nextAction">The action to chain</param>
        /// <returns>A new builder</returns>
        public IActionBuilder<TNextAction, TNextResult> Then<TNextAction, TNextResult>(TNextAction nextAction)
            where TNextAction : class, IAction<TNextResult>
        {
            return new ActionBuilder<TNextAction, TNextResult>(nextAction);
        }
    }
}

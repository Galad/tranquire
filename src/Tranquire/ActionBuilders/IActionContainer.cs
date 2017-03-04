namespace Tranquire.ActionBuilders
{
    /// <summary>
    /// Represent an object that contains an action
    /// </summary>
    /// <typeparam name="TAction">The action type</typeparam>
    /// <typeparam name="TResult">The action result type</typeparam>
    public interface IActionContainer<TAction, TResult>
        where TAction : class, IAction<TResult>
    {
        /// <summary>
        /// Gets the action
        /// </summary>
        TAction Action { get; }
    }
}

namespace Tranquire
{
    /// <summary>
    /// Allow the execution of actions in a Given or When context.
    /// </summary>
    public interface IActionExecutor
    {
        /// <summary>
        /// Execute the given <see cref = "IGivenCommand{T, TResult}"/> when the actor changes the state of the system in order to use it with <see cref = "When{TResult}(IWhenCommand{TResult})"/>
        /// </summary>
        /// <param name = "performable">A <see cref = "IGivenCommand{T, TResult}"/> representing the action performed by the actor</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <typeparam name="T">The ability type</typeparam>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        TResult Given<T, TResult>(IGivenCommand<T, TResult> performable);
        /// <summary>
        /// Execute the given <see cref = "IWhenCommand{T, TResult}"/> when the actor uses the system
        /// </summary>
        /// <param name = "performable">A <see cref = "IWhenCommand{T, TResult}"/> representing the action performed by the actor</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <typeparam name="T">The ability type</typeparam>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        TResult When<T, TResult>(IWhenCommand<T, TResult> performable);
        /// <summary>
        /// Execute the given <see cref = "IGivenCommand{TResult}"/> when the actor changes the state of the system in order to use it with <see cref = "Given{TResult}(IGivenCommand{TResult})"/>
        /// </summary>
        /// <param name = "performable">A <see cref = "IGivenCommand{TResult}"/> representing the action performed by the actor</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        TResult Given<TResult>(IGivenCommand<TResult> performable);
        /// <summary>
        /// Execute the given <see cref = "IWhenCommand{TResult}"/> when the actor uses the system
        /// </summary>
        /// <param name = "performable">A <see cref = "IWhenCommand{TResult}"/> representing the action performed by the actor</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        TResult When<TResult>(IWhenCommand<TResult> performable);
    }
}
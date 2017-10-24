namespace Tranquire
{
    /// <summary>
    /// Allow the execution of actions in a Given or When context.
    /// </summary>
    public interface IActionExecutor
    {
        /// <summary>
        /// Execute the given <see cref = "IGivenCommand{TResult}"/> when the actor changes the state of the system in order to use it with <see cref = "Given{TResult}(IGivenCommand{TResult})"/>
        /// </summary>
        /// <param name = "command">A <see cref = "IGivenCommand{TResult}"/> representing the action performed by the actor</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        TResult Given<TResult>(IGivenCommand<TResult> command);
        /// <summary>
        /// Execute the given <see cref = "IWhenCommand{TResult}"/> when the actor uses the system
        /// </summary>
        /// <param name = "command">A <see cref = "IWhenCommand{TResult}"/> representing the action performed by the actor</param>
        /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        TResult When<TResult>(IWhenCommand<TResult> command);
    }
}
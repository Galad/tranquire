namespace Tranquire
{
    /// <summary>
    /// Allow the execution of actions in a Given or When context.
    /// </summary>
    public interface IActionExecutor
    {
        /// <summary>
        /// Execute the given <see cref = "IGivenCommand"/> when the actor changes the state of the system in order to use it with <see cref = "AttemptsTo(IWhenCommand)"/>
        /// </summary>
        /// <param name = "performable">A <see cref = "IGivenCommand"/> representing the action performed by the actor</param>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        void WasAbleTo<T>(IGivenCommand<T> performable);
        /// <summary>
        /// Execute the given <see cref = "IWhenCommand"/> when the actor uses the system
        /// </summary>
        /// <param name = "performable">A <see cref = "IWhenCommand"/> representing the action performed by the actor</param>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        void AttemptsTo<T>(IWhenCommand<T> performable);
        /// <summary>
        /// Execute the given <see cref = "IGivenCommand"/> when the actor changes the state of the system in order to use it with <see cref = "AttemptsTo(IWhenCommand)"/>
        /// </summary>
        /// <param name = "performable">A <see cref = "IGivenCommand"/> representing the action performed by the actor</param>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        void WasAbleTo(IGivenCommand performable);
        /// <summary>
        /// Execute the given <see cref = "IWhenCommand"/> when the actor uses the system
        /// </summary>
        /// <param name = "performable">A <see cref = "IWhenCommand"/> representing the action performed by the actor</param>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        void AttemptsTo(IWhenCommand performable);
    }
}
namespace Tranquire
{
    /// <summary>
    /// Represent a action executed in order to put the system in a given context
    /// </summary>
    /// <typeparam name="T">The depending ability type</typeparam>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public interface IGivenCommand<T, out TResult> : IGivenCommand<TResult>
    {
        /// <summary>
        /// Execute the action with the given actor
        /// </summary>        
        /// <param name = "actor">The actor used to execute the action</param>
        /// <param name="ability">The ability required for this action by the actor</param>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        TResult ExecuteGivenAs(IActor actor, T ability);
    }

    /// <summary>
    /// Represent a action executed in order to put the system in a given context
    /// </summary>
    /// <typeparam name="TResult">The type returned by the action. Use the <see cref="Unit"/> to represent void actions</typeparam>
    public interface IGivenCommand<out TResult> : INamed
    {
        /// <summary>
        /// Execute the action with the given actor
        /// </summary>        
        /// <param name = "actor">The actor used to execute the action</param>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        TResult ExecuteGivenAs(IActor actor);
    }
}
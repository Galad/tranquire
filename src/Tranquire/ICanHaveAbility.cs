namespace Tranquire
{
    /// <summary>
    /// Represent an actor which use the system under test. The actor can be given capabilities, such as browsing the web, with the method <see cref = "Can{T}(T)"/>
    /// </summary>
    public interface ICanHaveAbility
    {
        /// <summary>
        /// Give to the actor the given capability
        /// </summary>
        /// <typeparam name = "T">The type of the capability</typeparam>
        /// <param name = "doSomething">The capability</param>
        /// <returns>The current <see cref = "IActor"/> instance, allowing to chain calls</returns>
        IActor Can<T>(T doSomething)where T : class;
    }
}
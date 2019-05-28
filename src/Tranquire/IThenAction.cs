namespace Tranquire
{
    /// <summary>
    /// Represents an action that verifies the answer of a question
    /// </summary>
    /// <typeparam name="T">The result type</typeparam>
    public interface IThenAction<out T> : IAction<T>
    {
        /// <summary>
        /// Gets the question
        /// </summary>
        INamed Question { get; }
    }
}

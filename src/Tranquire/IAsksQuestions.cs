namespace Tranquire
{
    /// <summary>
    /// Represent an object that can ask questions about the current state of the system
    /// </summary>
    public interface IAsksQuestions
    {
        /// <summary>
        /// Ask a question about the current state of the system
        /// </summary>
        /// <typeparam name = "TAnswer">Type answer's type</typeparam>
        /// <param name = "question">A <see cref = "IQuestion{TAnswer}"/> instance representing the question to ask</param>
        /// <returns>The answer to the question.</returns>
        TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question);
    }
}
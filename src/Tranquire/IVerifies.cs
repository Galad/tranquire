namespace Tranquire
{
    /// <summary>
    /// Represent an object that can verifies the answer of a question
    /// </summary>
    public interface IVerifies
    {
        /// <summary>
        /// Verifies the answer of the given question
        /// </summary>
        /// <typeparam name="TAnswer">The answer type</typeparam>
        /// <param name="question">The question to verify the answer from</param>
        /// <param name="verifyAction">The action that verifies the answer. This action can throw assertion exceptions (using the Assert of a unit test framework) to indicates that the verification fails.</param>
        /// <returns>The answer, when the verification succeeds</returns>
        TAnswer Then<TAnswer>(IQuestion<TAnswer> question, System.Action<TAnswer> verifyAction);
    }
}
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
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="question">The question to verify the answer from</param>
        /// <param name="verifyAction">The action that verifies the answer. This action can throw assertion exceptions (using the Assert of a unit test framework) to indicates that the verification fails.</param>
        /// <returns>The result of the <paramref name="verifyAction"/>, when the verification succeeds</returns>
        TResult Then<TAnswer, TResult>(IQuestion<TAnswer> question, System.Func<TAnswer, TResult> verifyAction);
    }
}
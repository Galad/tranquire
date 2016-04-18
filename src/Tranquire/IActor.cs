namespace Tranquire
{
    /// <summary>
    /// Represent an actor who can execute actions or asks questions
    /// </summary>
    public interface IActor
    {
        /// <summary>
        /// Execute an action with an ability
        /// </summary>
        /// <typeparam name = "TGiven">The ability required for the Given context</typeparam>
        /// <typeparam name = "TWhen">The ability required for the When context</typeparam>
        /// <param name = "action">The action to execute</param>
        void Execute<TGiven, TWhen>(IAction<TGiven, TWhen> action);
        /// <summary>
        /// Execute an action
        /// </summary>
        /// <param name = "action">The action to execute</param>
        void Execute(IAction action);
        /// <summary>
        /// Ask a question about the current state of the system
        /// </summary>
        /// <typeparam name = "TAnswer">Type answer's type</typeparam>
        /// <param name = "question">A <see cref = "IQuestion{TAnswer}"/> instance representing the question to ask</param>
        /// <returns>The answer to the question.</returns>
        TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question);
        /// <summary>
        /// Ask a question about the current state of the system with an ability
        /// </summary>
        /// <typeparam name = "TAnswer">Type answer's type</typeparam>
        /// <typeparam name = "TAbility">Type of the required ability</typeparam>
        /// <param name = "question">A <see cref = "IQuestion{TAnswer}"/> instance representing the question to ask</param>
        /// <returns>The answer to the question.</returns>
        TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question);
    }
}
namespace Tranquire.Reporting
{
    /// <summary>
    /// Indicates whether an action or a question can send a notification
    /// </summary>
    public interface ICanNotify
    {
        /// <summary>
        /// Indicates wether an action can send a notification
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action">The action to check</param>
        /// <returns>True if the action can send a notification, otherwise fase</returns>
        bool Action<TResult>(IAction<TResult> action);
        
        /// <summary>
        /// Indicates wether an question can send a notification
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question to check</param>
        /// <returns>True if the question can send a notification, otherwise fase</returns>
        bool Question<TResult>(IQuestion<TResult> question);

        /// <summary>
        /// Indicates wether an question can send a notification
        /// </summary>
        /// <typeparam name="TAbility"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="question">The question to check</param>
        /// <returns>True if the question can send a notification, otherwise fase</returns>
        bool Question<TAbility, TResult>(IQuestion<TAbility, TResult> question);
    }
}

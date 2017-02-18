namespace Tranquire.Reporting
{
    /// <summary>
    /// Represents the content type of the execution information
    /// </summary>
    public enum ActionNotificationContentType
    {
        /// <summary>
        /// The action is about to start
        /// </summary>
        BeforeActionExecution,
        /// <summary>
        /// The content contains information about the action duration
        /// </summary>
        AfterActionExecution,
        /// <summary>
        /// The action raised an error
        /// </summary>
        ExecutionError
    }
}

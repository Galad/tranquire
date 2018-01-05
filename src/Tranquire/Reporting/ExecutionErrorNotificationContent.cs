using System;

namespace Tranquire.Reporting
{
    /// <summary>
    /// Before the action
    /// </summary>
    public class ExecutionErrorNotificationContent : IActionNotificationContent
    {
        /// <summary>
        /// Gets the exception that has been raised
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ExecutionErrorNotificationContent"/>
        /// </summary>
        /// <param name="exception">The exception that has been raised</param>
        public ExecutionErrorNotificationContent(Exception exception)
        {
            Exception = exception;
        }
        /// <summary>
        /// Gets the content type
        /// </summary>
        public ActionNotificationContentType NotificationContentType => ActionNotificationContentType.ExecutionError;
    }
}

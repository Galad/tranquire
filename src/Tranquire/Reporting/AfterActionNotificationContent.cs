using System;

namespace Tranquire.Reporting
{
    /// <summary>
    /// Before the action
    /// </summary>
    public class AfterActionNotificationContent : IActionNotificationContent
    {
        /// <summary>
        /// Gets the action's duration
        /// </summary>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="AfterActionNotificationContent"/>
        /// </summary>
        /// <param name="duration"></param>
        public AfterActionNotificationContent(TimeSpan duration)
        {
            Duration = duration;
        }
        /// <summary>
        /// Gets the content type
        /// </summary>
        public ActionNotificationContentType NotificationContentType => ActionNotificationContentType.AfterActionExecution;
    }
}

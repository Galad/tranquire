namespace Tranquire.Reporting
{
    /// <summary>
    /// Before the action
    /// </summary>
    public class BeforeActionNotificationContent : IActionNotificationContent
    {
        /// <summary>
        /// Gets the content type
        /// </summary>
        public ActionNotificationContentType NotificationContentType => ActionNotificationContentType.BeforeActionExecution;
    }
}

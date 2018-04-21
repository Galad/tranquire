namespace Tranquire.Reporting
{
    /// <summary>
    /// Before the action
    /// </summary>
    public class BeforeActionNotificationContent : IActionNotificationContent
    {
        public BeforeActionNotificationContent(CommandType commandType)
        {
            CommandType = commandType;
        }

        /// <summary>
        /// Gets the content type
        /// </summary>
        public ActionNotificationContentType NotificationContentType => ActionNotificationContentType.BeforeActionExecution;

        public CommandType CommandType { get; }
    }

    public enum CommandType
    {
        Action,        
        Question
    }
}

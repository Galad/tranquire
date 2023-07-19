namespace Tranquire.Reporting;

/// <summary>
/// Represent the content of a notification action
/// </summary>
public interface IActionNotificationContent
{
    /// <summary>
    /// Indicates the content type
    /// </summary>
    ActionNotificationContentType NotificationContentType { get; }
}

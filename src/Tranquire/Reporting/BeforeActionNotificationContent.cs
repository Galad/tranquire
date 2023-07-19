using System;

namespace Tranquire.Reporting;

/// <summary>
/// Before the action
/// </summary>
public class BeforeActionNotificationContent : IActionNotificationContent
{
    /// <summary>
    /// Creates a new instance of <see cref="BeforeActionNotificationContent"/>
    /// </summary>
    /// <param name="startDate">Date when the action started</param>
    /// <param name="commandType">The command type</param>
    public BeforeActionNotificationContent(DateTimeOffset startDate, CommandType commandType)
    {
        StartDate = startDate;
        CommandType = commandType;
    }

    /// <summary>
    /// Gets the content type
    /// </summary>
    public ActionNotificationContentType NotificationContentType => ActionNotificationContentType.BeforeActionExecution;

    /// <summary>
    /// Gets the date when the action started
    /// </summary>
    public DateTimeOffset StartDate { get; }
    /// <summary>
    /// Gets the command type
    /// </summary>
    public CommandType CommandType { get; }
}

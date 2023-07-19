using System;

namespace Tranquire.Reporting;

/// <summary>
/// Before the action
/// </summary>
public class BeforeFirstActionNotificationContent : IActionNotificationContent
{
    /// <summary>
    /// Creates a new instance of <see cref="BeforeFirstActionNotificationContent"/>
    /// </summary>
    /// <param name="startDate">Date when the action started</param>
    /// <param name="actionContext">The action context</param>
    public BeforeFirstActionNotificationContent(
        DateTimeOffset startDate,
        ActionContext actionContext)
    {
        StartDate = startDate;
        ActionContext = actionContext;
    }

    /// <inheritdoc />
    public ActionNotificationContentType NotificationContentType => ActionNotificationContentType.BeforeFirstActionExecution;

    /// <summary>
    /// Gets the date when the action started
    /// </summary>
    public DateTimeOffset StartDate { get; }

    /// <summary>
    /// Gets the action context
    /// </summary>
    public ActionContext ActionContext { get; }
}

/// <summary>
/// Represent the execution context of an action
/// </summary>
public enum ActionContext
{
    /// <summary>
    /// Given context
    /// </summary>
    Given,
    /// <summary>
    /// When context
    /// </summary>
    When
}

using System;

namespace Tranquire.Reporting;

/// <summary>
/// After a verification
/// </summary>
public class AfterThenNotificationContent : IActionNotificationContent
{
    /// <summary>
    /// Creates a new instance of <see cref="AfterThenNotificationContent"/>
    /// </summary>
    /// <param name="duration">The duration of the verification</param>
    /// <param name="outcome">The outcome of the verification</param>
    /// <param name="exception">A <see cref="System.Exception"/> object containing the error of the failure details, when the <paramref name="outcome"/> is <see cref="ThenOutcome.Error"/> or <see cref="ThenOutcome.Pass"/></param>
    public AfterThenNotificationContent(TimeSpan duration, ThenOutcome outcome, Exception exception = null)
    {
        Duration = duration;
        Outcome = outcome;
        Exception = exception;
    }

    /// <inheritdoc />
    public ActionNotificationContentType NotificationContentType => ActionNotificationContentType.AfterThen;

    /// <summary>
    /// Gets the duration
    /// </summary>
    public TimeSpan Duration { get; }
    /// <summary>
    /// Gets the verification outcome
    /// </summary>
    public ThenOutcome Outcome { get; }
    /// <summary>
    /// Gets the exception when <see cref="Outcome"/> is <see cref="ThenOutcome.Error"/> or <see cref="ThenOutcome.Failed"/>
    /// </summary>
    public Exception Exception { get; }
}
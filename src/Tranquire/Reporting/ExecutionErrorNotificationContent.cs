﻿using System;

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
        /// Gets the action's duration
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ExecutionErrorNotificationContent"/>
        /// </summary>
        /// <param name="exception">The exception that has been raised</param>
        /// <param name="duration">The action's duration</param>
        public ExecutionErrorNotificationContent(Exception exception, TimeSpan duration)
        {
            this.Exception = exception;
            this.Duration = duration;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ExecutionErrorNotificationContent"/> with a <see cref="Duration"/> of zero
        /// </summary>
        /// <param name="exception">The exception that has been raised</param>
        public ExecutionErrorNotificationContent(Exception exception) : this(exception, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Gets the content type
        /// </summary>
        public ActionNotificationContentType NotificationContentType => ActionNotificationContentType.ExecutionError;
    }
}

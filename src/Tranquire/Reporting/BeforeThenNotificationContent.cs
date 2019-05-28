using System;

namespace Tranquire.Reporting
{
    /// <summary>
    /// Before a verification
    /// </summary>
    public sealed class BeforeThenNotificationContent : IActionNotificationContent, INotificationHandler
    {
        /// <summary>
        /// Creates a new instance of<see cref="BeforeThenNotificationContent"/>
        /// </summary>
        /// <param name="startDate">The start date of the verification</param>
        /// <param name="question">The question which answer is verified</param>
        public BeforeThenNotificationContent(DateTimeOffset startDate, INamed question)
        {
            StartDate = startDate;
            Question = question ?? throw new ArgumentNullException(nameof(question));
        }

        /// <inheritdoc />
        public ActionNotificationContentType NotificationContentType => ActionNotificationContentType.BeforeThen;

        /// <summary>
        /// Gets the start date
        /// </summary>
        public DateTimeOffset StartDate { get; }
        /// <summary>
        /// Gets the action which answer is verified
        /// </summary>
        public INamed Question { get; }

        void INotificationHandler.Handle(XmlDocumentObserver observer, INamed named)
        {
            observer.HandleBeforeThen(this, named);
        }
    }
}

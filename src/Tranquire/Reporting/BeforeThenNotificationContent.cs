using System;

namespace Tranquire.Reporting
{
    /// <summary>
    /// Before a verification
    /// </summary>
    /// <typeparam name="T">The type of the answer that is verified</typeparam>
    public sealed class BeforeThenNotificationContent<T> : IActionNotificationContent, INotificationHandler
    {
        /// <summary>
        /// Creates a new instance of<see cref="BeforeThenNotificationContent{T}"/>
        /// </summary>
        /// <param name="startDate">The start date of the verification</param>
        /// <param name="question">The question which answer is verified</param>
        public BeforeThenNotificationContent(DateTimeOffset startDate, IQuestion<T> question)
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
        public IQuestion<T> Question { get; }

        void INotificationHandler.Handle(XmlDocumentObserver observer, INamed named)
        {
            observer.HandleBeforeThen(this, named);
        }
    }
}

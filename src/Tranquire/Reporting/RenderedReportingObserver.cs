using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Reporting
{
    /// <summary>
    /// An observer which render a <see cref="ActionNotification"/> as a string and call an inner <see cref="IObserver{T}"/>
    /// </summary>
    public class RenderedReportingObserver : IObserver<ActionNotification>
    {
        /// <summary>
        /// Gets the observer
        /// </summary>
        public IObserver<string> Observer { get; }
        private readonly Func<ActionNotification, string> _renderer;

        /// <summary>
        /// Creates a new instance of <see cref="RenderedReportingObserver"/>
        /// </summary>
        /// <param name="observer">The observer to render to</param>
        /// <param name="renderer">The rendering function which transforms a <see cref="ActionNotification"/> to a string</param>
        public RenderedReportingObserver(
            IObserver<string> observer,
            Func<ActionNotification, string> renderer)
        {
            Observer = observer ?? throw new ArgumentNullException(nameof(observer));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member        
        public void OnCompleted()
        {
            Observer.OnCompleted();
        }

        public void OnError(Exception error)
        {
            Observer.OnError(error);
        }

        public void OnNext(ActionNotification value)
        {
            Observer.OnNext(_renderer(value));
        }

        public static string DefaultRenderer(ActionNotification notification)
        {
            switch (notification.Content.NotificationContentType)
            {
                case ActionNotificationContentType.BeforeActionExecution:
                    return RenderBefore(notification);
                case ActionNotificationContentType.AfterActionExecution:
                    return RenderAfter(notification, notification.Content as AfterActionNotificationContent);
                case ActionNotificationContentType.ExecutionError:
                    return RenderError(notification, notification.Content as ExecutionErrorNotificationContent);
                default:
                    throw new NotSupportedException(notification.Content.NotificationContentType.ToString() + " is not supported");
            }
        }

        private static string RenderError(ActionNotification notification, ExecutionErrorNotificationContent executionErrorNotificationContent)
        {
            return GetPrefix(notification.Depth) + "Error in " + notification.Action.Name + "\n" + executionErrorNotificationContent.Exception.Message;
        }

        private static string RenderAfter(ActionNotification notification, AfterActionNotificationContent afterActionNotificationContent)
        {
            return GetPrefix(notification.Depth) + $"Ending   {notification.Action.Name} ({afterActionNotificationContent.Duration.ToString("g", CultureInfo.CurrentCulture)})";
        }

        private static string RenderBefore(ActionNotification notification)
        {
            return GetPrefix(notification.Depth) + "Starting " + notification.Action.Name;
        }

        private static string GetPrefix(int depth)
        {
            if (depth == 0)
            {
                return " ";
            }
            if (depth == 1)
            {
                return "--- ";
            }
            return new string(Enumerable.Repeat(' ', (depth - 1) * 3).ToArray()) + "|--- ";
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}

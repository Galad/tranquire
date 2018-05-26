using System;
using System.Globalization;
using System.Linq;

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
               
        /// <summary>
        /// Gets the rendered
        /// </summary>
        public Func<ActionNotification, string> Renderer { get; }

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
            Renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        /// <inheritsdoc />
        public void OnCompleted()
        {
            Observer.OnCompleted();
        }

        /// <inheritsdoc />
        public void OnError(Exception error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            Observer.OnError(error);
        }

        /// <inheritsdoc />
        public void OnNext(ActionNotification value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Observer.OnNext(Renderer(value));
        }

        /// <summary>
        /// Gets a default renderer
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public static string DefaultRenderer(ActionNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            
            switch (notification.Content.NotificationContentType)
            {
                case ActionNotificationContentType.BeforeActionExecution:
                    return RenderBefore(notification);
                case ActionNotificationContentType.AfterActionExecution:
                    return RenderAfter(notification, notification.Content as AfterActionNotificationContent);
                case ActionNotificationContentType.ExecutionError:
                    return RenderError(notification, notification.Content as ExecutionErrorNotificationContent);
                default:
                    return "Unknown action";
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
    }
}

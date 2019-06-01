using System;
using System.Linq;

namespace Tranquire.Reporting
{

    /// <summary>
    /// Add reporting capabilities to an actor
    /// </summary>
    public class ReportingActor : IActor
    {
        private static INotificationContentFactory _thenNotificationContentFactory = new ThenNotificationContentFactory();
        private static INotificationContentFactory _commandNotificationContentFactory = new CommandNotificationContentfactory();
        private static INotificationContentFactory _defaultNotificationContentFactory = new DefaultNotificationContentFactory();

        /// <summary>
        /// Gets the actor
        /// </summary>
        public IActor Actor { get; }
        /// <summary>
        /// Gets the observer used to send the notifications
        /// </summary>
        public IObserver<ActionNotification> Observer { get; }
        private int _depth = 0;
        /// <summary>
        /// Gets the actor name
        /// </summary>
        public string Name => Actor.Name;

        /// <summary>
        /// Gets the <see cref="IMeasureDuration"/> instance passed to the constructor
        /// </summary>
        public IMeasureDuration MeasureTime { get; }
        /// <summary>
        /// Gets the <see cref="ICanNotify"/> instance passed to the constructor
        /// </summary>
        public ICanNotify CanNotify { get; }

        /// <summary>
        /// Create a new instance of <see cref="ReportingActor"/>
        /// </summary>
        /// <param name="observer">An <see cref="IObserver{T}"/> instance which is called when a notification occurs</param>
        /// <param name="actor">The given actor</param>
        /// <param name="measureTime">A <see cref="IMeasureDuration"/> instance used to measure the execution time of a function</param>
        /// <param name="canNotify">A <see cref="ICanNotify"/> instance that is used to filter actions that should not send a notification</param>
        public ReportingActor(
            IObserver<ActionNotification> observer,
            IActor actor,
            IMeasureDuration measureTime,
            ICanNotify canNotify)
        {
            Observer = observer ?? throw new ArgumentNullException(nameof(observer));
            Actor = actor ?? throw new ArgumentNullException(nameof(actor));
            MeasureTime = measureTime ?? throw new ArgumentNullException(nameof(measureTime));
            CanNotify = canNotify ?? throw new ArgumentNullException(nameof(canNotify));
        }

        private class CanAlwaysNotify : CanNotify { }
        /// <summary>
        /// Create a new instance of <see cref="ReportingActor"/>
        /// </summary>
        /// <param name="observer">An <see cref="IObserver{T}"/> instance which is called when a notification occurs</param>
        /// <param name="actor">The given actor</param>
        /// /// <param name="measureTime">A <see cref="IMeasureDuration"/> instance used to measure the execution time of a function</param>
        public ReportingActor(IObserver<ActionNotification> observer, IActor actor, IMeasureDuration measureTime) : this(observer, actor, measureTime, new CanAlwaysNotify())
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            return ExecuteNotifyingAction(() => CanNotify.Question(question), () => Actor.AsksFor(question), question, CommandType.Question);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TAnswer AsksForWithAbility<TAbility, TAnswer>(IQuestion<TAbility, TAnswer> question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            return Actor.AsksForWithAbility(question);
        }
#pragma warning restore CS0618 // Type or member is obsolete

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (!CanNotify.Action(action))
            {
                return Actor.Execute(action);
            }
            return ExecuteNotifyingAction(() => CanNotify.Action(action), () => Actor.Execute(action), action, CommandType.Action);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TResult ExecuteWithAbility<TAbility, TResult>(IAction<TAbility, TResult> action)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
#pragma warning disable CS0618 // Type or member is obsolete
            return Actor.ExecuteWithAbility(action);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private TResult ExecuteNotifyingAction<TResult>(
            Func<bool> canNotify,
            Func<TResult> executeAction,
            INamed action,
            CommandType commandType)
        {
            if (!canNotify())
            {
                return executeAction();
            }
            _depth++;
            var notificationFactory = GetNotificationContentFactories<TResult>(action);
            Observer.OnNext(new ActionNotification(action, _depth, notificationFactory.CreateBefore<TResult>(action, MeasureTime.Now, commandType)));
            var (duration, result, exception) = MeasureTime.Measure(executeAction);
            if (exception == null)
            {
                Observer.OnNext(new ActionNotification(action, _depth, notificationFactory.CreateAfter(duration)));
                _depth--;
                return result;
            }
            else
            {
                Observer.OnNext(new ActionNotification(action, _depth, notificationFactory.CreateError(exception, duration)));
                _depth--;
                throw exception;
            }

        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private static INotificationContentFactory GetNotificationContentFactories<T>(INamed action)
        {
            if (action is IThenAction<T>)
            {
                return _thenNotificationContentFactory;
            }
            if (action is CommandAction<T>)
            {
                return _commandNotificationContentFactory;
            }
            return _defaultNotificationContentFactory;
        }
    }
}

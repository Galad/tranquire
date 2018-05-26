using System;
using System.Linq;

namespace Tranquire.Reporting
{

    /// <summary>
    /// Add reporting capabilities to an actor
    /// </summary>
    public class ReportingActor : IActor
    {
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
            Guard.ForNull(observer, nameof(observer));
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(measureTime, nameof(measureTime));
            Guard.ForNull(canNotify, nameof(canNotify));
            Observer = observer;
            Actor = actor;
            MeasureTime = measureTime;
            CanNotify = canNotify;
        }

        private class CanAlwaysNotify : CanNotify { }
        /// <summary>
        /// Create a new instance of <see cref="ReportingActor"/>
        /// </summary>
        /// <param name="observer">An <see cref="IObserver{T}"/> instance which is called when a notification occurs</param>
        /// <param name="actor">The given actor</param>
        public ReportingActor(IObserver<ActionNotification> observer, IActor actor) : this(observer, actor, new DefaultMeasureDuration(), new CanAlwaysNotify())
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            Guard.ForNull(question, nameof(question));
            return ExecuteNotifyingAction(() => CanNotify.Question(question), () => Actor.AsksFor(question), question, CommandType.Question);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TAnswer AsksForWithAbility<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            Guard.ForNull(question, nameof(question));
            return Actor.AsksForWithAbility(question);
        }
#pragma warning restore CS0618 // Type or member is obsolete

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            Guard.ForNull(action, nameof(action));
            if (!CanNotify.Action(action))
            {
                return Actor.Execute(action);
            }
            return ExecuteNotifyingAction(() => CanNotify.Action(action), () => Actor.Execute(action), action, CommandType.Action);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TResult ExecuteWithAbility<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            Guard.ForNull(action, nameof(action));
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
            var (createBefore, createAfter, createError) = GetNotificationContentFactories<TResult>(action, commandType);
            Observer.OnNext(new ActionNotification(action, _depth, createBefore(DateTimeOffset.MinValue)));
            try
            {
                var result = MeasureTime.Measure(executeAction);
                Observer.OnNext(new ActionNotification(action, _depth, createAfter(result.Item1)));
                return result.Item2;
            }
            catch (Exception ex)
            {
                Observer.OnNext(new ActionNotification(action, _depth, createError(ex)));
                throw;
            }
            finally
            {
                _depth--;
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        private static (
            Func<DateTimeOffset, IActionNotificationContent>,
            Func<TimeSpan, IActionNotificationContent>,
            Func<Exception, IActionNotificationContent>
            )
            GetNotificationContentFactories<T>(INamed action, CommandType commandType)
        {
            if(action is ThenAction<T> thenAction)
            {
                return
                (
                    date => new BeforeThenNotificationContent<T>(date, thenAction.Question),
                    time => new AfterThenNotificationContent(time, ThenOutcome.Pass),
                    error => new AfterThenNotificationContent(TimeSpan.Zero, GetOutcome(error), error)
                );
            }
            return
                (
                    date => new BeforeActionNotificationContent(date, commandType),
                    time => new AfterActionNotificationContent(time),
                    error => new ExecutionErrorNotificationContent(error)
                );
        }

        private static readonly string[] _knownNamespaces = new[]
        {
            "Xunit.Sdk.",
            "NUnit.Framework.",
            "Microsoft.VisualStudio.TestTools.UnitTesting."
        };

        private static ThenOutcome GetOutcome(Exception error)
        {
            if (_knownNamespaces.Any(error.GetType().FullName.StartsWith))
            {
                return ThenOutcome.Failed;
            }            
            return ThenOutcome.Error;
        }
    }
}

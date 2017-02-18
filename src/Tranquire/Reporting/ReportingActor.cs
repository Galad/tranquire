using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Gets the <see cref="IMeasureDuration"/> instance passed in the constructor
        /// </summary>
        public IMeasureDuration MeasureTime { get; }

        /// <summary>
        /// Create a new instance of <see cref="ReportingActor"/>
        /// </summary>
        /// <param name="observer">An <see cref="IObserver{T}"/> instance which is called when a notification occurs</param>
        /// <param name="actor">The given actor</param>
        /// <param name="measureTime">A <see cref="IMeasureDuration"/> instance used to measure the execution time of a function</param>
        public ReportingActor(IObserver<ActionNotification> observer, IActor actor, IMeasureDuration measureTime)
        {
            Guard.ForNull(observer, nameof(observer));
            Guard.ForNull(actor, nameof(actor));
            Guard.ForNull(measureTime, nameof(measureTime));
            Observer = observer;
            Actor = actor;
            MeasureTime = measureTime;
        }

        /// <summary>
        /// Create a new instance of <see cref="ReportingActor"/>
        /// </summary>
        /// <param name="observer">An <see cref="IObserver{T}"/> instance which is called when a notification occurs</param>
        /// <param name="actor">The given actor</param>
        public ReportingActor(IObserver<ActionNotification> observer, IActor actor):this(observer, actor, new DefaultMeasureDuration())
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return ExecuteNotifyingAction(() => Actor.AsksFor(question), question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            return ExecuteNotifyingAction(() => Actor.AsksFor(question), question);
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return ExecuteNotifyingAction(() => Actor.Execute(action), action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            return ExecuteNotifyingAction(() => Actor.Execute(action), action);
        }

        private TResult ExecuteNotifyingAction<TResult>(Func<TResult> executeAction, INamed action)
        {
            Guard.ForNull(action, nameof(action));            
            _depth++;
            Observer.OnNext(new ActionNotification(action, _depth, new BeforeActionNotificationContent()));
            try
            {
                var result = MeasureTime.Measure(executeAction);
                Observer.OnNext(new ActionNotification(action, _depth, new AfterActionNotificationContent(result.Item1)));
                return result.Item2;
            }
            catch (Exception ex)
            {
                Observer.OnNext(new ActionNotification(action, _depth, new ExecutionErrorNotificationContent(ex)));
                throw;
            }
            finally
            {
                _depth--;
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}

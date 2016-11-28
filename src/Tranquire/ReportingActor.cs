using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
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
        public IObserver<string> Observer { get; }
        private int _depth = 0;
        /// <summary>
        /// Gets the actor name
        /// </summary>
        public string Name => Actor.Name;

        /// <summary>
        /// Create a new instance of <see cref="ReportingActor"/>
        /// </summary>
        /// <param name="observer">An <see cref="IObserver{T}"/> instance which is called when a notification occurs</param>
        /// <param name="actor">The given actor</param>
        public ReportingActor(IObserver<string> observer, IActor actor)
        {
            Guard.ForNull(observer, nameof(observer));
            Guard.ForNull(actor, nameof(actor));
            Observer = observer;
            Actor = actor;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {            
            return ExecuteNotifyingAction(() => Actor.AsksFor(question), "Asking for question", question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            return ExecuteNotifyingAction(() => Actor.AsksFor(question), "Asking for question", question);
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return ExecuteNotifyingAction(() => Actor.Execute(action), "Executing", action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            return ExecuteNotifyingAction(() => Actor.Execute(action), "Executing", action);
        }

        private TResult ExecuteNotifyingAction<TResult>(System.Func<TResult> executeAction, string prefix, object action)
        {
            Guard.ForNull(action, nameof(action));
            var actionName = prefix + " : " + action.ToString();
            _depth++;
            Notifiy(actionName);
            var result = executeAction();
            //Notifiy("(Completed) " + actionName);
            _depth--;
            return result;
        }

        private void Notifiy(string value)
        {
            Observer.OnNext(new string(Enumerable.Repeat('-', _depth * 3).ToArray()) + value);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}

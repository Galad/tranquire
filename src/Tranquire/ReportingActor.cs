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
        private readonly IActor _actor;
        private readonly IObserver<string> _observer;
        private Stack<object> _callStack = new Stack<object>();
        /// <summary>
        /// Gets the actor name
        /// </summary>
        public string Name => _actor.Name;

        /// <summary>
        /// Create a new instance of <see cref="ReportingActor"/>
        /// </summary>
        /// <param name="observer">An <see cref="IObserver{T}"/> instance which is called when a notification occurs</param>
        /// <param name="actor">The given actor</param>
        public ReportingActor(IObserver<string> observer, IActor actor)
        {
            _observer = observer;
            _actor = actor;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {            
            return ExecuteNotifyingQuestion(() => _actor.AsksFor(question), "Asking for question", question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            return ExecuteNotifyingQuestion(() => _actor.AsksFor(question), "Asking for question", question);
        }
        
        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return ExecuteNotifyingAction(() => _actor.Execute(action), "Execute", action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            return ExecuteNotifyingAction(() => _actor.Execute(action), "Execute", action);
        }
               
        private TResult ExecuteNotifyingAction<TResult>(System.Func<TResult> executeAction, string prefix, object action)
        {
            var actionName = prefix + " : " + action.ToString();
            _callStack.Push(action);
            Notifiy(actionName);
            var result = executeAction();
            //Notifiy("(Completed) " + actionName);
            _callStack.Pop();
            return result;
        }

        private T ExecuteNotifyingQuestion<T>(System.Func<T> executeQuestion, string prefix, object question)
        {
            _callStack.Push(question);
            var actionName = prefix + " : " + question.ToString();
            Notifiy(actionName);
            var result = executeQuestion();
            //Notifiy("(Completed) " + actionName);
            _callStack.Pop();
            return result;
        }

        private void Notifiy(string value)
        {
            _observer.OnNext(new string(Enumerable.Repeat('-', _callStack.Count * 3).ToArray()) + value);
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire
{
    public class ReportingActor : IActorFacade
    {
        private readonly IActorFacade _actor;
        private readonly IObserver<string> _observer;
        private Stack<object> _callStack = new Stack<object>();
        public string Name => _actor.Name;

        public ReportingActor(IObserver<string> observer, IActorFacade actor)
        {
            _observer = observer;
            _actor = actor;
        }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {            
            return ExecuteNotifyingQuestion(() => _actor.AsksFor(question), "Asking for question", question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            return ExecuteNotifyingQuestion(() => _actor.AsksFor(question), "Asking for question", question);
        }

        public TResult AttemptsTo<TResult>(IWhenCommand<TResult> performable)
        {
            return ExecuteNotifyingAction(() => _actor.AttemptsTo(performable), "AttemptsTo", performable);
        }

        public TResult AttemptsTo<T, TResult>(IWhenCommand<T, TResult> performable)
        {
            return ExecuteNotifyingAction(() => _actor.AttemptsTo(performable), "AttemptsTo", performable);
        }

        public IActorFacade Can<T>(T doSomething) where T : class
        {
            return _actor.Can(doSomething);
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return ExecuteNotifyingAction(() => _actor.Execute(action), "Execute", action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            return ExecuteNotifyingAction(() => _actor.Execute(action), "Execute", action);
        }

        public TResult WasAbleTo<TResult>(IGivenCommand<TResult> performable)
        {
            return ExecuteNotifyingAction(() => _actor.WasAbleTo(performable), "WasAbleTo", performable);
        }

        public TResult WasAbleTo<T, TResult>(IGivenCommand<T, TResult> performable)
        {
            return ExecuteNotifyingAction(() => _actor.WasAbleTo(performable), "WasAbleTo", performable);
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
    }
}

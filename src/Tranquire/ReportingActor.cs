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

        public ReportingActor(IObserver<string> observer, IActorFacade actor)
        {
            _observer = observer;
            _actor = actor;
        }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            _observer.OnNext("Asking for question of " + typeof(TAnswer).Name);
            return _actor.AsksFor(question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            return _actor.AsksFor(question);
        }

        public TResult AttemptsTo<TResult>(IWhenCommand<TResult> performable)
        {
            _observer.OnNext("AttemptsTo " + performable.GetType().Name);
            return _actor.AttemptsTo(performable);
        }

        public TResult AttemptsTo<T, TResult>(IWhenCommand<T, TResult> performable)
        {
            _observer.OnNext("AttemptsTo " + performable.GetType().Name);
            return _actor.AttemptsTo(performable);
        }

        public IActorFacade Can<T>(T doSomething) where T : class
        {
            return new ReportingActor(_observer, _actor.Can(doSomething));
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return _actor.Execute(action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            return _actor.Execute(action);
        }

        public TResult WasAbleTo<TResult>(IGivenCommand<TResult> performable)
        {
            return _actor.WasAbleTo(performable);
        }

        public TResult WasAbleTo<T, TResult>(IGivenCommand<T, TResult> performable)
        {
            return _actor.WasAbleTo(performable);
        }
    }
}

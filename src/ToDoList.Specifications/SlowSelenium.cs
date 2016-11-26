using System;
using System.Threading;
using Tranquire;
using Tranquire.Selenium;

namespace ToDoList.Specifications
{
    internal class SlowSelenium : IActor
    {
        private const int WaitTime = 500;
        private IActor _actor;

        public SlowSelenium(IActor actor)
        {
            this._actor = actor;
        }

        public string Name => _actor.Name;

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return _actor.AsksFor(question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            if (typeof(TAbility) == typeof(BrowseTheWeb))
            {
                Thread.Sleep(WaitTime);
            }
            var value = _actor.AsksFor(question);
            if (typeof(TAbility) == typeof(BrowseTheWeb))
            {
                Thread.Sleep(WaitTime);
            }
            return value;
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return _actor.Execute(action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            if (typeof(TGiven) != typeof(BrowseTheWeb) && typeof(TWhen) != typeof(BrowseTheWeb))
            {
                return _actor.Execute(action);
            }
            return _actor.Execute(new SlowSeleniumAction<TGiven,TWhen, TResult>(action));
        }
        
        private sealed class SlowSeleniumAction<TGiven, TWhen, TResult> : IAction<TGiven, TWhen, TResult>
        {
            private IAction<TGiven, TWhen, TResult> action;

            public SlowSeleniumAction(IAction<TGiven, TWhen, TResult> action)
            {
                this.action = action;
            }

            public TResult ExecuteGivenAs(IActor actor, TGiven ability)
            {
                Thread.Sleep(WaitTime);
                var value = action.ExecuteGivenAs(actor, ability);
                Thread.Sleep(WaitTime);
                return value;
            }

            public TResult ExecuteWhenAs(IActor actor, TWhen ability)
            {
                Thread.Sleep(WaitTime);
                var value = action.ExecuteWhenAs(actor, ability);
                Thread.Sleep(WaitTime);
                return value;
            }
        }
    }
}
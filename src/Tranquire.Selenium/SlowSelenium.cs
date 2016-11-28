using System;
using System.Threading;

namespace Tranquire.Selenium
{
    public class SlowSelenium : IActor
    {        
        private IActor _actor;
        private TimeSpan _delay;
        private int DelayMilliseconds => (int)_delay.TotalMilliseconds;

        public SlowSelenium(IActor actor, TimeSpan delay)
        {
            this._actor = actor;
            _delay = delay;
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
                Thread.Sleep(DelayMilliseconds);
            }
            var value = _actor.AsksFor(question);
            if (typeof(TAbility) == typeof(BrowseTheWeb))
            {
                Thread.Sleep(DelayMilliseconds);
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
            return _actor.Execute(new SlowSeleniumAction<TGiven,TWhen, TResult>(action, DelayMilliseconds));
        }
        
        private sealed class SlowSeleniumAction<TGiven, TWhen, TResult> : IAction<TGiven, TWhen, TResult>
        {
            private IAction<TGiven, TWhen, TResult> action;
            private int _delay;

            public SlowSeleniumAction(IAction<TGiven, TWhen, TResult> action, int delay)
            {
                this.action = action;
                _delay = delay;
            }

            public TResult ExecuteGivenAs(IActor actor, TGiven ability)
            {
                Thread.Sleep(_delay);
                var value = action.ExecuteGivenAs(actor, ability);
                Thread.Sleep(_delay);
                return value;
            }

            public TResult ExecuteWhenAs(IActor actor, TWhen ability)
            {
                Thread.Sleep(_delay);
                var value = action.ExecuteWhenAs(actor, ability);
                Thread.Sleep(_delay);
                return value;
            }
        }
    }
}
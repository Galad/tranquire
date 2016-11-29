using System;
using System.Threading;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Slow actions and questions that requires Selenium. Can be used in combination of <see cref="HighlightTarget"/>
    /// </summary>
    public class SlowSelenium : IActor
    {
        private IActor _actor;
        private TimeSpan _delay;
        private int DelayMilliseconds => (int)_delay.TotalMilliseconds;

        /// <summary>
        /// Creates a new instance of <see cref="SlowSelenium"/>
        /// </summary>
        /// <param name="actor">The decorated actor</param>
        /// <param name="delay">The wait time. A delay is observerd before and after the action is performed.</param>
        public SlowSelenium(IActor actor, TimeSpan delay)
        {
            this._actor = actor;
            _delay = delay;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Name => _actor.Name;

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return _actor.AsksFor(question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            var browseTheWebQuestion = question as IQuestion<TAnswer, BrowseTheWeb>;
            var targeted = question as ITargeted;
            if (browseTheWebQuestion != null && targeted != null)
            {
                return _actor.AsksFor(new SlowSeleniumQuestion<TAnswer, BrowseTheWeb>(browseTheWebQuestion, DelayMilliseconds, targeted));
            }
            return _actor.AsksFor(question);
        }

        private sealed class SlowSeleniumQuestion<TAnswer, TResult> : IQuestion<TAnswer, BrowseTheWeb>, ITargeted
        {
            private int _delay;
            private readonly IQuestion<TAnswer, BrowseTheWeb> _question;
            private readonly ITargeted _targeted;

            public SlowSeleniumQuestion(IQuestion<TAnswer, BrowseTheWeb> question, int delay, ITargeted targeted)
            {
                _question = question;
                _delay = delay;
                _targeted = targeted;
            }

            public ITarget Target => _targeted.Target;

            public TAnswer AnsweredBy(IActor actor, BrowseTheWeb ability)
            {
                Thread.Sleep(_delay);
                var value = _question.AnsweredBy(actor, ability);
                Thread.Sleep(_delay);
                return value;
            }
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return _actor.Execute(action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            var targeted = action as ITargeted;
            if (typeof(TGiven) != typeof(BrowseTheWeb) && typeof(TWhen) != typeof(BrowseTheWeb) || targeted == null)
            {
                return _actor.Execute(action);
            }
            return _actor.Execute(new SlowSeleniumAction<TGiven, TWhen, TResult>(action, DelayMilliseconds, targeted));
        }

        private sealed class SlowSeleniumAction<TGiven, TWhen, TResult> : IAction<TGiven, TWhen, TResult>, ITargeted
        {
            private IAction<TGiven, TWhen, TResult> action;
            private int _delay;
            private readonly ITargeted _targeted;

            public SlowSeleniumAction(IAction<TGiven, TWhen, TResult> action, int delay, ITargeted targeted)
            {
                this.action = action;
                _delay = delay;
                _targeted = targeted;
            }

            public ITarget Target => _targeted.Target;

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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
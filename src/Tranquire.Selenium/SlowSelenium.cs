using System;
using System.Threading;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Slow actions and questions that requires Selenium. Can be used in combination of <see cref="HighlightTarget"/>
    /// </summary>
    public class SlowSelenium : IActor
    {
        /// <summary>
        /// Gets the actor
        /// </summary>
        public IActor Actor { get; }
        /// <summary>
        /// Gets the delay
        /// </summary>
        public TimeSpan Delay { get; }
        private int DelayMilliseconds => (int)Delay.TotalMilliseconds;

        /// <summary>
        /// Creates a new instance of <see cref="SlowSelenium"/>
        /// </summary>
        /// <param name="actor">The decorated actor</param>
        /// <param name="delay">The wait time. A delay is observerd before and after the action is performed.</param>
        public SlowSelenium(IActor actor, TimeSpan delay)
        {
            this.Actor = actor;
            Delay = delay;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Name => Actor.Name;

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return Actor.AsksFor(question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            var browseTheWebQuestion = question as IQuestion<TAnswer, BrowseTheWeb>;
            var targeted = question as ITargeted;
            if (browseTheWebQuestion != null && targeted != null)
            {
                return Actor.AsksFor(new SlowSeleniumQuestion<TAnswer, BrowseTheWeb>(browseTheWebQuestion, DelayMilliseconds, targeted));
            }
            return Actor.AsksFor(question);
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

            public string Name => $"[Delayed of {_delay}ms] " + _question.Name;

            public ITarget Target => _targeted.Target;

            public TAnswer AnsweredBy(IActor actor, BrowseTheWeb ability)
            {
                Thread.Sleep(_delay);
                var value = _question.AnsweredBy(actor, ability);
                Thread.Sleep(_delay);
                return value;
            }

            public override string ToString() => Name;
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return Actor.Execute(action);
        }

        public TResult ExecuteWithAbility<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            var targeted = action as ITargeted;
            if (typeof(TGiven) != typeof(BrowseTheWeb) && typeof(TWhen) != typeof(BrowseTheWeb) || targeted == null)
            {
                return Actor.ExecuteWithAbility(action);
            }
            return Actor.ExecuteWithAbility(new SlowSeleniumAction<TGiven, TWhen, TResult>(action, DelayMilliseconds, targeted));
        }

        private sealed class SlowSeleniumAction<TGiven, TWhen, TResult> : Action<TGiven, TWhen, TResult>, ITargeted
        {
            private IAction<TGiven, TWhen, TResult> action;
            private int _delay;
            private readonly ITargeted _targeted;
            public override string Name => $"[Delayed of {_delay}ms] " + action.Name;

            public SlowSeleniumAction(IAction<TGiven, TWhen, TResult> action, int delay, ITargeted targeted)
            {
                this.action = action;
                _delay = delay;
                _targeted = targeted;
            }

            public ITarget Target => _targeted.Target;

            protected override TResult ExecuteGiven(IActor actor, TGiven ability)
            {
                Thread.Sleep(_delay);
                var value = action.ExecuteGivenAs(actor, ability);
                Thread.Sleep(_delay);
                return value;
            }

            protected override TResult ExecuteWhen(IActor actor, TWhen ability)
            {
                Thread.Sleep(_delay);
                var value = action.ExecuteWhenAs(actor, ability);
                Thread.Sleep(_delay);
                return value;
            }

            public override string ToString() => Name;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
using OpenQA.Selenium;
using System;

namespace Tranquire.Selenium
{
    public class HighlightTarget : IActor
    {
        private IActor _actor;

        public HighlightTarget(IActor actor)
        {
            this._actor = actor;
        }

        public string Name => _actor.Name;

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question) => _actor.AsksFor(question);

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            if (typeof(TAbility) == typeof(BrowseTheWeb) && typeof(ITargeted).IsAssignableFrom(question.GetType()))
            {
                return _actor.AsksFor(new HighlightedQuestion<TAnswer>((IQuestion<TAnswer, BrowseTheWeb>)question));
            }
            return _actor.AsksFor(question);
        }

        private sealed class HighlightedQuestion<TAnswer> : IQuestion<TAnswer, BrowseTheWeb>
        {
            private IQuestion<TAnswer, BrowseTheWeb> question;
            private ITargeted Targeted => (ITargeted)question;

            public HighlightedQuestion(IQuestion<TAnswer, BrowseTheWeb> question)
            {
                this.question = question;
            }

            public TAnswer AnsweredBy(IActor actor, BrowseTheWeb ability)
            {
                return Execute(ability, Targeted, () => question.AnsweredBy(actor, ability));
            }
        }
        
        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return _actor.Execute(action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            if ((typeof(TGiven) == typeof(BrowseTheWeb) || typeof(TWhen) == typeof(BrowseTheWeb)) && typeof(ITargeted).IsAssignableFrom(action.GetType()))
            {
                return _actor.Execute(new HighlightedAction<TGiven, TWhen, TResult>(action));
            }
            return _actor.Execute(action);
        }

        private sealed class HighlightedAction<TGiven, TWhen, TResult> : IAction<TGiven, TWhen, TResult>
        {
            private IAction<TGiven, TWhen, TResult> _action;
            private ITargeted Targeted => (ITargeted)_action;

            public HighlightedAction(IAction<TGiven, TWhen, TResult> action)
            {
                _action = action;
            }

            public TResult ExecuteGivenAs(IActor actor, TGiven ability)
            {
                if (IsBrowseTheWeb<TGiven>())
                {
                    var a = ability as BrowseTheWeb;
                    return Execute(a, Targeted, () => _action.ExecuteGivenAs(actor, ability));
                }
                return _action.ExecuteGivenAs(actor, ability);
            }

            private static bool IsBrowseTheWeb<T>()
            {
                return typeof(T) == typeof(BrowseTheWeb);
            }

            public TResult ExecuteWhenAs(IActor actor, TWhen ability)
            {
                if (IsBrowseTheWeb<TGiven>())
                {
                    var a = ability as BrowseTheWeb;
                    return Execute(a, Targeted, () => _action.ExecuteWhenAs(actor, ability));
                }
                return _action.ExecuteWhenAs(actor, ability);
            }
        }

        public static TResult Execute<TResult>(BrowseTheWeb browseTheWeb, ITargeted targeted, Func<TResult> execute)
        {
            var webElements = targeted.Target.ResoveAllFor(browseTheWeb);
            foreach (var el in webElements)
            {
                ((IJavaScriptExecutor)browseTheWeb.Driver).ExecuteScript($"arguments[0].style.border = 'thick solid #FFF467';", el);
            }
            try
            {
                return execute();
            }
            finally
            {
                var webElements2 = targeted.Target.ResoveAllFor(browseTheWeb);
                foreach (var el in webElements2)
                {
                    ((IJavaScriptExecutor)browseTheWeb.Driver).ExecuteScript($"arguments[0].style.border = 'thick solid #ACD372';", el);
                }
            }
        }
    }
}
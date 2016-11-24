using OpenQA.Selenium;
using System;
using Tranquire;
using Tranquire.Selenium;

namespace ToDoList.Specifications
{
    public class HighlightTarget : IActorFacade
    {
        private IActorFacade _actor;

        public HighlightTarget(IActorFacade actor)
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

        public TResult AttemptsTo<TResult>(IWhenCommand<TResult> performable)
        {
            return _actor.AttemptsTo(performable);
        }

        public TResult AttemptsTo<T, TResult>(IWhenCommand<T, TResult> performable)
        {
            if (typeof(T) == typeof(BrowseTheWeb) && typeof(ITargeted).IsAssignableFrom(performable.GetType()))
            {
                return _actor.AttemptsTo(new HighlightedWhenCommand<TResult>((IWhenCommand<BrowseTheWeb, TResult>)performable));
            }
            return _actor.AttemptsTo(performable);
        }

        private sealed class HighlightedWhenCommand<TResult> : IWhenCommand<BrowseTheWeb, TResult>
        {
            private IWhenCommand<BrowseTheWeb, TResult> _command;
            private ITargeted Targeted => (ITargeted)_command;

            public HighlightedWhenCommand(IWhenCommand<BrowseTheWeb, TResult> command)
            {
                _command = command;
            }

            public TResult ExecuteWhenAs(IActor actor, BrowseTheWeb ability)
            {
                return Execute(ability, Targeted, () => _command.ExecuteWhenAs(actor, ability));
            }
        }

        public IActorFacade Can<T>(T doSomething) where T : class
        {
            return _actor.Can(doSomething);
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

        public TResult WasAbleTo<TResult>(IGivenCommand<TResult> performable)
        {
            return _actor.WasAbleTo(performable);
        }

        public TResult WasAbleTo<T, TResult>(IGivenCommand<T, TResult> performable)
        {
            if (typeof(T) == typeof(BrowseTheWeb) && typeof(ITargeted).IsAssignableFrom(performable.GetType()))
            {
                return _actor.WasAbleTo(new HighlightedGivenCommand<TResult>((IGivenCommand<BrowseTheWeb, TResult>)performable));
            }
            return _actor.WasAbleTo(performable);
        }

        private sealed class HighlightedGivenCommand<TResult> : IGivenCommand<BrowseTheWeb, TResult>
        {
            private IGivenCommand<BrowseTheWeb, TResult> _command;

            public HighlightedGivenCommand(IGivenCommand<BrowseTheWeb, TResult> command)
            {
                _command = command;
            }

            public TResult ExecuteGivenAs(IActor actor, BrowseTheWeb ability)
            {
                return Execute(ability, (ITargeted)_command, () => _command.ExecuteGivenAs(actor, ability));
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
                foreach (var el in webElements)
                {
                    ((IJavaScriptExecutor)browseTheWeb.Driver).ExecuteScript($"arguments[0].style.border = 'thick solid #ACD372';", el);
                }
            }
        }
    }
}
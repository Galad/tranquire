using OpenQA.Selenium;
using System;
using System.Drawing;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Highlight a target when an action occurs on it. Actions implementing <see cref="ITargeted"/> are identified as actions using a target, which will be higlighted.
    /// </summary>
    public class HighlightTarget : IActor
    {
        private IActor _actor;

        private class HighlighActions
        {
            public string BeginHighlightJsAction { get; }
            public string EndHighlighJsAction { get; }

            public HighlighActions(
                string beginHighlightJsAction,
                string endHighlighJsAction)
            {
                BeginHighlightJsAction = beginHighlightJsAction;
                EndHighlighJsAction = endHighlighJsAction;
            }
        }

        private HighlighActions _highlightActions;

        /// <summary>
        /// Creates a new instance of <see cref="HighlightTarget"/>
        /// </summary>
        /// <param name="actor">The decorated actor</param>
        /// <param name="beginHighlightJsAction">The js action executed before the action or question starts. The targeted element is passed in parameter on the js script.</param>
        /// <param name="endHighlighJsAction">The js action executed after the action or question ends. The targeted element is passed in parameter on the js script.</param>
        public HighlightTarget(
            IActor actor,
            string beginHighlightJsAction,
            string endHighlighJsAction)
        {
            this._actor = actor;
            _highlightActions = new HighlighActions(beginHighlightJsAction, endHighlighJsAction);
        }

        /// <summary>
        /// Creates a new instance of <see cref="HighlightTarget"/>
        /// </summary>
        /// <param name="actor">The decorated actor</param>
        /// <param name="beginHighlighColor">The color applied to the targeted element before the action or question starts</param>
        /// <param name="endHighlighColor">The color applied to the targeted element after the action or question ends</param>
        public HighlightTarget(
            IActor actor,
            Color beginHighlighColor,
            Color endHighlighColor) : this(
                actor,
                SetBorderJsAction(beginHighlighColor),
                SetBorderJsAction(endHighlighColor))
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="HighlightTarget"/>
        /// </summary>
        public HighlightTarget(IActor actor) : this(actor, Color.Purple, Color.LightGreen)
        {
        }

        private static string SetBorderJsAction(Color color)
        {
            var htmlColor = ColorTranslator.ToHtml(color);
            return $"arguments[0].style.border = 'thick solid {htmlColor}';";
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Name => _actor.Name;

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question) => _actor.AsksFor(question);

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            if (typeof(TAbility) == typeof(BrowseTheWeb) && typeof(ITargeted).IsAssignableFrom(question.GetType()))
            {
                return _actor.AsksFor(new HighlightedQuestion<TAnswer>((IQuestion<TAnswer, BrowseTheWeb>)question, _highlightActions));
            }
            return _actor.AsksFor(question);
        }

        private sealed class HighlightedQuestion<TAnswer> : IQuestion<TAnswer, BrowseTheWeb>
        {
            private IQuestion<TAnswer, BrowseTheWeb> question;
            private HighlighActions _highlightActions;

            private ITargeted Targeted => (ITargeted)question;

            public HighlightedQuestion(IQuestion<TAnswer, BrowseTheWeb> question, HighlighActions highlightActions)
            {
                this.question = question;
                _highlightActions = highlightActions;
            }

            public TAnswer AnsweredBy(IActor actor, BrowseTheWeb ability)
            {
                return Execute(ability, Targeted, () => question.AnsweredBy(actor, ability), _highlightActions);
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
                return _actor.Execute(new HighlightedAction<TGiven, TWhen, TResult>(action, _highlightActions));
            }
            return _actor.Execute(action);
        }

        private sealed class HighlightedAction<TGiven, TWhen, TResult> : IAction<TGiven, TWhen, TResult>
        {
            private IAction<TGiven, TWhen, TResult> _action;
            private ITargeted Targeted => (ITargeted)_action;
            private HighlighActions _highlightActions;

            public HighlightedAction(IAction<TGiven, TWhen, TResult> action, HighlighActions highlightActions)
            {
                _action = action;
                _highlightActions = highlightActions;
            }

            public TResult ExecuteGivenAs(IActor actor, TGiven ability)
            {
                if (IsBrowseTheWeb<TGiven>())
                {
                    var a = ability as BrowseTheWeb;
                    return Execute(a, Targeted, () => _action.ExecuteGivenAs(actor, ability), _highlightActions);
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
                    return Execute(a, Targeted, () => _action.ExecuteWhenAs(actor, ability), _highlightActions);
                }
                return _action.ExecuteWhenAs(actor, ability);
            }
        }

        private static TResult Execute<TResult>(BrowseTheWeb browseTheWeb, ITargeted targeted, Func<TResult> execute, HighlighActions actions)
        {
            Highlight(browseTheWeb, targeted, actions.BeginHighlightJsAction);
            var result = execute();
            Highlight(browseTheWeb, targeted, actions.EndHighlighJsAction);
            return result;
        }

        private static void Highlight(BrowseTheWeb browseTheWeb, ITargeted targeted, string action)
        {
            var webElements = targeted.Target.ResoveAllFor(browseTheWeb);
            foreach (var el in webElements)
            {
                ((IJavaScriptExecutor)browseTheWeb.Driver).ExecuteScript(action, el);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
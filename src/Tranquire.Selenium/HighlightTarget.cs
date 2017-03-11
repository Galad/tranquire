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
        /// <summary>
        /// Gets the actor
        /// </summary>
        public IActor Actor { get; }

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
            this.Actor = actor;
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
        public string Name => Actor.Name;

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question) => Actor.AsksFor(question);

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            if (typeof(TAbility) == typeof(WebBrowser) && typeof(ITargeted).IsAssignableFrom(question.GetType()))
            {
                return Actor.AsksFor(new HighlightedQuestion<TAnswer>((IQuestion<TAnswer, WebBrowser>)question, _highlightActions));
            }
            return Actor.AsksFor(question);
        }

        private sealed class HighlightedQuestion<TAnswer> : Question<TAnswer, WebBrowser>
        {
            private IQuestion<TAnswer, WebBrowser> question;
            private HighlighActions _highlightActions;
            private ITargeted Targeted => (ITargeted)question;
            public override string Name => "[Highlighted] " + question.Name;

            public HighlightedQuestion(IQuestion<TAnswer, WebBrowser> question, HighlighActions highlightActions)
            {
                this.question = question;
                _highlightActions = highlightActions;
            }

            protected override TAnswer Answer(IActor actor, WebBrowser ability)
            {
                return Execute(ability, Targeted, () => question.AnsweredBy(actor, ability), _highlightActions);
            }
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return Actor.Execute(action);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TResult ExecuteWithAbility<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            if ((typeof(TGiven) == typeof(WebBrowser) || typeof(TWhen) == typeof(WebBrowser)) && typeof(ITargeted).IsAssignableFrom(action.GetType()))
            {
                return Actor.ExecuteWithAbility(new HighlightedAction<TGiven, TWhen, TResult>(action, _highlightActions));
            }
            return Actor.ExecuteWithAbility(action);
        }

        private sealed class HighlightedAction<TGiven, TWhen, TResult> : Action<TGiven, TWhen, TResult>
        {
            private IAction<TGiven, TWhen, TResult> _action;
            private ITargeted Targeted => (ITargeted)_action;
            private HighlighActions _highlightActions;
            public override string Name => "[Highlighted] " + _action.Name;

            public HighlightedAction(IAction<TGiven, TWhen, TResult> action, HighlighActions highlightActions)
            {
                _action = action;
                _highlightActions = highlightActions;
            }

            protected override TResult ExecuteGiven(IActor actor, TGiven ability)
            {
                if (IsWebBrowser<TGiven>())
                {
                    var a = ability as WebBrowser;
                    return Execute(a, Targeted, () => _action.ExecuteGivenAs(actor, ability), _highlightActions);
                }
                return _action.ExecuteGivenAs(actor, ability);
            }

            private static bool IsWebBrowser<T>()
            {
                return typeof(T) == typeof(WebBrowser);
            }

            protected override TResult ExecuteWhen(IActor actor, TWhen ability)
            {
                if (IsWebBrowser<TGiven>())
                {
                    var a = ability as WebBrowser;
                    return Execute(a, Targeted, () => _action.ExecuteWhenAs(actor, ability), _highlightActions);
                }
                return _action.ExecuteWhenAs(actor, ability);
            }

            public override string ToString() => Name;
        }
#pragma warning restore CS0618 // Type or member is obsolete

        private static TResult Execute<TResult>(WebBrowser webBrowser, ITargeted targeted, Func<TResult> execute, HighlighActions actions)
        {
            Highlight(webBrowser, targeted, actions.BeginHighlightJsAction);
            var result = execute();
            Highlight(webBrowser, targeted, actions.EndHighlighJsAction);
            return result;
        }

        private static void Highlight(WebBrowser webBrowser, ITargeted targeted, string action)
        {
            var webElements = targeted.Target.ResoveAllFor(webBrowser);
            foreach (var el in webElements)
            {
                ((IJavaScriptExecutor)webBrowser.Driver).ExecuteScript(action, el);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
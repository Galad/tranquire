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

        private readonly HighlighActions _highlightActions;

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
            var htmlColor = ToHtml(color);
            return $"arguments[0].style.border = 'thick solid {htmlColor}';";
        }

        /// <summary>
        /// Gets the HTML color code from a <see cref="Color"/> value
        /// </summary>
        /// <param name="color">The color</param>
        /// <returns>The HTML encoded color (Ex: #DF0101)</returns>
        public static string ToHtml(Color color)
        {
            if (color.IsEmpty)
            {
                return string.Empty;
            }

            if (color.IsNamedColor)
            {
                if (color == Color.LightGray)
                {
                    // special case due to mismatch between Html and enum spelling
                    return "LightGrey";
                }
                return color.Name;
            }
            var colorString = "#" + color.R.ToString("X2", null) +
                                    color.G.ToString("X2", null) +
                                    color.B.ToString("X2", null);

            return colorString;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Name => Actor.Name;

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question) => Actor.AsksFor(question);

#pragma warning disable CS0618 // Type or member is obsolete
        public TAnswer AsksForWithAbility<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            if (typeof(TAbility) == typeof(WebBrowser) && question is ITargeted)
            {
                return Actor.AsksForWithAbility(new HighlightedQuestion<TAnswer>((IQuestion<TAnswer, WebBrowser>)question, _highlightActions));
            }
            return Actor.AsksForWithAbility(question);
        }

        private sealed class HighlightedQuestion<TAnswer> : QuestionBase<TAnswer, WebBrowser>
        {
            private readonly IQuestion<TAnswer, WebBrowser> question;
            private readonly HighlighActions _highlightActions;
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
#pragma warning restore CS0618 // Type or member is obsolete
        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return Actor.Execute(action);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TResult ExecuteWithAbility<TAbility, TResult>(IAction<TAbility, TResult> action)
        {
            if (typeof(TAbility) == typeof(WebBrowser) && action is ITarget)
            {
                return Actor.ExecuteWithAbility(new HighlightedAction<TAbility, TResult>(action, _highlightActions));
            }
            return Actor.ExecuteWithAbility(action);
        }

        private sealed class HighlightedAction<TAbility, TResult> : ActionBase<TAbility, TResult>
        {
            private readonly IAction<TAbility, TResult> _action;
            private readonly HighlighActions _highlightActions;
            private ITargeted Targeted => (ITargeted)_action;
            public override string Name => "[Highlighted] " + _action.Name;

            public HighlightedAction(IAction<TAbility, TResult> action, HighlighActions highlightActions)
            {
                _action = action;
                _highlightActions = highlightActions;
            }

            protected override TResult ExecuteGiven(IActor actor, TAbility ability)
            {
                if (IsWebBrowser<TAbility>())
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

            protected override TResult ExecuteWhen(IActor actor, TAbility ability)
            {
                if (IsWebBrowser<TAbility>())
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
            var webElements = targeted.Target.ResolveAllFor(webBrowser);
            foreach (var el in webElements)
            {
                ((IJavaScriptExecutor)webBrowser.Driver).ExecuteScript(action, el);
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
using OpenQA.Selenium;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Take a screenshot of the web browser after the action or the question ends
    /// </summary>
    public class TakeScreenshot : IActor
    {        
        /// <summary>
        /// Creates a new instance of <see cref="TakeScreenshot"/>
        /// </summary>
        /// <param name="actor">The decorated actor</param>
        /// <param name="nextScreenshotName">A function returning the name of the next screenshot. Each name should be different.</param>
        public TakeScreenshot(IActor actor, Func<string> nextScreenshotName)
        {
            if (actor == null) throw new ArgumentNullException(nameof(actor));
            if (nextScreenshotName == null) throw new ArgumentNullException(nameof(nextScreenshotName));
            Actor = actor;
            NextScreenshotName = nextScreenshotName;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IActor Actor { get; }
        public string Name => Actor.Name;
        public Func<string> NextScreenshotName { get; }

        private static TResult ExecuteTakeScreenshot<TResult, TAbility>(
            TAbility ability,
            Func<TResult> execute,
            Func<string> nextScreenshotName)
        {
            try
            {                
                return execute();
            }
            finally
            {
                var webBrowser = ability as WebBrowser;
                if (webBrowser != null)
                {
                    if (!Directory.Exists("Screenshots"))
                    {
                        Directory.CreateDirectory("Screenshots");
                    }
                    var name = nextScreenshotName();
                    ((ITakesScreenshot)webBrowser.Driver).GetScreenshot().SaveAsFile($"Screenshots\\{name}.jpg", ScreenshotImageFormat.Jpeg);
                }
            }
        }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return Actor.AsksFor(question);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            return Actor.AsksFor(new TakeScreenshotQuestion<TAnswer, TAbility>(question, NextScreenshotName));
        }

        private sealed class TakeScreenshotQuestion<TAnswer, TAbility> : Question<TAnswer, TAbility>
        {
            private readonly IQuestion<TAnswer, TAbility> _question;
            private readonly Func<string> _nextScreenshotName;
            
            public TakeScreenshotQuestion(IQuestion<TAnswer, TAbility> question, Func<string> nextScreenshotName)
            {
                _question = question;
                _nextScreenshotName = nextScreenshotName;
            }

            protected override TAnswer Answer(IActor actor, TAbility ability)
            {
                return ExecuteTakeScreenshot(ability, () => _question.AnsweredBy(actor, ability), _nextScreenshotName);
            }

            /// <summary>
            /// Gets the action's name
            /// </summary>
            public override string Name => "[Take screenshot] " + _question.Name;
        }
#pragma warning restore CS0618 // Type or member is obsolete

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return Actor.Execute(action);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TResult ExecuteWithAbility<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            return Actor.ExecuteWithAbility(new TakeScreenshotAction<TGiven, TWhen, TResult>(action, NextScreenshotName));
        }

        private sealed class TakeScreenshotAction<TGiven, TWhen, TResult> : Action<TGiven, TWhen, TResult>
        {
            private readonly IAction<TGiven, TWhen, TResult> _action;
            private readonly Func<string> _nextScreenshotName;

            public TakeScreenshotAction(IAction<TGiven, TWhen, TResult> action, Func<string> nextScreenshotName)
            {
                _action = action;
                _nextScreenshotName = nextScreenshotName;
            }

            protected override TResult ExecuteGiven(IActor actor, TGiven ability)
            {
                return ExecuteTakeScreenshot(ability, () => _action.ExecuteGivenAs(actor, ability), _nextScreenshotName);
            }

            protected override TResult ExecuteWhen(IActor actor, TWhen ability)
            {
                return ExecuteTakeScreenshot(ability, () => _action.ExecuteWhenAs(actor, ability), _nextScreenshotName);
            }

            /// <summary>
            /// Gets the action's name
            /// </summary>
            public override string Name => "[Take screenshot] " + _action.Name;
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

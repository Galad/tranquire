using OpenQA.Selenium;
using System;
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
        /// <param name="directory">The directory where the screenshots are saved</param>
        /// <param name="nextScreenshotName">A function returning the name of the next screenshot. Each name should be different.</param>
        public TakeScreenshot(IActor actor, string directory, Func<string> nextScreenshotName)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            Actor = actor ?? throw new ArgumentNullException(nameof(actor));
            NextScreenshotName = nextScreenshotName ?? throw new ArgumentNullException(nameof(nextScreenshotName));
            Directory = directory;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IActor Actor { get; }
        public string Name => Actor.Name;
        public Func<string> NextScreenshotName { get; }
        public string Directory { get; }

        private static TResult ExecuteTakeScreenshot<TResult, TAbility>(
            TAbility ability,
            Func<TResult> execute,
            string directory,
            Func<string> nextScreenshotName)
        {
            try
            {
                return execute();
            }
            finally
            {
                if (ability is WebBrowser webBrowser)
                {
                    if (!System.IO.Directory.Exists(directory))
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }
                    var name = nextScreenshotName();
                    ((ITakesScreenshot)webBrowser.Driver).GetScreenshot().SaveAsFile(Path.Combine(directory, $"{name}.jpg"), ScreenshotImageFormat.Jpeg);
                }
            }
        }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return Actor.AsksFor(question);
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TAnswer AsksForWithAbility<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            return Actor.AsksForWithAbility(new TakeScreenshotQuestion<TAnswer, TAbility>(question, Directory, NextScreenshotName));
        }

        private sealed class TakeScreenshotQuestion<TAnswer, TAbility> : Question<TAnswer, TAbility>
        {
            private readonly IQuestion<TAnswer, TAbility> _question;
            private readonly Func<string> _nextScreenshotName;
            private readonly string _directory;

            public TakeScreenshotQuestion(IQuestion<TAnswer, TAbility> question, string directory, Func<string> nextScreenshotName)
            {
                _question = question;
                _nextScreenshotName = nextScreenshotName;
                _directory = directory;
            }

            protected override TAnswer Answer(IActor actor, TAbility ability)
            {
                return ExecuteTakeScreenshot(ability, () => _question.AnsweredBy(actor, ability), _directory, _nextScreenshotName);
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
            return Actor.ExecuteWithAbility(new TakeScreenshotAction<TGiven, TWhen, TResult>(action, Directory, NextScreenshotName));
        }

        private sealed class TakeScreenshotAction<TGiven, TWhen, TResult> : Action<TGiven, TWhen, TResult>
        {
            private readonly IAction<TGiven, TWhen, TResult> _action;
            private readonly Func<string> _nextScreenshotName;
            private readonly string _directory;

            public TakeScreenshotAction(IAction<TGiven, TWhen, TResult> action, string directory, Func<string> nextScreenshotName)
            {
                _action = action;
                _nextScreenshotName = nextScreenshotName;
                _directory = directory;
            }

            protected override TResult ExecuteGiven(IActor actor, TGiven ability)
            {
                return ExecuteTakeScreenshot(ability, () => _action.ExecuteGivenAs(actor, ability), _directory, _nextScreenshotName);
            }

            protected override TResult ExecuteWhen(IActor actor, TWhen ability)
            {
                return ExecuteTakeScreenshot(ability, () => _action.ExecuteWhenAs(actor, ability), _directory, _nextScreenshotName);
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

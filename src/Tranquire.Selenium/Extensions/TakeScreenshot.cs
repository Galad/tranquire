using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tranquire.Selenium.Extensions
{
    /// <summary>
    /// Take a screenshot of the web browser after the action or the question ends
    /// </summary>
    public sealed class TakeScreenshot : IActor
    {
        /// <summary>
        /// Creates a new instance of <see cref="TakeScreenshot"/>
        /// </summary>
        /// <param name="actor">The decorated actor</param>
        /// <param name="nextScreenshotName">A function returning the name of the next screenshot. Each name should be different.</param>
        /// <param name="screenshotsObserver">An observer that can be used to notify that a screenshot was taken</param>
        /// 
        public TakeScreenshot(IActor actor, Func<string> nextScreenshotName, IObserver<ScreenshotInfo> screenshotsObserver)
        {
            Actor = actor ?? throw new ArgumentNullException(nameof(actor));
            NextScreenshotName = nextScreenshotName ?? throw new ArgumentNullException(nameof(nextScreenshotName));
            ScreenshotObserver = screenshotsObserver ?? throw new ArgumentNullException(nameof(screenshotsObserver));            
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IActor Actor { get; }
        public string Name => Actor.Name;
        public Func<string> NextScreenshotName { get; }
        public IObserver<ScreenshotInfo> ScreenshotObserver { get; }

        private static TResult ExecuteTakeScreenshot<TResult, TAbility>(
            TAbility ability,
            Func<TResult> execute,
            TakeScreenshot takeScreenshot)
        {
            try
            {
                return execute();
            }
            finally
            {
                if (ability is WebBrowser webBrowser)
                {                    
                    var name = takeScreenshot.NextScreenshotName();
                    var screenshot = ((ITakesScreenshot)webBrowser.Driver).GetScreenshot();
                    takeScreenshot.ScreenshotObserver.OnNext(new ScreenshotInfo(screenshot, name));
                    //.SaveAsFile(Path.Combine(directory, $"{name}.jpg"), ScreenshotImageFormat.Jpeg);
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
            return Actor.AsksForWithAbility(new TakeScreenshotQuestion<TAnswer, TAbility>(question, this));
        }

        private sealed class TakeScreenshotQuestion<TAnswer, TAbility> : Question<TAnswer, TAbility>
        {
            private readonly IQuestion<TAnswer, TAbility> _question;
            private readonly TakeScreenshot _takeScreenshot;
                        
            public TakeScreenshotQuestion(
                IQuestion<TAnswer, TAbility> question, 
                TakeScreenshot takeScreenshot)
            {
                _question = question;
                _takeScreenshot = takeScreenshot;                
            }

            protected override TAnswer Answer(IActor actor, TAbility ability)
            {
                return ExecuteTakeScreenshot(ability, () => _question.AnsweredBy(actor, ability), _takeScreenshot);
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
            return Actor.ExecuteWithAbility(new TakeScreenshotAction<TGiven, TWhen, TResult>(action, this));
        }

        private sealed class TakeScreenshotAction<TGiven, TWhen, TResult> : Action<TGiven, TWhen, TResult>
        {
            private readonly IAction<TGiven, TWhen, TResult> _action;
            private readonly TakeScreenshot _takeScreenshot;

            public TakeScreenshotAction(
                IAction<TGiven, TWhen, TResult> action, 
                TakeScreenshot takeScreenshot)
            {
                _action = action;
                _takeScreenshot = takeScreenshot;
            }

            protected override TResult ExecuteGiven(IActor actor, TGiven ability)
            {
                return ExecuteTakeScreenshot(ability, () => _action.ExecuteGivenAs(actor, ability), _takeScreenshot);
            }

            protected override TResult ExecuteWhen(IActor actor, TWhen ability)
            {
                return ExecuteTakeScreenshot(ability, () => _action.ExecuteWhenAs(actor, ability), _takeScreenshot);
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

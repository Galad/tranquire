using OpenQA.Selenium;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace Tranquire.Selenium
{
    public class TakeScreenshot : IActor
    {
        public IActor Actor { get; }
        
        public TakeScreenshot(IActor actor, Func<string> nextScreenshotName)
        {
            if (actor == null) throw new ArgumentNullException(nameof(actor));
            if (nextScreenshotName == null) throw new ArgumentNullException(nameof(nextScreenshotName));
            Actor = actor;
            NextScreenshotName = nextScreenshotName;
        }

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
                var browseTheWeb = ability as BrowseTheWeb;
                if (browseTheWeb != null)
                {
                    if (!Directory.Exists("Screenshots"))
                    {
                        Directory.CreateDirectory("Screenshots");
                    }
                    var name = nextScreenshotName();
                    ((ITakesScreenshot)browseTheWeb.Driver).GetScreenshot().SaveAsFile($"Screenshots\\{name}.jpg", ImageFormat.Jpeg);
                }
            }
        }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return Actor.AsksFor(question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            return Actor.AsksFor(new TakeScreenshotQuestion<TAnswer, TAbility>(question, NextScreenshotName));
        }

        private sealed class TakeScreenshotQuestion<TAnswer, TAbility> : IQuestion<TAnswer, TAbility>
        {
            private readonly IQuestion<TAnswer, TAbility> _question;
            private readonly Func<string> _nextScreenshotName;
            
            public TakeScreenshotQuestion(IQuestion<TAnswer, TAbility> question, Func<string> nextScreenshotName)
            {
                _question = question;
                _nextScreenshotName = nextScreenshotName;
            }

            public TAnswer AnsweredBy(IActor actor, TAbility ability)
            {
                return ExecuteTakeScreenshot(ability, () => _question.AnsweredBy(actor, ability), _nextScreenshotName);
            }
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return Actor.Execute(action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            return Actor.Execute(new TakeScreenshotAction<TGiven, TWhen, TResult>(action, NextScreenshotName));
        }

        private sealed class TakeScreenshotAction<TGiven, TWhen, TResult> : IAction<TGiven, TWhen, TResult>
        {
            private readonly IAction<TGiven, TWhen, TResult> _action;
            private readonly Func<string> _nextScreenshotName;

            public TakeScreenshotAction(IAction<TGiven, TWhen, TResult> action, Func<string> nextScreenshotName)
            {
                _action = action;
                _nextScreenshotName = nextScreenshotName;
            }

            public TResult ExecuteGivenAs(IActor actor, TGiven ability)
            {
                return ExecuteTakeScreenshot(ability, () => _action.ExecuteGivenAs(actor, ability), _nextScreenshotName);
            }

            public TResult ExecuteWhenAs(IActor actor, TWhen ability)
            {
                return ExecuteTakeScreenshot(ability, () => _action.ExecuteWhenAs(actor, ability), _nextScreenshotName);
            }
        }
    }
}

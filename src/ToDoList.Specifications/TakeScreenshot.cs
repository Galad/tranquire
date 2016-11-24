using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire;
using Tranquire.Selenium;

namespace ToDoList.Specifications
{
    public class TakeScreenshot : IActorFacade
    {
        public IActorFacade Actor { get; }
        private readonly Session _session = new Session();

        private class Session
        {
            public readonly string Name = Guid.NewGuid().ToString();
            public int ScreenshotId;
        }

        public TakeScreenshot(IActorFacade actor)
        {
            if (actor == null) throw new ArgumentNullException(nameof(actor));
            Actor = actor;
        }

        public string Name => Actor.Name;

        private static TResult ExecuteTakeScreenshot<TResult, TAbility>(
            TAbility ability, 
            Func<TResult> execute,
            Session session)
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
                    if(!Directory.Exists("Screenshots"))
                    {
                        Directory.CreateDirectory("Screenshots");
                    }
                    ((ITakesScreenshot)browseTheWeb.Driver).GetScreenshot().SaveAsFile($"Screenshots\\{session.Name}_{(session.ScreenshotId++).ToString()}.jpg", ImageFormat.Jpeg);
                }
            }
        }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            return Actor.AsksFor(question);
        }

        public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            return Actor.AsksFor(new TakeScreenshotQuestion<TAnswer, TAbility>(question, _session));
        }

        private sealed class TakeScreenshotQuestion<TAnswer, TAbility> : IQuestion<TAnswer, TAbility>
        {
            private readonly IQuestion<TAnswer, TAbility> question;
            private readonly Session _session;

            public TakeScreenshotQuestion(IQuestion<TAnswer, TAbility> question, Session session)
            {
                this.question = question;
                _session = session;
            }

            public TAnswer AnsweredBy(IActor actor, TAbility ability)
            {
                return ExecuteTakeScreenshot(ability, () => question.AnsweredBy(actor, ability), _session);
            }
        }

        public TResult AttemptsTo<TResult>(IWhenCommand<TResult> performable)
        {
            return Actor.AttemptsTo(performable);
        }

        public TResult AttemptsTo<T, TResult>(IWhenCommand<T, TResult> performable)
        {
            return Actor.AttemptsTo(performable);
        }

        private sealed class TakeScreenshotWhen<TAbility, TResult> : IWhenCommand<TAbility, TResult>
        {
            private IWhenCommand<TAbility, TResult> command;
            private readonly Session _session;

            public TakeScreenshotWhen(IWhenCommand<TAbility, TResult> command, Session session)
            {
                this.command = command;
                _session = session;
            }

            public TResult ExecuteWhenAs(IActor actor, TAbility ability)
            {
                return ExecuteTakeScreenshot(ability, () => command.ExecuteWhenAs(actor, ability), _session);
            }
        }

        public IActorFacade Can<T>(T doSomething) where T : class
        {
            return Actor.Can(doSomething);
        }

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return Actor.Execute(action);
        }

        public TResult Execute<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
        {
            return Actor.Execute(new TakeScreenshotAction<TGiven, TWhen, TResult>(action, _session));
        }

        private sealed class TakeScreenshotAction<TGiven, TWhen, TResult> : IAction<TGiven, TWhen, TResult>
        {
            private readonly IAction<TGiven, TWhen, TResult> action;
            private readonly Session _session;

            public TakeScreenshotAction(IAction<TGiven, TWhen, TResult> action, Session session)
            {
                this.action = action;
                _session = session;
            }

            public TResult ExecuteGivenAs(IActor actor, TGiven ability)
            {
                return ExecuteTakeScreenshot(ability, () => action.ExecuteGivenAs(actor, ability), _session);
            }

            public TResult ExecuteWhenAs(IActor actor, TWhen ability)
            {
                return ExecuteTakeScreenshot(ability, () => action.ExecuteWhenAs(actor, ability), _session);
            }
        }

        public TResult WasAbleTo<TResult>(IGivenCommand<TResult> performable)
        {
            return Actor.WasAbleTo(performable);
        }

        public TResult WasAbleTo<T, TResult>(IGivenCommand<T, TResult> performable)
        {
            return Actor.WasAbleTo(new TakeScreenshotGiven<T, TResult>(performable, _session));
        }

        private sealed class TakeScreenshotGiven<TAbility, TResult> : IGivenCommand<TAbility, TResult>
        {
            private IGivenCommand<TAbility, TResult> command;
            private readonly Session _session;

            public TakeScreenshotGiven(IGivenCommand<TAbility, TResult> command, Session session)
            {
                this.command = command;
                _session = session;
            }

            public TResult ExecuteGivenAs(IActor actor, TAbility ability)
            {
                return ExecuteTakeScreenshot(ability, () => command.ExecuteGivenAs(actor, ability), _session);
            }
        }
    }
}

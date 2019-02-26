using System;

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
        /// <param name="takeScreenshotStrategy">The strategy used to take screenshots</param>
        public TakeScreenshot(IActor actor, Func<string> nextScreenshotName, IObserver<ScreenshotInfo> screenshotsObserver, ITakeScreenshotStrategy takeScreenshotStrategy)
        {
            Actor = actor ?? throw new ArgumentNullException(nameof(actor));
            NextScreenshotName = nextScreenshotName ?? throw new ArgumentNullException(nameof(nextScreenshotName));
            ScreenshotObserver = screenshotsObserver ?? throw new ArgumentNullException(nameof(screenshotsObserver));
            TakeScreenshotStrategy = takeScreenshotStrategy ?? throw new ArgumentNullException(nameof(takeScreenshotStrategy));
        }

        /// <summary>
        /// Creates a new instance of <see cref="TakeScreenshot"/>
        /// </summary>
        /// <param name="actor">The decorated actor</param>
        /// <param name="nextScreenshotName">A function returning the name of the next screenshot. Each name should be different.</param>
        /// <param name="screenshotsObserver">An observer that can be used to notify that a screenshot was taken</param>
        public TakeScreenshot(IActor actor, Func<string> nextScreenshotName, IObserver<ScreenshotInfo> screenshotsObserver)
            :this(actor, nextScreenshotName, screenshotsObserver, new AlwaysTakeScreenshotStrategy())
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IActor Actor { get; }
        public string Name => Actor.Name;
        public Func<string> NextScreenshotName { get; }
        public IObserver<ScreenshotInfo> ScreenshotObserver { get; }
        public ITakeScreenshotStrategy TakeScreenshotStrategy { get; }

        private static TResult ExecuteTakeScreenshot<TResult, TAbility>(
            TAbility ability,
            IActor actor,
            Func<TResult> execute,
            TakeScreenshot takeScreenshot)
        {
            return takeScreenshot.TakeScreenshotStrategy.ExecuteTakeScreenshot(
                ability,
                actor,
                execute,
                takeScreenshot.NextScreenshotName,
                takeScreenshot.ScreenshotObserver
                );
        }

        public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
        {
            if (question.Name == TakeScreenshotOnErrorStrategy.GetWebBrowserQuestionName)
            {
                return Actor.AsksFor(question);
            }

            return Actor.AsksFor(
                Tranquire.Questions.Create<TAnswer>(
                    question.Name, 
                    a => ExecuteTakeScreenshot(Unit.Default, a, () => question.AnsweredBy(a), this)
                    )
                );
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TAnswer AsksForWithAbility<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
        {
            if (question.Name == TakeScreenshotOnErrorStrategy.GetWebBrowserQuestionName)
            {
                return Actor.AsksForWithAbility(question);
            }
            return Actor.AsksForWithAbility(new TakeScreenshotQuestion<TAnswer, TAbility>(question, this));
        }

        private sealed class TakeScreenshotQuestion<TAnswer, TAbility> : QuestionBase<TAnswer, TAbility>
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
                return ExecuteTakeScreenshot(ability, actor, () => _question.AnsweredBy(actor, ability), _takeScreenshot);
            }

            /// <summary>
            /// Gets the action's name
            /// </summary>
            public override string Name => "[Take screenshot] " + _question.Name;
        }
#pragma warning restore CS0618 // Type or member is obsolete

        public TResult Execute<TResult>(IAction<TResult> action)
        {
            return Actor.Execute(new TakeScreenshotActionUnit<TResult>(action, this));
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public TResult ExecuteWithAbility<TAbility, TResult>(IAction<TAbility, TResult> action)
        {
            return Actor.ExecuteWithAbility(new TakeScreenshotAction<TAbility, TResult>(action, this));
        }

        private sealed class TakeScreenshotAction<TAbility, TResult> : ActionBase<TAbility, TResult>
        {
            private readonly IAction<TAbility, TResult> _action;
            private readonly TakeScreenshot _takeScreenshot;

            public TakeScreenshotAction(
                IAction<TAbility, TResult> action, 
                TakeScreenshot takeScreenshot)
            {
                _action = action;
                _takeScreenshot = takeScreenshot;
            }

            protected override TResult ExecuteGiven(IActor actor, TAbility ability)
            {
                return ExecuteTakeScreenshot(ability, actor, () => _action.ExecuteGivenAs(actor, ability), _takeScreenshot);
            }

            protected override TResult ExecuteWhen(IActor actor, TAbility ability)
            {
                return ExecuteTakeScreenshot(ability, actor, () => _action.ExecuteWhenAs(actor, ability), _takeScreenshot);
            }

            /// <summary>
            /// Gets the action's name
            /// </summary>
            public override string Name => "[Take screenshot] " + _action.Name;
        }

        private sealed class TakeScreenshotActionUnit<TResult> : Tranquire.ActionBase<TResult>
        {
            private readonly IAction<TResult> _action;
            private readonly TakeScreenshot _takeScreenshot;

            public TakeScreenshotActionUnit(
                IAction<TResult> action,
                TakeScreenshot takeScreenshot)
            {
                _action = action;
                _takeScreenshot = takeScreenshot;
            }

            protected override TResult ExecuteGiven(IActor actor)
            {
                return ExecuteTakeScreenshot(Unit.Default, actor, () => _action.ExecuteGivenAs(actor), _takeScreenshot);
            }

            protected override TResult ExecuteWhen(IActor actor)
            {
                return ExecuteTakeScreenshot(Unit.Default, actor, () => _action.ExecuteWhenAs(actor), _takeScreenshot);
            }

            /// <summary>
            /// Gets the action's name
            /// </summary>
            public override string Name => _action.Name;
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

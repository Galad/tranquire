using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow;
using ToDoList.Automation.Actions;
using Tranquire;
using Tranquire.Reporting;
using Tranquire.Selenium;
using Tranquire.Selenium.Extensions;

namespace ToDoList.Specifications
{
    [Binding]
    public class ToDoListSteps : StepsBase
    {
        private readonly StringBuilder _reportingStringBuilder;
        
        public ToDoListSteps(ScenarioContext context) : base(context)
        {
            _reportingStringBuilder = new StringBuilder();
        }

        [BeforeScenario]
        public void Before()
        {
            var options = new ChromeOptions();
            if (IsLiveUnitTesting)
            {
                options.AddArguments("--headless", "--disable-gpu");
            }
            var driver = new ChromeDriver(options);
            var screenshotName = Context.ScenarioInfo.Title;
#if DEBUG
            var delay = IsLiveUnitTesting ? TimeSpan.Zero : TimeSpan.FromSeconds(1);
#else
            var delay = TimeSpan.Zero;
#endif
            if (IsLiveUnitTesting)
            {
                delay = TimeSpan.Zero;
            }
            var actor = new Actor("John")
                            .WithSeleniumReporting(
                                new SeleniumReportingConfiguration(
                                    GetScreenshotsPath(),
                                    screenshotName)
                                    .AddTextObservers(new DebugObserver())
                                    .WithCanNotify(new CanNotify())
                                    .WithTakeScreenshotStrategy(new TakeScreenshotOnErrorStrategy()),                                
                                out var seleniumReporter)
                            .HighlightTargets()
                            .SlowSelenium(delay)
                            .CanUse(WebBrowser.With(driver))
                            .CanUse(seleniumReporter);            
            Context.Set(actor);
            Context.Set(driver);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            actor.Given(Open.TheApplication());
        }

        private string GetScreenshotsPath()
        {
            return Path.Combine(GetTestDirectory(), "Screenshots", Context.ScenarioInfo.Title);
        }

        public static bool IsLiveUnitTesting => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "Microsoft.CodeAnalysis.LiveUnitTesting.Runtime");

        private static string GetTestDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            return Path.GetDirectoryName(codeBasePath);
        }

        [AfterScenario]
        public void After()
        {        
            Debug.WriteLine(_reportingStringBuilder.ToString());
            Context.Actor().Given(new SaveScreenshotsAction());
            var htmReport = Context.Actor().AsksFor(new ActorHtmlReportQuestion());
            Context.Get<ChromeDriver>().Dispose();
            File.WriteAllText(Path.Combine(GetScreenshotsPath(), "Report.html"), htmReport);
        }

        [Given(@"I have an empty to-do list")]
        public void GivenIHaveAnEmptyTo_DoList()
        {
        }

        [Given(@"I add the item ""(.*)""")]
        public void GivenIAddTheItem(string item)
        {
            Context.Actor().Given(ToDoItem.AddAToDoItem(item));
        }

        [When(@"I add the item ""(.*)""")]
        public void WhenIAddTheItem(string item)
        {
            Context.Actor().When(ToDoItem.AddAToDoItem(item));
        }

        [Given(@"I have a list with the items ""(.*)""")]
        public void GivenIHaveAListWithTheItems(ImmutableArray<string> items)
        {
            Context.Actor().Given(ToDoItem.AddToDoItems(items));
        }

        [When(@"I remove the item ""(.*)""")]
        public void WhenIRemoveTheItem(string item)
        {
            Context.Actor().When(ToDoItem.RemoveAToDoItem(item));
        }

        private class ActorHtmlReportQuestion : Question<string, ISeleniumReporter>
        {
            public const string Id = "227F5D1E-F89D-455E-93B4-9A2A1D808545";
            public override string Name => Id;

            protected override string Answer(IActor actor, ISeleniumReporter ability) => ability.GetHtmlDocument();
        }

        private sealed class SaveScreenshotsAction : ActionUnit<ISeleniumReporter>
        {
            public const string Id = "57BF81C4-26A6-45CD-8D70-6F50425CB578";
            public override string Name => Id;

            protected override void ExecuteWhen(IActor actor, ISeleniumReporter ability) => ability.SaveScreenshots();
        }

        private sealed class CanNotify : ICanNotify
        {
            public bool Action<TResult>(IAction<TResult> action)
            {
                return action == null || !action.Name.EndsWith(SaveScreenshotsAction.Id);
            }

            public bool Question<TResult>(IQuestion<TResult> question)
            {
                return question == null || !question.Name.EndsWith(ActorHtmlReportQuestion.Id);
            }
        }
    }
}

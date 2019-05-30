using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using ToDoList.Automation;
using ToDoList.Automation.UI;
using ToDoList.Domain;
using Tranquire;
using Tranquire.Reporting;
using Tranquire.Selenium;
using Tranquire.Selenium.Extensions;

namespace ToDoList.Specifications
{
    [Binding]
    public class Setup : StepsBase
    {
        private static IDisposable WebServer;
        private static IWebHost Host;
        private static Func<HttpClient> ClientFactory;
        private readonly StringBuilder _reportingStringBuilder;

        public Setup(ScenarioContext context) : base(context)
        {
            _reportingStringBuilder = new StringBuilder();
        }

        [BeforeTestRun]
        public static void StartWebServer()
        {
            var testAssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // Remove the "\\bin\\Debug\\netcoreapp2.1"
            var solutionPath = testAssemblyPath.Substring(0, testAssemblyPath.LastIndexOf(@"\bin\", StringComparison.Ordinal));

            // Important to ensure that npm loads and is pointing to correct directory
            Directory.SetCurrentDirectory(Path.Join(solutionPath, @"src\ToDoList"));

            var webHostBuilder = WebHost.CreateDefaultBuilder()
                                        .UseUrls("http://localhost:5000/")
                                        .UseStartup<Startup>();

            if (TestLevel == TestLevel.UI)
            {
                var webServer = webHostBuilder.Build();
                webServer.Start();
                WebServer = Host = webServer;
                ClientFactory = () => new HttpClient() { BaseAddress = new Uri("http://localhost:5000/", UriKind.Absolute) };
            }
            else if (TestLevel == TestLevel.Api)
            {
                var server = new TestServer(webHostBuilder);
                WebServer = server;
                Host = server.Host;
                ClientFactory = () => server.CreateClient();
            }
        }

        [AfterTestRun]
        public static void StopWebServer()
        {
            WebServer.Dispose();
        }

        [StepArgumentTransformation]
        public ImmutableArray<string> TransformToListOfString(string commaSeparatedList)
        {
            return commaSeparatedList.Split(',').ToImmutableArray();
        }

        [Before(Order = 99)]
        public void Before()
        {
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

            var actorSource = new Actor("John");
            IActorFacade actor = actorSource;
            if (TestLevel == TestLevel.UI)
            {
                var driver = CreateWebDriver();
                actor = actorSource.WithSeleniumReporting(
                                        new SeleniumReportingConfiguration(
                                            GetScreenshotsPath(),
                                            screenshotName)
                                            .AddTextObservers(new DebugObserver())
                                            .WithCanNotify(new CanNotify())
                                            .WithScreenshotPng()
                                            .WithTakeScreenshotStrategy(new TakeScreenshotOnErrorStrategy()),
                                        out var seleniumReporter)
                                   .HighlightTargets()
                                   .SlowSelenium(delay)
                                   .CanUse(seleniumReporter)
                                   .CanUse(WebBrowser.With(driver));

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                Context.Set(driver);
            }
            // Indicates what level of automation is supported by the current scenario            
            var levels = new[] { TestLevel.UI, TestLevel.Api }.Where(l => l >= TestLevel).ToArray();
            actor = actor.CanUse<IActionTags<TestLevel>>(ActionTags.Create(levels))
                         .CanUse(ClientFactory());

            Context.Set(actor);

            if (TestLevel == TestLevel.UI)
            {
                actor.Given(Open.TheApplication());
            }
        }

        private static ChromeDriver CreateWebDriver()
        {
            var service = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(typeof(ToDoListSteps).Assembly.Location));
            var options = new ChromeOptions();
            if (IsLiveUnitTesting)
            {
                options.AddArguments("--headless", "--disable-gpu");
            }
            var driver = new ChromeDriver(service, options);
            return driver;
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

        [After]
        public void After()
        {
            var toDoItemRepository = (IToDoItemRepository)Host.Services.GetService(typeof(IToDoItemRepository));
            toDoItemRepository.Clear();
            if (TestLevel != TestLevel.UI)
            {
                return;
            }
            Debug.WriteLine(_reportingStringBuilder.ToString());
            Context.Actor().Given(new SaveScreenshotsAction());
            var htmReport = Context.Actor().AsksFor(new ActorHtmlReportQuestion());
            Context.Get<ChromeDriver>().Dispose();
            File.WriteAllText(Path.Combine(GetScreenshotsPath(), "Report.html"), htmReport);
        }

        private class ActorHtmlReportQuestion : QuestionBase<ISeleniumReporter, string>
        {
            public const string Id = "227F5D1E-F89D-455E-93B4-9A2A1D808545";
            public override string Name => Id;

            protected override string Answer(IActor actor, ISeleniumReporter ability) => ability.GetHtmlDocument();
        }

        private sealed class SaveScreenshotsAction : ActionBaseUnit<ISeleniumReporter>
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

        private class DebugObserver : IObserver<string>
        {
            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(string value)
            {
                Debug.Write(value);
            }
        }

        private static TestLevel TestLevel
        {
            get
            {
                var level = Environment.GetEnvironmentVariable("TEST_LEVEL");
                if (level == null)
                {
                    return TestLevel.UI;
                }

                return Enum.Parse<TestLevel>(level);
            }
        }
    }
}

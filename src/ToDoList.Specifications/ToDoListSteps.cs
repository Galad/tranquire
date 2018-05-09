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
        private IObserver<ScreenshotInfo> _observer;

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
            var delay = TimeSpan.FromSeconds(1);
#else
            var delay = TimeSpan.Zero;
#endif
            if (IsLiveUnitTesting)
            {
                delay = TimeSpan.Zero;
            }
            var debugObserver = new DebugObserver();
            var xmlDocumentReporting = new XmlDocumentObserver();
            var reportingObserver = new CompositeObserver<ActionNotification>(
                xmlDocumentReporting,
                new RenderedReportingObserver(debugObserver, RenderedReportingObserver.DefaultRenderer)
                );
            Context.Set(xmlDocumentReporting);
            _observer = new CompositeObserver<ScreenshotInfo>(
                new SaveScreenshotsToFileOnComplete(Path.Combine(GetTestDirectory(), "Screenshots")),
                new ScreenshotInfoToActionAttachmentObserverAdapter(xmlDocumentReporting),
                new RenderedScreenshotInfoObserver(debugObserver)
                );
            var actor = new Actor("John")
                            .WithReporting(reportingObserver)
                            .TakeScreenshots(screenshotName, _observer)
                            .HighlightTargets()
                            .SlowSelenium(delay)
                            .CanUse(WebBrowser.With(driver));
            Context.Set(actor);
            Context.Set(driver);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            actor.Given(Open.TheApplication());
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
            _observer.OnCompleted();
            Context.Get<ChromeDriver>().Dispose();            
            Debug.WriteLine(_reportingStringBuilder.ToString());
            var xmlDocument = Context.Get<XmlDocumentObserver>();
            Debug.WriteLine(xmlDocument.GetXmlDocument().ToString());            
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
    }
}

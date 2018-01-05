using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Collections.Immutable;
using System.IO;
using TechTalk.SpecFlow;
using ToDoList.Automation.Actions;

namespace ToDoList.Specifications
{
    [Binding]
    public class Setup
    {
        private static IDisposable _webServer;

        [BeforeTestRun]
        public static void StartWebServer()
        {
            _webServer = WebApp.Start(Open.RootUrl, BuildHost);
        }

        [AfterTestRun]
        public static void StopWebServer()
        {
            _webServer.Dispose();
        }

        private static void BuildHost(IAppBuilder builder)
        {
            builder.UseFileServer(new FileServerOptions()
            {
                FileSystem = new PhysicalFileSystem(Path.GetDirectoryName(typeof(Setup).Assembly.Location)),
                EnableDirectoryBrowsing = true
            });
        }

        [StepArgumentTransformation]
        public ImmutableArray<string> TransformToListOfString(string commaSeparatedList)
        {
            return commaSeparatedList.Split(',').ToImmutableArray();
        }
    }
}

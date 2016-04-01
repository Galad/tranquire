using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Owin;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.FileSystems;
using ToDoList.Automation.Actions;
using System.IO;
using ToDoList;

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
    }
}

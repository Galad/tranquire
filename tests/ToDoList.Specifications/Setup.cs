using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Immutable;
using System.IO;
using TechTalk.SpecFlow;

namespace ToDoList.Specifications
{
    [Binding]
    public class Setup
    {
        private static IWebHost _webServer;
        
        [BeforeTestRun]
        public static void StartWebServer()
        {
            var testAssemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // Remove the "\\bin\\Debug\\netcoreapp2.1"
            var solutionPath = testAssemblyPath.Substring(0, testAssemblyPath.LastIndexOf(@"\bin\", StringComparison.Ordinal));            

            // Important to ensure that npm loads and is pointing to correct directory
            Directory.SetCurrentDirectory(Path.Join(solutionPath, @"src\ToDoList"));
            var webHostBuilder = WebHost.CreateDefaultBuilder()
                                        .UseUrls("http://localhost:5000/")
                                        .UseStartup<Startup>();
            _webServer = webHostBuilder.Build();
            _webServer.Start();            
        }

        [AfterTestRun]
        public static void StopWebServer()
        {
            _webServer.Dispose();
        }

        [StepArgumentTransformation]
        public ImmutableArray<string> TransformToListOfString(string commaSeparatedList)
        {
            return commaSeparatedList.Split(',').ToImmutableArray();
        }
    }
}

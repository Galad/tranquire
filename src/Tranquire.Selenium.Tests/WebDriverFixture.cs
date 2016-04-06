using Microsoft.Owin.Hosting;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.Infrastructure;
using Microsoft.Owin.FileSystems;
using System.Threading;

namespace Tranquire.Selenium.Tests
{
    public class WebDriverFixture : IDisposable
    {
        public static int Port = 30000;
        private readonly IDisposable _host;
        public Actor Actor { get; }
        private readonly int _port;

        public WebDriverFixture()
        {
            _port = Interlocked.Increment(ref Port);
            _host = WebApp.Start(RootUrl, BuildHost);
            WebDriver = new FirefoxDriver();
            Actor = new Actor("James");
            Actor.Can(BrowseTheWeb.With(WebDriver));
        }

        public string RootUrl => "http://localhost:" + _port.ToString();

        public void NavigateTo(string localFileName)
        {
            WebDriver.Navigate().GoToUrl($"{RootUrl}/{typeof(WebDriverFixture).Namespace}.{localFileName}");
        }

        private void BuildHost(IAppBuilder builder)
        {
            builder.UseFileServer(new FileServerOptions()
            {
                FileSystem = new EmbeddedResourceFileSystem(typeof(WebDriverFixture).Assembly),
                EnableDirectoryBrowsing = true
            });
        }

        public FirefoxDriver WebDriver { get; }

        public void Dispose()
        {
            WebDriver.Dispose();
            _host.Dispose();
        }
    }
}

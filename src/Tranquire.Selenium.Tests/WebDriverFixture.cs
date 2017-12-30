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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tranquire.Selenium.Tests
{
    
    public sealed class WebDriverFixture : IDisposable
    {
        public static int Port = 30000;
        private readonly IDisposable _host;
        public Actor Actor { get; }
        private readonly int _port;        
        public IWebDriver WebDriver { get; }

        #region Driver creation methods
        private static IWebDriver createChromeDriver()
        {
            //for windows`
            var opts = new ChromeOptions();
            //opts.AddArgument("test-type");
            opts.AddArgument("no-sandbox");
            var driver = new ChromeDriver(opts);

            return driver;
        }
        private static IWebDriver createFirefoxDriver()
        {
            var driver = new FirefoxDriver();

            return driver;
        }
        #endregion

        public static IWebDriver CreateDriver()
        {
            return createFirefoxDriver();
        }
        public WebDriverFixture()
        {
            _port = Interlocked.Increment(ref Port);
            _host = WebApp.Start(RootUrl, BuildHost);
            WebDriver = CreateDriver();

            Actor = (Actor)(new Actor("James").CanUse(WebBrowser.With(WebDriver)));
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<WebDriver>k__BackingField")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            WebDriver.Dispose();
            _host.Dispose();
        }
    }
}

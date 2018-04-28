using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using OpenQA.Selenium.Chrome;
using Owin;
using System;
using System.Linq;
using System.Threading;

namespace Tranquire.Selenium.Tests
{
    public sealed class WebDriverFixture : IDisposable
    {
        public static int Port = 30000;
        private readonly IDisposable _host;
        public Actor Actor { get; }
        private readonly int _port;
        public ChromeDriver WebDriver { get; }

        public WebDriverFixture()
        {
            _port = Interlocked.Increment(ref Port);
            _host = WebApp.Start(RootUrl, BuildHost);
            var options = new ChromeOptions();
            if (IsLiveUnitTesting)
            {                
                options.AddArguments("--headless", "--disable-gpu");
            }
            WebDriver = new ChromeDriver(options);
            Actor = (Actor)(new Actor("James").CanUse(WebBrowser.With(WebDriver)));
        }

        public string RootUrl => "http://localhost:" + _port.ToString();

        public static bool IsLiveUnitTesting => AppDomain.CurrentDomain.GetAssemblies()
            .Any(a => a.GetName().Name == "Microsoft.CodeAnalysis.LiveUnitTesting.Runtime");

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

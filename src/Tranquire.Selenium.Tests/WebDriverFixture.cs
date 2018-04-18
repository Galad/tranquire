using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using OpenQA.Selenium.Chrome;
using Owin;
using System;
using System.Threading;
using Xunit.Abstractions;

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
            WebDriver = new ChromeDriver();
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

        private class XUnitObserver : IObserver<string>
        {
            private readonly ITestOutputHelper _helper;

            public XUnitObserver(ITestOutputHelper helper)
            {
                _helper = helper;
            }

            public void OnCompleted()
            {
                _helper.WriteLine("Completed");
            }

            public void OnError(Exception error)
            {
                _helper.WriteLine("Error " + error.Message);
            }

            public void OnNext(string value)
            {
                _helper.WriteLine(value);
            }
        }

        private class EmptyTestOutputHelper : ITestOutputHelper
        {
            public void WriteLine(string message)
            {
            }

            public void WriteLine(string format, params object[] args)
            {
            }
        }
    }
}

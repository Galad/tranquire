using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium.Chrome;
#if NET48
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
#else
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Tranquire.Selenium.Tests;

public sealed class WebDriverFixture : IDisposable
{
    public static int Port = 30000;
    private readonly IDisposable _host;
    public IActorFacade Actor { get; }
    private readonly int _port;
    public ChromeDriver WebDriver { get; }

    public WebDriverFixture()
    {
        _port = Interlocked.Increment(ref Port);
#if NET48
        _host = WebApp.Start(RootUrl, builder => builder.UseFileServer(new FileServerOptions() {
            FileSystem = new EmbeddedResourceFileSystem(typeof(WebDriverFixture).Assembly),
            EnableDirectoryBrowsing = true
        }));
#else
        var fileProvider = new EmbeddedFileProvider(typeof(WebDriverFixture).Assembly, typeof(WebDriverFixture).Namespace);
        // build host on RootUrl, and serve static files from the current assembly embedded resources.
        var builder = WebHost.CreateDefaultBuilder()
            .UseUrls(RootUrl)
            .Configure(app => app.UseStaticFiles(new StaticFileOptions() {
                FileProvider = fileProvider
            })
            .UseDirectoryBrowser(new DirectoryBrowserOptions() {
                FileProvider= fileProvider,
                RequestPath = ""
            }))
            .ConfigureLogging(c => c.AddDebug());
        builder.ConfigureServices(s => s.AddDirectoryBrowser());
        var host = builder.Build();
        host.Start();
        _host = host;
#endif
        var options = new ChromeOptions();
        options.AddArguments("--headless", "--disable-gpu");
        WebDriver = new ChromeDriver(options);
        Actor = (Actor)(new Actor("James").CanUse(WebBrowser.With(WebDriver)));
    }

    public string RootUrl => "http://localhost:" + _port.ToString();

    public static bool IsLiveUnitTesting => AppDomain.CurrentDomain.GetAssemblies()
        .Any(a => a.GetName().Name == "Microsoft.CodeAnalysis.LiveUnitTesting.Runtime");

    public void NavigateTo(string localFileName)
    {
        WebDriver.Navigate().GoToUrl($"{RootUrl}/{GetResourceFileName(localFileName)}");
    }

    private static string GetResourceFileName(string localFileName)
    {
#if NET48
        return $"{typeof(WebDriverFixture).Namespace}.{localFileName}";
#else
        return localFileName;
#endif
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<WebDriver>k__BackingField")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public void Dispose()
    {
        WebDriver.Dispose();
        _host.Dispose();
    }
}

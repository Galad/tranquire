using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Tranquire.Selenium;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
public partial class WebBrowser : IWebDriver
{
    public string CurrentWindowHandle => Driver.CurrentWindowHandle;
    public string PageSource => Driver.PageSource;
    public string Title => Driver.Title;

#pragma warning disable CA1056 // Implementation of IWebDriver
    public string Url
#pragma warning restore CA1056
    {
        get { return Driver.Url; }
        set { Driver.Url = value; }
    }

    public ReadOnlyCollection<string> WindowHandles => Driver.WindowHandles;

    public void Close()
    {
        Driver.Close();
    }

    public void Dispose()
    {
        Driver.Dispose();
    }

    public IWebElement FindElement(By by)
    {
        return Driver.FindElement(by);
    }

    public ReadOnlyCollection<IWebElement> FindElements(By by)
    {
        return Driver.FindElements(by);
    }

    public IOptions Manage()
    {
        return Driver.Manage();
    }

    public INavigation Navigate()
    {
        return Driver.Navigate();
    }

    public void Quit()
    {
        Driver.Quit();
    }

    public ITargetLocator SwitchTo()
    {
        return Driver.SwitchTo();
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member


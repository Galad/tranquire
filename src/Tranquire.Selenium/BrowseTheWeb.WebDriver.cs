using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Tranquire.Selenium
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public partial class BrowseTheWeb : IWebDriver
    {

        public string CurrentWindowHandle => Driver.CurrentWindowHandle;
        public string PageSource => Driver.PageSource;
        public string Title => Driver.Title;

        public string Url
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
}

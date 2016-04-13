using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium
{
    /// <summary>
    /// Represent the ability to use a web browser with selenium
    /// </summary>
    public partial class BrowseTheWeb : IAbility
    {
        /// <summary>
        /// Gets the Selenium <see cref="IWebDriver"/>
        /// </summary>
        public IWebDriver Driver { get; }
        
        /// <summary>
        /// Creates a new instance of <see cref="BrowseTheWeb"/>
        /// </summary>
        /// <param name="driver">The driver to use to browse the web</param>        
        public BrowseTheWeb(IWebDriver driver)
        {
            Driver = driver;
        }

        /// <summary>
        /// Returns a new <see cref="BrowseTheWeb"/> instance
        /// </summary>
        /// <param name="driver">The driver to use</param>
        /// <returns></returns>
        public static BrowseTheWeb With(IWebDriver driver)
        {
            return new BrowseTheWeb(driver);
        }
    }
}

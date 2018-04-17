using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// Represent a target located by a <see cref="By"/> object
    /// </summary>    
    public class TargetBy : TargetByBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="TargetBy"/>
        /// </summary>
        /// <param name="by">The <see cref="By"/> locator</param>
        /// <param name="name">The target name</param>
        public TargetBy(By by, string name) : base(by, name)
        {
        }

        /// <summary>
        /// Return the <see cref="ISearchContext"/> for the current target
        /// </summary>
        /// <param name="webDriver"></param>
        /// <returns></returns>
        protected override ISearchContext SearchContext(IWebDriver webDriver)
        {
            return webDriver;
        }
    }
}
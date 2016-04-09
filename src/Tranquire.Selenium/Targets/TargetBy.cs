using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    /// <summary>
    /// Represent a target located by a <see cref="By"/> object
    /// </summary>    
    public class TargetBy : TargetByBase
    {
        public TargetBy(By by, string name): base (by, name)
        {
        }

        protected override ISearchContext SearchContext(IActor actor)
        {
            return actor.BrowseTheWeb();
        }
    }
}
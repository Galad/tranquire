using OpenQA.Selenium;

namespace Tranquire.Selenium.Targets
{
    public class TargetBy : TargetByBase
    {
        public TargetBy(By by): base (by)
        {
        }

        protected override ISearchContext SearchContext(IActor actor)
        {
            return actor.BrowseTheWeb();
        }
    }
}
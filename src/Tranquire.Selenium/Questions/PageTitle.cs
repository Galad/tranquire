namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// A question returning the page title
    /// </summary>
    public class PageTitle : IQuestion<string, BrowseTheWeb>
    {
        /// <summary>
        /// Returns the page title
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public string AnsweredBy(IActor actor, BrowseTheWeb ability)
        {
            return ability.Driver.Title;
        }
    }
}
namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// A question returning the page title
    /// </summary>
    public class PageTitle : IQuestion<string, WebBrowser>
    {
        /// <summary>
        /// Returns the page title
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        public string AnsweredBy(IActor actor, WebBrowser ability)
        {
            return ability.Driver.Title;
        }

        /// <summary>
        /// Gets the action's name
        /// </summary>
        public string Name => "Page title";
    }
}
namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Allow to ask question about the page state
    /// </summary>
    public static class Page
    {
        /// <summary>
        /// Returns a question about the page title
        /// </summary>
        /// <returns>A question returning the page title</returns>
        public static IQuestion<string> Title() => new PageTitle();

        /// <summary>
        /// Returns a question about the current URL
        /// </summary>
        /// <returns>A question returning the current URL</returns>
        public static IQuestion<string> Url() => new PageUrl();


        /// <summary>
        /// Gets a question that returns the Page html
        /// </summary>
        public static IQuestion<string> Html() => new PageHtml();
    }
}
namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Provide Html for a WebBrowser
    /// </summary>
    internal class PageHtml : QuestionBase<string, WebBrowser>
    {
        /// <inheritsdoc />
        public override string Name => "What is the current page HTML ?";

        /// <inheritsdoc />
        protected override string Answer(IActor actor, WebBrowser ability)
        {
            return ability.Driver.PageSource;
        }
    }
}

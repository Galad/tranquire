namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// Provide Html for a WebBrowser
    /// </summary>
    public class Html : Question<string, WebBrowser>
    {
        /// <inheritsdoc />
        public override string Name => "What is the current page HTML ?";

        /// <inheritsdoc />
        protected override string Answer(IActor actor, WebBrowser ability)
        {
            return ability.Driver.PageSource;
        }

        /// <summary>
        /// Gets a question that returns the Page html
        /// </summary>
        public static IQuestion<string> OfPage { get; } = new Html();
    }
}

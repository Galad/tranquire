namespace Tranquire.Selenium.Questions
{
    /// <summary>
    /// A question returning the page URL
    /// </summary>
    public class PageUrl : QuestionBase<string, WebBrowser>
    {
        /// <summary>
        ///  Returns the page URL
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="ability"></param>
        /// <returns></returns>
        protected override string Answer(IActor actor, WebBrowser ability)
        {
            return ability.Driver.Url;
        }

        /// <summary>
        /// Gets the action's name
        /// </summary>
        public override string Name => "Page URL";
    }
}

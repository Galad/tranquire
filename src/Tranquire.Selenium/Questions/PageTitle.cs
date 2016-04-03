namespace Tranquire.Selenium.Questions
{
    public class PageTitle : IQuestion<string>
    {
        public string AnsweredBy(IActor actor)
        {
            return actor.BrowseTheWeb().Title;
        }
    }
}
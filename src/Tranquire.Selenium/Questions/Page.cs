namespace Tranquire.Selenium.Questions
{
    public class Page
    {
        public static IQuestion<string> Title()
        {
            return new PageTitle();
        }

        public static IQuestion<string> Url()
        {
            return new PageUrl();
        }
    }
}
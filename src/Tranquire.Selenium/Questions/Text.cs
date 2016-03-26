using OpenQA.Selenium;

namespace Tranquire.Selenium.Questions
{
    public class Text : SingleUIState<string>
    {
        public Text(ITarget target): base (target, new StringConverter())
        {
        }

        public static Text Of(ITarget target)
        {
            return new Text(target);
        }

        protected override string ResolveFor(IWebElement element)
        {
            return element.Text;
        }
    }
}
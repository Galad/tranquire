using Tranquire;
using Tranquire.Selenium;

namespace ToDoList.Automation.Actions
{
    public class Navigate : Tranquire.ActionUnit<WebBrowser>
    {
        private readonly string _url;

        public Navigate(string v)
        {
            this._url = v;
        }

        public static IAction<Unit> To(string siteUrl)
        {
            return new Navigate(siteUrl);
        }

        protected override void ExecuteWhen(IActor actor, WebBrowser ability)
        {
            ability.Navigate().GoToUrl(_url);
        }

        public override string Name => "Navigate to " + _url;
    }
}

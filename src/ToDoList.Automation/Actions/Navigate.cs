using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Automation.Actions
{
    public class Navigate : Tranquire.ActionUnit<BrowseTheWeb>
    {
        private string _url;

        public Navigate(string v)
        {
            this._url = v;
        }

        public static IAction<BrowseTheWeb, BrowseTheWeb, Unit> To(string siteUrl)
        {
            return new Navigate(siteUrl);
        }

        protected override void ExecuteWhen(IActor actor, BrowseTheWeb ability)
        {
            ability.Navigate().GoToUrl(_url);
        }

        public override string ToString()
        {
            return "Navigate to " + _url;
        }
    }
}

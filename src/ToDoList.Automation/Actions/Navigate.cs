using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Automation.Actions
{
    public class Navigate : Tranquire.Action<BrowseTheWeb>
    {
        private string _url;

        public Navigate(string v)
        {
            this._url = v;
        }

        public static IAction<BrowseTheWeb, BrowseTheWeb> To(string siteUrl)
        {
            return new Navigate(siteUrl);
        }

        protected override void ExecuteWhen(IActor actor, BrowseTheWeb ability)
        {
            ability.Navigate().GoToUrl(_url);
        }
    }
}

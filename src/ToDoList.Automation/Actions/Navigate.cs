using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Automation.Actions
{
    public class Navigate : Tranquire.Action
    {
        private string _url;

        public Navigate(string v)
        {
            this._url = v;
        }

        public static IAction ToMyProfile()
        {
            return new Navigate("myprofile");
        }

        public static IAction To(string siteUrl)
        {
            return new Navigate(siteUrl);
        }

        protected override void ExecuteWhen(IActor actor)
        {
            actor.BrowseTheWeb().Navigate().GoToUrl(_url);
        }
    }
}

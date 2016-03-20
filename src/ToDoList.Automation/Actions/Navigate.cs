using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Automation.Actions
{
    public class Navigate : IAction, ITask
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

        public static ITask To(string siteUrl)
        {
            return new Navigate(siteUrl);
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            actor.AbilityTo<BrowseTheWeb>().AsActor(actor).Driver.Navigate().GoToUrl(_url);            
            return actor;
        }
    }
}

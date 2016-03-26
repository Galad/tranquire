using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions.Hits
{
    public class HitId : IAction
    {
        private string id;
        private string _keys;

        public HitId(string id, string _keys)
        {
            this.id = id;
            this._keys = _keys;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            actor.BrowseTheWeb().FindElement(By.Id(id)).SendKeys(_keys);
            return actor;            
        }
    }
}

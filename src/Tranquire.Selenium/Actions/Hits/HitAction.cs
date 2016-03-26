using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tranquire.Selenium.Actions.Hits
{
    public class HitAction : TargetedAction
    {
        private string _keys;

        public HitAction(ITarget target, string keys)
            :base(target)
        {
            Guard.ForNull(keys, nameof(keys));         
            this._keys = keys;
        }

        protected override void ExecuteAction<T>(T actor, IWebElement element)
        {
            element.SendKeys(_keys);
        }
    }
}

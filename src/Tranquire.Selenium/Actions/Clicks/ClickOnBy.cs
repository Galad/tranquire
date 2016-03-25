using OpenQA.Selenium;
using System;

namespace Tranquire.Selenium.Actions.Clicks
{
    public class ClickOnBy : IAction
    {        
        private readonly By _by;

        public ClickOnBy(By by)
        {
            if(by == null)
            {
                throw new ArgumentNullException(nameof(by));
            }
            _by = by;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {            
            actor.BrowseTheWeb()
                 .FindElement(_by)                 
                 .Click();
            return actor;
        }
    }
}
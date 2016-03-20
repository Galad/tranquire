using OpenQA.Selenium;
using System;

namespace Tranquire.Selenium
{
    public class ClickAction : IAction
    {
        private IActor actor;
        private string xpath;

        public ClickAction(string xpath, IActor actor)
        {
            this.xpath = xpath;
            this.actor = actor;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            actor.AbilityTo<BrowseTheWeb>()
                 .Driver
                 .FindElement(By.XPath(xpath))                 
                 .Click();
            return actor;
        }
    }
}
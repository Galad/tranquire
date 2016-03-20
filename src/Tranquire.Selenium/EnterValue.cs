using OpenQA.Selenium;
using Tranquire;
using System;

namespace Tranquire.Selenium
{
    public class EnterValue : Tranquire.IAction
    {
        private readonly string id;
        private readonly string _value;

        public EnterValue(string id, string value)
        {
            this.id = id;
            _value = value;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            actor.AbilityTo<BrowseTheWeb>().Driver.FindElement(By.Id(id)).SendKeys(_value);
            return actor;
        }
    }
}
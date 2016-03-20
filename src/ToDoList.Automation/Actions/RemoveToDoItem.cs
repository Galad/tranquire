using System;
using Tranquire;
using Tranquire.Selenium;
using OpenQA.Selenium;

namespace ToDoList.Automation.Actions
{
    public class RemoveToDoItem : IAction
    {
        private string item;

        public RemoveToDoItem(string item)
        {
            this.item = item;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            var xpath = $"//*[@class='view' and contains(.,'{item}')]//button[@class='destroy']";
            var element = actor.AbilityTo<BrowseTheWeb>().Driver.FindElement(By.XPath(xpath));
            ((IJavaScriptExecutor)actor.AbilityTo<BrowseTheWeb>().Driver).ExecuteScript("arguments[0].click();", element);            
            return actor;
        }
    }
}
using System;
using Tranquire;
using Tranquire.Selenium;
using OpenQA.Selenium;

namespace ToDoList.Automation.Actions
{
    public class RemoveToDoItem : ITask
    {
        private string item;

        public RemoveToDoItem(string item)
        {
            this.item = item;
        }

        public T PerformAs<T>(T actor) where T : IActor
        {
            var element = ToDoPage.RemoveToDoItemButton.Of(item).ResolveFor(actor);
            ((IJavaScriptExecutor)actor.AbilityTo<BrowseTheWeb>().Driver).ExecuteScript("arguments[0].click();", element);            
            return actor;
        }
    }
}
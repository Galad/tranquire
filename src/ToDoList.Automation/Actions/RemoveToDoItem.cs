using System;
using Tranquire;
using Tranquire.Selenium.Actions;

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
            actor.AttemptsTo(JsClick.On(ToDoPage.RemoveToDoItemButton.Of(item)));
            return actor;
        }
    }    
}
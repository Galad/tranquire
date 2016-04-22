using System;
using Tranquire;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.Actions
{
    public class RemoveToDoItem : Tranquire.ActionUnit
    {
        private string item;

        public RemoveToDoItem(string item)
        {
            this.item = item;
        }

        protected override void ExecuteWhen(IActor actor)
        {
            actor.Execute(JsClick.On(ToDoPage.RemoveToDoItemButton.Of(item)));            
        }

        public override string ToString()
        {
            return "Remove an item '" + item + "'";
        }
    }    
}
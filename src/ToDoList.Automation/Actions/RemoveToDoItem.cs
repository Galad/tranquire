using System;
using Tranquire;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.Actions
{
    public class RemoveToDoItem : Tranquire.Action
    {
        private string item;

        public RemoveToDoItem(string item)
        {
            this.item = item;
        }

        protected override void ExecuteWhen(IActionCommand command, IActor actor)
        {
            command.Execute(JsClick.On(ToDoPage.RemoveToDoItemButton.Of(item)));            
        }
    }    
}
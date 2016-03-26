using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium;
using Tranquire.Selenium.Targets;

namespace ToDoList.Automation.Actions
{
    public static class ToDoPage
    {
        public static readonly ITargetWithParameters RemoveToDoItemButton =
            Target.The("Remove an item button")
                  .LocatedBy("//*[@class='view' and contains(.,'{0}')]//button[@class='destroy']", By.XPath);
        public static readonly ITarget NewToDoItemInput = Target.The("New todo item input").LocatedBy(By.Id("new-todo"));
        public static readonly ITarget ToDoItemsLeftCounter = Target.The("To do items left counter").LocatedBy(By.Id("todo-count"));
        public static readonly ITarget ToDoItem = Target.The("To do item").LocatedBy(By.CssSelector("ul.todo-list li"));
        public static readonly ITarget ToDoItemName = Target.The("To do item name").LocatedBy(By.CssSelector("div label"));        
    }
}

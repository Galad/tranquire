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
    }
}

using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.Actions
{
    public class ToDoItem : Tranquire.Task<BrowseTheWeb>
    {
        public ToDoItem(string title):
            base(Enter.TheValue(title).Into(ToDoPage.NewToDoItemInput),
                 Hit.Enter().Into(ToDoPage.NewToDoItemInput))
        {
        }     

        public static IAction RemoveAToDoItem(string item)
        {
            return new RemoveToDoItem(item);
        }

        public static IAction<BrowseTheWeb, BrowseTheWeb> AddAToDoItem(string title)
        {
            return new ToDoItem(title);
        }

        public static IAction<BrowseTheWeb, BrowseTheWeb> AddToDoItems(ImmutableArray<string> items)
        {
            return new Tranquire.Task<BrowseTheWeb>(items.Select(i => new ToDoItem(i)).ToArray());
        }
    }
}

using System;
using Tranquire;
using System.Collections.Immutable;
using Tranquire.Selenium;

namespace ToDoList.Automation.Questions
{
    public class TheItems
    {
        public static IQuestion<ImmutableArray<Model.ToDoItem>, BrowseTheWeb> Displayed()
        {
            return new DisplayedItems();
        }

        public static IQuestion<int> Remaining()
        {
            return new RemainingItems();
        }
    }
}
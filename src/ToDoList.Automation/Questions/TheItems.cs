using System;
using Tranquire;
using System.Collections.Immutable;

namespace ToDoList.Automation.Questions
{
    public class TheItems
    {
        public static IQuestion<ImmutableArray<Model.ToDoItem>> Displayed()
        {
            return new DisplayedItems();
        }

        public static IQuestion<int> Remaining()
        {
            return new RemainingItems();
        }
    }
}
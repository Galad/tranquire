using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Automation.Model;
using Tranquire;

namespace ToDoList.Automation
{
    public static class Get
    {
        public static IQuestion<Task<ImmutableArray<ToDoItem>>> ToDoItems { get; } = Questions.CreateTagged(
            "Gets the to-do items",
            (TestLevel.Api, Api.Get.TheToDoItems),
            (TestLevel.UI, UI.Get.ToDoItems.Select(Task.FromResult))
            );

        public static IQuestion<int> RemainingItems => UI.Get.RemainingItems.Tagged(TestLevel.UI);
    }
}

using System;
using ToDoList.Automation.Actions;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.Questions
{
    public class RemainingItems : IQuestion<int>
    {
        public int AnsweredBy(IActor actor)
        {
            return actor.AsksFor(Text.Of(ToDoPage.ToDoItemsLeftCounter).AsInteger());
        }
    }
}
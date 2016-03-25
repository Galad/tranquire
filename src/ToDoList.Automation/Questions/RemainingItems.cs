using System;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.Questions
{
    public class RemainingItems : IQuestion<int>
    {
        public int AnsweredBy(IActor actor)
        {
            return actor.AsksFor(Text.Of("#todo-count").AsInteger());
        }
    }
}
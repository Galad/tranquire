using System;
using System.Collections.Immutable;
using Tranquire;
using Tranquire.Selenium;
using Tranquire.Selenium.Questions;

namespace ToDoList.Automation.Questions
{
    internal class DisplayedItems : IQuestion<ImmutableArray<string>>
    {
        public ImmutableArray<string> AnsweredBy(IActor actor)
        {
            return actor.AsksFor(Text.Of("ul.todo-list li div label").AsStringArray());
        }
    }
}
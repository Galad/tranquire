using FluentAssertions;
using OpenQA.Selenium.Firefox;
using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TechTalk.SpecFlow;
using ToDoList.Automation.Actions;
using ToDoList.Automation.Questions;

namespace ToDoList.Specifications
{
    [Binding]
    [Scope(Feature = "AddToDoItems")]
    public class AddToDoItemsSteps : ToDoListSteps
    {
        public AddToDoItemsSteps(ScenarioContext context) : base(context)
        {
        }

        [Then(@"the to-do items list should contain ""(.*)""")]
        public void ThenTheTo_DoItemsListShouldContain(string item)
        {
            Context.Actor().AsksFor(TheItems.Displayed()).Should().Contain(item);
        }        

        [Then(@"the to-do items list should not contain ""(.*)""")]
        public void ThenTheTo_DoItemsListShouldNotContain(string item)
        {
            Context.Actor().AsksFor(TheItems.Displayed()).Should().NotContain(item);
        }

        [Then(@"the to-do items list should contain ""(.*)"" (.*) times")]
        public void ThenTheTo_DoItemsListShouldContainTimes(string item, int times)
        {
            Context.Actor().AsksFor(TheItems.Displayed())
                           .Where(i => i == item)
                           .Should()
                           .HaveCount(times, "Expected to have {0} items in collection", times);
        }       
    }

    public static class SpecContext
    {
        public static IActor Actor(this ScenarioContext context)
        {
            return context.Get<IActor>();
        }
    }
}

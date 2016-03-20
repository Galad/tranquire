using FluentAssertions;
using System;
using TechTalk.SpecFlow;
using ToDoList.Automation.Questions;

namespace ToDoList.Specifications
{
    [Binding]
    [Scope(Feature = "RemainingCount")]
    public class RemainingCountSteps : ToDoListSteps
    {
        public RemainingCountSteps(ScenarioContext context) : base(context)
        {
        }

        [Then(@"I should have (.*) items remaining")]
        public void ThenIShouldHaveItemsRemaining(int expectedCount)
        {
            Context.Actor().AsksFor(TheItems.Remaining()).Should().Be(expectedCount);
        }
    }
}

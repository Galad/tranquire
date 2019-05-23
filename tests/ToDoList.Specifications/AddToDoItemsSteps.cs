using FluentAssertions;
using System.Linq;
using TechTalk.SpecFlow;
using ToDoList.Automation.Questions;
using Tranquire;

namespace ToDoList.Specifications
{
    [Binding]
    [Scope(Feature = "AddToDoItems")]
    public class AddToDoItemsSteps : StepsBase
    {
        public AddToDoItemsSteps(ScenarioContext context) : base(context)
        {
        }

        [Then(@"the to-do items list should contain ""(.*)""")]
        public void ThenTheTo_DoItemsListShouldContain(string item)
        {
            Context.Actor().Then(TheItems.Displayed(), items => items.Should().Contain(i => i.Name == item));
        }

        [Then(@"the to-do items list should not contain ""(.*)""")]
        public void ThenTheTo_DoItemsListShouldNotContain(string item)
        {
            Context.Actor().Then(TheItems.Displayed(), items => items.Should().NotContain(i => i.Name == item));
        }

        [Then(@"the to-do items list should contain ""(.*)"" (.*) times")]
        public void ThenTheTo_DoItemsListShouldContainTimes(string item, int times)
        {
            Context.Actor().Then(TheItems.Displayed(), items => 
                           items.Where(i => i.Name == item)
                                .Should()
                                .HaveCount(times, "Expected to have {0} items in collection", times)
                                );
        }
    }

    public static class SpecContext
    {
        public static IActorFacade Actor(this ScenarioContext context)
        {
            return context.Get<IActorFacade>();
        }
    }
}

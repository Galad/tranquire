using FluentAssertions;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using ToDoList.Automation;
using Tranquire;

namespace ToDoList.Specifications
{
    [Binding]
    public class ToDoListSteps : StepsBase
    {
        public ToDoListSteps(ScenarioContext context) : base(context)
        {
        }

        [Given(@"I have an empty to-do list")]
        public void GivenIHaveAnEmptyTo_DoList()
        {
        }

        [Given(@"I add the item ""(.*)""")]
        public async Task GivenIAddTheItem(string item)
        {
            await Context.Actor().Given(Add.TheToDoItem(item));
        }

        [When(@"I add the item ""(.*)""")]
        public async Task WhenIAddTheItem(string item)
        {
            await Context.Actor().When(Add.TheToDoItem(item));
        }

        [Given(@"I have a list with the items ""(.*)""")]
        public async Task GivenIHaveAListWithTheItems(ImmutableArray<string> items)
        {
            await Context.Actor().Given(Add.TheToDoItems(items));
        }

        [When(@"I remove the item ""(.*)""")]
        public async Task WhenIRemoveTheItem(string item)
        {
            await Context.Actor().When(Remove.TheToDoItem(item));
        }

        [Then(@"the to-do items list should contain ""(.*)""")]
        public async Task ThenTheTo_DoItemsListShouldContain(string item)
        {
            await Context.Actor().Then(Get.ToDoItems, async itemsTask =>
            {
                var items = await itemsTask;
                items.Should().Contain(i => i.Name == item);
            });
        }

        [Then(@"the to-do items list should not contain ""(.*)""")]
        public async Task ThenTheTo_DoItemsListShouldNotContain(string item)
        {
            await Context.Actor().Then(Get.ToDoItems, async itemsTask =>
            {
                var items = await itemsTask;
                items.Should().NotContain(i => i.Name == item);
            });
        }

        [Then(@"the to-do items list should contain ""(.*)"" (.*) times")]
        public async Task ThenTheTo_DoItemsListShouldContainTimes(string item, int times)
        {
            await Context.Actor().Then(Get.ToDoItems, async itemsTask =>
            {
                var items = await itemsTask;
                items.Where(i => i.Name == item)
                     .Should()
                     .HaveCount(times, "Expected to have {0} items in collection", times);
            });
        }
    }
}

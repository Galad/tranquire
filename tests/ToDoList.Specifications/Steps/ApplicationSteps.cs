using TechTalk.SpecFlow;
using ToDoList.Automation.UI;

namespace ToDoList.Specifications.Steps
{
    [Binding]
    public class ApplicationSteps : StepsBase
    {
        public ApplicationSteps(ScenarioContext context) : base(context)
        {
        }

        [When(@"I open the application")]
        public void WhenIOpenTheApplication()
        {
            Context.Actor().When(Open.TheApplication());
        }

        [Given(@"the application in opened")]
        public void GivenTheApplicationInOpened()
        {
            Context.Actor().Given(Open.TheApplication());
        }
    }
}

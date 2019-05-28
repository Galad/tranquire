using TechTalk.SpecFlow;
using Tranquire;

namespace ToDoList.Specifications
{
    public static class SpecContext
    {
        public static IActorFacade Actor(this ScenarioContext context)
        {
            return context.Get<IActorFacade>();
        }
    }
}

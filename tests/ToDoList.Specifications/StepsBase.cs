using TechTalk.SpecFlow;

namespace ToDoList.Specifications;

public abstract class StepsBase
{
    public ScenarioContext Context { get; }

    public StepsBase(ScenarioContext context)
    {
        Context = context;
    }
}

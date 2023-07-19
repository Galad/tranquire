using Tranquire;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.UI;

public static class Open
{
    public const string RootUrl = "http://localhost:5000";

    public static IAction<Unit> TheApplication()
    {
        return Navigate.To(RootUrl);
    }
}

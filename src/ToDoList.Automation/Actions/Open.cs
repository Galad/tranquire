using System;
using Tranquire;
using Tranquire.Selenium.Actions;

namespace ToDoList.Automation.Actions
{
    public static class Open
    {
        public const string SiteUrl = RootUrl + "ToDo/index.html";
        public const string RootUrl = "http://localhost:57897/";

        public static IAction<Unit> TheApplication()
        {
            return Navigate.To(new Uri(SiteUrl));
        }
    }
}

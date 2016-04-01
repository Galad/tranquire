using Tranquire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Automation.Actions
{
    public static class Open
    {
        public const string SiteUrl = RootUrl + "/ToDo/";
        public const string RootUrl = "http://localhost:57897/";

        public static IPerformable TheApplication()
        {
            return Navigate.To(SiteUrl);
        }
    }
}

using FluentAssertions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using Tranquire;
using Tranquire.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Automation.Actions;
using ToDoList.Automation.Questions;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var webDriver = new FirefoxDriver())
            //{                
            //    IActor actor = new Actor("John");
            //    actor.Can(BrowseTheWeb.With(webDriver))
            //         .Given(Open.TheApplication())
            //         .When(ToDoItem.AddAToDoItem("buy some milk"));

            //    actor.AsksFor(TheItems.Displayed()).Should().Contain("buy some milk");
            //}
            //using (var a = new WebDriverFixture())
            //{
            //    a.NavigateTo("Targets.html");
            //    Console.Read();
            //}
            //Console.Read();
        }
    }

}

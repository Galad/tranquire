using Moq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions
{
    public class WaitTests : WebDriverTest
    {
        public WaitTests(WebDriverFixture fixture) : base(fixture, "Wait.html")
        {
        }

        [Theory, DomainAutoData]
        public void Sut_IsAction(Wait sut)
        {
            Assert.IsAssignableFrom<Action<BrowseTheWeb>>(sut);
        }
        
        [Theory, DomainAutoData]
        public void Execute_ShouldWait(string id)
        {
            //arrange
            var target = Target.The("element to wait for").LocatedBy(By.Id(id));
            InsertElement(id);
            //act
            Fixture.Actor.AttemptsTo(Wait.UntilTargetIsPresent(target));            
            //assert
            var actual = Answer(Presence.Of(target).Value);
            Assert.True(actual);
        }


        [Theory, DomainAutoData]
        public void Execute_WhenTimeout_ShouldThrow(string id)
        {
            //arrange
            var target = Target.The("element to wait for").LocatedBy(By.Id(id));
            InsertElement(id);
            //act
            Assert.Throws<TimeoutException>(() => Fixture.Actor.AttemptsTo(Wait.UntilTargetIsPresent(target).Timeout(TimeSpan.FromMilliseconds(100))));
        }

        private void InsertElement(string id)
        {
            var js = "var element = document.createElement('div');" +
                                 "element.id = '" + id + "';" +
                                 "element.innerText = '" + id + "';" +
                                 "document.body.appendChild(element)";
            js = "setTimeout(function(){" + js + "}, 1000);";
            Fixture.WebDriver.ExecuteScript(js);
        }

        [Theory, DomainAutoData]
        public void UntilQuestionIsAnswered_ShouldWait(string expected)
        {
            //arrange
            var target = Target.The("element to wait for").LocatedBy(By.Id("ChangeTextElement"));
            ChangeText(expected);
            var question = Text.Of(target).Value;
            //act
            Fixture.Actor.AttemptsTo(Wait.UntilQuestionIsAnswered(question, t => t == expected));
            //arrange
            var actual = Answer(question);
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void UntilQuestionIsAnswered_WhenTimeout_ShouldThrow(string expected)
        {
            //arrange
            var target = Target.The("element to wait for").LocatedBy(By.Id("ChangeTextElement"));
            ChangeText(expected);
            var question = Text.Of(target).Value;
            //act
            Assert.Throws<TimeoutException>(() =>
            Fixture.Actor.AttemptsTo(Wait.UntilQuestionIsAnswered(question, t => t == expected)
                                         .Timeout(TimeSpan.FromMilliseconds(100))
                                    ));
        }

        private void ChangeText(string expected)
        {
            var js = $"var element = document.getElementById('ChangeTextElement').innerText = '{expected}';";
            js = "setTimeout(function(){" + js + "}, 1000);";
            Fixture.WebDriver.ExecuteScript(js);
        }
    }
}

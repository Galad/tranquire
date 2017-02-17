using Moq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public class PageTests : WebDriverTest
    {
        public PageTests(WebDriverFixture fixture) : base(fixture, "Page.html")
        {
        }

        [Fact]
        public void Title_ShouldReturnCorrectValue()
        {
            //arrange
            var expected = Guid.NewGuid().ToString();
            var action = new Mock<CompositeAction>().Object
                .And(Enter.TheValue(expected).Into(Target.The("page title").LocatedBy(By.Id("PageTitle"))))
                .And(Click.On(Target.The("change title button").LocatedBy(By.Id("ChangeTitle"))));
            Fixture.Actor.WasAbleTo(action);
            var question = Page.Title();
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Url_ShouldReturnCorrectValue()
        {
            //arrange
            var expected = Fixture.WebDriver.Url + "?" + Guid.NewGuid().ToString();
            Fixture.WebDriver.Navigate().GoToUrl(expected);
            var question = Page.Url();
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public class Actions : WebDriverTest
    {
        public Actions(WebDriverFixture fixture) : base(fixture, "Actions.html")
        {
            
        }

        [Theory]
        [InlineData("LinkToClick")]
        [InlineData("ButtonToClick")]
        public void Click_ShouldExecuteClick(string buttonId)
        {
            TestClick(buttonId, Click.On);
        }

        [Theory]
        [InlineData("LinkToClick")]
        [InlineData("ButtonToClick")]
        public void JsClick_ShouldExecuteClick(string buttonId)
        {
            TestClick(buttonId, JsClick.On);
        }

        private void TestClick(string buttonId, Func<ITarget, IAction> click)
        {
            //arrange
            Fixture.WebDriver.Navigate().Refresh();
            var inputTarget = Target.The("click input").LocatedBy(By.Id("ClickInput"));
            var clickTarget = Target.The("click target").LocatedBy(By.Id(buttonId));
            var expectedClickContent = Target.The("expected click content").LocatedBy(By.Id("ClickExpectedContent"));
            var expected = Guid.NewGuid().ToString();
            Fixture.Actor.WasAbleTo(Enter.TheValue(expected).Into(inputTarget));
            //act            
            Fixture.Actor.AttemptsTo(click(clickTarget));
            //assert
            var actual = Answer(Text.Of(expectedClickContent).Value);
            Assert.Equal(expected, actual);
        }
    }
}

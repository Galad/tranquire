using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public class ClickWhenTargetIsNotClickableYet : WebDriverTest
    {
        public ClickWhenTargetIsNotClickableYet(WebDriverFixture fixture) : base(fixture, "ClickWhenTargetIsNotClickableYet.html")
        {
        }

        [Theory, DomainAutoData]
        public void Execute_ShouldWait(string expected)
        {
            //arrange
            var target = Target.The("element to wait for").LocatedBy(By.Id("ClickableElement"));
            RemoveOverlay( expected, 1000);
            //act                 
            Fixture.Actor.AttemptsTo(Click.On(target).AllowRetry());
            //assert            
            var actual = Answer(Value.Of(Target.The("expected value").LocatedBy(By.Id("ExpectedValue"))).Value);
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Execute_WhenTimeoutExpires_ShouldThrow(string expected)
        {
            //arrange
            var target = Target.The("element to wait for").LocatedBy(By.Id("ClickableElement"));
            RemoveOverlay(expected, 1500);
            //act and assert           
            Assert.Throws<WebDriverException>(() =>
                Fixture.Actor.AttemptsTo(Click.On(target).AllowRetry().During(TimeSpan.FromMilliseconds(500)))
             );
        }

        private void RemoveOverlay(string expected, int time)
        {
            var js = $"setupTest('{expected}', {time});";
            Fixture.WebDriver.ExecuteScript(js);
        }
    }
}

using OpenQA.Selenium;
using AutoFixture.Xunit2;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions
{
    public class EnterTests : WebDriverTest
    {
        public EnterTests(WebDriverFixture fixture) : base(fixture, "Actions.html")
        {
        }

        [Theory, AutoData]
        public void EnterNewValue_ShouldReturnCorrectValue(string expected)
        {
            //arrange
            var target = Target.The("new value").LocatedBy(By.Id("EnterNewValue"));
            var action = Enter.TheNewValue(expected).Into(target);
            //act
            Fixture.Actor.When(action);
            var actual = Answer(Value.Of(target));
            //assert
            Assert.Equal(expected, actual);
        }
    }
}

﻿using OpenQA.Selenium;
using System;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions
{
    public class ClickWhenTargetIsNotClickableYet : WebDriverTest
    {
        public ClickWhenTargetIsNotClickableYet(WebDriverFixture fixture) : base(fixture, "ClickWhenTargetIsNotClickableYet.html")
        {
        }

        [Theory, DomainAutoData]
        public void Execute_When_ShouldWait(string expected)
        {
            TestExecute(expected, a => Fixture.Actor.When(a));
        }

        [Theory, DomainAutoData]
        public void Execute_Given_ShouldWait(string expected)
        {
            TestExecute(expected, a => Fixture.Actor.Given(a));
        }

        private void TestExecute(string expected, System.Action<Tranquire.IAction<Unit>> execute)
        {
            //arrange
            var target = Target.The("element to wait for").LocatedBy(By.Id("ClickableElement"));
            RemoveOverlay(expected, 1000);
            //act                 
            execute(Click.On(target).AllowRetry());
            //assert            
            var actual = Answer(Value.Of(Target.The("expected value").LocatedBy(By.Id("ExpectedValue"))));
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Execute_WhenTimeoutExpires_ShouldThrow(string expected)
        {
            //arrange
            var target = Target.The("element to wait for").LocatedBy(By.Id("ClickableElement"));
            RemoveOverlay(expected, 1500);
            //act and assert           
            Assert.ThrowsAny<Exception>(() =>
                Fixture.Actor.When(Click.On(target).AllowRetry().During(TimeSpan.FromMilliseconds(500)))
             );
        }

        private void RemoveOverlay(string expected, int time)
        {
            var js = $"setupTest('{expected}', {time});";
            Fixture.WebDriver.ExecuteScript(js);
        }
    }
}

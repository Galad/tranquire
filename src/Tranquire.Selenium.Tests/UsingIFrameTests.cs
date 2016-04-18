using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Actions;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public class UsingIFrameTests : WebDriverTest
    {
        public UsingIFrameTests(WebDriverFixture fixture) : base(fixture, "PageWithIFrame.html")
        {
        }

        [Fact]
        public void UsingIFrame_ShouldAllowToReachElementsInIFrame()
        {
            //arrange
            var iframe = Target.The("iframe").LocatedBy(By.Id("IFrame"));
            var expectedElement = Target.The("element in iframe").LocatedBy(By.Id("ElementInIFrame"));
            //act
            UsingIFrame.LocatedBy(iframe).ExecuteFor(Fixture.Actor);
            var element = expectedElement.ResolveFor(Fixture.WebDriver);
            //assert
            Assert.NotNull(element);
        }

        [Fact]
        public void UsingIFrame_WhenDisposed_ShouldAllowToReachElementsOutsideIFrame()
        {
            //arrange
            var iframe = Target.The("iframe").LocatedBy(By.Id("IFrame"));
            var expectedElement = Target.The("element outside iframe").LocatedBy(By.Id("ElementOutsideIFrame"));
            //act
            UsingIFrame.LocatedBy(iframe).ExecuteFor(Fixture.Actor).Dispose();
            var element = expectedElement.ResolveFor(Fixture.WebDriver);
            //assert
            Assert.NotNull(element);
        }
    }
}

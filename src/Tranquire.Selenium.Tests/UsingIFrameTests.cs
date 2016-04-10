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
            UsingIFrame.For(Fixture.Actor).LocatedBy(iframe);
            var element = expectedElement.ResolveFor(Fixture.Actor);
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
            UsingIFrame.For(Fixture.Actor).LocatedBy(iframe).Dispose();
            var element = expectedElement.ResolveFor(Fixture.Actor);
            //assert
            Assert.NotNull(element);
        }
    }
}

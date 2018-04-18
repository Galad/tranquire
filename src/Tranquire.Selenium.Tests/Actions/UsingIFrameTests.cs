using OpenQA.Selenium;
using System;
using Tranquire.Selenium.Actions;
using Xunit;
using Xunit.Abstractions;

namespace Tranquire.Selenium.Tests.Actions
{
    public class UsingIFrameTests : WebDriverTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public UsingIFrameTests(WebDriverFixture fixture, ITestOutputHelper testOutputHelper) : base(fixture, "PageWithIFrame.html")
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void UsingIFrame_ShouldAllowToReachElementsInIFrame()
        {
            //arrange
            var iframe = Target.The("iframe").LocatedBy(By.Id("IFrame"));
            var expectedElement = Target.The("element in iframe").LocatedBy(By.Id("ElementInIFrame"));
            //act            
            ExecuteWithRetry(() =>
            {                
                using (Fixture.Actor.When(UsingIFrame.LocatedBy(iframe)))
                {
                    var element = expectedElement.ResolveFor(Fixture.WebDriver);
                    //assert
                    Assert.NotNull(element);
                }
            });
        }

        [Fact]
        public void UsingIFrame_WhenDisposed_ShouldAllowToReachElementsOutsideIFrame()
        {
            //arrange
            var iframe = Target.The("iframe").LocatedBy(By.Id("IFrame"));
            var expectedElement = Target.The("element outside iframe").LocatedBy(By.Id("ElementOutsideIFrame"));
            //act      
            ExecuteWithRetry(() =>
            {                
                Fixture.Actor.When(UsingIFrame.LocatedBy(iframe)).Dispose();
            });
            var element = expectedElement.ResolveFor(Fixture.WebDriver);
            //assert
            Assert.NotNull(element);
        }

        private void ExecuteWithRetry(Action action)
        {
            var start = DateTimeOffset.Now;            
            while (true)
            {
                try
                {
                    action();                    
                    return;
                }
                catch (NoSuchFrameException) when (DateTimeOffset.Now.Subtract(start) < TimeSpan.FromSeconds(5))
                {
                    testOutputHelper.WriteLine(Fixture.WebDriver.PageSource);
                    System.Threading.Thread.Sleep(200);
                    ReloadPage();
                }
            }
        }
    }
}

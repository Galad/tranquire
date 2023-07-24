using System;
using OpenQA.Selenium;
using Tranquire.Selenium.Actions;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions;

public class UsingIFrameTests : WebDriverTest
{
#if NET48
    private const string IFrameId = "IFrame";
#else
    private const string IFrameId = "IFrameShortName";
#endif
    public UsingIFrameTests(WebDriverFixture fixture) : base(fixture, "PageWithIFrame.html")
    {
    }

    [Fact]
    public void UsingIFrame_ShouldAllowToReachElementsInIFrame()
    {
        //arrange
        var iframe = Target.The("iframe").LocatedBy(By.Id(IFrameId));
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
        var iframe = Target.The("iframe").LocatedBy(By.Id(IFrameId));
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
        // Switching to an iframe seems to fail sometimes, so we reload the page until it stops failing
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
                System.Threading.Thread.Sleep(200);
                ReloadPage();
            }
        }
    }
}

using OpenQA.Selenium;
using Tranquire.Selenium.Actions;
using Tranquire.Selenium.Questions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions;

public class OpenContextMenuTests : WebDriverTest
{
    public OpenContextMenuTests(WebDriverFixture fixture) : base(fixture, "OpenContextMenu.html")
    {
    }

    [Theory, DomainAutoData]
    public void Execute_ShouldOpenContextMenu(string expected)
    {
        //arrange
        var js = $"setupTest('{expected}');";
        Fixture.WebDriver.ExecuteScript(js);
        var target = Target.The("element where to open the context menu").LocatedBy(By.Id("ClickableElement"));
        var expectedClickContent = Target.The("expected click content").LocatedBy(By.Id("ExpectedValue"));
        var action = OpenContextMenu.On(target);
        //act
        Fixture.Actor.When(action);
        //assert
        var actual = Answer(Value.Of(expectedClickContent));
        Assert.Equal(expected, actual);
    }
}

using System.Linq;
using Moq;
using OpenQA.Selenium;
using Xunit;

namespace Tranquire.Selenium.Tests.Targets;

[Trait("Category", "")]
public class TargetsTests : WebDriverTest
{
    public const string PElementId = "pelement";
    public const string PElementText = "A simple p element text";
    public const string PElementClass = "pelementclass";
    public const string UlElement = "ulelement";
    public static readonly string[] LiElementsContent = new[] { "Red", "Blue", "Yellow", "Green", "Black" };
    public const string YellowClass = "yellow";
    public const string YellowContent = "Yellow";
    public const string DivContainer = "divcontainer";
    public const string RelativeText = "Relative Text";
    public const string RelativeText2 = "Relative Text2";
    public const string MultipleRelativeText = "Multiple Relative Text";

    public TargetsTests(WebDriverFixture fixture) : base(fixture, "Targets.html")
    {
    }

    [Fact]
    public void ResolveFor_LocatedById_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target by id")
                           .LocatedBy(By.Id(PElementId))
                           .ResolveFor(Fixture.WebDriver);
        Assert.Equal(PElementText, actual.Text);
    }

    [Fact]
    public void ResolveFor_LocatedByCssId_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target by css")
                           .LocatedBy(By.CssSelector("#" + PElementId))
                           .ResolveFor(Fixture.WebDriver);
        Assert.Equal(PElementText, actual.Text);
    }

    [Fact]
    public void ResolveFor_LocatedByCssClass_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target by class")
                           .LocatedBy(By.CssSelector("." + PElementClass))
                           .ResolveFor(Fixture.WebDriver);
        Assert.Equal(PElementText, actual.Text);
    }

    [Fact]
    public void ResolveFor_LocatedByFormattableId_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target formattable by id")
                           .LocatedBy(PElementId, s => By.Id(s))
                           .Of()
                           .ResolveFor(Fixture.WebDriver);
        Assert.Equal(PElementText, actual.Text);
    }

    [Fact]
    public void ResolveFor_LocatedByFormattableId_WithParameters_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target formattable by id with parameter")
                           .LocatedBy("{0}", s => By.Id(s))
                           .Of(PElementId)
                           .ResolveFor(Fixture.WebDriver);
        Assert.Equal(PElementText, actual.Text);
    }

    [Fact]
    public void ResolveFor_LocatedByFormattableIdGeneric_WithParameters_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target formattable by id with parameter")
                           .LocatedBy<string>(s => By.Id(s))
                           .Of(PElementId)
                           .ResolveFor(Fixture.WebDriver);
        Assert.Equal(PElementText, actual.Text);
    }

    [Fact]
    public void ResolveAllFor_LocatedByCss_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target by id")
                           .LocatedBy(By.CssSelector($"ul#{UlElement} li"))
                           .ResolveAllFor(Fixture.WebDriver)
                           .Select(w => w.Text);
        Assert.Equal(LiElementsContent, actual);
    }

    [Fact]
    public void ResolveAllFor_LocatedByFormattableCss_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target by id")
                           .LocatedBy($"ul#{UlElement} li", s => By.CssSelector(s))
                           .Of()
                           .ResolveAllFor(Fixture.WebDriver)
                           .Select(w => w.Text);
        Assert.Equal(LiElementsContent, actual);
    }

    [Fact]
    public void ResolveAllFor_LocatedByFormattableCss_WithParameters_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target by id")
                           .LocatedBy("ul#{0} li", s => By.CssSelector(s))
                           .Of(UlElement)
                           .ResolveAllFor(Fixture.WebDriver)
                           .Select(w => w.Text);
        Assert.Equal(LiElementsContent, actual);
    }

    [Fact]
    public void ResolveAllFor_LocatedByFormattableXPath_WithParameters_ShouldReturnCorrectValue()
    {
        var actual = Target.The("target by id")
                           .LocatedBy("//ul[@id='{0}']/li[not(contains(@class, '{1}'))]", s => By.XPath(s))
                           .Of(UlElement, YellowClass)
                           .ResolveAllFor(Fixture.WebDriver)
                           .Select(w => w.Text);
        Assert.Equal(LiElementsContent.Where(s => s != YellowContent), actual);
    }

    [Fact]
    public void ResolveFor_LocatedByCss_RelativeToOtherTarget_ShouldReturnCorrectValue()
    {
        var containerTarget = Target.The("container")
                                    .LocatedBy(By.Id(DivContainer));

        var actual = Target.The("relative text")
                           .LocatedBy(By.CssSelector("div p"))
                           .RelativeTo(containerTarget)
                           .ResolveFor(Fixture.WebDriver)
                           .Text;
        Assert.Equal(RelativeText, actual);
    }

    [Fact]
    public void ResolveFor_RelativeTo_WithMultipleTargets_ShouldReturnCorrectValue()
    {
        // use xpath so that targets are relative and not combined CSS selectors
        var container1 = Target.The("container1").LocatedBy(By.XPath("//div[@class='relative1']"));
        var container2 = Target.The("container2").LocatedBy(By.XPath("div[@class='relative2']"));
        var container3 = Target.The("container3").LocatedBy(By.XPath("div[@class='relative3']"));
        var actual = Target.The("relative text")
                           .LocatedBy(By.XPath("p[@class='final']"))
                           .RelativeTo(container3)
                           .RelativeTo(container2)
                           .RelativeTo(container1)
                           .ResolveFor(Fixture.WebDriver)
                           .Text;
        Assert.Equal(RelativeText2, actual);
    }

    [Fact]
    public void ResolveAllFor_RelativeTo_WithMultipleTargets_ShouldReturnCorrectValue()
    {
        // use xpath so that targets are relative and not combined CSS selectors
        var container1 = Target.The("container1").LocatedBy(By.XPath("//div[@class='multiple-relative1']"));
        var container2 = Target.The("container2").LocatedBy(By.XPath("div[@class='multiple-relative2']"));
        var container3 = Target.The("container3").LocatedBy(By.XPath("div[@class='multiple-relative3']"));
        var actual = Target.The("relative text")
                           .LocatedBy(By.XPath("p[@class='final']"))
                           .RelativeTo(container3)
                           .RelativeTo(container2)
                           .RelativeTo(container1)
                           .ResolveAllFor(Fixture.WebDriver)
                           .Select(e => e.Text)
                           .ToArray();
        Assert.Equal(Enumerable.Repeat(MultipleRelativeText, 4), actual);
    }

    [Fact]
    public void ResolveFor_LocatedByWebElement_ShouldReturnCorrectValue()
    {
        //arrange
        var expected = new Mock<IWebElement>();
        var sut = Target.The("element")
                        .LocatedByWebElement(expected.Object);
        //act
        var actual = sut.ResolveFor(Fixture.WebDriver);
        //assert
        Assert.Equal(expected.Object, actual);
    }

    [Fact]
    public void ResolveAllFor_LocatedByWebElement_ShouldReturnCorrectValue()
    {
        //arrange
        var element = new Mock<IWebElement>();
        var sut = Target.The("element")
                        .LocatedByWebElement(element.Object);
        //act
        var actual = sut.ResolveAllFor(Fixture.WebDriver);
        //assert
        var expected = new[] { element.Object };
        Assert.Equal(expected, actual);
    }
}

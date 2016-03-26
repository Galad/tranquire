using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public class TargetsTests : IClassFixture<WebDriverFixture>
    {
        private readonly WebDriverFixture _fixture;

        public const string PElementId = "pelement";
        public const string PElementText = "A simple p element text";
        public const string PElementClass = "pelementclass";
        public const string UlElement = "ulelement";
        public static readonly string[] LiElementsContent = new[] { "Red", "Blue", "Yellow", "Green", "Black" };
        public static readonly string YellowClass = "yellow";
        public static readonly string YellowContent = "Yellow";

        public TargetsTests(WebDriverFixture fixture)
        {
            fixture.NavigateTo("Targets.html");
            _fixture = fixture;
        }

        [Fact]
        public void ResolveFor_LocatedById_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy(By.Id(PElementId))
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveFor_LocatedByCssId_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by css")
                               .LocatedBy(By.CssSelector("#" + PElementId))
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveFor_LocatedByCssClass_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by class")
                               .LocatedBy(By.CssSelector("." + PElementClass))
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveFor_LocatedByFormattableId_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target formattable by id")
                               .LocatedBy(s => By.Id(s), PElementId)
                               .Of()
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveFor_LocatedByFormattableId_WithParameters_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target formattable by id with parameter")
                               .LocatedBy(s => By.Id(s), "{0}")
                               .Of(PElementId)
                               .ResolveFor(_fixture.Actor);
            Assert.Equal(PElementText, actual.Text);
        }

        [Fact]
        public void ResolveAllFor_LocatedByCss_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy(By.CssSelector($"ul#{UlElement} li"))
                               .ResoveAllFor(_fixture.Actor)
                               .Select(w => w.Text);
            Assert.Equal(LiElementsContent, actual);
        }

        [Fact]
        public void ResolveAllFor_LocatedByFormattableCss_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy(s => By.CssSelector(s), $"ul#{UlElement} li")
                               .Of()
                               .ResoveAllFor(_fixture.Actor)
                               .Select(w => w.Text);
            Assert.Equal(LiElementsContent, actual);
        }

        [Fact]
        public void ResolveAllFor_LocatedByFormattableCss_WithParameters_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy(s => By.CssSelector(s), "ul#{0} li")
                               .Of(UlElement)
                               .ResoveAllFor(_fixture.Actor)
                               .Select(w => w.Text);
            Assert.Equal(LiElementsContent, actual);
        }

        [Fact]
        public void ResolveAllFor_LocatedByFormattableXPath_WithParameters_ShouldReturnCorrectValue()
        {
            var actual = Target.The("target by id")
                               .LocatedBy(s => By.XPath(s), "//ul[@id='{0}']/li[not(contains(@class, '{1}'))]")
                               .Of(UlElement, YellowClass)
                               .ResoveAllFor(_fixture.Actor)
                               .Select(w => w.Text);
            Assert.Equal(LiElementsContent.Where(s => s != YellowContent), actual);
        }
    }
}

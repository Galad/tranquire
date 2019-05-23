using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using Moq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Targets;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Targets
{
    public class TargetsUnitTests
    {
        [Theory]
        [DomainInlineAutoData(typeof(RelativeTarget))]
        [DomainInlineAutoData(typeof(TargetBy))]
        [DomainInlineAutoData(typeof(TargetByWebElement))]
        [DomainInlineAutoData(typeof(TargetBuilder))]
        [DomainInlineAutoData(typeof(TargetByParameterizable))]
        [DomainInlineAutoData(typeof(TargetByParameterizable<string>))]
        [DomainInlineAutoData(typeof(Target))]
        public void Sut_VerifyGuardClauses(Type type, GuardClauseAssertion assertion)
        {
            assertion.Verify(type);
        }

        [Theory]
        [DomainInlineAutoData(typeof(RelativeTarget))]
        [DomainInlineAutoData(typeof(TargetBy))]
        [DomainInlineAutoData(typeof(TargetByWebElement))]
        public void ToString_ShouldContainName(Type type, IFixture fixture)
        {
            // act
            var target = fixture.Create(type, new SpecimenContext(fixture)) as ITarget;
            // assert
            Assert.Contains(target.Name, target.ToString());
        }

        [Fact]
        public void RelativeTo_LocatedByWebElement_ShouldReturnCorrectValue()
        {
            //arrange
            var element = new Mock<IWebElement>();
            var sut = Target.The("element")
                            .LocatedByWebElement(element.Object);
            var other = Target.The("other target")
                              .LocatedBy(By.Id("id"));
            //act
            var actual = sut.RelativeTo(other);
            //assert
            Assert.Equal(sut, actual);
        }

        public static IEnumerable<object[]> RelativeToComposableCss
        {
            get
            {
                return new[]
                {
                    new object[] { By.Id("a"), By.Id("b"), "#a #b" },
                    new object[] { By.Id("a"), By.ClassName("b"), "#a .b" },
                    new object[] { By.ClassName("a"), By.Id("b"), ".a #b" },
                    new object[] { By.CssSelector("#a .b c"), By.Id("d"), "#a .b c #d" },
                    new object[] { By.CssSelector("#a .b c"), By.ClassName("d"), "#a .b c .d" },
                    new object[] { By.CssSelector("#a .b c"), By.CssSelector("d #e .f"), "#a .b c d #e .f" },
                    new object[] { By.Name("a"), By.Id("b"), "[name=a] #b" },
                    new object[] { By.TagName("a"), By.Id("b"), "a #b" }                    
                };
            }
        }

        [Theory]
        [MemberData(nameof(RelativeToComposableCss))]
        public void RelativeTo_WithComposableBy_ShouldReturnByCss(By sourceBy, By relativeBy, string expectedCss)
        {
            // arrange
            var source = Target.The("source").LocatedBy(sourceBy);
            var relative = Target.The("relative").LocatedBy(relativeBy);
            // act
            var actual = relative.RelativeTo(source) as TargetBy;
            // assert            
            Assert.NotNull(actual);
            var expected = By.CssSelector(expectedCss);
            Assert.Equal(expected, actual.By);
        }

        private class ByCustom : By
        {
            public override string ToString() => "By something else";
        }

        public static IEnumerable<object[]> RelativeToNotComposableCss
        {
            get
            {
                var element = new Mock<IWebElement>().Object;
                return new[]
                {
                    new object[] { Target.The("source").LocatedBy(By.Id("a")), Target.The("relative").LocatedBy(By.LinkText("b")) },
                    new object[] { Target.The("source").LocatedBy(By.Id("a")), Target.The("relative").LocatedBy(By.PartialLinkText("b")) },
                    new object[] { Target.The("source").LocatedBy(By.Id("a")), Target.The("relative").LocatedBy(By.XPath("b")) },
                    new object[] { Target.The("source").LocatedBy(By.LinkText("a")), Target.The("relative").LocatedBy(By.Id("b")) },
                    new object[] { Target.The("source").LocatedBy(By.PartialLinkText("a")), Target.The("relative").LocatedBy(By.Id("b")) },
                    new object[] { Target.The("source").LocatedBy(By.XPath("a")), Target.The("relative").LocatedBy(By.Id("b")) },
                    new object[] { Target.The("source").LocatedByWebElement(element), Target.The("relative").LocatedBy(By.Id("b")) },
                    new object[] { Target.The("source").LocatedBy(new ByCustom()), Target.The("relative").LocatedBy(By.Id("a")) }
                };
            }
        }

        [Theory]
        [MemberData(nameof(RelativeToNotComposableCss))]
        public void RelativeTo_WithNotComposableBy_ShouldReturnRelativeTo(ITarget source, ITarget relative)
        {
            // arrange
            // act
            var actual = relative.RelativeTo(source) as RelativeTarget;
            // assert            
            Assert.NotNull(actual);
            Assert.Equal(source, actual.TargetSource);
        }

        #region Name
        [Theory, AutoData]
        public void Name_LocatedBy_ShouldReturnCorrectValue(string expected)
        {
            // arrange
            var sut = Target.The(expected).LocatedBy(By.Id("id"));
            // act
            var actual = sut.Name;
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void Name_LocatedByFormattable_ShouldReturnCorrectValue(string expected)
        {
            // arrange
            var sut = Target.The(expected).LocatedBy("{0}", By.Id).Of("id");
            // act
            var actual = sut.Name;
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Name_LocatedByWebElement_ShouldReturnCorrectValue(IWebElement webElement, string expected)
        {
            // arrange
            var sut = Target.The(expected).LocatedByWebElement(webElement);
            // act
            var actual = sut.Name;
            // assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region ToString
        [Theory, AutoData]
        public void ToString_LocatedBy_ShouldReturnCorrectValue(string name)
        {
            // arrange
            var sut = Target.The(name).LocatedBy(By.Id("id"));
            // act
            var actual = sut.ToString();
            // assert
            var expected = $"{name} ({By.Id("id").ToString()})";
            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void ToString_LocatedByFormattable_ShouldReturnCorrectValue(string name)
        {
            // arrange
            var sut = Target.The(name).LocatedBy("{0}", By.Id).Of("id");
            // act
            var actual = sut.ToString();
            // assert
            var expected = $"{name} ({By.Id("id").ToString()})";
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ToString_LocatedByWebElement_ShouldReturnCorrectValue(IWebElement webElement, string name)
        {
            // arrange
            var sut = Target.The(name).LocatedByWebElement(webElement);
            // act
            var actual = sut.ToString();
            // assert
            var expected = $"{name} (web element: {webElement.TagName})";
            Assert.Equal(expected, actual);
        }
        #endregion
    }
}

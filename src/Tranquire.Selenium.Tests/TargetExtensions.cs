using AutoFixture.Idioms;
using Moq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public class TargetExtensionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(TargetExtensions));
        }

        [Theory, DomainAutoData]
        public void ResolveFor_ShouldCallSearchContextFindElement(ISearchContext searchContext, IWebElement expected)
        {
            // arrange
            var target = Target.The("target").LocatedBy(By.Id("a"));
            Mock.Get(searchContext).Setup(e => e.FindElement(By.Id("a"))).Returns(expected);
            // act
            var actual = target.ResolveFor(searchContext);
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ResolveAllFor_ShouldCallSearchContextFindElement(ISearchContext searchContext, IWebElement[] expected)
        {
            // arrange
            var target = Target.The("target").LocatedBy(By.Id("a"));
            Mock.Get(searchContext).Setup(e => e.FindElements(By.Id("a"))).Returns(new ReadOnlyCollection<IWebElement>(expected));
            // act
            var actual = target.ResolveAllFor(searchContext);
            // assert
            Assert.Equal(expected, actual);
        }
    }
}

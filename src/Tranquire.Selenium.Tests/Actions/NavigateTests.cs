using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OpenQA.Selenium;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using Tranquire.Selenium.Actions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions
{
    public class NavigateTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Navigate));
        }

        [Theory, DomainAutoData]
        public void To_ShouldReturnCorrectValue(Uri uri)
        {
            var actual = Navigate.To(uri);
            actual.ShouldBeEquivalentTo(new Navigate(uri));
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(Navigate sut)
        {
            sut.Name.Should().Be($"Navigate to {sut.Uri}");
        }
        
        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldReturnCorrectValue(            
            Navigate sut,
            [Frozen]IWebDriver webDriver,
            WebBrowser ability,
            Mock<INavigation> navigation,
            IActor actor)
        {
            //arrange
            Mock.Get(webDriver).Setup(w => w.Navigate()).Returns(navigation.Object);
            //act
            sut.ExecuteGivenAs(actor, ability);
            //assert
            navigation.Verify(n => n.GoToUrl(sut.Uri));
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldReturnCorrectValue(
            Navigate sut,
            [Frozen]IWebDriver webDriver,
            WebBrowser ability,
            Mock<INavigation> navigation,
            IActor actor)
        {
            //arrange
            Mock.Get(webDriver).Setup(w => w.Navigate()).Returns(navigation.Object);
            //act
            sut.ExecuteWhenAs(actor, ability);
            //assert
            navigation.Verify(n => n.GoToUrl(sut.Uri));
        }
    }
}

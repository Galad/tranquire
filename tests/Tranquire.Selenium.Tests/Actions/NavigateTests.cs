using System;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using OpenQA.Selenium;
using Tranquire.Selenium.Actions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions;

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
        actual.Should().BeEquivalentTo(new Navigate(uri));
    }

    [Theory, DomainAutoData]
    public void To_StringUrl_ShouldReturnCorrectValue(Uri uri)
    {
        var actual = Navigate.To(uri.ToString());
        actual.Should().BeEquivalentTo(new Navigate(uri));
    }

    [Theory]
    [DomainInlineAutoData("")]
    [DomainInlineAutoData("invalid")]
    [DomainInlineAutoData("/relative")]
    public void To_StringUrl_WithInvalidUrl_ShouldReturnCorrectValue(string url)
    {
        var ex = Assert.Throws<ArgumentException>("url", () => Navigate.To(url));
        Assert.StartsWith($"The URL {url} is invalid. The URL should be a valid absolute URL, such as http://localhost/myApp/", ex.Message);
    }

    [Theory, DomainAutoData]
    public void Name_ShouldReturnCorrectValue(Navigate sut)
    {
        sut.Name.Should().Be($"Navigate to {sut.Uri}");
    }

    [Theory, DomainAutoData]
    public void ExecuteGivenAs_ShouldReturnCorrectValue(
        Navigate sut,
        [Frozen] IWebDriver webDriver,
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
        [Frozen] IWebDriver webDriver,
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

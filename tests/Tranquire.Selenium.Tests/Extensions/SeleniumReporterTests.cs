using System;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Tranquire.Reporting;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Extensions;

public class SeleniumReporterTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(SeleniumReporter));
    }

    [Theory, DomainAutoData]
    public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
    {
        assertion.Verify<SeleniumReporter>(r => r.ScreenshotInfoObserver);
        assertion.Verify<SeleniumReporter>(r => r.XmlDocumentObserver);
    }

    [Theory, DomainAutoData]
    public void Sut_IsSeleniumReporter(SeleniumReporter sut)
    {
        sut.Should().BeAssignableTo<ISeleniumReporter>();
    }

    [Theory, DomainAutoData]
    public void GetXmlDocument_ShouldReturnCorrectValue(
        [Frozen] IMeasureDuration measureDuration,
        [Greedy] XmlDocumentObserver expectedDocumentObserver,
        INamed named,
        IFixture fixture,
        DateTimeOffset date)
    {
        // arrange
        fixture.Customize<XmlDocumentObserver>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
        var sut = fixture.Create<SeleniumReporter>();
        Mock.Get(measureDuration).Setup(m => m.Now).Returns(date);
        SetupObserver(expectedDocumentObserver, named);
        SetupObserver(sut.XmlDocumentObserver, named);
        var expected = expectedDocumentObserver.GetXmlDocument();
        // act
        var actual = sut.GetXmlDocument();
        // assert
        actual.ToString().Should().Be(expected.ToString());
    }

    [Theory, DomainAutoData]
    public void GetHtmlDocument_ShouldReturnCorrectValue(
        SeleniumReporter sut,
        [Greedy] XmlDocumentObserver expectedDocumentObserver,
        INamed named)
    {
        // arrange

        SetupObserver(expectedDocumentObserver, named);
        SetupObserver(sut.XmlDocumentObserver, named);
        var expected = expectedDocumentObserver.GetHtmlDocument();
        // act
        var actual = sut.GetHtmlDocument();
        // assert
        actual.Should().Be(expected);
    }

    private static void SetupObserver(XmlDocumentObserver expectedDocumentObserver, INamed named)
    {
        expectedDocumentObserver.OnNext(new ActionNotification(named, 0, new BeforeActionNotificationContent(DateTimeOffset.MinValue, CommandType.Action)));
        expectedDocumentObserver.OnNext(new ActionNotification(named, 0, new AfterActionNotificationContent(TimeSpan.FromSeconds(1))));
    }

    [Theory, DomainAutoData]
    public void SaveScreenshots_ShouldCallOnCompleted(
        SeleniumReporter sut)
    {
        // arrange            
        // act
        sut.SaveScreenshots();
        // assert
        Mock.Get(sut.ScreenshotInfoObserver).Verify(s => s.OnCompleted());
    }
}

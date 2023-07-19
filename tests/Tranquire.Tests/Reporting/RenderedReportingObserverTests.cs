using System;
using System.Globalization;
using AutoFixture;
using AutoFixture.Idioms;
using Moq;
using Tranquire.Reporting;
using Xunit;

namespace Tranquire.Tests.Reporting;

public class RenderedReportingObserverTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(RenderedReportingObserver));
    }

    [Theory, DomainAutoData]
    public void Sut_ConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
    {
        assertion.Verify(typeof(RenderedReportingObserver));
    }

    [Theory, DomainAutoData]
    public void OnError_ShouldCallOnError(RenderedReportingObserver sut, Exception exception)
    {
        // act
        sut.OnError(exception);
        // assert
        Mock.Get(sut.Observer).Verify(o => o.OnError(exception));
    }

    [Theory, DomainAutoData]
    public void OnCompleted_ShouldCallOnCompleted(RenderedReportingObserver sut)
    {
        // act
        sut.OnCompleted();
        // assert
        Mock.Get(sut.Observer).Verify(o => o.OnCompleted());
    }

    [Theory, DomainAutoData]
    public void OnNext_ShouldCallOnNext(
        ActionNotification notification,
        string expected,
        IFixture fixture)
    {
        // arrange
        var renderer = new Mock<Func<ActionNotification, string>>();
        renderer.Setup(r => r(notification)).Returns(expected);
        fixture.Inject(renderer.Object);
        var sut = fixture.Create<RenderedReportingObserver>();
        // act
        sut.OnNext(notification);
        // assert
        Mock.Get(sut.Observer).Verify(o => o.OnNext(expected));
    }

    [Theory]
    [DomainInlineAutoData(0, " ")]
    [DomainInlineAutoData(1, "---")]
    [DomainInlineAutoData(2, "   |--")]
    [DomainInlineAutoData(10, "                           |--")]
    public void DefaultRenderer_WithBeforeNotification_ShouldReturnCorrectValue(
        int depth,
        string prefix,
        BeforeActionNotificationContent notificationContent,
        INamed named)
    {
        // arrange
        // act
        var actual = RenderedReportingObserver.DefaultRenderer(new ActionNotification(named, depth, notificationContent));
        // assert
        var expected = prefix + " Starting " + named.Name;
    }

    [Theory]
    [DomainInlineAutoData(0, " ")]
    [DomainInlineAutoData(1, "---")]
    [DomainInlineAutoData(2, "   |--")]
    [DomainInlineAutoData(10, "                           |--")]
    public void DefaultRenderer_WithAfterNotification_ShouldReturnCorrectValue(
        int depth,
        string prefix,
        AfterActionNotificationContent notificationContent,
        INamed named)
    {
        // arrange
        // act
        var actual = RenderedReportingObserver.DefaultRenderer(new ActionNotification(named, depth, notificationContent));
        // assert
        var expected = prefix + $"Ending   {named.Name} ({notificationContent.Duration.ToString("g", CultureInfo.CurrentCulture)})";
    }

    [Theory]
    [DomainInlineAutoData(0, " ")]
    [DomainInlineAutoData(1, "---")]
    [DomainInlineAutoData(2, "   |--")]
    [DomainInlineAutoData(10, "                           |--")]
    public void DefaultRenderer_WithErrorNotification_ShouldReturnCorrectValue(
        int depth,
        string prefix,
        ExecutionErrorNotificationContent notificationContent,
        INamed named)
    {
        // arrange
        // act
        var actual = RenderedReportingObserver.DefaultRenderer(new ActionNotification(named, depth, notificationContent));
        // assert
        var expected = prefix + $"Error in {named.Name}\n({notificationContent.Exception.Message}";
    }
}


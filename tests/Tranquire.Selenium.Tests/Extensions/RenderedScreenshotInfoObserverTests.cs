using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using System;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Extensions
{
    public class RenderedScreenshotInfoObserverTests : WebDriverTest
    {
        public RenderedScreenshotInfoObserverTests(WebDriverFixture fixture) : base(fixture, "Actions.html")
        {
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion, IFixture fixture)
        {
            fixture.Register(() => Fixture.WebDriver.GetScreenshot());
            assertion.Verify(typeof(RenderedScreenshotInfoObserver));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitializedMember(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify<RenderedScreenshotInfoObserver>(o => o.Observer);
        }
        
        [Theory, DomainAutoData]
        public void Sut_ShouldBeObserverOfScreenshotInfo(RenderedScreenshotInfoObserver sut)
        {
            sut.Should().BeAssignableTo<IObserver<ScreenshotInfo>>(); 
        }

        [Theory, DomainAutoData]
        public void OnCompleted_ShouldCallOnCompleted(RenderedScreenshotInfoObserver sut)
        {
            // act
            sut.OnCompleted();
            // assert
            Mock.Get(sut.Observer).Verify(o => o.OnCompleted());
        }

        [Theory, DomainAutoData]
        public void OnError_ShouldCallOnError(RenderedScreenshotInfoObserver sut, Exception exception)
        {
            // act
            sut.OnError(exception);
            // assert
            Mock.Get(sut.Observer).Verify(o => o.OnError(exception));
        }
        
        [Theory, DomainAutoData]
        public void OnNext_ShouldCallOnNext(RenderedScreenshotInfoObserver sut, string expected)
        {
            // arrange
            var screenshotInfo = new ScreenshotInfo(Fixture.WebDriver.GetScreenshot(), expected);
            // act
            sut.OnNext(screenshotInfo);
            // assert
            Mock.Get(sut.Observer).Verify(o => o.OnNext(screenshotInfo.FileName + sut.Format.Extension));
        }
    }
}

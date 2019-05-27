using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using System;
using System.IO;
using Tranquire.Reporting;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Extensions
{
    public class ScreenshotInfoToActionAttachmentObserverAdapterTests : WebDriverTest
    {
        public ScreenshotInfoToActionAttachmentObserverAdapterTests(WebDriverFixture fixture) : base(fixture, "Actions.html")
        {
            
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion, IFixture fixture)
        {
            fixture.Register(() => Fixture.WebDriver.GetScreenshot());
            assertion.Verify(typeof(ScreenshotInfoToActionAttachmentObserverAdapter));
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeObserverOfScreenshotInfo(ScreenshotInfoToActionAttachmentObserverAdapter sut)
        {
            sut.Should().BeAssignableTo<IObserver<ScreenshotInfo>>();
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldInitializeObserver(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify<ScreenshotInfoToActionAttachmentObserverAdapter>(o => o.AttachmentObserver);
        }

        [Theory, DomainAutoData]
        public void OnCompleted_ShouldCallOnCompleted(
            ScreenshotInfoToActionAttachmentObserverAdapter sut)
        {
            // act
            sut.OnCompleted();
            // assert
            Mock.Get(sut.AttachmentObserver).Verify(o => o.OnCompleted());
        }
        
        [Theory, DomainAutoData]
        public void OnError_ShouldCallOnError(
            ScreenshotInfoToActionAttachmentObserverAdapter sut, Exception exception)
        {
            // act
            sut.OnError(exception);
            // assert
            Mock.Get(sut.AttachmentObserver).Verify(o => o.OnError(exception));
        }

        [Theory, DomainAutoData]
        public void OnNext_ShouldCallOnNext(
            ScreenshotInfoToActionAttachmentObserverAdapter sut,
            string filename)
        {
            // arrange
            var screenshotInfo = new ScreenshotInfo(Fixture.WebDriver.GetScreenshot(), filename);
            // act
            sut.OnNext(screenshotInfo);
            // assert
            var expected = new ActionFileAttachment(filename, string.Empty);
            Mock.Get(sut.AttachmentObserver).Verify(o => o.OnNext(It.Is<ActionFileAttachment>(a => 
                    a.FilePath == filename + sut.Format.Extension &&
                    a.Description == string.Empty)));
        }
    }
}

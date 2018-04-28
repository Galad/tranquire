using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Extensions
{
    public class SaveScreenshotsToFileOnNextTests : WebDriverTest
    {
        public SaveScreenshotsToFileOnNextTests(WebDriverFixture fixture) : base(fixture, "Actions.html")
        {
        }

        private static string CreateTestDirectoryPath() => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            Guid.NewGuid().ToString()
            );

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion, IFixture fixture)
        {
            fixture.Register(() => Fixture.WebDriver.GetScreenshot());
            assertion.Verify(typeof(SaveScreenshotsToFileOnNext));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyDirectoryIsInjected(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(SaveScreenshotsToFileOnNext).GetProperty(nameof(SaveScreenshotsToFileOnNext.Directory)));
        }

        [Theory, DomainAutoData]
        public void OnError_ShouldDoNothing(SaveScreenshotsToFileOnNext sut)
        {
            sut.OnError(new Exception());
        }

        [Theory, DomainAutoData]
        public void OnComplete_WhenNotCallingOnNext_ShouldNotSaveFiles(IFixture fixture)
        {
            // arrange
            fixture.Inject(CreateTestDirectoryPath());
            var sut = fixture.Create<SaveScreenshotsToFileOnNext>();
            // act
            sut.OnCompleted();
            // assert
            Assert.False(Directory.Exists(sut.Directory), "The directory " + sut.Directory + " has been created");
        }

        [Theory, DomainAutoData]
        public void OnNext_ShouldSaveFiles(IFixture fixture)
        {
            // arrange
            fixture.Inject(CreateTestDirectoryPath());
            var sut = fixture.Create<SaveScreenshotsToFileOnNext>();
            ScreenshotInfo GetScreenshotInfo()
            {
                var screenshot = ((ITakesScreenshot)base.Fixture.WebDriver).GetScreenshot();
                return new ScreenshotInfo(screenshot, Guid.NewGuid().ToString());
            }
            var screenshotInfo = GetScreenshotInfo();
            // act
            sut.OnNext(screenshotInfo);
            var actual = Directory.GetFiles(sut.Directory).Select(Path.GetFileName);
            // assert
            var expected = new[] { screenshotInfo.FileName + ".jpg" };
            actual.Should().BeEquivalentTo(expected);
        }
    }
}

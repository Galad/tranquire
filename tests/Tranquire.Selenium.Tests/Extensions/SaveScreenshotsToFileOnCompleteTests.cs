using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using OpenQA.Selenium;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Extensions;

public class SaveScreenshotsToFileOnCompleteTests : WebDriverTest
{
    public SaveScreenshotsToFileOnCompleteTests(WebDriverFixture fixture) : base(fixture, "Actions.html")
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
        assertion.Verify(typeof(SaveScreenshotsToFileOnComplete));
    }

    [Theory, DomainAutoData]
    public void Sut_VerifyDirectoryIsInjected(ConstructorInitializedMemberAssertion assertion)
    {
        assertion.Verify(typeof(SaveScreenshotsToFileOnComplete).GetProperty(nameof(SaveScreenshotsToFileOnComplete.Directory)));
    }

    [Theory, DomainAutoData]
    public void OnError_ShouldDoNothing(SaveScreenshotsToFileOnComplete sut)
    {
        sut.OnError(new Exception());
    }

    [Theory, DomainAutoData]
    public void OnComplete_WhenNotCallingOnNext_ShouldNotSaveFiles(IFixture fixture)
    {
        // arrange
        fixture.Inject(CreateTestDirectoryPath());
        var sut = fixture.Create<SaveScreenshotsToFileOnComplete>();
        // act
        sut.OnCompleted();
        // assert
        Assert.Empty(Directory.GetFiles(sut.Directory));
    }

    [Theory]
    [DomainInlineAutoData(1)]
    [DomainInlineAutoData(2)]
    [DomainInlineAutoData(10)]
    public void OnComplete_WhenCallingOnNext_ShouldSaveFiles(int count, IFixture fixture)
    {
        // arrange
        fixture.Inject(CreateTestDirectoryPath());
        var sut = fixture.Create<SaveScreenshotsToFileOnComplete>();
        ScreenshotInfo GetScreenshotInfo()
        {
            var screenshot = ((ITakesScreenshot)base.Fixture.WebDriver).GetScreenshot();
            return new ScreenshotInfo(screenshot, Guid.NewGuid().ToString());
        }
        var screenshots = Enumerable.Range(1, count).Select(_ => GetScreenshotInfo()).ToArray();

        foreach (var screenshotInfo in screenshots)
        {
            sut.OnNext(screenshotInfo);
        }
        // act
        sut.OnCompleted();
        var actual = Directory.GetFiles(sut.Directory).Select(Path.GetFileName);
        // assert
        var expected = screenshots.Select(s => s.FileName + sut.Format.Extension);
        actual.Should().BeEquivalentTo(expected);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using Tranquire.Reporting;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests;

public class ActorExtensionsTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(ActorExtensions));
    }

    [Theory, DomainAutoData]
    public void Sut_VerifyActorDecoratorBehavior(ActorDecoratorExtensionAssertion assertion)
    {
        assertion.Verify(typeof(ActorExtensions));
    }

    [Theory, DomainAutoData]
    public void TakeScreenshots_ShouldDecorateActor(
        [Modest] Actor actor,
        TakeScreenshot expected,
        string name,
        string directory)
    {
        //arrange       
        expected = new TakeScreenshot(expected.Actor, expected.NextScreenshotName, new SaveScreenshotsToFileOnNext(directory, ScreenshotFormat.Jpeg));
        //act
        var actual = ActorExtensions.TakeScreenshots(actor, directory, name).InnerActorBuilder(expected.Actor);
        //assert
        actual.Should().BeOfType<TakeScreenshot>().Which.Should().BeEquivalentTo(expected, o => o.Excluding(t => t.NextScreenshotName));
    }

    [Theory, DomainAutoData]
    public void TakeScreenshots_NextScreenshotName_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        TakeScreenshot expected,
        string expectedName,
        string directory)
    {
        //arrange
        //act
        var actual = ActorExtensions.TakeScreenshots(actor, directory, expectedName).InnerActorBuilder(expected.Actor);
        //assert
        actual.Should().BeOfType<TakeScreenshot>().Which.NextScreenshotName().Should().Be(expectedName + "_01");
    }

    [Theory, DomainAutoData]
    public void TakeScreenshots_NextScreenshotName_CalledMultipleTimes_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        TakeScreenshot expected,
        string expectedName,
        string directory)
    {
        //arrange
        //act
        var actual = ActorExtensions.TakeScreenshots(actor, directory, expectedName).InnerActorBuilder(expected.Actor);
        //assert            
        var func = actual.Should().BeOfType<TakeScreenshot>().Which.NextScreenshotName;
        var expectedNames = Enumerable.Range(1, 99).Select(i => expectedName + "_" + i.ToString("00"));
        var actualNames = Enumerable.Range(1, 99).Select(_ => func());
        actualNames.Should().BeEquivalentTo(expectedNames);
    }

    [Theory, DomainAutoData]
    public void TakeScreenshots_NextScreenshotName_WithFormat_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        TakeScreenshot expected,
        string startName,
        string endName,
        string directory)
    {
        //arrange
        var expectedName = $"{startName}0001{endName}";
        //act
        var actual = ActorExtensions.TakeScreenshots(actor, directory, $"{startName}{{0:0000}}{endName}").InnerActorBuilder(expected.Actor);
        //assert
        actual.Should().BeOfType<TakeScreenshot>().Which.NextScreenshotName().Should().Be(expectedName);
    }

    [Theory, DomainAutoData]
    public void TakeScreenshots_NextScreenshotName_WithFormatCalledMultipleTimes_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        TakeScreenshot expected,
        string startName,
        string endName,
        string directory)
    {
        //arrange
        var expectedName = $"{startName}0001{endName}";
        //act
        var actual = ActorExtensions.TakeScreenshots(actor, directory, $"{startName}{{0:0000}}{endName}").InnerActorBuilder(expected.Actor);
        //assert
        var func = actual.Should().BeOfType<TakeScreenshot>().Which.NextScreenshotName;
        var expectedNames = Enumerable.Range(1, 99).Select(i => $"{startName}{i:0000}{endName}");
        var actualNames = Enumerable.Range(1, 99).Select(_ => func());
        actualNames.Should().BeEquivalentTo(expectedNames);
    }

    [Theory, DomainAutoData]
    public void TakeScreenshots_NextScreenshotName_WithFormatWith2Placeholders_ShouldThrow(
        [Modest] Actor actor,
        string startName,
        string endName,
        string directory)
    {
        //arrange            
        //act
        new System.Action(() => ActorExtensions.TakeScreenshots(actor, directory, $"{startName}{{0:0000}}{endName}{{1}}"))
                  .Should().ThrowExactly<FormatException>();

        //assert            
    }

    [Theory, DomainAutoData]
    public void HighlighTargets_ShouldDecorateActor(
       [Modest] Actor actor,
       [Modest] HighlightTarget expected)
    {
        //arrange
        //act
        var actual = ActorExtensions.HighlightTargets(actor).InnerActorBuilder(expected.Actor);
        //assert
        actual.Should().BeOfType<HighlightTarget>().Which.Should().BeEquivalentTo(expected);
    }

    [Theory, DomainAutoData]
    public void SlowSelenium_ShouldDecorateActor(
       [Modest] Actor actor,
       [Modest] SlowSelenium expected)
    {
        //arrange
        //act
        var actual = ActorExtensions.SlowSelenium(actor, expected.Delay).InnerActorBuilder(expected.Actor);
        //assert
        actual.Should().BeOfType<SlowSelenium>().Which.Should().BeEquivalentTo(expected);
    }

    private static readonly ITakeScreenshotStrategy _defaultTakeScreenshotStrategy = new AlwaysTakeScreenshotStrategy();

    public static IEnumerable<object[]> ReportingConfigurations
    {
        get
        {
            var fixture = new Fixture().Customize(new DomainCustomization());
            fixture.Customize<Actor>(c => c.FromFactory(new MethodInvoker(new ModestConstructorQuery())));
            {
                yield return new object[]
                {
                    fixture.Create<Actor>(),
                    fixture.Create<IActor>(),
                    fixture.Create<SeleniumReportingConfiguration>(),
                    Array.Empty<IObserver<string>>(),
                    SeleniumReportingConfiguration.DefaultCanNotify,
                    _defaultTakeScreenshotStrategy,
                    ScreenshotFormat.Jpeg
                };
            }
            {
                var observers = fixture.CreateMany<IObserver<string>>().ToArray();
                yield return new object[]
                    {
                    fixture.Create<Actor>(),
                    fixture.Create<IActor>(),
                    fixture.Create<SeleniumReportingConfiguration>().AddTextObservers(observers),
                    observers,
                    SeleniumReportingConfiguration.DefaultCanNotify,
                    _defaultTakeScreenshotStrategy,
                    ScreenshotFormat.Jpeg
                    };
            }
            {
                var canNotify = fixture.Create<ICanNotify>();
                yield return new object[]
                    {
                    fixture.Create<Actor>(),
                    fixture.Create<IActor>(),
                    fixture.Create<SeleniumReportingConfiguration>().WithCanNotify(canNotify),
                    Array.Empty<IObserver<string>>(),
                    new CompositeCanNotify(
                        canNotify,
                        SeleniumReportingConfiguration.DefaultCanNotify
                    ),
                    _defaultTakeScreenshotStrategy,
                    ScreenshotFormat.Jpeg
                    };
            }
            {
                var takeScreenshotStrategy = fixture.Create<ITakeScreenshotStrategy>();
                yield return new object[]
                    {
                    fixture.Create<Actor>(),
                    fixture.Create<IActor>(),
                    fixture.Create<SeleniumReportingConfiguration>().WithTakeScreenshotStrategy(takeScreenshotStrategy),
                    Array.Empty<IObserver<string>>(),
                    SeleniumReportingConfiguration.DefaultCanNotify,
                    takeScreenshotStrategy,
                    ScreenshotFormat.Jpeg
                    };
            }
            {
                var takeScreenshotStrategy = fixture.Create<ITakeScreenshotStrategy>();
                var canNotify = fixture.Create<ICanNotify>();
                var observers = fixture.CreateMany<IObserver<string>>().ToArray();
                yield return new object[]
                    {
                    fixture.Create<Actor>(),
                    fixture.Create<IActor>(),
                    fixture.Create<SeleniumReportingConfiguration>().WithTakeScreenshotStrategy(takeScreenshotStrategy)
                                                                    .WithCanNotify(canNotify)
                                                                    .AddTextObservers(observers),
                    observers,
                    new CompositeCanNotify(
                        canNotify,
                        SeleniumReportingConfiguration.DefaultCanNotify
                    ),
                    takeScreenshotStrategy,
                    ScreenshotFormat.Jpeg
                    };
            }
            {
                yield return new object[]
                {
                    fixture.Create<Actor>(),
                    fixture.Create<IActor>(),
                    fixture.Create<SeleniumReportingConfiguration>().WithScreenshotFormat(ScreenshotFormat.Png),
                    Array.Empty<IObserver<string>>(),
                    SeleniumReportingConfiguration.DefaultCanNotify,
                    _defaultTakeScreenshotStrategy,
                    ScreenshotFormat.Png
                };
            }
        }
    }

    [Theory, MemberData(nameof(ReportingConfigurations))]
    public void WithSeleniumReporting_WithConfiguration_ShouldReturnCorrectValue(
        [Modest] Actor actor,
        IActor iactor,
        SeleniumReportingConfiguration configuration,
        IObserver<string>[] observers,
        ICanNotify canNotify,
        ITakeScreenshotStrategy takeScreenshotStrategy,
        ScreenshotFormat format)
    {
        var actual = ActorExtensions.WithSeleniumReporting(
                actor,
                configuration,
                out var actualSeleniumReporter
                );
        TestWithSeleniumReporting(actualSeleniumReporter,
                                  actual,
                                  actor,
                                  iactor,
                                  configuration.ScreenshotDirectory,
                                  configuration.ScreenshotNameOrFormat,
                                  observers,
                                  canNotify,
                                  takeScreenshotStrategy,
                                  format);
    }

    private static void TestWithSeleniumReporting(
        ISeleniumReporter actualSeleniumReporter,
        Actor actual,
        Actor actor,
        IActor iactor,
        string screenshotDirectory,
        string screenshotName,
        IObserver<string>[] observers,
        ICanNotify canNotify,
        ITakeScreenshotStrategy takeScreenshotStrategy,
        ScreenshotFormat format)
    {
        // assert 
        var xmlDocumentObserver = new XmlDocumentObserver();
        var takeScreenshot = actual.InnerActorBuilder(iactor).Should().BeOfType<TakeScreenshot>().Which;
        var expectedTakeScreenshot = ActorExtensions.TakeScreenshots(
            actor,
            screenshotName,
            new CompositeObserver<ScreenshotInfo>(
                new SaveScreenshotsToFileOnComplete(screenshotDirectory, format),
                new ScreenshotInfoToActionAttachmentObserverAdapter(xmlDocumentObserver, format),
                new RenderedScreenshotInfoObserver(new CompositeObserver<string>(observers), format)
                ),
            takeScreenshotStrategy
            )
            .InnerActorBuilder(iactor) as TakeScreenshot;
        takeScreenshot.Should().BeEquivalentTo(expectedTakeScreenshot,
                                               o => o.Excluding(a => a.Actor)
                                                     .Excluding(a => a.NextScreenshotName)
                                                     .ExcludingDateTimeOffset()
                                                     .RespectingRuntimeTypes(),
                                               "Take screenshot");
        var actualScreenshotNames = Enumerable.Range(0, 10).Select(_ => takeScreenshot.NextScreenshotName());
        var expectedScreenshotNames = Enumerable.Range(0, 10).Select(_ => expectedTakeScreenshot.NextScreenshotName());
        actualScreenshotNames.Should().BeEquivalentTo(expectedScreenshotNames, "Screenshot names");

        var reportingActor = takeScreenshot.Actor.Should().BeOfType<ReportingActor>().Which;
        var expectedReportingActor = actor.WithReporting(
            new CompositeObserver<ActionNotification>(
                xmlDocumentObserver,
                new RenderedReportingObserver(
                    new CompositeObserver<string>(observers),
                    RenderedReportingObserver.DefaultRenderer
                    )
                ),
                canNotify
            )
            .InnerActorBuilder(iactor) as ReportingActor;
        reportingActor.Should().BeEquivalentTo(expectedReportingActor,
                                               o => o.Excluding(a => a.Actor)
                                                     .ExcludingDateTimeOffset()
                                                     .RespectingRuntimeTypes(),
                                               "Reporting actor");

        var expectedSeleniumReporter = new SeleniumReporter(xmlDocumentObserver, new SaveScreenshotsToFileOnComplete(screenshotDirectory, format));
        actualSeleniumReporter.Should().BeEquivalentTo(expectedSeleniumReporter,
                                                       o => o.ExcludingDateTimeOffset(),
                                                       "Selenium reporter");
    }
}

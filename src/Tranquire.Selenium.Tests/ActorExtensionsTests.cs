using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tranquire.Reporting;
using Tranquire.Selenium.Extensions;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests
{
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
            [Modest]Actor actor,
            TakeScreenshot expected,
            string name,
            string directory)
        {
            //arrange       
            expected = new TakeScreenshot(expected.Actor, expected.NextScreenshotName, new SaveScreenshotsToFileOnNext(directory));
            //act
            var actual = ActorExtensions.TakeScreenshots(actor, directory, name).InnerActorBuilder(expected.Actor);
            //assert
            actual.Should().BeOfType<TakeScreenshot>().Which.Should().BeEquivalentTo(expected, o => o.Excluding(t => t.NextScreenshotName));
        }

        [Theory, DomainAutoData]
        public void TakeScreenshots_NextScreenshotName_ShouldReturnCorrectValue(
            [Modest]Actor actor,
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
            [Modest]Actor actor,
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
            [Modest]Actor actor,
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
            [Modest]Actor actor,
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
            var expectedNames = Enumerable.Range(1, 99).Select(i => $"{startName}{i.ToString("0000")}{endName}");
            var actualNames = Enumerable.Range(1, 99).Select(_ => func());
            actualNames.Should().BeEquivalentTo(expectedNames);
        }

        [Theory, DomainAutoData]
        public void TakeScreenshots_NextScreenshotName_WithFormatWith2Placeholders_ShouldThrow(
            [Modest]Actor actor,
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
           [Modest]Actor actor,
           [Modest]HighlightTarget expected)
        {
            //arrange
            //act
            var actual = ActorExtensions.HighlightTargets(actor).InnerActorBuilder(expected.Actor);
            //assert
            actual.Should().BeOfType<HighlightTarget>().Which.Should().BeEquivalentTo(expected);
        }

        [Theory, DomainAutoData]
        public void SlowSelenium_ShouldDecorateActor(
           [Modest]Actor actor,
           [Modest]SlowSelenium expected)
        {
            //arrange
            //act
            var actual = ActorExtensions.SlowSelenium(actor, expected.Delay).InnerActorBuilder(expected.Actor);
            //assert
            actual.Should().BeOfType<SlowSelenium>().Which.Should().BeEquivalentTo(expected);
        }

        private static readonly ITakeScreenshotStrategy _defaultTakeScreenshotStrategy = new AlwaysTakeScreenshotStrategy();
        [Theory, DomainAutoData]
        public void WithSeleniumReporting_ShouldReturnCorrectValue(
            [Modest]Actor actor,
            IActor iactor,
            string screenshotDirectory,
            string screenshotName,
            IObserver<string>[] observers)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var actual = ActorExtensions.WithSeleniumReporting(
                    actor,
                    screenshotDirectory,
                    screenshotName,
                    out var actualSeleniumReporter,
                    observers
                    );
#pragma warning restore CS0618 // Type or member is obsolete
            var canNotify = new CompositeCanNotify();
            TestWithSeleniumReporting(actualSeleniumReporter,
                                      actual,
                                      actor,
                                      iactor,
                                      screenshotDirectory,
                                      screenshotName,
                                      observers,
                                      canNotify,
                                      _defaultTakeScreenshotStrategy);
        }

        [Theory, DomainAutoData]
        public void WithSeleniumReporting_WithCanNotify_ShouldReturnCorrectValue(
            [Modest]Actor actor,
            IActor iactor,
            string screenshotDirectory,
            string screenshotName,
            IObserver<string>[] observers,
            ICanNotify canNotify)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var actual = ActorExtensions.WithSeleniumReporting(
                    actor,
                    screenshotDirectory,
                    screenshotName,
                    canNotify,
                    out var actualSeleniumReporter,
                    observers
                    );
#pragma warning restore CS0618 // Type or member is obsolete
            TestWithSeleniumReporting(actualSeleniumReporter,
                                      actual,
                                      actor,
                                      iactor,
                                      screenshotDirectory,
                                      screenshotName,
                                      observers,
                                      canNotify,
                                      _defaultTakeScreenshotStrategy);
        }

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
                        new CompositeCanNotify(),
                        _defaultTakeScreenshotStrategy
                    };
                }
                {
                    var observers = fixture.CreateMany<IObserver<string>>().ToArray();
                    yield return new object[]
                        {
                        fixture.Create<Actor>(),
                        fixture.Create<IActor>(),
                        fixture.Create<SeleniumReportingConfiguration>().AddTextObserver(observers),
                        observers,
                        new CompositeCanNotify(),
                        _defaultTakeScreenshotStrategy
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
                        canNotify,
                        _defaultTakeScreenshotStrategy
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
                        new CompositeCanNotify(),
                        takeScreenshotStrategy
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
                                                                        .AddTextObserver(observers),
                        observers,
                        canNotify,
                        takeScreenshotStrategy
                        };
                }
            }
        }

        [Theory, MemberData(nameof(ReportingConfigurations))]
        public void WithSeleniumReporting_WithConfiguration_ShouldReturnCorrectValue(
            [Modest]Actor actor,
            IActor iactor,
            SeleniumReportingConfiguration configuration,
            IObserver<string>[] observers,
            ICanNotify canNotify,
            ITakeScreenshotStrategy takeScreenshotStrategy)
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
                                      takeScreenshotStrategy);
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
            ITakeScreenshotStrategy takeScreenshotStrategy)
        {            
            // assert 
            var xmlDocumentObserver = new XmlDocumentObserver();
            var takeScreenshot = actual.InnerActorBuilder(iactor).Should().BeOfType<TakeScreenshot>().Which;
            var expectedTakeScreenshot = ActorExtensions.TakeScreenshots(
                actor,
                screenshotName,
                new CompositeObserver<ScreenshotInfo>(
                    new ScreenshotInfoToActionAttachmentObserverAdapter(xmlDocumentObserver),
                    new RenderedScreenshotInfoObserver(new CompositeObserver<string>(observers)),
                    new SaveScreenshotsToFileOnComplete(screenshotDirectory)
                    ),
                takeScreenshotStrategy
                )
                .InnerActorBuilder(iactor) as TakeScreenshot;
            takeScreenshot.Should().BeEquivalentTo(expectedTakeScreenshot, o => o.Excluding(a => a.Actor)
                                                                                 .Excluding(a => a.NextScreenshotName)
                                                                                 .RespectingRuntimeTypes());
            var actualScreenshotNames = Enumerable.Range(0, 10).Select(_ => takeScreenshot.NextScreenshotName());
            var expectedScreenshotNames = Enumerable.Range(0, 10).Select(_ => expectedTakeScreenshot.NextScreenshotName());
            actualScreenshotNames.Should().BeEquivalentTo(expectedScreenshotNames);

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
            reportingActor.Should().BeEquivalentTo(expectedReportingActor, o => o.Excluding(a => a.Actor)
                                                                                 .Excluding(a => a.MeasureTime.Now)
                                                                                 .RespectingRuntimeTypes());

            var expectedSeleniumReporter = new SeleniumReporter(xmlDocumentObserver, new SaveScreenshotsToFileOnComplete(screenshotDirectory));
            actualSeleniumReporter.Should().BeEquivalentTo(expectedSeleniumReporter);
        }
    }
}

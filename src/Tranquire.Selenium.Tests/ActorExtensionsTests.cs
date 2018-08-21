using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using System;
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

        [Theory, DomainAutoData]
        public void WithSeleniumReporting_ShouldReturnCorrectValue(
            [Modest]Actor actor,
            IActor iactor,
            string screenshotDirectory,
            string screenshotName,
            IObserver<string> observers)
        {
            TestWithSeleniumReporting(actor, iactor, screenshotDirectory, screenshotName, observers);
        }

        [Theory, DomainAutoData]
        public void WithSeleniumReporting_WithCanNotify_ShouldReturnCorrectValue(
            [Modest]Actor actor,
            IActor iactor,
            string screenshotDirectory,
            string screenshotName,
            IObserver<string> observers,
            ICanNotify canNotify)
        {
            TestWithSeleniumReporting(actor, iactor, screenshotDirectory, screenshotName, observers, canNotify);
        }

        private static void TestWithSeleniumReporting(
            Actor actor, 
            IActor iactor, 
            string screenshotDirectory, 
            string screenshotName, 
            IObserver<string> observers,
            ICanNotify canNotify = null)
        {
            // arrange
            // act
            Actor actual;
            ISeleniumReporter actualSeleniumReporter;
            if (canNotify == null)
            {
                actual = ActorExtensions.WithSeleniumReporting(
                    actor,
                    screenshotDirectory,
                    screenshotName,
                    out actualSeleniumReporter,
                    observers
                    );
                canNotify = new CompositeCanNotify();
            }
            else
            {
                actual = ActorExtensions.WithSeleniumReporting(
                    actor,
                    screenshotDirectory,
                    screenshotName,
                    canNotify,
                    out actualSeleniumReporter,
                    observers
                    );
            }
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
                    )
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

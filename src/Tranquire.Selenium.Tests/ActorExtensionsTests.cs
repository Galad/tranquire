using FluentAssertions;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Tranquire.Tests;
using Xunit;
using System.Linq;
using System;

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
            string name)
        {
            //arrange
            //act
            var actual = ActorExtensions.TakeScreenshots(actor, expected.Directory, name).InnerActorBuilder(expected.Actor);
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
    }
}

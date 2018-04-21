using FluentAssertions;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
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
            string name)
        {
            //arrange
            //act
            var actual = ActorExtensions.TakeScreenshots(actor, expected.Directory, name).InnerActorBuilder(expected.Actor);
            //assert
            actual.Should().BeOfType<TakeScreenshot>().Which.ShouldBeEquivalentTo(expected, o => o.Excluding(t => t.NextScreenshotName));
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
            actual.Should().BeOfType<TakeScreenshot>().Which.NextScreenshotName().Should().Contain(expectedName);
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
            actual.Should().BeOfType<HighlightTarget>().Which.ShouldBeEquivalentTo(expected);
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
            actual.Should().BeOfType<SlowSelenium>().Which.ShouldBeEquivalentTo(expected);
        }
    }
}

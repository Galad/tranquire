using System;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Tranquire.Reporting;
using Xunit;

namespace Tranquire.Tests
{
    public class ActorExtensionsTest
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
        public void WithReporting_ShouldDecorateActor(
            [Modest]Actor actor,
            ReportingActor expected)
        {
            //arrange            
            //act
            var actual = ActorExtensions.WithReporting(actor, expected.Observer, expected.MeasureTime).InnerActorBuilder(expected.Actor);
            //assert
            actual.Should().BeOfType<ReportingActor>().Which.Should().BeEquivalentTo(expected);
        }

        [Theory, DomainAutoData]
        public void WithReporting_WithIObserverOfString_ShouldDecorateActor(
            [Modest]Actor actor,
            ReportingActor expected,
            IObserver<string> observer)
        {
            //arrange
            //act
            var actual = ActorExtensions.WithReporting(actor, observer).InnerActorBuilder(expected.Actor);
            //assert
            actual.Should().BeOfType<ReportingActor>()
                .Which.Observer.Should().BeOfType<RenderedReportingObserver>()
                .Which.Observer.Should().Be(observer);
        }


        [Theory, DomainAutoData]
        public void WithReporting_WithIObserverOfStringAndCanNotify_ShouldDecorateActor(
            [Modest]Actor actor,
            ReportingActor expected,
            IObserver<string> observer,
            ICanNotify canNotify)
        {
            //arrange
            //act
            var actual = ActorExtensions.WithReporting(actor, observer, canNotify).InnerActorBuilder(expected.Actor);
            //assert
            var actorAssertion = actual.Should().BeOfType<ReportingActor>();
            actorAssertion.Which.Observer.Should().BeOfType<RenderedReportingObserver>()
                          .Which.Observer.Should().Be(observer);
            actorAssertion.Which.CanNotify.Should().Be(canNotify);
        }
    }
}

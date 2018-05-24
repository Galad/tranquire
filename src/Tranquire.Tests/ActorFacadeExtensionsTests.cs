using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using Tranquire;
using Xunit;

namespace Tranquire.Tests
{
    public class ActorFacadeExtensionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ActorFacadeExtensions));
        }

        [Theory]
        [DomainInlineAutoData(0)]
        [DomainInlineAutoData(1)]
        [DomainInlineAutoData(3)]
        public void Given_ShouldCallGiven(int count, IActorFacade actor, IFixture fixture)
        {
            // arrange
            var mockActor = Mock.Get(actor);
            var expected = fixture.CreateMany<object>(count);
            var commands = fixture.CreateMany<IGivenCommand<object>>(count)
                                  .Zip(expected, (command, exp) =>
                                  {
                                      mockActor.Setup(a => a.Given(command)).Returns(exp);
                                      return command;
                                  })
                                  .ToArray();
            // act            
            var actual = ActorFacadeExtensions.Given(actor, commands);
            // assert            
            actual.Should().BeEquivalentTo(expected);
        }


        [Theory]
        [DomainInlineAutoData(0)]
        [DomainInlineAutoData(1)]
        [DomainInlineAutoData(3)]
        public void When_ShouldCallWhen(int count, IActorFacade actor, IFixture fixture)
        {
            // arrange
            var mockActor = Mock.Get(actor);
            var expected = fixture.CreateMany<object>(count);
            var commands = fixture.CreateMany<IWhenCommand<object>>(count)
                                  .Zip(expected, (command, exp) =>
                                  {
                                      mockActor.Setup(a => a.When(command)).Returns(exp);
                                      return command;
                                  })
                                  .ToArray();
            // act            
            var actual = ActorFacadeExtensions.When(actor, commands);
            // assert            
            actual.Should().BeEquivalentTo(expected);
        }
    }
}

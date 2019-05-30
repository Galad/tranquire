using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
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

        [Theory, DomainAutoData]
        public void CanUseIf_WhenPredicateIsTrue_ShouldAddCapability(IActorFacade actor, IActorFacade expected, object capability)
        {
            // arrange
            Mock.Get(actor).Setup(a => a.CanUse(capability)).Returns(expected);
            // act
            var actual = actor.CanUseIf(() => capability, true);
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void CanUseIf_WhenPredicateIsFalse_ShouldNotAddCapability(IActorFacade actor, object capability)
        {
            // act
            var actual = actor.CanUseIf(() => capability, false);
            // assert
            Assert.Equal(actor, actual);
        }

        [Theory, DomainAutoData]
        public async Task Then_Async_ShouldExecuteAction(
            IVerifies verifies,
            IActor actor,
            object expected)
        {
            // arrange
            var mockAction = new Mock<Action<object>>();
            var task = Task.FromResult(expected);
            var question = Questions.FromResult(task);
            Mock.Get(verifies).Setup(a => a.Then(It.IsAny<IQuestion<Task<object>>>(), It.IsAny<Func<Task<object>, Task<object>>>()))
                           .Returns((IQuestion<Task<object>> q, Func<Task<object>, Task<object>> f) =>
                           {
                               return f(q.AnsweredBy(actor));
                           });
            // act
            var actual = await verifies.Then(question, mockAction.Object);
            // assert
            Assert.Equal(expected, actual);
            mockAction.Verify(a => a(expected));
        }

        [Theory, DomainAutoData]
        public void Then_ShouldCallAction(
            IVerifies verifies,
            IActor actor,
            object expected)
        {
            // arrange
            var verification = new Mock<Action<object>>();
            var question = Questions.FromResult(expected);
            Mock.Get(verifies).Setup(a => a.Then(It.IsAny<IQuestion<object>>(), It.IsAny<Func<object, object>>()))
                           .Returns((IQuestion<object> q, Func<object, object> f) =>
                           {
                               return f(q.AnsweredBy(actor));
                           });
            // act
            var actual = verifies.Then(question, verification.Object);
            // assert
            Assert.Equal(expected, actual);
            verification.Verify(v => v(expected));
        }
    }
}

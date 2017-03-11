using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Actions));
        }

        #region FromResult
        [Theory, DomainAutoData]
        public void FromResult_WhenExecutingWhen_ShouldReturnCorrectValue(
    IActor actor,
    object expected
    )
        {
            //arrange
            var sut = Actions.FromResult(expected);
            //act
            var actual = sut.ExecuteWhenAs(actor);
            //act
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void FromResult_WhenExecutingGiven_ShouldReturnCorrectValue(
            IActor actor,
            object expected
            )
        {
            //arrange
            var sut = Actions.FromResult(expected);
            //act
            var actual = sut.ExecuteGivenAs(actor);
            //act
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void FromResult_NameShouldReturnCorrectValue(
            string value
            )
        {
            //arrange
            var sut = Actions.FromResult(value);
            //act
            var actual = sut.Name;
            //act
            var expected = "Returns " + value;
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void FromResult_ToStringShouldBeName(
            string value
            )
        {
            //arrange
            var sut = Actions.FromResult(value);
            //act
            var actual = sut.ToString();
            //act
            var expected = sut.Name;
            actual.Should().Be(expected);
        }
        #endregion
        
        #region Empty
        public class InvocationCountActor : IActor
        {
            public string Name => "";
            public int Invocations { get; private set; }

            public TAnswer AsksFor<TAnswer>(IQuestion<TAnswer> question)
            {
                Invocations++;
                return default(TAnswer);
            }

#pragma warning disable CS0618 // Type or member is obsolete
            public TAnswer AsksFor<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                Invocations++;
                return default(TAnswer);
            }

#pragma warning disable CS0618 // Type or member is obsolete
            public TResult ExecuteWithAbility<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                Invocations++;
                return default(TResult);
            }

            public TResult Execute<TResult>(IAction<TResult> action)
            {
                Invocations++;
                return default(TResult);
            }
        }

        [Theory, DomainAutoData]
        public void Empty_WhenExecutingWhen_ShouldNotCallActor(InvocationCountActor actor)
        {
            //arrange
            var sut = Actions.Empty;            
            //act
            sut.ExecuteWhenAs(actor);
            //assert
            actor.Invocations.Should().Be(0);
        }

        [Theory, DomainAutoData]
        public void Empty_WhenExecutingGiven_ShouldNotCallActor(InvocationCountActor actor)
        {
            //arrange
            var sut = Actions.Empty;
            //act
            sut.ExecuteGivenAs(actor);
            //assert
            actor.Invocations.Should().Be(0);
        }

        [Theory, DomainAutoData]
        public void Empty_NameShouldNotCallActor(InvocationCountActor actor)
        {
            //arrange
            var sut = Actions.Empty;
            //act
            var actual = sut.Name;
            //assert
            actual.Should().Be("Empty action");
        }

        [Theory, DomainAutoData]
        public void Empty_ToStringShouldReturnName(InvocationCountActor actor)
        {
            //arrange
            var sut = Actions.Empty;
            //act
            var actual = sut.ToString();
            //assert
            actual.Should().Be(sut.Name);
        }
        #endregion
    }
}

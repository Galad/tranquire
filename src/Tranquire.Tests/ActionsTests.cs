using FluentAssertions;
using Moq;
using AutoFixture.Idioms;
using System;
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
                return Increment<TAnswer>();
            }

#pragma warning disable CS0618 // Type or member is obsolete
            public TAnswer AsksForWithAbility<TAnswer, TAbility>(IQuestion<TAnswer, TAbility> question)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                return Increment<TAnswer>();
            }

#pragma warning disable CS0618 // Type or member is obsolete
            public TResult ExecuteWithAbility<TGiven, TWhen, TResult>(IAction<TGiven, TWhen, TResult> action)
#pragma warning restore CS0618 // Type or member is obsolete
            {
                return Increment<TResult>();
            }

            public TResult Execute<TResult>(IAction<TResult> action)
            {
                return Increment<TResult>();
            }

            private TResult Increment<TResult>()
            {
                Invocations++;
                return default;
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

        [Fact]
        public void Empty_NameShouldNotCallActor()
        {
            //arrange
            var sut = Actions.Empty;
            //act
            var actual = sut.Name;
            //assert
            actual.Should().Be("Empty action");
        }

        [Fact]
        public void Empty_ToStringShouldReturnName()
        {
            //arrange
            var sut = Actions.Empty;
            //act
            var actual = sut.ToString();
            //assert
            actual.Should().Be(sut.Name);
        }
        #endregion

        #region Create without ability
        [Theory, DomainAutoData]
        public void Create_ShouldReturnActionWithCorrectName(
            string expectedName,
            System.Action<IActor> action
            )
        {
            //arrange
            //act
            var actual = Actions.Create(expectedName, action);
            //assert
            Assert.Equal(expectedName, actual.Name);
        }

        [Theory, DomainAutoData]
        public void Create_ShouldNotCallAction(
            string name
            )
        {
            //arrange
            var action = new Mock<System.Action<IActor>>();
            //act
            var actual = Actions.Create(name, action.Object);
            //assert
            action.Verify(a => a(It.IsAny<IActor>()), Times.Never());
        }

        [Theory, DomainAutoData]
        public void Create_ShouldReturnActionExecutingDelegateWhenCallingExecuteWhenAs(
            string name,
            IActor actor
            )
        {
            //arrange
            var action = new Mock<System.Action<IActor>>();
            //act
            var actual = Actions.Create(name, action.Object);
            actual.ExecuteWhenAs(actor);
            //assert
            action.Verify(a => a(actor), Times.Once());
        }

        [Theory, DomainAutoData]
        public void Create_ShouldReturnActionExecutingDelegateWhenCallingExecuteGivenAs(
            string name,
            IActor actor
            )
        {
            //arrange
            var action = new Mock<System.Action<IActor>>();
            //act
            var actual = Actions.Create(name, action.Object);
            actual.ExecuteGivenAs(actor);
            //assert
            action.Verify(a => a(actor), Times.Once());
        }
        #endregion

        #region Create with ability
        [Theory, DomainAutoData]
        public void Create_WithAbility_ShouldReturnActionWithCorrectName(
            string expectedName,
            System.Action<IActor, object> action
            )
        {
            //arrange
            //act
            var actual = Actions.Create(expectedName, action);
            //assert
            Assert.Equal(expectedName, actual.Name);
        }

        [Theory, DomainAutoData]
        public void Create_WithAbility_ShouldNotCallAction(
            string name
            )
        {
            //arrange
            var action = new Mock<System.Action<IActor, object>>();
            //act
            var actual = Actions.Create<object>(name, action.Object);
            //assert
            action.Verify(a => a(It.IsAny<IActor>(), It.IsAny<object>()), Times.Never());
        }

        [Theory, DomainAutoData]
        public void Create_WithAbility_ShouldReturnActionExecutingDelegateWhenCallingExecuteWhenAs(
            string name,
            IActor actor,
            object ability
            )
        {
            //arrange
            var action = new Mock<System.Action<IActor, object>>();
            //act
            var actual = Actions.Create<object>(name, action.Object);
            actual.ExecuteWhenAs(actor, ability);
            //assert
            action.Verify(a => a(actor, ability), Times.Once());
        }

        [Theory, DomainAutoData]
        public void Create_WithAbility_ShouldReturnActionExecutingDelegateWhenCallingExecuteGivenAs(
            string name,
            IActor actor,
            object ability
            )
        {
            //arrange
            var action = new Mock<System.Action<IActor, object>>();
            //act
            var actual = Actions.Create<object>(name, action.Object);
            actual.ExecuteGivenAs(actor, ability);
            //assert
            action.Verify(a => a(actor, ability), Times.Once());
        }
        #endregion

        #region Create returning value without ability
        [Theory, DomainAutoData]
        public void Create_ReturningValue_ShouldReturnActionWithCorrectName(
            string expectedName,
            System.Func<IActor, object> action
            )
        {
            //arrange
            //act
            var actual = Actions.Create(expectedName, action);
            //assert
            Assert.Equal(expectedName, actual.Name);
        }

        [Theory, DomainAutoData]
        public void Create_ReturningValue_ShouldNotCallAction(
            string name
            )
        {
            //arrange
            var actionFunc = new Mock<Func<IActor, object>>();
            //act
            var actual = Actions.Create(name, actionFunc.Object);
            //assert
            actionFunc.Verify(a => a(It.IsAny<IActor>()), Times.Never());
        }

        [Theory, DomainAutoData]
        public void Create_ReturningValue_ShouldReturnActionExecutingDelegateWhenCallingExecuteWhenAs(
            string name,
            IActor actor,
            object expected
            )
        {
            //arrange
            var actionFunc = new Mock<Func<IActor, object>>();
            actionFunc.Setup(a => a(actor)).Returns(expected);
            //act
            var actualAction = Actions.Create(name, actionFunc.Object);
            var actual = actualAction.ExecuteWhenAs(actor);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Create_ReturningValue_ShouldReturnActionExecutingDelegateWhenCallingExecuteGivenAs(
         string name,
            IActor actor,
            object expected
            )
        {
            //arrange
            var actionFunc = new Mock<Func<IActor, object>>();
            actionFunc.Setup(a => a(actor)).Returns(expected);
            //act
            var actualAction = Actions.Create(name, actionFunc.Object);
            var actual = actualAction.ExecuteGivenAs(actor);
            //assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region Create returning value with ability
        [Theory, DomainAutoData]
        public void Create_WithAbilityAndReturningValue_ShouldReturnActionWithCorrectName(
            string expectedName,
            System.Func<IActor, object, object> action
            )
        {
            //arrange
            //act
            var actual = Actions.Create(expectedName, action);
            //assert
            Assert.Equal(expectedName, actual.Name);
        }

        [Theory, DomainAutoData]
        public void Create_WithAbilityAndReturningValue_ShouldNotCallAction(
            string name
            )
        {
            //arrange
            var actionFunc = new Mock<Func<IActor, object, object>>();
            //act
            var actual = Actions.Create(name, actionFunc.Object);
            //assert
            actionFunc.Verify(a => a(It.IsAny<IActor>(), It.IsAny<object>()), Times.Never());
        }

        [Theory, DomainAutoData]
        public void Create_WithAbilityAndReturningValue_ShouldReturnActionExecutingDelegateWhenCallingExecuteWhenAs(
            string name,
            IActor actor,
            object ability,
            object expected
            )
        {
            //arrange
            var actionFunc = new Mock<Func<IActor, object, object>>();
            actionFunc.Setup(a => a(actor, ability)).Returns(expected);
            //act
            var actualAction = Actions.Create(name, actionFunc.Object);
            var actual = actualAction.ExecuteWhenAs(actor, ability);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Create_WithAbilityAndReturningValue_ShouldReturnActionExecutingDelegateWhenCallingExecuteGivenAs(
         string name,
            IActor actor,
            object ability,
            object expected
            )
        {
            //arrange
            var actionFunc = new Mock<Func<IActor, object, object>>();
            actionFunc.Setup(a => a(actor, ability)).Returns(expected);
            //act
            var actualAction = Actions.Create(name, actionFunc.Object);
            var actual = actualAction.ExecuteGivenAs(actor, ability);
            //assert
            Assert.Equal(expected, actual);
        }
        #endregion

        #region DispatchedGivenWhen

        [Theory, DomainAutoData]
        public void DispatchedGivenWhen_ShouldReturnCorrectValue(
              string name,
              IAction<object> givenAction,
              IAction<object> whenAction)
        {
            //arrange
            //act
            var actual = Actions.CreateDispatched(name, givenAction, whenAction);
            //assert
            var expected = new DispatchAction<object>(name, givenAction, whenAction);
            actual.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
}

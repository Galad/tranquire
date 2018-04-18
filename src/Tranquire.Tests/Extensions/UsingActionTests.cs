using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Idioms;
using System;
using System.Collections.Generic;
using Tranquire.Extensions;
using Xunit;

namespace Tranquire.Tests.Extensions
{
    public class UsingActionTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(UsingAction<object>));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(UsingAction<object>).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(string actionName, string disposableActionName, UsingAction<object> sut)
        {
            // arrange
            Mock.Get(sut.ActionToExecute).Setup(a => a.Name).Returns(actionName);
            Mock.Get(sut.DisposableAction).Setup(a => a.Name).Returns(disposableActionName);
            // act
            var actual = sut.Name;
            // assert
            var expected = $"Using {disposableActionName} with {actionName}";
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhen_ShouldCallDisposeOnActionResult(IDisposable disposable, IActor actor, UsingAction<object> sut)
        {
            // arrange
            Mock.Get(actor).Setup(a => a.Execute(sut.DisposableAction)).Returns(disposable);
            // act
            sut.ExecuteWhenAs(actor);
            // assert
            Mock.Get(disposable).Verify(d => d.Dispose());
        }

        [Theory, DomainAutoData]
        public void ExecuteGiven_ShouldCallDisposeOnActionResult(IDisposable disposable, IActor actor, UsingAction<object> sut)
        {
            // arrange
            Mock.Get(actor).Setup(a => a.Execute(sut.DisposableAction)).Returns(disposable);
            // act
            sut.ExecuteGivenAs(actor);
            // assert
            Mock.Get(disposable).Verify(d => d.Dispose());
        }

        [Theory, DomainAutoData]
        public void ExecuteWhen_WhenActionToExecuteThrows_ShouldCallDisposeOnActionResult(
            IDisposable disposable,
            IActor actor,
            UsingAction<object> sut)
        {
            // arrange
            Mock.Get(actor).Setup(a => a.Execute(sut.DisposableAction)).Returns(disposable);
            Mock.Get(actor).Setup(a => a.Execute(sut.ActionToExecute)).Throws(new Exception());
            // act
            try
            {
                sut.ExecuteWhenAs(actor);
            }
            catch (Exception)
            {
            }
            // assert
            Mock.Get(disposable).Verify(d => d.Dispose());
        }

        [Theory, DomainAutoData]
        public void ExecuteGiven_WhenActionToExecuteThrows_ShouldCallDisposeOnActionResult(
            IDisposable disposable,
            IActor actor,
            UsingAction<object> sut)
        {
            // arrange
            Mock.Get(actor).Setup(a => a.Execute(sut.DisposableAction)).Returns(disposable);
            Mock.Get(actor).Setup(a => a.Execute(sut.ActionToExecute)).Throws(new Exception());
            // act
            try
            {
                sut.ExecuteGivenAs(actor);
            }
            catch (Exception)
            {
            }
            // assert
            Mock.Get(disposable).Verify(d => d.Dispose());
        }

        [Theory, DomainAutoData]
        public void ExecuteWhen_ReturnCorrectValue(object expected, IActor actor, UsingAction<object> sut)
        {
            // arrange
            Mock.Get(actor).Setup(a => a.Execute(sut.ActionToExecute)).Returns(expected);
            // act
            var actual = sut.ExecuteWhenAs(actor);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteGiven_ReturnCorrectValue(object expected, IActor actor, UsingAction<object> sut)
        {
            // arrange
            Mock.Get(actor).Setup(a => a.Execute(sut.ActionToExecute)).Returns(expected);
            // act
            var actual = sut.ExecuteGivenAs(actor);
            // assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhen_ShouldCreateDisposableBeforeExecutingAction(IDisposable disposable, IActor actor, UsingAction<object> sut)
        {
            // arrange
            var callOrder = new List<string>();
            var mockActor = Mock.Get(actor);
            mockActor.Setup(a => a.Execute(sut.DisposableAction)).Returns(() =>
            {
                callOrder.Add("createDisposable");
                return disposable;
            });
            mockActor.Setup(a => a.Execute(sut.ActionToExecute)).Callback(() => callOrder.Add("executeAction"));
            ;
            // act
            sut.ExecuteWhenAs(actor);
            // assert
            var expected = new[] { "createDisposable", "executeAction" };
            callOrder.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Theory, DomainAutoData]
        public void ExecuteGiven_ShouldCreateDisposableBeforeExecutingAction(IDisposable disposable, IActor actor, UsingAction<object> sut)
        {
            // arrange
            var callOrder = new List<string>();
            var mockActor = Mock.Get(actor);
            mockActor.Setup(a => a.Execute(sut.DisposableAction)).Returns(() =>
            {
                callOrder.Add("createDisposable");
                return disposable;
            });
            mockActor.Setup(a => a.Execute(sut.ActionToExecute)).Callback(() => callOrder.Add("executeAction"));
            ;
            // act
            sut.ExecuteGivenAs(actor);
            // assert
            var expected = new[] { "createDisposable", "executeAction" };
            callOrder.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }
    }
}

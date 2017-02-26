using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace Tranquire.Tests
{
    /// <summary>
    /// Contains tests for a standard IAction template behavior
    /// </summary>
    /// <typeparam name="TActionExecuteGiven"></typeparam>
    /// <typeparam name="TActionExecuteWhen"></typeparam>
    /// <typeparam name="TActionExecuteWhenAndGivenNotOverridden"></typeparam>
    public abstract class ActionTestsBase<TActionExecuteGiven, TActionExecuteWhen, TActionExecuteWhenAndGivenNotOverridden>
        where TActionExecuteGiven : IAction<object>, IWithActor
        where TActionExecuteWhen : IAction<object>, IWithActor
        where TActionExecuteWhenAndGivenNotOverridden : IAction<object>
    {
        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(Action<object> sut)
        {
            Assert.IsAssignableFrom(typeof(IAction<object>), sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(IFixture fixture)
        {
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(typeof(TActionExecuteGiven));
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorExecute(
            [Frozen] IAction<object> action,
            TActionExecuteWhen sut,
            Mock<IActor> actor,
            object expected)
        {
            //arrange
            actor.Setup(a => a.Execute(action)).Returns(expected);
            //act
            var actual = sut.ExecuteWhenAs(actor.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorExecute(
            [Frozen] IAction<object> action,
            TActionExecuteGiven sut,
            Mock<IActor> actor,
            object expected)
        {
            //arrange
            actor.Setup(a => a.Execute(action)).Returns(expected);
            //act
            var actual = sut.ExecuteGivenAs(actor.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_WhenExecuteGivenIsNotOverridden_ShouldCallActorExecute(
            [Frozen] IAction<object> action,
            TActionExecuteWhenAndGivenNotOverridden sut,
            Mock<IActor> actor,
            object expected)
        {
            //arrange
            actor.Setup(a => a.Execute(action)).Returns(expected);
            //act
            var actual = sut.ExecuteGivenAs(actor.Object);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldUseCorrectActor(TActionExecuteWhen sut, Mock<IActor> actor)
        {
            //arrange
            var expected = actor.Object;
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            Assert.Equal(expected, sut.Actor);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldUseCorrectActor(TActionExecuteGiven sut, Mock<IActor> actor)
        {
            //arrange
            var expected = actor.Object;
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            Assert.Equal(expected, sut.Actor);
        }

        [Theory, DomainAutoData]
        public void ToString_ShouldReturnCorrectValue(TActionExecuteWhen sut)
        {
            //arrange
            //act
            var actual = sut.ToString();
            //assert
            Assert.Equal(sut.Name, actual);
        }
    }
}

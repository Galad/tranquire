using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tranquire.Tests
{
    public class DispatchActionObjectTests : DispatchActionTests<object, DispatchAction<object>> { }
    public class DispatchActionUnitTests : DispatchActionTests<Unit, DispatchActionUnit> { }

    public abstract class DispatchActionTests<T, TSut> where TSut : DispatchAction<T>
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify<TSut>();
        }

        [Theory, DomainAutoData]
        public void Sut_IsAction(TSut sut)
        {
            sut.Should().BeAssignableTo<IAction<T>>();
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(TSut).GetProperty(nameof(DispatchAction<T>.Name)));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify<TSut>(s => new[]
            {
                s.GivenAction,
                s.WhenAction
            });
        }


        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldReturnCorrectValue(
            TSut sut,
            T expected,
            IActor actor)
        {
            //arrange
            Mock.Get(actor).Setup(a => a.Execute(sut.GivenAction))
                .Returns(expected)
                .Verifiable();
            //act
            var actual = sut.ExecuteGivenAs(actor);
            //assert
            Assert.Equal(expected, actual);
            Mock.Get(sut.GivenAction).Verify(); // in case of T is Unit, because we cannot use the control flow technique
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldReturnCorrectValue(
            TSut sut,
            T expected,
            IActor actor)
        {
            //arrange
            Mock.Get(actor).Setup(a => a.Execute(sut.WhenAction))
                .Returns(expected)
                .Verifiable();
            //act
            var actual = sut.ExecuteWhenAs(actor);
            //assert
            Assert.Equal(expected, actual);
            Mock.Get(sut.WhenAction).Verify(); // in case of T is Unit, because we cannot use the control flow technique
        }
    }
}

using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tranquire.Tests.Extensions;

public abstract class IfActionTestsBase<TIfAction, TActionInterface, TDecoratedAction> where TIfAction : INamed where TDecoratedAction : class, INamed
{
    [Theory, DomainAutoData]
    public void Sut_ShouldBeAction(TIfAction sut)
    {
        sut.Should().BeAssignableTo<TActionInterface>();
    }

    [Theory, DomainAutoData]
    public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
    {
        assertion.Verify(typeof(TIfAction).GetConstructors());
    }

    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(TIfAction));
    }

    [Theory, DomainAutoData]
    public void Name_ShouldReturnCorrectValue(
        [Frozen] Mock<TDecoratedAction> decoratedAction,
        TIfAction sut,
        string name
        )
    {
        //arrange
        decoratedAction.Setup(a => a.Name).Returns(name);
        //act
        var actual = sut.Name;
        //assert
        var expected = "[If] " + name;
        actual.Should().Be(expected);
    }
}

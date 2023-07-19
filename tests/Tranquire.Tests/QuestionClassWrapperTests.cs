using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using Moq;
using Xunit;

namespace Tranquire.Tests;

public class QuestionClassWrapperTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(QuestionClassWrapper<object>).GetConstructors());
    }

    [Theory, DomainAutoData]
    public void Name_ShouldReturnCorrectValue(
        string expected,
        [Frozen] IQuestion<object> innerQuestion,
        QuestionClassWrapper<object> sut)
    {
        // arrange
        Mock.Get(innerQuestion).Setup(q => q.Name).Returns(expected);
        // act
        var actual = sut.Name;
        // assert
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void AnsweredBy_ShouldReturnCorrectValue(
        object expected,
        [Frozen] IQuestion<object> innerQuestion,
        QuestionClassWrapper<object> sut,
        IActor actor)
    {
        // arrange
        Mock.Get(innerQuestion).Setup(q => q.AnsweredBy(actor)).Returns(expected);
        // act
        var actual = sut.AnsweredBy(actor);
        // assert
        Assert.Equal(expected, actual);
    }
}

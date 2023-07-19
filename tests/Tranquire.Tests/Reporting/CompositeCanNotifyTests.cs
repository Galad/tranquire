using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using Moq;
using Tranquire.Reporting;
using Xunit;

namespace Tranquire.Tests.Reporting;

public class CompositeCanNotifyTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CompositeCanNotify).GetConstructors());
    }

    [Theory]
    [DomainInlineAutoData(new bool[] { }, true)]
    [DomainInlineAutoData(new bool[] { true }, true)]
    [DomainInlineAutoData(new bool[] { false }, false)]
    [DomainInlineAutoData(new bool[] { false, false, false }, false)]
    [DomainInlineAutoData(new bool[] { true, false, false }, false)]
    [DomainInlineAutoData(new bool[] { false, true, false }, false)]
    [DomainInlineAutoData(new bool[] { false, false, true }, false)]
    [DomainInlineAutoData(new bool[] { true, true, true }, true)]
    public void Action_ShouldReturnCorrectValue(
        bool[] innerResults,
        bool expected,
        IFixture fixture,
        IAction<Unit> action)
    {
        // arrange
        var canNotifiy = fixture.CreateMany<ICanNotify>(innerResults.Length)
                                .Select((c, i) =>
                                {
                                    Mock.Get(c).Setup(cc => cc.Action(action)).Returns(innerResults[i]);
                                    return c;
                                })
                                .ToArray();
        fixture.Inject(canNotifiy);
        var sut = fixture.Create<CompositeCanNotify>();
        // act
        var actual = sut.Action(action);
        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [DomainInlineAutoData(new bool[] { }, true)]
    [DomainInlineAutoData(new bool[] { true }, true)]
    [DomainInlineAutoData(new bool[] { false }, false)]
    [DomainInlineAutoData(new bool[] { false, false, false }, false)]
    [DomainInlineAutoData(new bool[] { true, false, false }, false)]
    [DomainInlineAutoData(new bool[] { false, true, false }, false)]
    [DomainInlineAutoData(new bool[] { false, false, true }, false)]
    [DomainInlineAutoData(new bool[] { true, true, true }, true)]
    public void Question_ShouldReturnCorrectValue(
        bool[] innerResults,
        bool expected,
        IFixture fixture,
        IQuestion<Unit> question)
    {
        // arrange
        var canNotifiy = fixture.CreateMany<ICanNotify>(innerResults.Length)
                                .Select((c, i) =>
                                {
                                    Mock.Get(c).Setup(cc => cc.Question(question)).Returns(innerResults[i]);
                                    return c;
                                })
                                .ToArray();
        fixture.Inject(canNotifiy);
        var sut = fixture.Create<CompositeCanNotify>();
        // act
        var actual = sut.Question(question);
        // assert
        Assert.Equal(expected, actual);
    }
}

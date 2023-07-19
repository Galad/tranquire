using System;
using System.Linq;
using AutoFixture;
using AutoFixture.Idioms;
using Moq;
using Xunit;

namespace Tranquire.Tests;

public class CompositeObserverTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(CompositeObserver<object>).GetConstructors());
    }

    [Theory]
    [DomainInlineAutoData(0)]
    [DomainInlineAutoData(1)]
    [DomainInlineAutoData(2)]
    [DomainInlineAutoData(10)]
    public void Sut_ShouldBeEnumerableOfObservers(int count, IFixture fixture)
    {
        // arrange
        var observers = fixture.CreateMany<IObserver<object>>(count).ToArray();
        fixture.Inject(observers);
        var sut = fixture.Create<CompositeObserver<object>>();
        // act and assert
        Assert.Equal(observers, sut);
    }

    [Theory, DomainAutoData]
    public void OnCompleted_ShouldCallOnCompletedOnAllObservers(CompositeObserver<object> sut)
    {
        // act
        sut.OnCompleted();
        // assert
        foreach (var mock in sut.Select(Mock.Get))
        {
            mock.Verify(o => o.OnCompleted());
        }
    }

    [Theory, DomainAutoData]
    public void OnError_ShouldCallOnErrorOnAllObservers(CompositeObserver<object> sut, Exception exception)
    {
        // act
        sut.OnError(exception);
        // assert
        foreach (var mock in sut.Select(Mock.Get))
        {
            mock.Verify(o => o.OnError(exception));
        }
    }

    [Theory, DomainAutoData]
    public void OnNext_ShouldCallOnNextOnAllObservers(CompositeObserver<object> sut, object value)
    {
        // act
        sut.OnNext(value);
        // assert
        foreach (var mock in sut.Select(Mock.Get))
        {
            mock.Verify(o => o.OnNext(value));
        }
    }
}

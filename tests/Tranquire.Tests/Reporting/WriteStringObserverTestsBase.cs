using AutoFixture.Idioms;
using System;
using System.Text;
using Xunit;

namespace Tranquire.Tests.Reporting
{
    public abstract class WriteStringObserverTestsBase<T> : IDisposable
        where T : IObserver<string>
    {
        protected StringBuilder Out { get; }

        protected WriteStringObserverTestsBase(StringBuilder @out)
        {
            Out = @out;
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(T));
        }

        [Theory, DomainAutoData]
        public void OnComplete_ShouldWriteCorrectValue(
            T sut)
        {
            // act
            sut.OnCompleted();
            // assert
            Assert.Equal("Completed\r\n", Out.ToString());
        }

        [Theory, DomainAutoData]
        public void OnError_ShouldWriteCorrectValue(
            T sut,
            Exception exception)
        {
            // act
            sut.OnError(exception);
            // assert
            Assert.Equal("Error: " + exception.Message + "\r\n" + exception.StackTrace + "\r\n", Out.ToString());
        }

        [Theory, DomainAutoData]
        public void OnNext_ShouldWriteCorrectValue(
            T sut,
            string expected)
        {
            // act
            sut.OnNext(expected);
            // assert
            Assert.Equal(expected + "\r\n", Out.ToString());
        }

        [Theory, DomainAutoData]
        public void OnNext_MultipleTimes_ShouldWriteCorrectValue(
            T sut,
            string[] values)
        {
            // act
            foreach (var value in values)
            {
                sut.OnNext(value);
            }
            // assert
            var expected = string.Join("\r\n", values);
            Assert.Equal(expected + "\r\n", Out.ToString());
        }

        public virtual void Dispose() { }
    }
}

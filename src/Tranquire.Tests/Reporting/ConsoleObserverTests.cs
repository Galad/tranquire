using Moq;
using System;
using Tranquire.Reporting;
using Xunit;
using AutoFixture;
using AutoFixture.Idioms;
using System.IO;
using System.Text;

namespace Tranquire.Tests.Reporting
{
    public class ConsoleObserverTests : IDisposable
    {
        private readonly StringBuilder _out;
        private readonly TextWriter _standardOut;

        public ConsoleObserverTests()
        {
            _out = new StringBuilder();
            _standardOut = Console.Out;
            Console.SetOut(new StringWriter(_out));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ConsoleObserver));
        }

        [Theory, DomainAutoData]
        public void OnComplete_ShouldWriteCorrectValue(
            ConsoleObserver sut)
        {
            // act
            sut.OnCompleted();
            // assert
            Assert.Equal("Completed\r\n", _out.ToString());
        }

        [Theory, DomainAutoData]
        public void OnError_ShouldWriteCorrectValue(
            ConsoleObserver sut,
            Exception exception)
        {
            // act
            sut.OnError(exception);
            // assert
            Assert.Equal("Error: " + exception.Message + "\r\n" + exception.StackTrace + "\r\n", _out.ToString());
        }

        [Theory, DomainAutoData]
        public void OnNext_ShouldWriteCorrectValue(
            ConsoleObserver sut,
            string expected)
        {
            // act
            sut.OnNext(expected);
            // assert
            Assert.Equal(expected + "\r\n", _out.ToString());
        }

        [Theory, DomainAutoData]
        public void OnNext_MultipleTimes_ShouldWriteCorrectValue(
            ConsoleObserver sut,
            string[] values)
        {
            // act
            foreach (var value in values)
            {
                sut.OnNext(value);
            }
            // assert
            var expected = string.Join("\r\n", values);
            Assert.Equal(expected + "\r\n", _out.ToString());
        }

        public void Dispose()
        {
            Console.SetOut(_standardOut);
        }
    }
}

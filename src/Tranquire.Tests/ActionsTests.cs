using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Ploeh.AutoFixture.Idioms;
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
    }
}

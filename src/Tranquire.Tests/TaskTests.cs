using Moq;
using Ploeh.AutoFixture.Idioms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tranquire.Tests
{
    public class TaskTests
    {
        [Theory, DomainAutoData]
        public void Sut_ShouldBeTask(Task sut)
        {
            Assert.IsAssignableFrom<IAction>(sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Task).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorParameters(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(Task));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallAllPerformables(
            Task sut,
            IActor actor)
        {
            //arrange            
            //act
            sut.ExecuteGivenAs(actor);
            //assert
            foreach (var performable in sut.Actions)
            {
                Mock.Get(performable).Verify(p => p.ExecuteGivenAs(actor));
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldReturnCorrectValue(
            Task sut,
            IActor expected)
        {
            //arrange
            //act
            var actual = sut.ExecuteGivenAs(expected);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallAllPerformables(
           Task sut,
           IActor actor)
        {
            //arrange            
            //act
            sut.ExecuteWhenAs(actor);
            //assert
            foreach (var performable in sut.Actions)
            {
                Mock.Get(performable).Verify(p => p.ExecuteWhenAs(actor));
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldReturnCorrectValue(
            Task sut,
            IActor expected)
        {
            //arrange
            //act
            var actual = sut.ExecuteWhenAs(expected);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}

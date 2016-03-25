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
    public class CompositePerformableTests
    {
        [Theory, DomainAutoData]
        public void Sut_ShouldBeTask(CompositePerformable sut)
        {
            Assert.IsAssignableFrom<ITask>(sut);
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeAction(CompositePerformable sut)
        {
            Assert.IsAssignableFrom<IAction>(sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(CompositePerformable).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorParameters(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(CompositePerformable));
        }

        [Theory, DomainAutoData]
        public void PerformAs_ShouldCallAllPerformables(
            CompositePerformable sut,
            IActor actor)
        {
            //arrange            
            //act
            sut.PerformAs(actor);
            //assert
            foreach (var performable in sut.Actions)
            {
                Mock.Get(performable).Verify(p => p.PerformAs(actor));
            }
        }

        [Theory, DomainAutoData]
        public void PerformAs_ShouldReturnCorrectValue(
            CompositePerformable sut,
            IActor expected)
        {
            //arrange
            //act
            var actual = sut.PerformAs(expected);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}

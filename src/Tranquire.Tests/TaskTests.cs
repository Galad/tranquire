using Moq;
using Ploeh.AutoFixture.Idioms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tranquire.Tests
{
    public class TaskTests
    {
        [Theory, DomainAutoData]
        public void Sut_ShouldBeTask(Task<object> sut)
        {
            Assert.IsAssignableFrom<IAction<object, object>>(sut);
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Task<object>).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorParameters(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(Task<object>));
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallAllPerformables(
            Task<object> sut,
            object value,
            IActor actor)
        {
            //arrange            
            //act
            sut.ExecuteGivenAs(actor, value);
            //assert
            foreach (var performable in sut.Actions)
            {
                Mock.Get(performable).Verify(p => p.ExecuteGivenAs(actor, value));
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallAllPerformables(
           Task<object> sut,
           object value,
           IActor actor)
        {
            //arrange            
            //act
            sut.ExecuteWhenAs(actor, value);
            //assert
            foreach (var performable in sut.Actions)
            {
                Mock.Get(performable).Verify(p => p.ExecuteWhenAs(actor, value));
            }
        }
    }
}

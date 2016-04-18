using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Idioms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Tranquire.Tests
{
    public class TaskWithAbilityTests
    {
        [Theory, DomainAutoData]
        public void Sut_ShouldBeTask(Task<object> sut)
        {
            Assert.IsAssignableFrom<IAction>(sut);
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
        public void Sut_VerifyConstructorWithParams(IAction<object, object>[] expected)
        {
            //act            
            var sut = new Task<object>(expected);
            //assert
            sut.Actions.Should().Equal(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallAllPerformables(
            Task<object> sut,
            object value,
            Mock<IActor> actor)
        {
            //arrange            
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            foreach (var action in sut.Actions)
            {
                actor.Verify(a => a.Execute(action));
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallAllPerformables(
           Task<object> sut,
           object value,
           Mock<IActor> actor)
        {
            //arrange            
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            foreach (var action in sut.Actions)
            {
                actor.Verify(a => a.Execute(action));
            }
        }
    }
}

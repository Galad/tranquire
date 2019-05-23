using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Tests
{
    public class TranquireEnumerableExtensionsTests
    {
        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(TranquireEnumerableExtensions));
        }
        
        [Theory, DomainAutoData]
        public void ToAction_ShouldBeActionThatCallsAllActions(            
            IAction<Unit>[] actions, 
            string name,
            Mock<IActor> actor)
        {
            //arrange            
            //act
            var actual = actions.ToAction(name);
            actual.ExecuteWhenAs(actor.Object);
            //assert
            actual.Name.Should().Be(name);
            foreach (var action in actions)
            {
                actor.Verify(m => m.Execute(action));
            }
        }
    }
}

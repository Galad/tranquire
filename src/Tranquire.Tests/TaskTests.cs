using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using Ploeh.AutoFixture.Xunit2;
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
        public void Sut_ShouldBeAction(Task sut)
        {
            sut.Should().BeAssignableTo<IAction>();
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Task));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorParameters(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(Task).GetConstructors()
                                         .Where(c => !c.GetParameters()
                                                       .Select(pi => pi.ParameterType)
                                                       .SequenceEqual(new[] { typeof(Func<Task, Task>) })
                                                )
                            );
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorWithParams(IAction[] expected)
        {
            //act            
            var sut = new Task(expected);
            //assert
            sut.Actions.Should().Equal(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ShouldCallActorExecute(
            Task sut,
            Mock<IActor> actor
            )
        {
            //arrange
            //act
            sut.ExecuteGivenAs(actor.Object);
            //assert
            foreach (var action in sut.Actions)
            {
                actor.Verify(a => a.Execute(action), Times.Once());
            }
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ShouldCallActorExecute(
            Task sut,
            Mock<IActor> actor
            )
        {
            //arrange
            //act
            sut.ExecuteWhenAs(actor.Object);
            //assert
            foreach (var action in sut.Actions)
            {
                actor.Verify(a => a.Execute(action), Times.Once());
            }
        }

        [Theory, DomainAutoData]
        public void And_ShouldAddAction(
           Task sut,
           Mock<IActor> actor,
           IAction expected
           )
        {
            //arrange
            var existingActions = sut.Actions.ToArray();
            //act
            var actual = sut.And(expected);
            //assert            
            actual.Actions.Except(existingActions).Single().Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void And_WithAbility_ShouldWrapAction(           
           [FavorEnumerables]Task sut,
           Mock<IActor> actor,
           IAction<object, object> expected
           )
        {
            //arrange
            var existingActions = sut.Actions.ToArray();
            //act
            var actual = sut.And(expected);
            //assert            
            var actualAction = actual.Actions.Except(existingActions).Single();
            actualAction.Should()
                        .BeOfType<ActionWithAbilityToActionAdapter<object, object>>()
                        .Which
                        .Action
                        .Should()
                        .Be(expected);
        }

        [Theory, DomainAutoData]
        public void And_WithAbility_ShouldContainExistingAbility(
          [FavorEnumerables]Task sut,
          Mock<IActor> actor,
          IAction<object, object> action
          )
        {
            //arrange
            var existingActions = sut.Actions.ToArray();
            //act
            var actual = sut.And(action);
            //assert                        
            actual.Actions.Should().Contain(existingActions);
        }

        [Theory, DomainAutoData]
        public void Sut_WithTaskBuilder_ShouldHaveCorrectTasks(
           IAction[] expected
           )
        {
            //arrange
            //act
            var sut = new Task(t => expected.Aggregate(t, (tresult, tt) => tresult.And(tt)));
            //assert            
            sut.Actions.Should().Equal(expected);
        }
    }
}

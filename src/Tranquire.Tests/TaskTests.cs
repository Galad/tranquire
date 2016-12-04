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
            sut.Should().BeAssignableTo<IAction<Unit>>();
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
        public void Sut_VerifyConstructorWithParams(IAction<Unit>[] expected)
        {
            //act            
            var sut = new Mock<Task>(expected);
            //assert
            sut.Object.Actions.Should().Equal(expected);
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
           IAction<Unit> expected
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
           IFixture fixture,
           Mock<IActor> actor,
           IAction<object, object, Unit> expected
           )
        {
            //arrange
            var sut = new Mock<Task>(fixture.CreateMany<IAction<Unit>>()).Object;
            var existingActions = sut.Actions.ToArray();
            //act
            var actual = sut.And(expected);
            //assert            
            var actualAction = actual.Actions.Except(existingActions).Single();
            actualAction.Should()
                        .BeOfType<ActionWithAbilityToActionAdapter<object, object, Unit>>()
                        .Which
                        .Action
                        .Should()
                        .Be(expected);
        }

        [Theory, DomainAutoData]
        public void And_WithAbility_ShouldContainExistingAbility(
          IFixture fixture,
          Mock<IActor> actor,
          IAction<object, object, Unit> action
          )
        {
            //arrange
            var sut = new Mock<Task>(fixture.CreateMany<IAction<Unit>>()).Object;
            var existingActions = sut.Actions.ToArray();
            //act
            var actual = sut.And(action);
            //assert                        
            actual.Actions.Should().Contain(existingActions);
        }

        [Theory, DomainAutoData]
        public void Sut_WithTaskBuilder_ShouldHaveCorrectTasks(
           IAction<Unit>[] expected
           )
        {
            //arrange
            //act
            var sut = new Mock<Task>(new Func<Task, Task>(t => expected.Aggregate(t, (tresult, tt) => tresult.And(tt))));
            //assert            
            sut.Object.Actions.Should().Equal(expected);
        }

        public class ToStringTask : Task
        {
            public ToStringTask(string name)
            {
                Name = name;
            }
            public override string Name { get; }
        }

        [Theory, DomainAutoData]
        public void ToString_ShouldReturnCorrectValue(ToStringTask sut, string expected)
        {
            //act
            var actual = sut.ToString();
            //assert
            Assert.Equal(sut.Name, actual);
        }

        public class EnumeratorTask : Task
        {
            public EnumeratorTask(IAction<Unit>[] actions):base(actions)
            {
            }

            public override string Name => "";
        }

        [Theory, DomainAutoData]
        public void GetEnumerator_ShouldReturnCorrectValue(EnumeratorTask sut)
        {
            //act            
            //assert
            Assert.Equal(sut.Actions, sut);
        }
    }
}

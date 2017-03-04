using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ploeh.AutoFixture.Idioms;
using Tranquire.ActionBuilders;
using Tranquire.Tests.Extensions;
using Xunit;

namespace Tranquire.Tests
{
    public class ActionBuilderTests
    {
        public class TestAction : ActionUnit
        {
            public override string Name => "";

            protected override void ExecuteWhen(IActor actor)
            {
            }
        }

        public class NextAction : Action<object>
        {
            public override string Name => "";

            protected override object ExecuteWhen(IActor actor) => new object();
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            //test method guard clauses by hand because AutoFixture throws an error when testing a generic method having constraints
            assertion.Verify(typeof(ActionBuilder<TestAction, Unit>).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void Then_VerifyGuardClause(ActionBuilder<TestAction, Unit> sut)
        {
            Assert.Throws<ArgumentNullException>(() => ((IActionBuilder<TestAction, Unit>)sut).Then<IAction<Unit>, Unit>(default(IAction<Unit>)));
        }

        [Theory, DomainAutoData]
        public void Then_VerifyGuardClause(ActionBuilder sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Then<IAction<Unit>, Unit>(default(IAction<Unit>)));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyConstructorInitializedMembers(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(ActionBuilder<TestAction, Unit>).GetConstructors());
        }

        #region Then, ignoring previous results
        [Theory, DomainAutoData]
        public void Then_ShouldReturnCorrectValue(
            ActionBuilder sut,
            NextAction nextAction)
        {
            //arrange
            //act
            var actual = sut.Then<NextAction, object>(nextAction);
            //assert
            actual.Should().BeOfType<ActionBuilder<NextAction, object>>()
                           .Which.Action.Should().Be(nextAction);
        }

        [Theory, DomainAutoData]
        public void Then_ShouldReturnCorrectName(
            ActionBuilder sut,
            IAction<object> nextAction,
            string expected)
        {
            //arrange
            Mock.Get(nextAction).Setup(a => a.Name).Returns(expected);
            //act
            var actual = sut.Then<IAction<object>, object>(nextAction);
            //assert
            actual.Name.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Then_WithMultipleCalls_ShouldReturnCorrectName(
            ActionBuilder sut,
            IAction<object> nextAction1,
            IAction<object> nextAction2,
            IAction<object> nextAction3,
            string name1,
            string name2,
            string name3)
        {
            //arrange            
            Mock.Get(nextAction1).Setup(a => a.Name).Returns(name1);
            Mock.Get(nextAction2).Setup(a => a.Name).Returns(name2);
            Mock.Get(nextAction3).Setup(a => a.Name).Returns(name3);
            //act
            var actual = sut.Then<IAction<object>, object>(nextAction1)
                            .Then<IAction<object>, object>(nextAction2)
                            .Then<IAction<object>, object>(nextAction3);
            //assert
            var expected = $"{name1}, Then {name2}, Then {name3}";
            actual.Name.Should().Be(expected);
        }


        [Theory, DomainAutoData]
        public void ExecuteGivenAs_AfterCallingThen_ShouldReturnCorrectValue(
           ActionBuilder sut,
           Mock<IAction<object>> nextAction,
           IActor actor,
           object expected)
        {
            //arrange
            Mock.Get(actor).SetupExecuteGiven<object>();
            nextAction.Setup(a => a.ExecuteGivenAs(actor)).Returns(expected);
            var action = sut.Then<IAction<object>, object>(nextAction.Object);
            //act
            var actual = action.ExecuteGivenAs(actor);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_AfterCallingThen_ShouldReturnCorrectValue(
           ActionBuilder sut,
           Mock<IAction<object>> nextAction,
           IActor actor,
           object expected)
        {
            //arrange
            Mock.Get(actor).SetupExecuteWhen<object>();
            nextAction.Setup(a => a.ExecuteWhenAs(actor)).Returns(expected);
            var action = sut.Then<IAction<object>, object>(nextAction.Object);
            //act
            var actual = action.ExecuteWhenAs(actor);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_AfterCallingThenMultipleTimes_ShouldReturnCorrectValue(
          ActionBuilder sut,
          Mock<IAction<object>> nextAction1,
          Mock<IAction<object>> nextAction2,
          Mock<IAction<object>> nextAction3,
          IActor actor)
        {
            //arrange
            Mock.Get(actor).SetupExecuteGiven<object>();
            var callResult = new List<string>();
            nextAction1.Setup(a => a.ExecuteGivenAs(actor)).Callback<IActor>(a => callResult.Add("action1"));
            nextAction2.Setup(a => a.ExecuteGivenAs(actor)).Callback<IActor>(a => callResult.Add("action2"));
            nextAction3.Setup(a => a.ExecuteGivenAs(actor)).Callback<IActor>(a => callResult.Add("action3"));
            var action = sut.Then<IAction<object>, object>(nextAction1.Object)
                            .Then<IAction<object>, object>(nextAction2.Object)
                            .Then<IAction<object>, object>(nextAction3.Object);
            //act
            action.ExecuteGivenAs(actor);
            //assert
            var expected = new[] { "action1", "action2", "action3" };
            callResult.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_AfterCallingThenMultipleTimes_ShouldReturnCorrectValue(
          ActionBuilder sut,
          Mock<IAction<object>> nextAction1,
          Mock<IAction<object>> nextAction2,
          Mock<IAction<object>> nextAction3,
          IActor actor)
        {
            //arrange
            Mock.Get(actor).SetupExecuteWhen<object>();
            var callResult = new List<string>();
            nextAction1.Setup(a => a.ExecuteWhenAs(actor)).Callback<IActor>(a => callResult.Add("action1"));
            nextAction2.Setup(a => a.ExecuteWhenAs(actor)).Callback<IActor>(a => callResult.Add("action2"));
            nextAction3.Setup(a => a.ExecuteWhenAs(actor)).Callback<IActor>(a => callResult.Add("action3"));
            var action = sut.Then<IAction<object>, object>(nextAction1.Object)
                            .Then<IAction<object>, object>(nextAction2.Object)
                            .Then<IAction<object>, object>(nextAction3.Object);
            //act
            action.ExecuteWhenAs(actor);
            //assert
            var expected = new[] { "action1", "action2", "action3" };
            callResult.ShouldAllBeEquivalentTo(expected, o => o.WithStrictOrdering());
        }
        #endregion

        #region Then, chaining previous results
        [Theory, DomainAutoData]
        public void Then_ChainingPreviousResult_ShouldReturnCorrectValue(
            ActionBuilder sut,
            IAction<object> action1,
            IAction<object> action2,
            Mock<IFunc> funcs,
            object result1,
            IActor actor)
        {
            //arrange 
            var previousResult = new ActionResult<IAction<object>, object>(action1, result1);
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<object>>(previousResult)).Returns(action2);
            //act
            var actual = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<object>>);
            //assert
            actual.Should().BeOfType<ActionBuilderWithPreviousResult<IAction<object>, object, IAction<object>, object>>()
                           .Which.ActionFactory(previousResult).Should().Be(action2);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ChainingPreviousResult_ShouldReturnCorrectValue(
            ActionBuilder sut,
            IAction<object> action1,
            IAction<object> action2,
            Mock<IFunc> funcs,
            object result1,
            object expected,
            IActor actor)
        {
            //arrange 
            Mock.Get(actor).SetupExecuteGiven<object>();
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<object>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action1 && r.Result == result1)))
                 .Returns(action2);
            Mock.Get(action1).Setup(a => a.ExecuteGivenAs(actor)).Returns(result1);
            Mock.Get(action2).Setup(a => a.ExecuteGivenAs(actor)).Returns(expected);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<object>>);
            var actual = action.ExecuteGivenAs(actor);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ChainingPreviousResult_ShouldReturnCorrectValue(
           ActionBuilder sut,
           IAction<object> action1,
           IAction<object> action2,
           Mock<IFunc> funcs,
           object result1,
           object expected,
           IActor actor)
        {
            //arrange 
            Mock.Get(actor).SetupExecuteWhen<object>();
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<object>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action1 && r.Result == result1)))
                 .Returns(action2);
            Mock.Get(action1).Setup(a => a.ExecuteWhenAs(actor)).Returns(result1);
            Mock.Get(action2).Setup(a => a.ExecuteWhenAs(actor)).Returns(expected);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<object>>);
            var actual = action.ExecuteWhenAs(actor);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ChainingPreviousResults_ShouldReturnCorrectValue(
            ActionBuilder sut,
            IAction<object> action1,
            IAction<object> action2,
            IAction<string> action3,
            Mock<IFunc> funcs,
            object result1,
            object result2,
            string expected,
            IActor actor)
        {
            //arrange 
            Mock.Get(actor).SetupExecuteGiven<object>();
            Mock.Get(actor).SetupExecuteGiven<string>();
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<object>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action1 && r.Result == result1)))
                 .Returns(action2);
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<string>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action2 && r.Result == result2)))
                 .Returns(action3);
            Mock.Get(action1).Setup(a => a.ExecuteGivenAs(actor)).Returns(result1);
            Mock.Get(action2).Setup(a => a.ExecuteGivenAs(actor)).Returns(result2);
            Mock.Get(action3).Setup(a => a.ExecuteGivenAs(actor)).Returns(expected);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<object>>)
                            .Then<IAction<string>, string>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<string>>);
            var actual = action.ExecuteGivenAs(actor);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ChainingPreviousResults_ShouldReturnCorrectValue(
            ActionBuilder sut,
            IAction<object> action1,
            IAction<object> action2,
            IAction<string> action3,
            Mock<IFunc> funcs,
            object result1,
            object result2,
            string expected,
            IActor actor)
        {
            //arrange 
            Mock.Get(actor).SetupExecuteWhen<object>();
            Mock.Get(actor).SetupExecuteWhen<string>();
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<object>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action1 && r.Result == result1)))
                 .Returns(action2);
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<string>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action2 && r.Result == result2)))
                 .Returns(action3);
            Mock.Get(action1).Setup(a => a.ExecuteWhenAs(actor)).Returns(result1);
            Mock.Get(action2).Setup(a => a.ExecuteWhenAs(actor)).Returns(result2);
            Mock.Get(action3).Setup(a => a.ExecuteWhenAs(actor)).Returns(expected);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<object>>)
                            .Then<IAction<string>, string>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<string>>);
            var actual = action.ExecuteWhenAs(actor);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_ChainingPreviousResultsThenDiscardingResult_ShouldReturnCorrectValue(
            ActionBuilder sut,
            IAction<object> action1,
            IAction<object> action2,
            IAction<string> action3,
            IAction<object> action4,
            Mock<IFunc> funcs,
            object result1,
            object result2,
            object expected,
            IActor actor)
        {
            //arrange 
            Mock.Get(actor).SetupExecuteGiven<object>();
            Mock.Get(actor).SetupExecuteGiven<string>();
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<object>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action1 && r.Result == result1)))
                 .Returns(action2);
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<string>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action2 && r.Result == result2)))
                 .Returns(action3);
            Mock.Get(action1).Setup(a => a.ExecuteGivenAs(actor)).Returns(result1);
            Mock.Get(action2).Setup(a => a.ExecuteGivenAs(actor)).Returns(result2);
            Mock.Get(action4).Setup(a => a.ExecuteGivenAs(actor)).Returns(expected);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<object>>)
                            .Then<IAction<string>, string>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<string>>)
                            .Then<IAction<object>, object>(action4);
            var actual = action.ExecuteGivenAs(actor);
            //assert            
            actual.Should().Be(expected);
            Mock.Get(action3).Verify(a => a.ExecuteGivenAs(actor));
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_ChainingPreviousResultsThenDiscardingResult_ShouldReturnCorrectValue(
            ActionBuilder sut,
            IAction<object> action1,
            IAction<object> action2,
            IAction<string> action3,
            IAction<object> action4,
            Mock<IFunc> funcs,
            object result1,
            object result2,
            object expected,
            IActor actor)
        {
            //arrange 
            Mock.Get(actor).SetupExecuteWhen<object>();
            Mock.Get(actor).SetupExecuteWhen<string>();
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<object>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action1 && r.Result == result1)))
                 .Returns(action2);
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<string>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action2 && r.Result == result2)))
                 .Returns(action3);
            Mock.Get(action1).Setup(a => a.ExecuteWhenAs(actor)).Returns(result1);
            Mock.Get(action2).Setup(a => a.ExecuteWhenAs(actor)).Returns(result2);
            Mock.Get(action4).Setup(a => a.ExecuteWhenAs(actor)).Returns(expected);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<object>>)
                            .Then<IAction<string>, string>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<string>>)
                            .Then<IAction<object>, object>(action4);
            var actual = action.ExecuteWhenAs(actor);
            //assert            
            actual.Should().Be(expected);
            Mock.Get(action3).Verify(a => a.ExecuteWhenAs(actor));
        }

        [Theory, DomainAutoData]
        public void ExecuteWhenAs_DiscardingResultsTwiceThenChainingPreviousResult_ShouldReturnCorrectValue(
            ActionBuilder sut,
            IAction<object> action1,
            IAction<object> action2,
            IAction<string> action3,
            Mock<IFunc> funcs,            
            object result2,
            string expected,
            IActor actor)
        {
            //arrange 
            Mock.Get(actor).SetupExecuteWhen<object>();
            Mock.Get(actor).SetupExecuteWhen<string>();            
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<string>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action2 && r.Result == result2)))
                 .Returns(action3);
            Mock.Get(action2).Setup(a => a.ExecuteWhenAs(actor)).Returns(result2);
            Mock.Get(action3).Setup(a => a.ExecuteWhenAs(actor)).Returns(expected);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(action2)
                            .Then<IAction<string>, string>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<string>>);
            var actual = action.ExecuteWhenAs(actor);
            //assert            
            actual.Should().Be(expected);
            Mock.Get(action1).Verify(a => a.ExecuteWhenAs(actor), "First action has not been called");
            Mock.Get(action2).Verify(a => a.ExecuteWhenAs(actor), "Second action has not been called");
        }

        [Theory, DomainAutoData]
        public void ExecuteGivenAs_DiscardingResultsTwiceThenChainingPreviousResult_ShouldReturnCorrectValue(
            ActionBuilder sut,
            IAction<object> action1,
            IAction<object> action2,
            IAction<string> action3,
            Mock<IFunc> funcs,
            object result2,
            string expected,
            IActor actor)
        {
            //arrange 
            Mock.Get(actor).SetupExecuteGiven<object>();
            Mock.Get(actor).SetupExecuteGiven<string>();
            funcs.Setup(f => f.Func<ActionResult<IAction<object>, object>, IAction<string>>(It.Is<ActionResult<IAction<object>, object>>(r => r.Action == action2 && r.Result == result2)))
                 .Returns(action3);
            Mock.Get(action2).Setup(a => a.ExecuteGivenAs(actor)).Returns(result2);
            Mock.Get(action3).Setup(a => a.ExecuteGivenAs(actor)).Returns(expected);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(action2)
                            .Then<IAction<string>, string>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<string>>);
            var actual = action.ExecuteGivenAs(actor);
            //assert            
            actual.Should().Be(expected);
            Mock.Get(action1).Verify(a => a.ExecuteGivenAs(actor), "First action has not been called");
            Mock.Get(action2).Verify(a => a.ExecuteGivenAs(actor), "Second action has not been called");
        }

        [Theory, DomainAutoData]
        public void Then_ChainingPreviousResult_ShouldReturnCorrectName(
            ActionBuilder sut,
            IAction<object> action1,
            Mock<IFunc> funcs,
            string name1)
        {
            //arrange                         
            Mock.Get(action1).Setup(a => a.Name).Returns(name1);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(funcs.Object.Func<ActionResult<IAction<object>, object>, IAction<object>>);
            var actual = action.Name;
            //assert
            var name2 = typeof(IAction<object>).Name;
            var expected = $"{name1}, Then {name2}";
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void Then_ChainingPreviousResultsThenDiscardingResult_ShouldReturnCorrectName(
           ActionBuilder sut,
           IAction<object> action1,
           IAction<object> action2,
           IAction<string> action3,
           IAction<object> action4,
           string name1,
           string name4)
        {
            //arrange 
            Mock.Get(action1).Setup(a => a.Name).Returns(name1);
            Mock.Get(action4).Setup(a => a.Name).Returns(name4);
            //act
            var action = sut.Then<IAction<object>, object>(action1)
                            .Then<IAction<object>, object>(_ => action2)
                            .Then<IAction<string>, string>(_ => action3)
                            .Then<IAction<object>, object>(action4);
            var actual = action.Name;
            //assert            
            var name2 = typeof(IAction<object>).Name;
            var name3 = typeof(IAction<object>).Name;
            var expected = $"{name1}, Then {name2}, Then {name3}, Then {name4}";
            actual.Should().Be(expected);
        }
        #endregion
    }
}

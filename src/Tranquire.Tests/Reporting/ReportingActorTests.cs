using FluentAssertions;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Idioms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Tranquire.Reporting;
using Ploeh.AutoFixture.Xunit2;
using System.Linq.Expressions;
using Ploeh.AutoFixture.Kernel;

namespace Tranquire.Tests.Reporting
{
    public class ReportingActorTests
    {
        public class ReportingActorCustomization : ICustomization
        {               
            public void Customize(IFixture fixture)
            {
                fixture.Register<ICanNotify>(() => new Mock<CanNotify>() { CallBase = true }.Object);
            }
        }

        public class ReportingActorAutoDataAttribute : AutoDataAttribute
        {
            public ReportingActorAutoDataAttribute() : base(CreateFixture())
            {
            }
        }

        private static IFixture CreateFixture()
        {
            return new Fixture().Customize(new DomainCustomization())
                                .Customize(new ReportingActorCustomization())
                                .Customize(new ConstructorCustomization(typeof(ReportingActor), new GreedyConstructorQuery()));
        }

        [Theory, DomainAutoData]
        public void Sut_ShouldBeActor(
            ReportingActor sut)
        {
            sut.Should().BeAssignableTo<IActor>();
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(
            GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ReportingActor));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyInitializedMembers(
            ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(ReportingActor).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void Sut_UsingModestConstructor_MeasureDurationShouldBeDefaultMeasureDuration(
            [Modest]ReportingActor sut)
        {
            sut.MeasureTime.Should().BeOfType<DefaultMeasureDuration>();
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(
            ReportingActor sut,
            string expected)
        {
            //arrange
            Mock.Get(sut.Actor).Setup(a => a.Name).Returns(expected);
            //act
            var actual = sut.Name;
            //assert
            Assert.Equal(expected, actual);
        }
        
        [Theory, MemberData("ExecutionTestCases")]
        public void Sut_AllMethods_ShouldReturnInnerResult(
            Func<ReportingActor, INamed, object> executeAction,
            Func<INamed, Expression<Func<IActor, object>>> expression)
        {
            //arrange                  
            var fixture = CreateFixture();            
            var measureDuration = fixture.Freeze<Mock<IMeasureDuration>>();
            var duration = fixture.Create<TimeSpan>();
            var sut = fixture.Create<ReportingActor>();
            var action = fixture.Create<MockToString>();
            var expected = fixture.Create<object>();
            measureDuration.Setup(m => m.Measure(It.IsAny<Func<object>>()))
                           .Returns((Func<object> f) => Tuple.Create(duration, f()));
            Mock.Get(sut.Actor).Setup(expression(action)).Returns(expected);
            //act
            var actual = executeAction(sut, action);
            //assert
            Assert.Equal(expected, actual);
        }

        //Moq does not mock ToString if it is not overridden in the class
        public class MockToString : IQuestion<object>, IQuestion<object, Ability1>, IAction<object>, IAction<Ability1, Ability2, object>
        {
            public string ToStringValue { get; }
            public object Result { get; }

            public MockToString(string toStringValue, object result)
            {
                ToStringValue = toStringValue;
                Result = result;
            }

            public object AnsweredBy(IActor actor) => Result;
            public object AnsweredBy(IActor actor, Ability1 ability) => Result;
            public object ExecuteGivenAs(IActor actor) => new object();
            public object ExecuteGivenAs(IActor actor, Ability1 ability) => Result;
            public object ExecuteWhenAs(IActor actor) => Result;
            public object ExecuteWhenAs(IActor actor, Ability2 ability) => Result;
            public string Name => "";
            public override string ToString() => ToStringValue;
        }
        
        public static IEnumerable<object[]> ExecutionTestCases
        {
            get
            {
                return QuestionsTestCases.Concat(ActionTestCases);
            }
        }

        public static IEnumerable<object[]> ActionTestCases
        {
            get
            {                
                yield return ExecutionTestCasesValues((sut, action) => sut.Execute((IAction<object>)action), action => a => a.Execute((IAction<object>)action));
                yield return ExecutionTestCasesValues((sut, action) => sut.Execute((IAction<Ability1, Ability2, object>)action), action => a => a.Execute((IAction<Ability1, Ability2, object>)action));
            }
        }

        public static IEnumerable<object[]> QuestionsTestCases
        {
            get
            {
                yield return ExecutionTestCasesValues((sut, action) => sut.AsksFor((IQuestion<object>)action), action => a => a.AsksFor((IQuestion<object>)action));
                yield return ExecutionTestCasesValues((sut, action) => sut.AsksFor((IQuestion<object, Ability1>)action), action => a => a.AsksFor((IQuestion<object, Ability1>)action));                
            }
        }

        private static object[] ExecutionTestCasesValues(
            Func<ReportingActor, INamed, object> action,
            Func<INamed, Expression<Func<IActor, object>>> expression
            )
        {
            return new object[] { action, expression };
        }

        [Theory, MemberData("ExecutionTestCases")]
        public void Sut_AllMethods_ShouldCallOnNext(
            Func<ReportingActor, INamed, object> executeAction,
            Func<INamed, Expression<Func<IActor, object>>> dummy)
        {
            //arrange                  
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            var measureDuration = fixture.Freeze<Mock<IMeasureDuration>>();
            var expectedDuration = fixture.Create<TimeSpan>();
            var sut = fixture.Create<ReportingActor>();
            var action = fixture.Create<MockToString>();
            measureDuration.Setup(m => m.Measure(It.IsAny<Func<object>>())).Returns(Tuple.Create(expectedDuration, new object()));
            //act
            executeAction(sut, action);
            //assert
            var expected = new[]{
                new ActionNotification(action, 1, new BeforeActionNotificationContent()),
                new ActionNotification(action, 1, new AfterActionNotificationContent(expectedDuration))
            };
            observer.Values.ShouldAllBeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        [Theory, MemberData("ExecutionTestCases")]
        public void Sut_AllMethods_Recursive_ShouldCallOnNext(
            Func<ReportingActor, INamed, object> executeAction,
            Func<INamed, Expression<Func<IActor, object>>> expression)
        {
            //arrange 
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            var measureDuration = fixture.Freeze<Mock<IMeasureDuration>>();
            var sut = fixture.Create<ReportingActor>();
            const int NumberOfActions = 4;
            var actions = fixture.CreateMany<MockToString>(NumberOfActions).ToArray();
            var expectedDurations = fixture.CreateMany<TimeSpan>(NumberOfActions).ToArray();
            for (var i = 0; i < actions.Length - 1; i++)
            {
                var ii = i;
                Mock.Get(sut.Actor).Setup(expression(actions[ii]))
                                   .Returns(() =>
                                   {
                                       var innerResult = executeAction(sut, actions[ii + 1]);
                                       return actions[ii].Result;                                       
                                   });
                measureDuration.Setup(m => m.Measure(It.IsAny<Func<object>>()))
                               .Returns((Func<object> f) =>
                               {
                                   var result = f();
                                   var resultIndex = Array.FindIndex(actions, a => a.Result == result);
                                   var duration = expectedDurations[resultIndex == -1 ? NumberOfActions - 1 : resultIndex];                                   
                                   return Tuple.Create(duration, result);
                               });
            }
            //act
            executeAction(sut, actions[0]);
            //assert
            var expected = new[]{
                new ActionNotification(actions[0], 1, new BeforeActionNotificationContent()),
                new ActionNotification(actions[1], 2, new BeforeActionNotificationContent()),
                new ActionNotification(actions[2], 3, new BeforeActionNotificationContent()),
                new ActionNotification(actions[3], NumberOfActions, new BeforeActionNotificationContent()),
                new ActionNotification(actions[3], NumberOfActions, new AfterActionNotificationContent(expectedDurations[3])),
                new ActionNotification(actions[2], 3, new AfterActionNotificationContent(expectedDurations[2])),
                new ActionNotification(actions[1], 2, new AfterActionNotificationContent(expectedDurations[1])),
                new ActionNotification(actions[0], 1, new AfterActionNotificationContent(expectedDurations[0]))
            };
            observer.Values.ShouldAllBeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        [Theory, MemberData("ExecutionTestCases")]
        public void Sut_AllMethods_WhenErrorOccurs_ShouldThrow(
          Func<ReportingActor, INamed, object> executeAction,
          Func<INamed, Expression<Func<IActor, object>>> dummy)
        {
            //arrange   
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            var measureDuration = fixture.Freeze<Mock<IMeasureDuration>>();
            var sut = fixture.Create<ReportingActor>();
            var action = fixture.Create<MockToString>();
            var expected = fixture.Create<Exception>();
            measureDuration.Setup(m => m.Measure(It.IsAny<Func<object>>())).Throws(expected);
            //act and assert
            var actual = Assert.Throws<Exception>(() => executeAction(sut, action));
            Assert.Equal(expected, actual);
        }

        [Theory, MemberData("ExecutionTestCases")]
        public void Sut_AllMethods_WhenErrorOccurs_ShouldCallOnNext(
           Func<ReportingActor, INamed, object> executeAction,
           Func<INamed, Expression<Func<IActor, object>>> dummy)
        {
            //arrange                  
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            var measureDuration = fixture.Freeze<Mock<IMeasureDuration>>();            
            var sut = fixture.Create<ReportingActor>();
            var action = fixture.Create<MockToString>();
            var exception = fixture.Create<Exception>();
            measureDuration.Setup(m => m.Measure(It.IsAny<Func<object>>())).Throws(exception);
            //act
            try
            {
                executeAction(sut, action);
            }
            catch { }
            //assert
            var expected = new[]{
                new ActionNotification(action, 1, new BeforeActionNotificationContent()),
                new ActionNotification(action, 1, new ExecutionErrorNotificationContent(exception))
            };
            observer.Values.ShouldAllBeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        [Theory, MemberData("ExecutionTestCases")]
        public void Sut_AllMethods_Recursive_WhenErrorOccurs_ShouldCallOnNext(
            Func<ReportingActor, INamed, object> executeAction,
            Func<INamed, Expression<Func<IActor, object>>> expression)
        {
            //arrange 
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            var measureDuration = fixture.Freeze<Mock<IMeasureDuration>>();
            var sut = fixture.Create<ReportingActor>();
            const int NumberOfActions = 4;
            var actions = fixture.CreateMany<MockToString>(NumberOfActions).ToArray();            
            var exception = fixture.Create<Exception>();

            for (var i = 0; i < actions.Length - 1; i++)
            {
                var ii = i;
                Mock.Get(sut.Actor).Setup(expression(actions[ii]))
                                   .Returns(() => executeAction(sut, actions[ii + 1]));
            }
            Mock.Get(sut.Actor).Setup(expression(actions.Last()))
                               .Throws(exception);
            measureDuration.Setup(m => m.Measure(It.IsAny<Func<object>>()))
                               .Returns((Func<object> f) => Tuple.Create(TimeSpan.Zero, f()));
            //act
            new Action(() => { executeAction(sut, actions[0]); }).ShouldThrow<Exception>().And.Should().Be(exception);            
            //assert
            var expected = new[]{
                new ActionNotification(actions[0], 1, new BeforeActionNotificationContent()),
                new ActionNotification(actions[1], 2, new BeforeActionNotificationContent()),
                new ActionNotification(actions[2], 3, new BeforeActionNotificationContent()),
                new ActionNotification(actions[3], NumberOfActions, new BeforeActionNotificationContent()),
                new ActionNotification(actions[3], NumberOfActions, new ExecutionErrorNotificationContent(exception)),
                new ActionNotification(actions[2], 3, new ExecutionErrorNotificationContent(exception)),
                new ActionNotification(actions[1], 2, new ExecutionErrorNotificationContent(exception)),
                new ActionNotification(actions[0], 1, new ExecutionErrorNotificationContent(exception))
            };
            observer.Values.ShouldAllBeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        [Fact]
        public void Sut_AllMethods_WhenActionIsAdapterToActionUnit_ShouldNotCallOnNext()
        {
            //arrange                  
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);                        
            var sut = fixture.Create<ReportingActor>();
            var action = fixture.Create<MockToString>();
            var adaptedAction = action.AsActionWithoutAbility();            
            var expected = fixture.Create<object>();
            Mock.Get(sut.Actor).Setup(a => a.Execute(adaptedAction)).Returns(expected);
            //act
            var actual = sut.Execute(adaptedAction);
            //assert            
            observer.Values.Should().BeEmpty();
            actual.Should().Be(expected);
        }

        [Theory, ReportingActorAutoDataAttribute]
        public void Execute_WhenCanNotifyReturnsFalse_ShouldNotNotify(
            [Frozen]TestObserver<ActionNotification> observer,
            [Frozen]ICanNotify canNotify,
            ReportingActor sut,
            IAction<object> action)
        {
            //arrange                                          
            Mock.Get(canNotify).Setup(c => c.Action(action)).Returns(false);
            //act
            sut.Execute(action);
            //assert
            observer.Values.Should().BeEmpty();
        }
        
        [Theory, ReportingActorAutoDataAttribute]
        public void ExecuteWithAbility_WhenCanNotifyReturnsFalse_ShouldNotNotify(
            [Frozen]TestObserver<ActionNotification> observer,
            [Frozen]ICanNotify canNotify,
            ReportingActor sut,
            IAction<Ability1, Ability2, object> action)
        {
            //arrange                                          
            Mock.Get(canNotify).Setup(c => c.Action(action)).Returns(false);
            //act
            sut.Execute(action);
            //assert
            observer.Values.Should().BeEmpty();
        }

        [Theory, ReportingActorAutoData]
        public void AsksFor_WhenCanNotifyReturnsFalse_ShouldNotNotify(
            [Frozen]TestObserver<ActionNotification> observer,
            [Frozen]ICanNotify canNotify,
            ReportingActor sut,
            IQuestion<object> question)
        {
            //arrange                                          
            Mock.Get(canNotify).Setup(c => c.Question(question)).Returns(false);
            //act
            sut.AsksFor(question);
            //assert
            observer.Values.Should().BeEmpty();
        }

        [Theory, ReportingActorAutoData]
        public void AsksFor_WithAbility_WhenCanNotifyReturnsFalse_ShouldNotNotify(
            [Frozen]TestObserver<ActionNotification> observer,
            [Frozen]ICanNotify canNotify,
            ReportingActor sut,
            IQuestion<object, Ability1> question)
        {
            //arrange                                          
            Mock.Get(canNotify).Setup(c => c.Question(question)).Returns(false);
            //act
            sut.AsksFor(question);
            //assert
            observer.Values.Should().BeEmpty();
        }
    }
}

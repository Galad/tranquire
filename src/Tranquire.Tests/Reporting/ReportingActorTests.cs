using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tranquire.Reporting;
using Xunit;

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
            public ReportingActorAutoDataAttribute() : base(CreateFixture)
            {
            }
        }

        public class ReportingActorInlineAutoDataAttribute : InlineAutoDataAttribute
        {
            public ReportingActorInlineAutoDataAttribute(params object[] values) : base(new ReportingActorAutoDataAttribute(), values)
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

        [Theory, MemberData(nameof(ExecutionTestCases))]
        public void Sut_AllMethods_ShouldReturnInnerResult(
            Func<ReportingActor, INamed, object> executeAction,
            Func<INamed, Expression<Func<IActor, object>>> expression,
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
            CommandType commandType)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
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
#pragma warning disable CS0618 // Type or member is obsolete
        public class MockToString : IQuestion<object>, IQuestion<object, Ability1>, IAction<object>, IAction<Ability1, Ability2, object>
#pragma warning restore CS0618 // Type or member is obsolete
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
                return NotifyingTestCases.Concat(NotNotifyingActionTestCases);
            }
        }

        public static IEnumerable<object[]> NotifyingTestCases
        {
            get
            {
                yield return ExecutionTestCasesValues((sut, action) => sut.AsksFor((IQuestion<object>)action), action => a => a.AsksFor((IQuestion<object>)action), CommandType.Question);
                yield return ExecutionTestCasesValues((sut, action) => sut.Execute((IAction<object>)action), action => a => a.Execute((IAction<object>)action), CommandType.Action);
            }
        }

        public static IEnumerable<object[]> NotNotifyingActionTestCases
        {
            get
            {
#pragma warning disable CS0618 // Type or member is obsolete
                yield return ExecutionTestCasesValues(
                    (sut, action) => sut.ExecuteWithAbility((IAction<Ability1, Ability2, object>)action),
                    action => a => a.ExecuteWithAbility((IAction<Ability1, Ability2, object>)action),
                    CommandType.Action);
                yield return ExecutionTestCasesValues(
                    (sut, action) => sut.AsksForWithAbility((IQuestion<object, Ability1>)action),
                    action => a => a.AsksForWithAbility((IQuestion<object, Ability1>)action),
                    CommandType.Question);
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        private static object[] ExecutionTestCasesValues(
            Func<ReportingActor, INamed, object> action,
            Func<INamed, Expression<Func<IActor, object>>> expression,
            CommandType commandType
            )
        {
            return new object[] { action, expression, commandType };
        }

        [Theory, MemberData(nameof(NotifyingTestCases))]
        public void Sut_AllMethods_ShouldCallOnNext(
            Func<ReportingActor, INamed, object> executeAction,
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
            Func<INamed, Expression<Func<IActor, object>>> dummy,
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
            CommandType commandType)
        {
            //arrange                  
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            var date = fixture.Freeze<DateTimeOffset>();
            var measureDuration = fixture.Freeze<Mock<IMeasureDuration>>();
            var expectedDuration = fixture.Create<TimeSpan>();
            var sut = fixture.Create<ReportingActor>();
            var action = fixture.Create<MockToString>();            
            measureDuration.Setup(m => m.Measure(It.IsAny<Func<object>>())).Returns(Tuple.Create(expectedDuration, new object()));
            //act
            executeAction(sut, action);
            //assert
            var expected = new[]{
                new ActionNotification(action, 1, new BeforeActionNotificationContent(date, commandType)),
                new ActionNotification(action, 1, new AfterActionNotificationContent(expectedDuration))
            };
            observer.Values.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        [Theory, MemberData(nameof(NotifyingTestCases))]
        public void Sut_AllMethods_Recursive_ShouldCallOnNext(
            Func<ReportingActor, INamed, object> executeAction,
            Func<INamed, Expression<Func<IActor, object>>> expression,
            CommandType commandType)
        {
            //arrange 
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            var date = fixture.Freeze<DateTimeOffset>();
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
            BeforeActionNotificationContent before() => new BeforeActionNotificationContent(date, commandType);
            var expected = new[]{
                new ActionNotification(actions[0], 1, before()),
                new ActionNotification(actions[1], 2, before()),
                new ActionNotification(actions[2], 3, before()),
                new ActionNotification(actions[3], NumberOfActions, before()),
                new ActionNotification(actions[3], NumberOfActions, new AfterActionNotificationContent(expectedDurations[3])),
                new ActionNotification(actions[2], 3, new AfterActionNotificationContent(expectedDurations[2])),
                new ActionNotification(actions[1], 2, new AfterActionNotificationContent(expectedDurations[1])),
                new ActionNotification(actions[0], 1, new AfterActionNotificationContent(expectedDurations[0]))
            };
            observer.Values.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        [Theory, MemberData(nameof(ExecutionTestCases))]
        public void Sut_AllMethods_WhenErrorOccurs_ShouldThrow(
          Func<ReportingActor, INamed, object> executeAction,
          Func<INamed, Expression<Func<IActor, object>>> expression,
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
          CommandType commandType)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
            //arrange   
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            fixture.Inject<IMeasureDuration>(new DefaultMeasureDuration());
            var sut = fixture.Create<ReportingActor>();
            var action = fixture.Create<MockToString>();
            var expected = fixture.Create<Exception>();
            Mock.Get(sut.Actor).Setup(expression(action)).Throws(expected);
            //act and assert
            var actual = Assert.Throws<Exception>(() => executeAction(sut, action));
            Assert.Equal(expected, actual);
        }

        [Theory, MemberData(nameof(NotifyingTestCases))]
        public void Sut_AllMethods_WhenErrorOccurs_ShouldCallOnNext(
           Func<ReportingActor, INamed, object> executeAction,
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
           Func<INamed, Expression<Func<IActor, object>>> dummy,
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
           CommandType commandType)
        {
            //arrange                  
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            var date = fixture.Freeze<DateTimeOffset>();
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
                new ActionNotification(action, 1, new BeforeActionNotificationContent(date, commandType)),
                new ActionNotification(action, 1, new ExecutionErrorNotificationContent(exception))
            };
            observer.Values.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        [Theory, MemberData(nameof(NotifyingTestCases))]
        public void Sut_AllMethods_Recursive_WhenErrorOccurs_ShouldCallOnNext(
            Func<ReportingActor, INamed, object> executeAction,
            Func<INamed, Expression<Func<IActor, object>>> expression,
            CommandType commandType)
        {
            //arrange 
            var fixture = CreateFixture();
            var observer = new TestObserver<ActionNotification>();
            fixture.Inject((IObserver<ActionNotification>)observer);
            var date = fixture.Freeze<DateTimeOffset>();
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
            new System.Action(() => { executeAction(sut, actions[0]); }).Should().Throw<Exception>().And.Should().Be(exception);
            //assert
            BeforeActionNotificationContent before() => new BeforeActionNotificationContent(date, commandType);
            var expected = new[]{
                new ActionNotification(actions[0], 1, before()),
                new ActionNotification(actions[1], 2, before()),
                new ActionNotification(actions[2], 3, before()),
                new ActionNotification(actions[3], NumberOfActions, before()),
                new ActionNotification(actions[3], NumberOfActions, new ExecutionErrorNotificationContent(exception)),
                new ActionNotification(actions[2], 3, new ExecutionErrorNotificationContent(exception)),
                new ActionNotification(actions[1], 2, new ExecutionErrorNotificationContent(exception)),
                new ActionNotification(actions[0], 1, new ExecutionErrorNotificationContent(exception))
            };
            observer.Values.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        [Theory, ReportingActorAutoData]
        public void ExecuteWithAbility_ShouldNotNotify(
            [Frozen]TestObserver<ActionNotification> observer,
            ReportingActor sut,
#pragma warning disable CS0618 // Type or member is obsolete
            IAction<Ability1, Ability2, object> action)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            //arrange                                                      
            //act
            sut.ExecuteWithAbility(action);
            //assert
            observer.Values.Should().BeEmpty();
        }

        [Theory, ReportingActorAutoData]
        public void AsksFor_WithAbility_ShouldNotNotify(
            [Frozen]TestObserver<ActionNotification> observer,
            ReportingActor sut,
#pragma warning disable CS0618 // Type or member is obsolete
            IQuestion<object, Ability1> question)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            //arrange                                                      
            //act
            sut.AsksForWithAbility(question);
            //assert
            observer.Values.Should().BeEmpty();
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
        public void Execute_WithThenAction_ShouldCallOnNext(
            [Frozen(Matching.ImplementedInterfaces)]TestObserver<ActionNotification> observer,
            [Frozen]DateTimeOffset date,
            [Frozen]IMeasureDuration measureDuration,
            ReportingActor sut,
            TimeSpan expectedDuration,
            ThenAction<object> thenAction
            )
        {
            //arrange                              
            Mock.Get(measureDuration).Setup(m => m.Measure(It.IsAny<Func<object>>())).Returns(Tuple.Create(expectedDuration, new object()));
            //act
            sut.Execute(thenAction);
            //assert
            var expected = new[]{
                new ActionNotification(thenAction, 1, new BeforeThenNotificationContent<object>(date, thenAction.Question)),
                new ActionNotification(thenAction, 1, new AfterThenNotificationContent(expectedDuration, ThenOutcome.Pass))
            };
            observer.Values.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        [Theory, ReportingActorAutoData]
        public void Execute_WithThenActionWithError_ShouldCallOnNext(
            [Frozen(Matching.ImplementedInterfaces)]TestObserver<ActionNotification> observer,
            [Frozen]DateTimeOffset date,
            [Frozen]IMeasureDuration measureDuration,
            ReportingActor sut,
            ThenAction<object> thenAction,
            Exception error
            )
        {
            //arrange                
            Mock.Get(measureDuration).Setup(m => m.Measure(It.IsAny<Func<object>>()))
                                     .Throws(error);
            //act
            try
            {
                sut.Execute(thenAction);
            }
            catch (Exception)
            {
            }
            //assert
            var expected = new[]{
                new ActionNotification(thenAction, 1, new BeforeThenNotificationContent<object>(date, thenAction.Question)),
                new ActionNotification(thenAction, 1, new AfterThenNotificationContent(TimeSpan.Zero, ThenOutcome.Error, error))
            };
            observer.Values.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }

        private static System.Action[] _assertions = new System.Action[]
        {
            () => Assert.True(false),            
            () => NUnit.Framework.Assert.Fail(),
            () => NUnit.Framework.Assert.That(true, NUnit.Framework.Is.False),
            () => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail()
        };

        [Theory]
        [ReportingActorInlineAutoData(0)]
        [ReportingActorInlineAutoData(1)]
        [ReportingActorInlineAutoData(2)]
        [ReportingActorInlineAutoData(3)]        
        public void Execute_WithThenActionWithAssertionError_ShouldCallOnNext(
            int i,
            [Frozen(Matching.ImplementedInterfaces)]TestObserver<ActionNotification> observer,
            [Frozen]DateTimeOffset date,
            [Frozen]IMeasureDuration measureDuration,
            ReportingActor sut,
            ThenAction<object> thenAction            
            )
        {
            //arrange                
            Exception expectedException = null;
            Mock.Get(measureDuration).Setup(m => m.Measure(It.IsAny<Func<object>>()))
                                     .Returns(() =>
                                     {
                                         try
                                         {
                                             _assertions[i]();
                                         }
                                         catch (Exception ex)
                                         {
                                             expectedException = ex;
                                             throw;
                                         }
                                         return Tuple.Create(TimeSpan.Zero, new object());
                                     });
            //act
            try
            {
                sut.Execute(thenAction);
            }
            catch (Exception)
            {
            }
            //assert
            var expected = new[]{
                new ActionNotification(thenAction, 1, new BeforeThenNotificationContent<object>(date, thenAction.Question)),
                new ActionNotification(thenAction, 1, new AfterThenNotificationContent(TimeSpan.Zero, ThenOutcome.Failed, expectedException))
            };
            observer.Values.Should().BeEquivalentTo(expected, o => o.RespectingRuntimeTypes());
        }
    }
}

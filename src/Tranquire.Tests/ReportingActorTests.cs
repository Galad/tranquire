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

namespace Tranquire.Tests
{
    public class ReportingActorTests
    {
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

        [Theory, DomainAutoData]
        public void AsksFor_ReturnsInnerResult(
           ReportingActor sut,
           IQuestion<object> question,
           object expected)
        {
            //arrange
            Mock.Get(sut.Actor).Setup(a => a.AsksFor(question)).Returns(expected);
            //act
            var actual = sut.AsksFor(question);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void AsksFor_WithAbility_ReturnsInnerResult(
           ReportingActor sut,
           IQuestion<object, Ability1> question,
           object expected)
        {
            //arrange
            Mock.Get(sut.Actor).Setup(a => a.AsksFor(question)).Returns(expected);
            //act
            var actual = sut.AsksFor(question);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Execute_ReturnsInnerResult(
          ReportingActor sut,
          IAction<object> action,
          object expected)
        {
            //arrange
            Mock.Get(sut.Actor).Setup(a => a.Execute(action)).Returns(expected);
            //act
            var actual = sut.Execute(action);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Execute_WithAbility_ReturnsInnerResult(
           ReportingActor sut,
           IAction<Ability1, Ability2, object> action,
           object expected)
        {
            //arrange
            Mock.Get(sut.Actor).Setup(a => a.Execute(action)).Returns(expected);
            //act
            var actual = sut.Execute(action);
            //assert
            Assert.Equal(expected, actual);
        }

        //Moq does not mock ToString if it is not overridden in the class
        public class MockToString : IQuestion<object>, IQuestion<object, Ability1>, IAction<object>, IAction<Ability1, Ability2, object>
        {
            public string ToStringValue { get; }

            public MockToString(string toStringValue)
            {
                ToStringValue = toStringValue;
            }

            public object AnsweredBy(IActor actor) => new object();
            public object AnsweredBy(IActor actor, Ability1 ability) => new object();
            public object ExecuteGivenAs(IActor actor) => new object();
            public object ExecuteGivenAs(IActor actor, Ability1 ability) => new object();
            public object ExecuteWhenAs(IActor actor) => new object();
            public object ExecuteWhenAs(IActor actor, Ability2 ability) => new object();

            public override string ToString() => ToStringValue;
        }

        [Theory, DomainAutoData]
        public void AsksFor_ShouldCallOnNext(
            ReportingActor sut,
            MockToString question)
        {
            //arrange                  
            //act
            sut.AsksFor((IQuestion<object>)question);
            //assert
            var expected = "---Asking for question : " + question.ToStringValue;
            Mock.Get(sut.Observer).Verify(o => o.OnNext(expected));
        }

        [Theory, DomainAutoData]
        public void AsksFor_WithAbility_ShouldCallOnNext(
            ReportingActor sut,
            MockToString question)
        {
            //arrange                  
            //act
            sut.AsksFor((IQuestion<object, Ability1>)question);
            //assert
            var expected = "---Asking for question : " + question.ToStringValue;
            Mock.Get(sut.Observer).Verify(o => o.OnNext(expected));
        }


        [Theory, DomainAutoData]
        public void Execute_ShouldCallOnNext(
            ReportingActor sut,
            MockToString action)
        {
            //arrange                  
            //act
            sut.Execute((IAction<object>)action);
            //assert
            var expected = "---Executing : " + action.ToStringValue;
            Mock.Get(sut.Observer).Verify(o => o.OnNext(expected));
        }

        [Theory, DomainAutoData]
        public void Execute_WithAbility_ShouldCallOnNext(
          ReportingActor sut,
          MockToString action)
        {
            //arrange                  
            //act
            sut.Execute((IAction<Ability1, Ability2, object>)action);
            //assert
            var expected = "---Executing : " + action.ToStringValue;
            Mock.Get(sut.Observer).Verify(o => o.OnNext(expected));
        }

        [Theory, DomainAutoData]
        public void AsksFor_Recursive_ShouldCallOnNext(
            ReportingActor sut,
            IFixture fixture)
        {
            //arrange            
            var questions = fixture.CreateMany<MockToString>(4).ToArray();            
            for (var i = 0; i < questions.Length - 1; i++)
            {
                var ii = i;
                Mock.Get(sut.Actor).Setup(a => a.AsksFor((IQuestion<object>)questions[ii]))
                                   .Returns(() => sut.AsksFor((IQuestion<object>)questions[ii + 1]));
            }            
            //act
            sut.AsksFor((IQuestion<object>)questions[0]);
            //assert
            var j = 1;
            foreach (var q in questions)
            {
                var expected = new String('-', 3 * j) + "Asking for question : " + q.ToStringValue;
                Mock.Get(sut.Observer).Verify(o => o.OnNext(expected));
                j++;
            }            
        }

        [Theory, DomainAutoData]
        public void AsksFor_WithAbility_Recursive_ShouldCallOnNext(
            ReportingActor sut,
            Ability1 ability,
            IFixture fixture)
        {
            //arrange            
            var questions = fixture.CreateMany<MockToString>(4).ToArray();
            for (var i = 0; i < questions.Length - 1; i++)
            {
                var ii = i;
                Mock.Get(sut.Actor).Setup(a => a.AsksFor((IQuestion<object, Ability1>)questions[ii]))
                                   .Returns(() => sut.AsksFor((IQuestion<object, Ability1>)questions[ii + 1]));
            }
            //act
            sut.AsksFor((IQuestion<object, Ability1>)questions[0]);
            //assert
            var j = 1;
            foreach (var q in questions)
            {
                var expected = new String('-', 3 * j) + "Asking for question : " + q.ToStringValue;
                Mock.Get(sut.Observer).Verify(o => o.OnNext(expected));
                j++;
            }
        }

        [Theory, DomainAutoData]
        public void Execute_Recursive_ShouldCallOnNext(
            ReportingActor sut,
            IFixture fixture)
        {
            //arrange            
            var actions = fixture.CreateMany<MockToString>(4).ToArray();
            for (var i = 0; i < actions.Length - 1; i++)
            {
                var ii = i;
                Mock.Get(sut.Actor).Setup(a => a.Execute((IAction<object>)actions[ii]))
                                   .Returns(() => sut.Execute((IAction<object>)actions[ii + 1]));
            }
            //act
            sut.Execute((IAction<object>)actions[0]);
            //assert
            var j = 1;
            foreach (var a in actions)
            {
                var expected = new String('-', 3 * j) + "Executing : " + a.ToStringValue;
                Mock.Get(sut.Observer).Verify(o => o.OnNext(expected));
                j++;
            }
        }

        [Theory, DomainAutoData]
        public void Execute_WithAbility_Recursive_ShouldCallOnNext(
            ReportingActor sut,
            IFixture fixture)
        {
            //arrange            
            var actions = fixture.CreateMany<MockToString>(4).ToArray();
            for (var i = 0; i < actions.Length - 1; i++)
            {
                var ii = i;
                Mock.Get(sut.Actor).Setup(a => a.Execute((IAction<Ability1, Ability2, object>)actions[ii]))
                                   .Returns(() => sut.Execute((IAction<Ability1, Ability2, object>)actions[ii + 1]));
            }
            //act
            sut.Execute((IAction<Ability1, Ability2, object>)actions[0]);
            //assert
            var j = 1;
            foreach (var a in actions)
            {
                var expected = new String('-', 3 * j) + "Executing : " + a.ToStringValue;
                Mock.Get(sut.Observer).Verify(o => o.OnNext(expected));
                j++;
            }
        }
    }
}

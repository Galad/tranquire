using FluentAssertions;
using Moq;
using Moq.Protected;
using AutoFixture.Idioms;
using System;
using Xunit;

namespace Tranquire.Tests
{
    public class QuestionWithAbilityTests
    {
        public class TestQuestion : QuestionBase<object, Ability1>
        {
            public override string Name { get; }

            public TestQuestion(string name)
            {
                Name = name;
            }

            protected override object Answer(IActor actor, Ability1 ability)
            {
                throw new NotImplementedException();
            }
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(QuestionBase<object, Ability1>));
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyInitializedMembers(ConstructorInitializedMemberAssertion assertion)
        {
            assertion.Verify(typeof(QuestionBase<object, Ability1>).GetConstructors());
        }

        [Theory, DomainAutoData]
        public void AnswerBy_ShouldReturnCorrectValue(
            QuestionBase<object, Ability1> sut,
            IActor actor,
            object expected)
        {
            //arrange
#pragma warning disable CS0618 // Type or member is obsolete
            Mock.Get(actor).Setup(a => a.AsksForWithAbility(sut)).Returns(expected);
#pragma warning restore CS0618 // Type or member is obsolete
            //act
            var actual = sut.AnsweredBy(actor);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void AnswerByWithAbility_ShouldReturnCorrectValue(
            QuestionBase<object, Ability1> sut,
            IActor actor,
            Ability1 ability,
            object expected)
        {
            //arrange
            Mock.Get(sut).Protected()
                         .Setup<object>("Answer", ItExpr.Is<IActor>(a => a == actor), ability)
                         .Returns(expected);
            //act
            var actual = sut.AnsweredBy(actor, ability);
            //assert
            actual.Should().Be(expected);
        }

        [Theory, DomainAutoData]
        public void ToString_ShouldReturnCorrectValue(
            TestQuestion sut)
        {
            //act
            var actual = sut.ToString();
            //assert
            actual.Should().Be(sut.Name);
        }
    }
}

﻿using FluentAssertions;
using Moq;
using Moq.Protected;
using AutoFixture.Idioms;
using System;
using Xunit;


namespace Tranquire.Tests
{
    public class QuestionTests
    {
        public class TestQuestion : Question<object>
        {
            public override string Name { get; }

            public TestQuestion(string name)
            {
                Name = name;
            }

            protected override object Answer(IActor actor)
            {
                throw new NotImplementedException();
            }
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(Question<object>));
        }

        [Theory, DomainAutoData]
        public void AnswerByWithAbility_ShouldReturnCorrectValue(
            Question<object> sut,
            IActor actor,
            object expected)
        {
            //arrange
            Mock.Get(sut).Protected()
                         .Setup<object>("Answer", ItExpr.Is<IActor>(a => a == actor))
                         .Returns(expected);
            //act
            var actual = sut.AnsweredBy(actor);
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

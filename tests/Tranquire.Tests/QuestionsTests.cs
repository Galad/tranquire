using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Idioms;
using FluentAssertions;
using Moq;
using Tranquire;
using Xunit;

namespace Tranquire.Tests;

public class QuestionsTests
{
    [Theory, DomainAutoData]
    public void Sut_VerifyGuardClause(GuardClauseAssertion assertion)
    {
        assertion.Verify(typeof(Questions));
    }

    [Theory, DomainAutoData]
    public void Create_ShouldReturnCorrectValue(IActor actor, object expected, string name)
    {
        // arrange
        var func = new Mock<Func<IActor, object>>();
        func.Setup(f => f(actor)).Returns(expected);
        // act
        var question = Questions.Create(name, func.Object);
        var actual = question.AnsweredBy(actor);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public void Create_Name_ShouldReturnCorrectValue(string expected)
    {
        // arrange
        // act
        var question = Questions.Create(expected, _ => default(object));
        var actual = question.Name;
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public void CreateWithAbility_ShouldReturnCorrectValue(
        IActor actor,
        object ability,
        object expected,
        string name)
    {
        // arrange
        var func = new Mock<Func<IActor, object, object>>();
        func.Setup(f => f(actor, ability)).Returns(expected);
        // act
        var question = Questions.Create(name, func.Object);
        var actual = question.AnsweredBy(actor, ability);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public void CreateWithAbility_Name_ShouldReturnCorrectValue(string expected)
    {
        // arrange
        // act
        var question = Questions.Create(expected, (IActor _, object __) => default(object));
        var actual = question.Name;
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public void FromResult_ShouldReturnCorrectValue(IActor actor, object expected)
    {
        // arrange
        // act
        var question = Questions.FromResult(expected);
        var actual = question.AnsweredBy(actor);
        // assert
        actual.Should().Be(expected);
    }

    [Theory, DomainAutoData]
    public void FromResult_Name_ShouldReturnCorrectValue(string value)
    {
        // arrange
        // act
        var question = Questions.FromResult(value);
        var actual = question.Name;
        // assert
        var expected = "Returns " + value;
        actual.Should().Be(expected);
    }

    #region Tagged questions
    [Theory]
    [DomainInlineAutoData("tag1", 1)]
    [DomainInlineAutoData("tag2", 2)]
    [DomainInlineAutoData("tag3", 3)]
    public void TaggedQuestion_ShouldReturnCorrectValue(
        string tag,
        int expected,
        IActor actor,
        Mock<IActionTags<string>> questionTags)
    {
        // arrange
        Mock.Get(actor).Setup(a => a.AsksFor(It.IsAny<IQuestion<int>>()))
                       .Returns((IQuestion<int> a) => a.AnsweredBy(actor));
        var questions = new (string tag, IQuestion<int> question)[]
        {
            ("tag1", Questions.FromResult(1)),
            ("tag2", Questions.FromResult(2)),
            ("tag3", Questions.FromResult(3))
        };
        var tags = questions.Select(aa => aa.tag).ToArray();
        questionTags.Setup(a => a.FindBestWhenTag(It.IsAny<IEnumerable<string>>()))
                  .Returns<IEnumerable<string>>(t => t.OrderBy(tt => tt).SequenceEqual(tags) ? tag : string.Empty);
        var taggedQuestion = Questions.CreateTagged(questions);
        // act
        var actual = taggedQuestion.AnsweredBy(actor, questionTags.Object);
        // assert
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void TaggedQuestion_WhenTheTagFoundIsNotCorrect_ShouldThrow(
        IActor actor,
        Mock<IActionTags<string>> questionTags)
    {
        // arrange
        var questions = new (string tag, IQuestion<int> question)[]
        {
            ("tag1", Questions.FromResult(1)),
            ("tag2", Questions.FromResult(2)),
            ("tag3", Questions.FromResult(3))
        };
        questionTags.Setup(a => a.FindBestWhenTag(It.IsAny<IEnumerable<string>>())).Returns("tag4");
        var taggedQuestion = Questions.CreateTagged(questions);
        // act and assert
        Assert.Throws<KeyNotFoundException>(() => taggedQuestion.AnsweredBy(actor, questionTags.Object));
    }

    [Fact]
    public void TaggedQuestion_Name_ShouldReturnCorrectValue()
    {
        // arrange
        var questions = new (string tag, IQuestion<int> question)[]
        {
            ("tag1", Questions.FromResult(1)),
            ("tag2", Questions.FromResult(2)),
            ("tag3", Questions.FromResult(3))
        };
        var taggedQuestion = Questions.CreateTagged(questions);
        // act
        var actual = taggedQuestion.Name;
        // assert
        var expected = "Tagged question with tag1, tag2, tag3";
        Assert.Equal(expected, actual);
    }

    [Theory, DomainAutoData]
    public void TaggedQuestion_OverridingName_ShouldReturnCorrectValue(string expected)
    {
        // arrange
        var questions = new (string tag, IQuestion<int> question)[]
        {
            ("tag1", Questions.FromResult(1)),
            ("tag2", Questions.FromResult(2)),
            ("tag3", Questions.FromResult(3))
        };
        var taggedQuestion = Questions.CreateTagged(expected, questions);
        // act
        var actual = taggedQuestion.Name;
        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TaggedQuestion_OverloadsShouldReturnSameQuestion()
    {
        // arrange
        var questions = new (string tag, IQuestion<int> question)[]
        {
            ("tag1", Questions.FromResult(1)),
            ("tag2", Questions.FromResult(2)),
            ("tag3", Questions.FromResult(3))
        };
        // act
        var question1 = Questions.CreateTagged(questions);
        var question2 = Questions.CreateTagged(question1.Name, questions);
        // assert
        question1.Should().BeEquivalentTo(question2, o => o.RespectingRuntimeTypes());
    }
    #endregion
}

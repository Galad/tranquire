using AutoFixture.Xunit2;
using FluentAssertions;
using OpenQA.Selenium;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public partial class QuestionsTests
    {
        [Theory]
        [InlineData("PresentElement", true)]
        [InlineData("NotPresentElement", false)]
        public void Presence_ShouldReturnCorrectValue(string id, bool expected)
        {
            //arrange
            var target = Target.The("presence element").LocatedBy(By.Id(id));
            var question = Presence.Of(target);
            //act
            var actual = Answer(question);
            //assert
            Assert.Equal(expected, actual);
        }

        [Theory, AutoData]
        public void Present_TargetShouldReturnCorrectValue(string name)
        {
            // arrange
            var expected = Target.The(name).LocatedBy(By.Id("id"));
            var sut = Presence.Of(expected);
            // act
            var actual = sut.Target;
            // assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}

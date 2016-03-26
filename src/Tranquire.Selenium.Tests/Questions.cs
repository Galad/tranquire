using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tranquire.Selenium.Questions;
using Xunit;

namespace Tranquire.Selenium.Tests
{
    public class Questions : IClassFixture<WebDriverFixture>
    {
        private readonly WebDriverFixture _fixture;

        public Questions(WebDriverFixture fixture)
        {
            fixture.NavigateTo("Questions.html");
            _fixture = fixture;
        }

        private ITarget CreateTarget(string id)
        {
            return Target.The("element").LocatedBy(By.Id(id));
        }

        private void TestQuestion<T>(string id, Func<Text, IQuestion<T>> getQuestion, T expected)
        {
            //arrange
            var question = getQuestion(Text.Of(CreateTarget(id)));
            //act
            var actual = question.AnsweredBy(_fixture.Actor);
            //assert            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TextQuestion_ShouldReturnCorrectValue()
        {
            TestQuestion("Text", t => t.AsText(), "Text value");
        }

        [Fact]
        public void IntegerQuestion_ShouldReturnCorrectValue()
        {
            TestQuestion("Integer", t => t.AsInteger(), 2016);
        }

        [Theory, InlineData("BooleanUpperCase"), InlineData("BooleanLowerCase")]
        public void BooleanQuestion_ShouldReturnCorrectValue(string id)
        {
            TestQuestion(id, t => t.AsBoolean(), true);
        }

        [Fact]
        public void DateTimeQuestion_ShouldReturnCorrectValue()
        {
            TestQuestion("DateTime", t => t.AsDateTime(), new DateTime(2016, 3, 26));
        }

        private void TestQuestionMany<T>(string id, Func<Text, IQuestion<ImmutableArray<T>>> getQuestion, IEnumerable<T> expected)
        {
            //arrange
            var targetSource = Target.The("many container").LocatedBy(By.Id("many"));
            var target = Target.The("many element")
                               .LocatedBy(By.CssSelector($"#{id} p"))
                               .RelativeTo(targetSource);
            var question = getQuestion(Text.Of(target));
            //act
            var actual = question.AnsweredBy(_fixture.Actor);
            //assert            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TextQuestionMany_ShouldReturnCorrectValue()
        {
            var expected = new[] { "Yellow", "Green", "Blue", "Red" };
            TestQuestionMany("many-text", t => t.Many().AsText(), expected);
        }

        [Fact]
        public void IntegerQuestionMany_ShouldReturnCorrectValue()
        {
            var expected = new[] { 2, 20, 99, 500 };
            TestQuestionMany("many-integer", t => t.Many().AsInteger(), expected);
        }

        [Fact]
        public void BooleanQuestionMany_ShouldReturnCorrectValue()
        {
            var expected = new[] { true, false, false, true };
            TestQuestionMany("many-boolean", t => t.Many().AsBoolean(), expected);
        }

        [Fact]
        public void DateQuestionMany_ShouldReturnCorrectValue()
        {
            var expected = new[] {
                new DateTime(2016, 3, 26),
                new DateTime(2016, 1, 1),
                new DateTime(2016, 1, 1, 19,1,0),
                new DateTime(1950, 9, 9),
            };
            TestQuestionMany("many-date", t => t.Many().AsDateTime(), expected);
        }
    }
}

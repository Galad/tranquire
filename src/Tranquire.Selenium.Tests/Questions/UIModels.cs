using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Tranquire.Selenium.Questions;
using Tranquire.Selenium.Questions.UIModels;
using Tranquire.Tests;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public class UIModels : WebDriverTest
    {
        public UIModels(WebDriverFixture fixture) : base(fixture, "UIModels.html")
        {
        }

        [Theory, DomainAutoData]
        public void Sut_VerifyGuardClauses(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(UIModel));
        }

        public class Model1
        {
            [Target(ByMethod.ClassName, "title")]
            public string Title { get; set; }

            [Target(ByMethod.ClassName, "value")]
            public int Value { get; set; }

            [Target(ByMethod.ClassName, "bool")]
            public bool Boolean { get; set; }

            [Target(ByMethod.Id, "date")]
            public DateTime Date { get; set; }

            [Target(ByMethod.Id, "double")]
            public double Double { get; set; }

            [Target(ByMethod.XPath, "span[@class='xpath']")]
            public string XPath { get; set; }

            [Target(ByMethod.CssSelector, ".css-selector")]
            public string CssSelector { get; set; }

            [Target(ByMethod.TagName, "div")]
            public string TagName { get; set; }

            [Target(ByMethod.Name, "the-name")]
            public string Name { get; set; }
        }

        [Fact]
        public void SingleModel_ShouldReturnCorrectValue()
        {
            //arrange
            var modelContainerTarget = Target.The("Model container").LocatedBy(By.ClassName("model1"));
            var question = UIModel.Of<Model1>(modelContainerTarget).WithCulture(CultureInfo.GetCultureInfo("en-US"));
            //act
            var actual = Answer(question);
            //assert
            var expected = new Model1()
            {
                Title = "The title",
                Value = 1,
                Boolean = true,
                Date = new DateTime(2019, 1, 1, 01, 00, 00),
                Double = 9.5,
                XPath = "The XPath text",
                CssSelector = "The CSS selector text",
                TagName = "The tag name text",
                Name = "The name text"
            };
            actual.Should().BeEquivalentTo(expected);
        }

        public class ModelWithUnsupportedType
        {
            [Target(ByMethod.ClassName, "title")]
            public object Name { get; set; }
        }


        [Fact]
        public void UnsupportedType_ShouldThrow()
        {
            //arrange
            var modelContainerTarget = Target.The("Model container").LocatedBy(By.ClassName("model1"));
            // act and assert
            Assert.Throws<NotSupportedException>(() => UIModel.Of<ModelWithUnsupportedType>(modelContainerTarget));
        }

        [Theory, DomainAutoData]
        public void Name_ShouldReturnCorrectValue(string expected)
        {
            //arrange
            var modelContainerTarget = Target.The("Model container").LocatedBy(By.ClassName("model1"));
            var question = UIModel.Of<Model1>(modelContainerTarget, expected);
            // act
            var actual = question.Name;
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory, DomainAutoData]
        public void Name_Many_ShouldReturnCorrectValue(string expected)
        {
            //arrange
            var modelContainerTarget = Target.The("Model container").LocatedBy(By.ClassName("model1"));
            var question = UIModel.Of<Model1>(modelContainerTarget, expected).Many();
            // act
            var actual = question.Name;
            // assert
            Assert.Equal(expected, actual);
        }

        public class Model2
        {
            [Target(ByMethod.Id, "text")]
            [TextContent]
            public string Text { get; set; }

            [Target(ByMethod.Id, "classes")]
            [Classes]
            public ImmutableArray<string> Classes { get; set; }

            [Target(ByMethod.Id, "css-value")]
            [CssValue("border-style")]
            public string CssValue { get; set; }

            [Target(ByMethod.Id, "enabled")]
            [Enabled]
            public bool EnabledTrue { get; set; }

            [Target(ByMethod.Id, "disabled")]
            [Enabled]
            public bool EnabledFalse { get; set; }

            [Target(ByMethod.Id, "html-attribute")]
            [HtmlAttribute("title")]
            public string HtmlAttribute { get; set; }

            [Target(ByMethod.Id, "presence")]
            [Presence]
            public bool PresenceTrue { get; set; }

            [Target(ByMethod.Id, "not-present")]
            [Presence]
            public bool PresenceFalse { get; set; }

            [Target(ByMethod.Id, "selected")]
            [Selected]
            public bool SelectedTrue { get; set; }

            [Target(ByMethod.Id, "not-selected")]
            [Selected]
            public bool SelectedFalse { get; set; }

            [Target(ByMethod.Id, "selected-value")]
            [SelectedValue]
            public int SelectedValue { get; set; }

            [Target(ByMethod.Id, "selected-values")]
            [SelectedValues]
            public ImmutableArray<int> SelectedValues { get; set; }

            [Target(ByMethod.Id, "selected-values")]
            [SelectedValues]
            public ImmutableArray<double> SelectedValuesDouble { get; set; }

            [Target(ByMethod.Id, "value")]
            [Value]
            public string Value { get; set; }

            [Target(ByMethod.Id, "visibility-visible")]
            [Visibility]
            public bool VisibilityTrue { get; set; }

            [Target(ByMethod.Id, "visibility-none")]
            [Visibility]
            public bool VisibilityFalse { get; set; }
        }

        [Fact]
        public void SingleModel_UsingUIStates_ShouldReturnCorrectValue()
        {
            //arrange
            var modelContainerTarget = Target.The("Model container").LocatedBy(By.ClassName("model2"));
            var question = UIModel.Of<Model2>(modelContainerTarget);
            //act
            var actual = Answer(question);
            //assert
            var expected = new Model2()
            {
                Text = "The text",
                Classes = ImmutableArray.Create("class1", "class2", "class3"),
                CssValue = "dashed",
                EnabledTrue = true,
                EnabledFalse = false,
                HtmlAttribute = "the title",
                PresenceTrue = true,
                PresenceFalse = false,
                SelectedTrue = true,
                SelectedFalse = false,
                SelectedValue = 2,
                SelectedValues = ImmutableArray.Create(2, 3),
                SelectedValuesDouble = ImmutableArray.Create(2.0, 3.0),
                Value = "the value",
                VisibilityTrue = true,
                VisibilityFalse = false
            };
            actual.Should().BeEquivalentTo(expected);
        }

        public class Model3
        {
            [Target(ByMethod.ClassName, "title")]
            public string Title { get; set; }
            [Target(ByMethod.ClassName, "value")]
            public int Value { get; set; }
            [Target(ByMethod.TagName, "input")]
            [Value]
            public string Text { get; set; }
        }

        [Fact]
        public void ManyModel_ShouldReturnCorrectValue()
        {
            //arrange
            var modelContainerTarget = Target.The("Model container").LocatedBy(By.CssSelector(".modelContainer .model3"));
            var question = UIModel.Of<Model3>(modelContainerTarget).WithCulture(CultureInfo.GetCultureInfo("en-US")).Many();
            //act
            var actual = Answer(question);
            //assert
            var expected = ImmutableArray.Create(
                new Model3()
                {
                    Text = "The text1",
                    Value = 1,
                    Title = "The title1"
                },
                new Model3()
                {
                    Text = "The text2",
                    Value = 2,
                    Title = "The title2"
                },
                new Model3()
                {
                    Text = "The text3",
                    Value = 3,
                    Title = "The title3"
                });
            actual.Should().BeEquivalentTo(expected);
        }
    }

    public class AttributesTests
    {
        public static IEnumerable<object[]> Attributes
        {
            get
            {
                return typeof(TargetAttribute)
                    .Assembly
                    .GetTypes()
                    .Where(t => typeof(TargetAttribute).IsAssignableFrom(t) || typeof(UIStateAttribute).IsAssignableFrom(t))
                    .Select(t => new object[] { t });
            }
        }

        [Theory, MemberData(nameof(Attributes))]
        public void VerifyAttributesGuardClauses(Type attributeType)
        {
            var fixture = new Fixture();
            var assertion = new GuardClauseAssertion(fixture);
            assertion.Verify(attributeType.GetConstructors());
        }
    }
}

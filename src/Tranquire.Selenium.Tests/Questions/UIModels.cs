using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Immutable;
using System.Globalization;
using Tranquire.Selenium.Questions;
using Tranquire.Selenium.Questions.UIModels;
using Xunit;

namespace Tranquire.Selenium.Tests.Questions
{
    public class UIModels : WebDriverTest
    {
        public UIModels(WebDriverFixture fixture) : base(fixture, "UIModels.html")
        {
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
                TagName = "The tag name text"
            };
            actual.Should().BeEquivalentTo(expected);
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
                Value = "the value",
                VisibilityTrue = true,
                VisibilityFalse = false
            };
            actual.Should().BeEquivalentTo(expected);
        }
    }
}

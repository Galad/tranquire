using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Tranquire.Selenium.Actions.UIModels;
using Tranquire.Selenium.Targets;
using Tranquire.Selenium.Questions;
using Tranquire.Selenium.Questions.UIModels;
using Xunit;
using OpenQA.Selenium;

namespace Tranquire.Selenium.Tests.Actions;
public class UpdateUIModelsTests : WebDriverTest
{
    public UpdateUIModelsTests(WebDriverFixture fixture) : base(fixture, "UpdateUIModels.html")
    {
    }

    private static ITarget Container => Target.The("Container").LocatedBy(By.Id("ContainerID"));
    private static readonly string[] ExpectedNumericSelection = { "2", "4" };
    private static readonly string[] PreselectedOptions = { "1", "3" };
    private static readonly string[] SingleSelectValues = { "2" };

    // Model for model-based overloads
    public class TestModel
    {
        [Target(ByMethod.Name, "NameOfText"), Enter, Value]
        public string NameOfText { get; set; }

        [Target(ByMethod.Name, "NameOfCheckbox"), Enter, Selected]
        public bool NameOfCheckbox { get; set; }

        // Reads the checked radio; writes through the group so no radio needs to be pre-selected.
        [Target(ByMethod.CssSelector, "input[name='NameOfRadio']:checked"), TargetWithParameters(ByMethod.Name, "NameOfRadio"), Enter, Value]
        public string NameOfRadio { get; set; }

        [Target(ByMethod.Id, "IdOfDate"), Enter, Value]
        public string IdOfDate { get; set; }

        [Target(ByMethod.Id, "IdOfTextArea"), Enter, Value]
        public string IdOfTextArea { get; set; }

        [Target(ByMethod.Id, "Seos"), Enter, SelectedValue]
        public string Seos { get; set; }

        [Target(ByMethod.Id, "MultiSeos"), Enter, SelectedValues]
        public ImmutableArray<string> MultiSeos { get; set; }
    }

    public class PlaceholderModel
    {
        [Target(ByMethod.Name, "DateWithLongYear"), Enter]
        public DateTime? DateWithLongYear { get; set; }

        [Target(ByMethod.Name, "DateWithShortYear"), Enter]
        public DateTime? DateWithShortYear { get; set; }

        [Target(ByMethod.Name, "DateWithSingleDay"), Enter]
        public DateTime? DateWithSingleDay { get; set; }

        [Target(ByMethod.Name, "DateWithInvalidFormat"), Enter]
        public DateTime? DateWithInvalidFormat { get; set; }

        [Target(ByMethod.Name, "DateWithTextPlaceholder"), Enter]
        public DateTimeOffset? DateWithTextPlaceholder { get; set; }

        [Target(ByMethod.Name, "QuantityInput"), Enter]
        public int? Quantity { get; set; }
    }

    [Fact]
    public void Update_WithDateValues_ShouldApplyDatePlaceholderFormats()
    {
        var date = new DateTime(2026, 10, 1);
        var model = new PlaceholderModel
        {
            DateWithLongYear = date,
            DateWithShortYear = date,
            DateWithSingleDay = date
        };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        Assert.Equal("01/10/2026", InputValue("DateWithLongYear"));
        Assert.Equal("01/10/26", InputValue("DateWithShortYear"));
        Assert.Equal("1/10/2026", InputValue("DateWithSingleDay"));
    }

    [Fact]
    public void Update_WithNonDatePlaceholderOrInvalidFormat_ShouldNotApplyPlaceholder()
    {
        var date = new DateTime(2026, 10, 21);
        var offset = new DateTimeOffset(date, TimeSpan.Zero);
        var model = new PlaceholderModel
        {
            DateWithInvalidFormat = date,
            DateWithTextPlaceholder = offset,
            Quantity = 42
        };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container).WithCulture(CultureInfo.InvariantCulture));

        Assert.Equal(date.ToString(CultureInfo.InvariantCulture), InputValue("DateWithInvalidFormat"));
        Assert.Equal(offset.ToString(CultureInfo.InvariantCulture), InputValue("DateWithTextPlaceholder"));
        Assert.Equal("42", InputValue("QuantityInput"));
    }

    public class DateInputModel
    {
        [Target(ByMethod.Name, "DateInput"), Enter]
        public DateTime? Date { get; set; }

        [Target(ByMethod.Name, "DateTimeLocalInput"), Enter]
        public DateTimeOffset? DateTimeLocal { get; set; }

        [Target(ByMethod.Name, "TimeInput"), Enter]
        public DateTime? Time { get; set; }

        [Target(ByMethod.Name, "DateAsText"), Enter]
        public string DateAsText { get; set; }
    }

    [Fact]
    public void Update_WithDateInputs_ShouldSetIsoValues()
    {
        var model = new DateInputModel
        {
            Date = new DateTime(2026, 10, 1),
            DateTimeLocal = new DateTimeOffset(2026, 10, 1, 13, 45, 0, TimeSpan.Zero),
            Time = new DateTime(2026, 10, 1, 8, 5, 0),
            DateAsText = "2026-12-31"
        };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        Assert.Equal("2026-10-01", InputValue("DateInput"));
        Assert.Equal("2026-10-01T13:45", InputValue("DateTimeLocalInput"));
        Assert.Equal("08:05", InputValue("TimeInput"));
        Assert.Equal("2026-12-31", InputValue("DateAsText"));
    }

    public class TextualInputModel
    {
        [Target(ByMethod.Name, "PasswordInput"), Enter]
        public string Password { get; set; }

        [Target(ByMethod.Name, "EmailInput"), Enter]
        public string Email { get; set; }

        [Target(ByMethod.Name, "NumberInput"), Enter]
        public int Number { get; set; }

        [Target(ByMethod.Name, "SearchInput"), Enter]
        public string Search { get; set; }

        [Target(ByMethod.Name, "TelephoneInput"), Enter]
        public string Telephone { get; set; }

        [Target(ByMethod.Name, "UrlInput"), Enter]
        public string Url { get; set; }
    }

    [Fact]
    public void Update_WithTextualInputs_ShouldEnterValues()
    {
        var model = new TextualInputModel
        {
            Password = "secret",
            Email = "test@example.com",
            Number = 42,
            Search = "search term",
            Telephone = "0123456789",
            Url = "https://example.com/"
        };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        Assert.Equal("secret", InputValue("PasswordInput"));
        Assert.Equal("test@example.com", InputValue("EmailInput"));
        Assert.Equal("42", InputValue("NumberInput"));
        Assert.Equal("search term", InputValue("SearchInput"));
        Assert.Equal("0123456789", InputValue("TelephoneInput"));
        Assert.Equal("https://example.com/", InputValue("UrlInput"));
    }

    public class DecimalInputModel
    {
        [Target(ByMethod.Name, "NumberInput"), Enter]
        public decimal Amount { get; set; }
    }

    [Fact]
    public void Update_WithDecimalUnderFrenchCulture_ShouldEnterInvariantNumber()
    {
        var action = UpdateUIModels.Update(new DecimalInputModel { Amount = 1.5m }, Container)
            .WithCulture(new CultureInfo("fr-FR"));

        Fixture.Actor.When(action);

        Assert.Equal("1.5", InputValue("NumberInput"));
    }

    public class CultureRoundTripModel
    {
        [Target(ByMethod.Name, "SearchInput"), Enter, Value]
        public double Amount { get; set; }

        [Target(ByMethod.Name, "DateWithTextPlaceholder"), Enter, Value]
        public DateTime Date { get; set; }
    }

    [Fact]
    public void Update_WithCulture_ShouldBeReadBackByUIModelWithSameCulture()
    {
        var culture = new CultureInfo("fr-FR");
        var model = new CultureRoundTripModel { Amount = 1.5, Date = new DateTime(2026, 10, 1, 13, 45, 0) };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container).WithCulture(culture));
        var result = Fixture.Actor.AsksFor(UIModel.Of<CultureRoundTripModel>(Container).WithCulture(culture));

        Assert.Equal("1,5", InputValue("SearchInput"));
        Assert.Equal(model.Amount, result.Amount);
        Assert.Equal(model.Date, result.Date);
    }

    public class NumericSelectModel
    {
        [Target(ByMethod.Id, "Seos"), Enter]
        public int Single { get; set; }

        [Target(ByMethod.Id, "MultiSeos"), Enter]
        public List<int> Multiple { get; set; }
    }

    [Fact]
    public void Update_WithNumericSelectValues_ShouldSelectMatchingOptions()
    {
        var model = new NumericSelectModel { Single = 3, Multiple = new List<int> { 2, 4 } };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        Assert.Equal("3", Fixture.WebDriver.FindElement(By.Id("Seos")).GetAttribute("value"));
        Assert.Equal(ExpectedNumericSelection, SelectedOptions("MultiSeos"));
    }

    public class SelectCollectionModel
    {
        [Target(ByMethod.Id, "MultiSeos"), Enter]
        public string[] Multiple { get; set; }

        [Target(ByMethod.Id, "Seos"), Enter]
        public string[] Single { get; set; }
    }

    [Theory]
    [InlineData("2")]
    [InlineData("")]
    public void Update_WithMultiSelectValues_ShouldReplaceExistingSelection(string values)
    {
        var expected = values.Length == 0 ? Array.Empty<string>() : values.Split(',');
        var multiSelect = Target.The("multi select").LocatedBy(By.Id("MultiSeos"));
        Fixture.Actor.When(Tranquire.Selenium.Actions.Select.TheValues(PreselectedOptions).Into(multiSelect));

        Fixture.Actor.When(UpdateUIModels.Update(new SelectCollectionModel { Multiple = expected }, Container));

        Assert.Equal(expected, SelectedOptions("MultiSeos"));
    }

    [Fact]
    public void Update_WithCollectionForSingleSelect_ShouldSelectTheValue()
    {
        Fixture.Actor.When(UpdateUIModels.Update(new SelectCollectionModel { Single = SingleSelectValues }, Container));

        Assert.Equal("2", Fixture.WebDriver.FindElement(By.Id("Seos")).GetAttribute("value"));
    }

    private string[] SelectedOptions(string selectId) =>
        Fixture.WebDriver.FindElement(By.Id(selectId))
            .FindElements(By.CssSelector("option:checked"))
            .Select(option => option.GetAttribute("value"))
            .ToArray();

    public class UnsupportedElementModel
    {
        [Target(ByMethod.Id, "EditableDiv"), Enter]
        public string Text { get; set; }
    }

    [Fact]
    public void Update_WithUnsupportedElement_ShouldExplainFailure()
    {
        var exception = Assert.Throws<NotSupportedException>(() =>
            Fixture.Actor.When(UpdateUIModels.Update(new UnsupportedElementModel { Text = "text" }, Container)));

        Assert.Contains("<div>", exception.Message);
        Assert.Equal(string.Empty, Fixture.WebDriver.FindElement(By.Id("EditableDiv")).Text);
    }

    [Fact]
    public void Update_WithNullModelValues_ShouldLeaveFieldsUnchanged()
    {
        var text = Fixture.WebDriver.FindElement(By.Name("NameOfText"));
        var textArea = Fixture.WebDriver.FindElement(By.Id("IdOfTextArea"));
        text.SendKeys("Existing text");
        textArea.SendKeys("Existing area");

        Fixture.Actor.When(UpdateUIModels.Update(new TextInputEnterModel(), Container));

        Assert.Equal("Existing text", text.GetAttribute("value"));
        Assert.Equal("Existing area", textArea.GetAttribute("value"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Replacement value")]
    public void Update_WithTextValue_ShouldReplaceExistingValue(string value)
    {
        var text = Fixture.WebDriver.FindElement(By.Name("NameOfText"));
        var textArea = Fixture.WebDriver.FindElement(By.Id("IdOfTextArea"));
        text.SendKeys("Existing value");
        textArea.SendKeys("Existing value");

        Fixture.Actor.When(UpdateUIModels.Update(new TextInputEnterModel { NameOfText = value, IdOfTextArea = value }, Container));

        Assert.Equal(value, text.GetAttribute("value"));
        Assert.Equal(value, textArea.GetAttribute("value"));
    }

    public class CheckboxTextModel
    {
        [Target(ByMethod.Name, "NameOfCheckbox"), Enter]
        public string Checkbox { get; set; }
    }

    [Fact]
    public void Update_WithNonBooleanCheckboxValue_ShouldExplainFailure()
    {
        var checkboxes = Fixture.WebDriver.FindElements(By.Name("NameOfCheckbox"));
        var initialSelections = checkboxes.Select(checkbox => checkbox.Selected).ToArray();

        Assert.Throws<NotSupportedException>(() =>
            Fixture.Actor.When(UpdateUIModels.Update(new CheckboxTextModel { Checkbox = "true" }, Container)));

        Assert.Equal(initialSelections, checkboxes.Select(checkbox => checkbox.Selected).ToArray());
    }

    public class RadioFallbackModel
    {
        [Target(ByMethod.Id, "RadioWithoutName"), Enter]
        public string WithoutName { get; set; }

        [Target(ByMethod.Id, "EmptyValueRadio"), Enter]
        public string EmptyValue { get; set; }
    }

    [Fact]
    public void Update_WithRadioFallback_ShouldSelectMatchingValue()
    {
        var model = new RadioFallbackModel { WithoutName = "selected", EmptyValue = string.Empty };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        Assert.True(Fixture.WebDriver.FindElement(By.Id("RadioWithoutName")).Selected);
        Assert.True(Fixture.WebDriver.FindElement(By.Id("EmptyValueRadio")).Selected);
    }

    public class UngroupedRadioModel
    {
        [Target(ByMethod.CssSelector, "input[data-test='ungrouped-radio']"), Enter]
        public string Choice { get; set; }
    }

    public class XPathTemplateModel
    {
        public string Key { get; set; }

        [Target(ByMethod.XPath, ".//input[@name='{Key}']"), Enter]
        public string Value { get; set; }

        [Target(ByMethod.XPath, ".//input[@name={Key}]"), Enter]
        public string UnquotedValue { get; set; }
    }

    public class CssTemplateModel
    {
        public string Key { get; set; }

        [Target(ByMethod.CssSelector, "input[name='{Key}']"), Enter]
        public string Value { get; set; }

        [Target(ByMethod.CssSelector, "input[name=\"{Key}\"]"), Enter]
        public string DoubleQuotedValue { get; set; }
    }

    [Fact]
    public void Update_WithRadioWithoutNameOrId_ShouldFindValueWithinContainer()
    {
        var model = new UngroupedRadioModel { Choice = "ungrouped" };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        Assert.True(Fixture.WebDriver.FindElement(By.CssSelector("input[data-test='ungrouped-radio']")).Selected);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Update_WithXPathTemplateValue_ShouldTargetExactField(bool quotedValue)
    {
        const string fieldName = "missing'] | .//input[@name='NameOfText";
        var model = new XPathTemplateModel
        {
            Key = fieldName,
            Value = quotedValue ? "Exact field" : null,
            UnquotedValue = quotedValue ? null : "Exact field"
        };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        Assert.Equal(string.Empty, Fixture.WebDriver.FindElement(By.Name("NameOfText")).GetAttribute("value"));
        Assert.Equal(
            "Exact field",
            Fixture.WebDriver.FindElement(By.CssSelector("[data-test='xpath-template-target']")).GetAttribute("value"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Update_WithCssTemplateValue_ShouldTargetExactField(bool singleQuoted)
    {
        const string fieldName = "x'], [name='NameOfText";
        var model = new CssTemplateModel
        {
            Key = fieldName,
            Value = singleQuoted ? "Exact field" : null,
            DoubleQuotedValue = singleQuoted ? null : "Exact field"
        };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        Assert.Equal(string.Empty, InputValue("NameOfText"));
        Assert.Equal(
            "Exact field",
            Fixture.WebDriver.FindElement(By.CssSelector("[data-test='css-template-target']")).GetAttribute("value"));
    }

    public class QuotedRadioModel
    {
        [Target(ByMethod.CssSelector, "input[name=\"Radio's Choice\"]"), Enter]
        public string Choice { get; set; }
    }

    [Fact]
    public void Update_WithQuotesInRadioNameAndValue_ShouldSelectMatchingRadio()
    {
        const string radioValue = "O'Reilly \"Special\"";

        Fixture.Actor.When(UpdateUIModels.Update(new QuotedRadioModel { Choice = radioValue }, Container));

        var radios = Fixture.WebDriver.FindElements(By.Name("Radio's Choice"));
        Assert.Equal(2, radios.Count);
        Assert.True(radios[0].Selected);
        Assert.False(radios[1].Selected);
        Assert.Equal(radioValue, radios[0].GetAttribute("value"));
    }

    [Fact]
    public void Update_WithModelAndContainerAndParameters_ShouldSetFields()
    {
        var model = new TestModel
        {
            NameOfText = "Model Text",
            NameOfCheckbox = false,
            NameOfRadio = "1",
            IdOfDate = "2024-06-03",
            IdOfTextArea = "Model text area",
            Seos = "4",
            MultiSeos = ImmutableArray.Create("1", "2")
        };

        var action = UpdateUIModels.Update(model, Container, "unused");
        Fixture.Actor.When(action);

        AssertFieldsFromModel(model);
    }

    [Fact]
    public void Update_WithModelAndContainer_ShouldSetFields()
    {
        var model = new TestModel
        {
            NameOfText = "Simple Model",
            NameOfCheckbox = true,
            NameOfRadio = "2",
            IdOfDate = "2024-06-04",
            IdOfTextArea = "Simple model text",
            Seos = "1",
            MultiSeos = ImmutableArray.Create("3", "4")
        };

        var action = UpdateUIModels.Update(model, Container);
        Fixture.Actor.When(action);

        AssertFieldsFromModel(model);
    }

    private string InputValue(string name) =>
        Fixture.WebDriver.FindElement(By.Name(name)).GetAttribute("value");

    private void AssertFieldsFromModel(TestModel expected)
    {
        var model = Fixture.Actor.AsksFor(UIModel.Of<TestModel>(Container));
        Assert.Equal(expected.NameOfText, model.NameOfText);
        Assert.Equal(expected.NameOfCheckbox, model.NameOfCheckbox);
        Assert.Equal(expected.NameOfRadio, model.NameOfRadio);
        Assert.Equal(expected.IdOfDate, model.IdOfDate);
        Assert.Equal(expected.IdOfTextArea, model.IdOfTextArea);
        Assert.Equal(expected.Seos, model.Seos);
        Assert.Equal(expected.MultiSeos.OrderBy(x => x), model.MultiSeos.OrderBy(x => x));
    }

    // Model that combines [Target] + [Enter] to drive the action side,
    // and [Value] to read back via UIModel on the question side.
    public class TextInputEnterModel
    {
        [Target(ByMethod.Name, "NameOfText"), Enter, Value]
        public string NameOfText { get; set; }

        [Target(ByMethod.Id, "IdOfTextArea"), Enter, Value]
        public string IdOfTextArea { get; set; }
    }

    [Fact]
    public void Update_WithModelAndContainer_UsingTargetAndEnterAttributes_ShouldEnterValues()
    {
        var model = new TextInputEnterModel
        {
            NameOfText = "Entered via [Target]+[Enter]",
            IdOfTextArea = "Textarea via [Target]+[Enter]",
        };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        var result = Fixture.Actor.AsksFor(UIModel.Of<TextInputEnterModel>(Container));
        Assert.Equal(model.NameOfText, result.NameOfText);
        Assert.Equal(model.IdOfTextArea, result.IdOfTextArea);
    }

    public sealed class CustomEnterAttribute : BaseFormModelAttribute
    {
        public override void PerformFieldAction(IActor actor, FormField field)
        {
            actor.Execute(Tranquire.Selenium.Actions.Enter.TheNewValue(field.Value?.ToString()).Into(field.Target));
        }
    }

    public class CustomFormActionModel
    {
        [Target(ByMethod.Name, "NameOfText"), CustomEnter, Value]
        public string NameOfText { get; set; }
    }

    [Fact]
    public void Update_WithCustomFormModelAttribute_ShouldInvokeAttributeAction()
    {
        var model = new CustomFormActionModel { NameOfText = "Custom attribute action" };

        Fixture.Actor.When(UpdateUIModels.Update(model, Container));

        var result = Fixture.Actor.AsksFor(UIModel.Of<CustomFormActionModel>(Container));
        Assert.Equal(model.NameOfText, result.NameOfText);
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    private static readonly string[] FirstMultiSeosValues = { "2", "4" };
    private static readonly string[] SecondMultiSeosValues = { "1", "3" };

    // Model for model-based overloads
    public class TestModel
    {
        [Target(ByMethod.Name, "NameOfText"), Enter, Value]
        public string NameOfText { get; set; }

        [Target(ByMethod.Name, "NameOfCheckbox"), Enter, Selected]
        public bool NameOfCheckbox { get; set; }

        [Target(ByMethod.CssSelector, "input[name='NameOfRadio']:checked"), Enter, Value]
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

    [Fact]
    public void Update_WithContainerAndValuesAndParameters_ShouldSetFields()
    {
        var values = new Dictionary<string, object>
        {
            { "NameOfText", "Test Text" },
            { "NameOfCheckbox", false },
            { "NameOfRadio", "2" },
            { "IdOfDate", "2024-06-01" },
            { "IdOfTextArea", "Some text" },
            { "Seos", "3" },
            { "MultiSeos", FirstMultiSeosValues }
        };

        var action = UpdateUIModels.Update(Container, values, "unused");
        Fixture.Actor.When(action);

        AssertFields(values);
    }

    [Fact]
    public void Update_WithParameterizedDictionaryKeys_ShouldResolveValuesWhenExecuted()
    {
        var values = new Dictionary<string, object>
        {
            { "NameOf{0}", "Initial text" }
        };
        var action = UpdateUIModels.Update(Container, values, "Text");
        values["NameOf{0}"] = "Updated text";

        Fixture.Actor.When(action);

        Assert.Equal("Updated text", Fixture.WebDriver.FindElement(By.Name("NameOfText")).GetAttribute("value"));
    }

    [Fact]
    public void Update_WithContainerAndValues_ShouldSetFields()
    {
        var values = new Dictionary<string, object>
        {
            { "NameOfText", "Another Text" },
            { "NameOfCheckbox", true },
            { "NameOfRadio", "3" },
            { "IdOfDate", "2024-06-02" },
            { "IdOfTextArea", "Another text" },
            { "Seos", "2" },
            { "MultiSeos", SecondMultiSeosValues }
        };

        var action = UpdateUIModels.Update(Container, values);
        Fixture.Actor.When(action);

        AssertFields(values);
    }

    [Fact]
    public void Update_WithContainerAndValues_ShouldUpdateRepeatedSameNameCheckboxes()
    {
        var values = new Dictionary<string, object>
        {
            { "NameOfCheckbox", true }
        };

        Fixture.Actor.When(UpdateUIModels.Update(Container, values));

        var checkboxes = Fixture.WebDriver.FindElements(By.Name("NameOfCheckbox"));
        Assert.Equal(2, checkboxes.Count);
        Assert.All(checkboxes, checkbox => Assert.True(checkbox.Selected));
    }

    [Fact]
    public void Update_WithContainerAndValuesAndParameters_ShouldUpdateRepeatedSameNameCheckboxes()
    {
        var values = new Dictionary<string, object>
        {
            { "NameOfCheckbox", true }
        };

        Fixture.Actor.When(UpdateUIModels.Update(Container, values, "unused"));

        var checkboxes = Fixture.WebDriver.FindElements(By.Name("NameOfCheckbox"));
        Assert.Equal(2, checkboxes.Count);
        Assert.All(checkboxes, checkbox => Assert.True(checkbox.Selected));
    }

    [Fact]
    public void Update_WithContainerAndValues_ShouldHandleQuotesInRadioNameAndValue()
    {
        const string radioName = "Radio's Choice";
        const string radioValue = "O'Reilly \"Special\"";
        var values = new Dictionary<string, object>
        {
            { radioName, radioValue }
        };

        Fixture.Actor.When(UpdateUIModels.Update(Container, values));

        var radios = Fixture.WebDriver.FindElements(By.Name(radioName));
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

    private void AssertFields(Dictionary<string, object> values)
    {
        var model = Fixture.Actor.AsksFor(UIModel.Of<TestModel>(Container));
        Assert.Equal(values["NameOfText"], model.NameOfText);
        Assert.Equal(values["NameOfCheckbox"], model.NameOfCheckbox);
        Assert.Equal(values["NameOfRadio"], model.NameOfRadio);
        Assert.Equal(values["IdOfDate"], model.IdOfDate);
        Assert.Equal(values["IdOfTextArea"], model.IdOfTextArea);
        Assert.Equal(values["Seos"], model.Seos);
        Assert.Equal(((string[])values["MultiSeos"]).OrderBy(x => x), model.MultiSeos.OrderBy(x => x));
    }

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
        public override void PerformFieldAction(
            IActor actor,
            string fieldKey,
            object value,
            string tagName,
            string type,
            ITarget fieldTarget,
            ITarget container)
        {
            actor.Execute(Tranquire.Selenium.Actions.Enter.TheNewValue(value?.ToString()).Into(fieldTarget));
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
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

/// <summary>
/// Tests for the collection overloads of <see cref="UpdateUIModels.Update{T}"/>.
/// Uses UpdateUIModelsCollection.html which has two row containers: #row_1 and #row_2,
/// each containing name and description text inputs located by their name attribute.
/// </summary>
public class UpdateUIModelsCollectionTests : WebDriverTest
{
    public UpdateUIModelsCollectionTests(WebDriverFixture fixture)
        : base(fixture, "UpdateUIModelsCollection.html")
    {
    }

    // Row container target parameterized by row id.
    private static readonly ITargetWithParameters RowContainer =
        Target.The("row").LocatedBy("row_{0}", By.Id);

    // Model for collection tests. The class-level [TargetWithParameters] drives
    // Update<T>(IEnumerable<T>) to resolve each row container from the Id property.
    // [Target]+[Enter] drives the action (writing), [Value] drives UIModel (reading).
    [TargetWithParameters(ByMethod.Id, "row_{Id}")]
    public class CollectionItem
    {
        public int Id { get; set; }

        [Target(ByMethod.Name, "name"), Enter, Value]
        public string Name { get; set; }

        [Target(ByMethod.Name, "description"), Enter, Value]
        public string Description { get; set; }
    }

    // A model type deliberately missing the class-level [TargetWithParameters] attribute,
    // used to verify that the appropriate exception is thrown.
    public class CollectionItemNoClassAttr
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [TargetWithParameters(ByMethod.Id, "row_{Id}")]
    public class NamedParameterizedItem
    {
        public int Id { get; set; }
        public string FieldKey { get; set; }

        [TargetWithParameters(ByMethod.Name, "field_{FieldKey}"), Enter]
        public string Name { get; set; }
    }

    [TargetWithParameters(ByMethod.Id, "row_{0}")]
    public class PositionalClassTargetItem
    {
        public int Id { get; set; }

        [Target(ByMethod.Name, "name"), Enter]
        public string Name { get; set; }
    }

    public class PositionalParameterizedItem
    {
        public string FieldKey { get; set; }

        [TargetWithParameters(ByMethod.Name, "field_{0}", "FieldKey"), Enter]
        public string Name { get; set; }
    }

    public class SuppliedParameterItem
    {
        public int Id { get; set; }
        public string FieldKey { get; set; }
        public string Name { get; set; }

        [TargetWithParameters(ByMethod.Name, "field_{0}"), Enter]
        public string TargetedName { get; set; }
    }

    public class KitchenModel
    {
        public RecipeModel Recipe { get; set; }
    }

    [Target(ByMethod.Id, "recipe")]
    public class RecipeModel
    {
        [Target(ByMethod.Id, "recipe_name"), Enter]
        public string Name { get; set; }

        [Target(ByMethod.Id, "ingredients")]
        public List<IngredientModel> Ingredients { get; set; }
    }

    [TargetWithParameters(ByMethod.Id, "ingredient_{Id}")]
    public class IngredientModel
    {
        public int Id { get; set; }

        [Target(ByMethod.Name, "ingredient_name"), Enter]
        public string Name { get; set; }
    }

    private static ITarget RowTarget(int id) =>
        Target.The($"row {id}").LocatedBy(By.Id($"row_{id}"));

    private void AssertRow(int id, string expectedName, string expectedDescription)
    {
        var result = Fixture.Actor.AsksFor(UIModel.Of<CollectionItem>(RowTarget(id)));
        Assert.Equal(expectedName, result.Name);
        Assert.Equal(expectedDescription, result.Description);
    }

    [Fact]
    public void Update_Collection_WithClassLevelAttribute_ShouldSetAllRows()
    {
        var items = new[]
        {
            new CollectionItem { Id = 1, Name = "Flour",  Description = "A cereal flour" },
            new CollectionItem { Id = 2, Name = "Water",  Description = "H2O" },
        };

        Fixture.Actor.When(UpdateUIModels.Update(items));

        AssertRow(1, "Flour", "A cereal flour");
        AssertRow(2, "Water", "H2O");
    }

    [Fact]
    public void Update_Collection_WithTargetSelectorAndFuncParams_ShouldSetAllRows()
    {
        var items = new[]
        {
            new CollectionItem { Id = 1, Name = "Salt",  Description = "NaCl" },
            new CollectionItem { Id = 2, Name = "Sugar", Description = "Sucrose" },
        };

        var action = UpdateUIModels.Update(
            items,
            RowContainer,
            (item, container) => container.Of(item.Id.ToString()),
            (Func<CollectionItem, string[]>)(_ => Array.Empty<string>()));

        Fixture.Actor.When(action);

        AssertRow(1, "Salt",  "NaCl");
        AssertRow(2, "Sugar", "Sucrose");
    }

    [Fact]
    public void Update_Collection_WithTargetSelectorAndParamsStrings_ShouldSetAllRows()
    {
        var items = new[]
        {
            new CollectionItem { Id = 1, Name = "Yeast", Description = "Leavening agent" },
            new CollectionItem { Id = 2, Name = "Oil",   Description = "Cooking oil" },
        };

        var action = UpdateUIModels.Update(
            items,
            RowContainer,
            (item, container) => container.Of(item.Id.ToString()));
        // params string[] overload — no property-level parameterized selectors on CollectionItem

        Fixture.Actor.When(action);

        AssertRow(1, "Yeast", "Leavening agent");
        AssertRow(2, "Oil",   "Cooking oil");
    }

    [Fact]
    public void Update_Collection_WithoutClassLevelAttribute_ShouldThrowInvalidOperationException()
    {
        var items = new[] { new CollectionItemNoClassAttr { Id = 1, Name = "X" } };
        var action = UpdateUIModels.Update(items);

        Assert.Throws<InvalidOperationException>(() => Fixture.Actor.When(action));
    }

    [Fact]
    public void Update_Collection_WithNamedPropertySelector_ShouldResolveModelProperty()
    {
        var items = new[]
        {
            new NamedParameterizedItem { Id = 1, FieldKey = "alpha", Name = "Named one" },
            new NamedParameterizedItem { Id = 2, FieldKey = "beta", Name = "Named two" },
        };

        Fixture.Actor.When(UpdateUIModels.Update(items));

        AssertInputValue(1, "field_alpha", "Named one");
        AssertInputValue(2, "field_beta", "Named two");
    }

    [Fact]
    public void Update_Collection_WithPositionalClassSelector_ShouldUseModelProperties()
    {
        var items = new[]
        {
            new PositionalClassTargetItem { Id = 1, Name = "Positional class one" },
            new PositionalClassTargetItem { Id = 2, Name = "Positional class two" },
        };

        Fixture.Actor.When(UpdateUIModels.Update(items));

        AssertInputValue(1, "name", "Positional class one");
        AssertInputValue(2, "name", "Positional class two");
    }

    [Fact]
    public void Update_Model_WithPositionalPropertySelector_ShouldResolveDeclaredModelProperty()
    {
        var model = new PositionalParameterizedItem { FieldKey = "alpha", Name = "Positional selector" };

        Fixture.Actor.When(UpdateUIModels.Update(model, RowTarget(1)));

        AssertInputValue(1, "field_alpha", "Positional selector");
    }

    [Fact]
    public void Update_Collection_WithParameterFunction_ShouldUseItemSpecificSelectorParameters()
    {
        var items = new[]
        {
            new SuppliedParameterItem { Id = 1, FieldKey = "alpha", Name = "unused", TargetedName = "Function one" },
            new SuppliedParameterItem { Id = 2, FieldKey = "beta", Name = "unused", TargetedName = "Function two" },
        };

        var action = UpdateUIModels.Update(
            items,
            RowContainer,
            (item, container) => container.Of(item.Id.ToString()),
            item => new[] { item.FieldKey });

        Fixture.Actor.When(action);

        AssertInputValue(1, "field_alpha", "Function one");
        AssertInputValue(2, "field_beta", "Function two");
    }

    [Fact]
    public void Update_Collection_WithSharedParameters_ShouldUseTheProvidedSelectorParameter()
    {
        var items = new[]
        {
            new SuppliedParameterItem { Id = 1, TargetedName = "Shared one" },
            new SuppliedParameterItem { Id = 2, TargetedName = "Shared two" },
        };

        var action = UpdateUIModels.Update(
            items,
            RowContainer,
            (item, container) => container.Of(item.Id.ToString()),
            "shared");

        Fixture.Actor.When(action);

        AssertInputValue(1, "field_shared", "Shared one");
        AssertInputValue(2, "field_shared", "Shared two");
    }

    [Fact]
    public void Update_NestedModel_ShouldUpdateNestedObjectAndCollection()
    {
        var model = new KitchenModel
        {
            Recipe = new RecipeModel
            {
                Name = "Flatbread",
                Ingredients = new List<IngredientModel>
                {
                    new IngredientModel { Id = 1, Name = "Flour" },
                    new IngredientModel { Id = 2, Name = "Water" },
                },
            },
        };
        var kitchen = Target.The("kitchen").LocatedBy(By.Id("kitchen"));

        Fixture.Actor.When(UpdateUIModels.Update(model, kitchen));

        Assert.Equal("Flatbread", Fixture.WebDriver.FindElement(By.Id("recipe_name")).GetAttribute("value"));
        Assert.Equal(
            "Flour",
            Fixture.WebDriver.FindElement(By.Id("ingredient_1")).FindElement(By.Name("ingredient_name")).GetAttribute("value"));
        Assert.Equal(
            "Water",
            Fixture.WebDriver.FindElement(By.Id("ingredient_2")).FindElement(By.Name("ingredient_name")).GetAttribute("value"));
    }

    private void AssertInputValue(int rowId, string inputName, string expectedValue)
    {
        var row = Fixture.WebDriver.FindElement(By.Id($"row_{rowId}"));
        Assert.Equal(expectedValue, row.FindElement(By.Name(inputName)).GetAttribute("value"));
    }
}

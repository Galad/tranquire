using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Xml;
using Moq;
using OpenQA.Selenium;
using Tranquire;
using Tranquire.Selenium.Actions.Enters;
using Tranquire.Selenium.Actions.UIModels;
using Tranquire.Selenium.Questions;
using Tranquire.Selenium.Questions.UIModels;
using Tranquire.Selenium.Targets;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions;

public class UpdateUIModelsUnitTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("YYYY-MM-DD", "yyyy-MM-dd")]
    [InlineData("YY/M/D", "yy/M/d")]
    [InlineData("dd/MM/yyyy", "dd/MM/yyyy")]
    [InlineData("DD/MM/YYYY hh:mm tt", "dd/MM/yyyy hh:mm tt")]
    [InlineData("Quantity", null)]
    [InlineData("Enter a date", null)]
    public void DateTimeFormat_ShouldMapOnlyDatePlaceholders(string placeholder, string expected)
    {
        Assert.Equal(expected, DateTimeFormatter.MapPlaceholderToDotNetFormat(placeholder));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("simple")]
    [InlineData("O'Reilly")]
    [InlineData("\"quoted\"")]
    [InlineData("O'Reilly \"Special\"")]
    [InlineData("'\"middle\"'")]
    public void XPathLiteral_ShouldRoundTripThroughXPathEvaluation(string value)
    {
        var navigator = new XmlDocument().CreateNavigator();

        var result = navigator.Evaluate(XPathStringLiteral.Format(value));

        Assert.Equal(value ?? string.Empty, result);
    }

    [Theory]
    [InlineData(ByMethod.XPath, "By.XPath: field")]
    [InlineData(ByMethod.CssSelector, "By.CssSelector: field")]
    [InlineData(ByMethod.Id, "By.Id: field")]
    [InlineData(ByMethod.Name, "By.Name: field")]
    [InlineData(ByMethod.ClassName, "By.ClassName[Contains]: field")]
    [InlineData(ByMethod.TagName, "By.TagName: field")]
    public void ResolveBy_ShouldUseRequestedLocator(ByMethod method, string expected)
    {
        Assert.Equal(expected, method.ToBy("field").ToString());
    }

    [Theory]
    [InlineData(ByMethod.Self)]
    [InlineData((ByMethod)int.MaxValue)]
    public void ResolveBy_WithUnsupportedMethod_ShouldExplainFailure(ByMethod method)
    {
        var exception = Assert.Throws<NotSupportedException>(() => method.ToBy("field"));

        Assert.Contains(method.ToString(), exception.Message);
    }

    [Fact]
    public void ResolveClassFormat_WithNullDeclaredParameters_ShouldUseModelProperties()
    {
        var attribute = new TargetWithParametersAttribute(ByMethod.Id, "row_{Id}", null);

        Assert.Empty(attribute.FormatParameters);
        Assert.Equal("row_42", ResolveTarget.ResolveClassLevelFormat(attribute, new SelectorModel { Id = 42 }));
    }

    [Fact]
    public void ResolveTargets_WithNullParameters_ShouldResolveLocator()
    {
        var model = new SelectorModel { Key = "alpha" };
        var property = typeof(SelectorModel).GetProperty(nameof(SelectorModel.Key));
        var attribute = new TargetWithParametersAttribute(ByMethod.Name, "field_{Key}");
        var target = ResolveTarget.ResolveTargets(
            property,
            model,
            model.Key,
            null,
            attribute,
            Target.The("container").LocatedBy(By.TagName("body")));
        var element = new Mock<IWebElement>().Object;
        var searchContext = new Mock<ISearchContext>();
        searchContext
            .Setup(context => context.FindElement(It.IsAny<By>()))
            .Returns(element);

        Assert.Same(element, target.ResolveFor(searchContext.Object));
        searchContext.Verify(
            context => context.FindElement(It.Is<By>(by => by.ToString() == "By.CssSelector: body [name=field_alpha]")),
            Times.Once);
    }

    [Theory]
    [InlineData("field_{Missing}", "{Missing}")]
    [InlineData("field_{0}", "{0}")]
    [InlineData("field_{2}", "{2}")]
    public void ResolveClassFormat_WithUnknownPlaceholder_ShouldExplainFailure(string format, string placeholder)
    {
        var attribute = new TargetWithParametersAttribute(ByMethod.Name, format);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            ResolveTarget.ResolveClassLevelFormat(attribute, new SelectorModel()));

        Assert.Contains(placeholder, exception.Message);
        Assert.Contains(nameof(SelectorModel), exception.Message);
    }

    [Fact]
    public void ResolveClassFormat_WithMissingDeclaredProperty_ShouldExplainFailure()
    {
        var attribute = new TargetWithParametersAttribute(ByMethod.Name, "field_{0}", "Missing");

        var exception = Assert.Throws<InvalidOperationException>(() =>
            ResolveTarget.ResolveClassLevelFormat(attribute, new SelectorModel()));

        Assert.Contains("Missing", exception.Message);
        Assert.Contains(nameof(SelectorModel), exception.Message);
    }

    [Fact]
    public void ResolveClassFormat_WithValueParameter_ShouldUseEmptyStringWithoutCurrentValue()
    {
        var attribute = new TargetWithParametersAttribute(ByMethod.Name, "field_{Value}", "Value");

        Assert.Equal("field_", ResolveTarget.ResolveClassLevelFormat(attribute, new SelectorModel()));
    }

    [Fact]
    public void ResolveClassFormat_WithValueParameterAndValueProperty_ShouldUseModelProperty()
    {
        var attribute = new TargetWithParametersAttribute(ByMethod.Name, "field_{0}", "Value");

        Assert.Equal("field_model", ResolveTarget.ResolveClassLevelFormat(attribute, new ValueModel { Value = "model" }));
    }

    [Fact]
    public void ResolveNamedFormat_WithValueProperty_ShouldPreferModelPropertyOverCurrentValue()
    {
        Assert.Equal(
            "field_model_current",
            ResolveTarget.ResolveNamedFormat("field_{Value}_{0}", new ValueModel { Value = "model" }, currentValue: "current"));
    }

    [Theory]
    [InlineData("field_{Key}")]
    [InlineData("field_{0}", "Key")]
    public void ResolveClassFormat_WithNullProperty_ShouldUseEmptyString(string format, params string[] formatParameters)
    {
        var attribute = new TargetWithParametersAttribute(ByMethod.Name, format, formatParameters);

        Assert.Equal("field_", ResolveTarget.ResolveClassLevelFormat(attribute, new SelectorModel()));
    }

    [Fact]
    public void ResolveNamedFormat_ShouldIgnoreWriteOnlyProperties()
    {
        Assert.Equal("field_alpha", ResolveTarget.ResolveNamedFormat("field_{Key}", new SelectorModel { Key = "alpha" }));
    }

    [Theory]
    [InlineData("'{Key}'", null, "")]
    [InlineData("'{Key}'", "alpha", "alpha")]
    [InlineData("\"{Key}\"", "O'Reilly", "O'Reilly")]
    [InlineData("'{Key}'", "\"quoted\"", "\"quoted\"")]
    [InlineData("'{Key}'", "O'Reilly \"quoted\"", "O'Reilly \"quoted\"")]
    [InlineData("'{Key}'", "missing' or @name='NameOfText", "missing' or @name='NameOfText")]
    [InlineData("'prefix_{Key}_{Id}'", "alpha", "prefix_alpha_42")]
    [InlineData("'{Key}'", "{Id}", "{Id}")]
    [InlineData("'fixed'", "unused", "fixed")]
    [InlineData("string({Key})", null, "")]
    [InlineData("string({Key})", "alpha", "alpha")]
    [InlineData("string({Key})", "O'Reilly \"quoted\"", "O'Reilly \"quoted\"")]
    public void ResolveNamedXPathFormat_ShouldEscapeValues(string format, string key, string expected)
    {
        var model = new SelectorModel { Id = 42, Key = key };
        var selector = ResolveTarget.ResolveNamedFormat(format, model, ByMethod.XPath);

        Assert.Equal(expected, new XmlDocument().CreateNavigator().Evaluate(selector));
    }

    [Theory]
    [InlineData(".//{Key}", "input", ".//input")]
    [InlineData(".//@{Key}", "name", ".//@name")]
    [InlineData(".//child::{Key}", "input", ".//child::input")]
    [InlineData(".//{Key}", "*", ".//*")]
    [InlineData(".//{Key}", "ns:input", ".//ns:input")]
    public void ResolveNamedXPathFormat_ShouldPreserveSafeNameTokens(string format, string key, string expected)
    {
        var selector = ResolveTarget.ResolveNamedFormat(
            format,
            new SelectorModel { Key = key },
            ByMethod.XPath);

        Assert.Equal(expected, selector);
    }

    [Theory]
    [InlineData("alpha")]
    [InlineData("001")]
    [InlineData("O'Reilly \"quoted\"")]
    [InlineData("'missing' or @name='NameOfText'")]
    public void ResolveNamedXPathFormat_WithUnquotedValue_ShouldMatchOnlyExactField(string key)
    {
        var selector = ResolveTarget.ResolveNamedFormat(
            ".//input[@name={Key}]", new SelectorModel { Key = key }, ByMethod.XPath);
        var document = new XmlDocument();
        document.LoadXml("<form><input name='NameOfText'/><input name='1'/><input/></form>");
        var target = (XmlElement)document.DocumentElement.LastChild;
        target.SetAttribute("name", key);
        var matches = document.SelectNodes(selector);

        Assert.Equal(1, matches.Count);
        Assert.Same(target, matches[0]);
    }

    [Theory]
    [InlineData(".//{Key}", "input | .//input")]
    [InlineData(".//@{Key}", "name | //@name")]
    [InlineData(".//child::{Key}", "input | .//input")]
    [InlineData(".//{Key}", "ancestor::input")]
    [InlineData(".//{Key}", "ns:bad:input")]
    public void ResolveNamedXPathFormat_WithUnsafeNameToken_ShouldRejectExpression(string format, string key)
    {
        Assert.Throws<XmlException>(() => ResolveTarget.ResolveNamedFormat(
            format, new SelectorModel { Key = key }, ByMethod.XPath));
    }

    [Theory]
    [InlineData(".//input[{Id}]", null, ".//input[2]")]
    [InlineData(".//input[ {Id} ]", null, ".//input[ 2 ]")]
    [InlineData(".//input[{Key}]", "2.0", ".//input[2.0]")]
    public void ResolveNamedXPathFormat_ShouldPreserveNumericPosition(string format, string key, string expected)
    {
        var selector = ResolveTarget.ResolveNamedFormat(format, new SelectorModel { Id = 2, Key = key }, ByMethod.XPath);

        Assert.Equal(expected, selector);
    }

    [Fact]
    public void ResolveNamedXPathFormat_WithUnsafePosition_ShouldRejectExpression()
    {
        Assert.Throws<FormatException>(() => ResolveTarget.ResolveNamedFormat(
            ".//input[{Key}]", new SelectorModel { Key = "1 or true()" }, ByMethod.XPath));
    }

    [Theory]
    [InlineData("input[name='{Key}']", @"a'b""c\d", @"input[name='a\'b\""c\\d']")]
    [InlineData("input[name=\"{Key}\"]", "line\nbreak", "input[name=\"line\\a break\"]")]
    [InlineData(@"[title='it\'s {Key}']", "x", @"[title='it\'s x']")]
    [InlineData("#row_{Id}", null, "#row_42")]
    [InlineData(".{Key}", "alpha-1_\u00e9", ".alpha-1_\u00e9")]
    [InlineData(".item{Key}", null, ".item")]
    public void ResolveNamedCssFormat_ShouldEscapeQuotedValuesAndKeepSafeTokens(string format, string key, string expected)
    {
        var selector = ResolveTarget.ResolveNamedFormat(
            format, new SelectorModel { Id = 42, Key = key }, ByMethod.CssSelector);

        Assert.Equal(expected, selector);
    }

    [Theory]
    [InlineData("a, b")]
    [InlineData("a b")]
    [InlineData("a]")]
    [InlineData("a\n")]
    public void ResolveNamedCssFormat_WithUnsafeUnquotedValue_ShouldRejectSelector(string key)
    {
        Assert.Throws<FormatException>(() => ResolveTarget.ResolveNamedFormat(
            ".{Key}", new SelectorModel { Key = key }, ByMethod.CssSelector));
    }

    [Fact]
    public void ResolveNamedFormat_WithNonXPathOrCssMethod_ShouldNotEscapeValues()
    {
        Assert.Equal(
            "field_a'b",
            ResolveTarget.ResolveNamedFormat("field_{Key}", new SelectorModel { Key = "a'b" }, ByMethod.Name));
    }

    [Fact]
    public void FieldAction_WithUnsupportedTag_ShouldExplainFailure()
    {
        var actor = new Mock<IActor>(MockBehavior.Strict);

        var exception = Assert.Throws<NotSupportedException>(() =>
            UpdateField.PerformFieldAction(actor.Object, Field("value", "button", "button", null)));

        Assert.Contains("'field'", exception.Message);
        Assert.Contains("<button>", exception.Message);
        actor.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("textarea", null, "1,5")]
    [InlineData("input", "text", "1,5")]
    [InlineData("input", null, "1,5")]
    [InlineData("input", "number", "1.5")]
    public void FieldAction_WithFrenchCulture_ShouldFormatWithCultureExceptNumberInputs(string tagName, string inputType, string expected)
    {
        var actor = new Mock<IActor>();
        var field = Target.The("field").LocatedBy(By.Id("field"));

        UpdateField.PerformFieldAction(actor.Object, Field(1.5m, tagName, inputType, field, culture: new CultureInfo("fr-FR")));

        actor.Verify(executor => executor.Execute(It.Is<EnterNewValue>(action => action.Value == expected)), Times.Once);
    }

    [Fact]
    public void FieldAction_WithDatePlaceholderAndNonDateValue_ShouldEnterValueUnchanged()
    {
        var actor = new Mock<IActor>();
        actor.Setup(executor => executor.AsksFor(It.IsAny<IQuestion<string>>())).Returns("DD/MM/YYYY");
        var field = Target.The("field").LocatedBy(By.Id("field"));

        UpdateField.PerformFieldAction(actor.Object, Field("2026-10-01", "input", "text", field));

        actor.Verify(executor => executor.Execute(It.Is<EnterNewValue>(action => action.Value == "2026-10-01")), Times.Once);
    }

    [Fact]
    public void FieldAction_WithDatePlaceholderAndDateTimeOffset_ShouldApplyPlaceholderFormat()
    {
        var actor = new Mock<IActor>();
        actor.Setup(executor => executor.AsksFor(It.IsAny<IQuestion<string>>())).Returns("DD/MM/YYYY");
        var field = Target.The("field").LocatedBy(By.Id("field"));
        var date = new DateTimeOffset(2026, 10, 1, 0, 0, 0, TimeSpan.Zero);

        UpdateField.PerformFieldAction(actor.Object, Field(date, "input", "text", field));

        actor.Verify(executor => executor.Execute(It.Is<EnterNewValue>(action => action.Value == "01/10/2026")), Times.Once);
    }

    [Fact]
    public void Update_WithCulture_ShouldFormatValuesWithThatCulture()
    {
        var actor = new Mock<IActor>();
        var container = Target.The("container").LocatedBy(By.Id("container"));
        var action = UpdateUIModels.Update(new DecimalModel { Amount = 1.5m }, container);

        var frenchAction = action.WithCulture(new CultureInfo("fr-FR"));
        frenchAction.ExecuteWhenAs(actor.Object);

        Assert.Equal(action.Name, frenchAction.Name);
        actor.Verify(executor => executor.Execute(It.Is<EnterNewValue>(enter => enter.Value == "1,5")), Times.Once);
    }

    [Fact]
    public void Update_WithNullCulture_ShouldThrow()
    {
        var action = UpdateUIModels.Update(new DecimalModel(), Target.The("container").LocatedBy(By.Id("container")));

        Assert.Equal("culture", Assert.Throws<ArgumentNullException>(() => action.WithCulture(null)).ParamName);
    }

    [Fact]
    public void FieldAction_WithNullSelectValue_ShouldExecuteSelectAction()
    {
        var actor = new Mock<IActor>(MockBehavior.Strict);
        actor.Setup(executor => executor.Execute(It.IsAny<IAction<Unit>>())).Returns(Unit.Default);
        var field = Target.The("field").LocatedBy(By.Id("field"));

        UpdateField.PerformFieldAction(actor.Object, Field(null, "select", null, field));

        actor.Verify(executor => executor.Execute(It.IsAny<IAction<Unit>>()), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("range")]
    public void FieldAction_WithUnrecognizedInputType_ShouldExecuteDefaultEnterAction(string inputType)
    {
        var actor = new Mock<IActor>();
        var field = Target.The("field").LocatedBy(By.Id("field"));

        UpdateField.PerformFieldAction(actor.Object, Field("42", "input", inputType, field));

        actor.Verify(executor => executor.Execute(It.Is<EnterNewValue>(action => action.Target == field && action.Value == "42")), Times.Once);
        actor.VerifyNoOtherCalls();
    }

    [Fact]
    public void FieldAction_WithUnrecognizedInputTypeAndNullValue_ShouldRejectNull()
    {
        var actor = new Mock<IActor>();
        var field = Target.The("field").LocatedBy(By.Id("field"));

        Assert.Throws<ArgumentNullException>(() =>
            UpdateField.PerformFieldAction(actor.Object, Field(null, "input", null, field)));

        actor.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("input", "text")]
    [InlineData("textarea", null)]
    public void FieldAction_WithNullTextValue_ShouldRejectNull(string tagName, string inputType)
    {
        var actor = new Mock<IActor>();
        var field = Target.The("field").LocatedBy(By.Id("field"));

        Assert.Throws<ArgumentNullException>(() =>
            UpdateField.PerformFieldAction(actor.Object, Field(null, tagName, inputType, field)));

        actor.Verify(executor => executor.Execute(It.IsAny<IAction<Unit>>()), Times.Never);
    }

    [Fact]
    public void FieldAction_WithNullDateInputValue_ShouldClearValue()
    {
        var actor = new Mock<IActor>();
        var field = Target.The("field").LocatedBy(By.Id("field"));

        UpdateField.PerformFieldAction(actor.Object, Field(null, "input", "date", field));

        actor.Verify(executor => executor.Execute(It.Is<SetInputValue>(action => action.Target == field && action.Value == null)), Times.Once);
    }

    [Fact]
    public void FieldAction_WithNullRadioValue_ShouldClickUngroupedRadio()
    {
        var actor = new Mock<IActor>();
        var container = Target.The("container").LocatedBy(By.Id("container"));
        var field = Target.The("field").LocatedBy(By.Id("field"));

        UpdateField.PerformFieldAction(actor.Object, Field(null, "input", "radio", field, container));

        actor.Verify(executor => executor.Execute(It.IsAny<IAction<Unit>>()), Times.Once);
    }

    [Fact]
    public void SetInputValue_ShouldDescribeTargetAndValue()
    {
        var action = new SetInputValue(Target.The("date field").LocatedBy(By.Id("date")), "2026-10-01");

        Assert.Equal("Set the value of date field to '2026-10-01'", action.Name);
    }

    [Fact]
    public void Update_WithCollectionAsSingleModel_ShouldRejectCollection()
    {
        var container = Target.The("container").LocatedBy(By.Id("container"));
        var models = new List<SelectorModel> { new SelectorModel() };

        Assert.Equal("value", Assert.Throws<ArgumentException>(() => UpdateUIModels.Update(models, container)).ParamName);
        Assert.Equal("value", Assert.Throws<ArgumentException>(() => UpdateUIModels.Update(models, container, "1")).ParamName);
    }

    [Fact]
    public void FormUpdate_WithDefaultArrayOrUntargetedProperty_ShouldSkipProperties()
    {
        var actor = new Mock<IActor>(MockBehavior.Strict);
        var container = Target.The("container").LocatedBy(By.Id("container"));

        FormUpdate.FillFormFromModel(new SkippedPropertiesModel(), container).ExecuteWhenAs(actor.Object);

        actor.VerifyNoOtherCalls();
    }

    [Fact]
    public void FormUpdate_WithBackReference_ShouldUpdateEachModelOnce()
    {
        var actor = new Mock<IActor>();
        actor.Setup(executor => executor.Execute(It.IsAny<IAction<Unit>>())).Returns(Unit.Default);
        var container = Target.The("container").LocatedBy(By.Id("container"));
        var parent = new ParentNode { Name = "parent" };
        parent.Child = new ChildNode { Name = "child", Parent = parent };

        FormUpdate.FillFormFromModel(parent, container).ExecuteWhenAs(actor.Object);

        actor.Verify(executor => executor.Execute(It.IsAny<IAction<Unit>>()), Times.Exactly(2));
    }

    [Fact]
    public void FormUpdate_WithMissingTypeAndTagName_ShouldUseDefaultInputDispatch()
    {
        var actor = new Mock<IActor>(MockBehavior.Strict);
        actor.Setup(executor => executor.AsksFor(It.IsAny<IQuestion<string>>())).Returns((string)null);
        actor.Setup(executor => executor.Execute(It.IsAny<IAction<Unit>>())).Returns(Unit.Default);
        var model = new DefaultDispatchModel { Value = "value" };
        var container = Target.The("container").LocatedBy(By.Id("container"));

        FormUpdate.FillFormFromModel(model, container).ExecuteWhenAs(actor.Object);

        actor.Verify(executor => executor.AsksFor(It.IsAny<IQuestion<string>>()), Times.Exactly(2));
        actor.Verify(executor => executor.Execute(It.IsAny<IAction<Unit>>()), Times.Once);
    }

    private static FormField Field(
        object value,
        string tagName,
        string type,
        ITarget target,
        ITarget container = null,
        CultureInfo culture = null) =>
        new("field", value, tagName, type, target, container, culture ?? CultureInfo.InvariantCulture);

    private sealed class DecimalModel
    {
        [Target(ByMethod.Id, "amount"), Enter]
        public decimal Amount { get; set; }
    }

    private sealed class DefaultDispatchModel
    {
        [Target(ByMethod.Id, "field"), Enter]
        public string Value { get; set; }
    }

    private sealed class SkippedPropertiesModel
    {
        [Target(ByMethod.Id, "values"), Enter]
        public ImmutableArray<string> Values { get; set; }

        [Enter]
        public string Untargeted { get; set; } = "value";
    }

    private sealed class ParentNode
    {
        [Target(ByMethod.Id, "parent"), Enter]
        public string Name { get; set; }

        public ChildNode Child { get; set; }
    }

    private sealed class ChildNode
    {
        [Target(ByMethod.Id, "child"), Enter]
        public string Name { get; set; }

        public ParentNode Parent { get; set; }
    }

    private sealed class ValueModel
    {
        public string Value { get; set; }
    }

    private sealed class SelectorModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string WriteOnly { set => throw new NotSupportedException(); }
    }
}
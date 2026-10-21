using System;
using System.Xml;
using Moq;
using OpenQA.Selenium;
using Tranquire.Selenium.Actions.Enters;
using Tranquire.Selenium.Actions.UIModels;
using Tranquire.Selenium.Questions.UIModels;
using Tranquire.Selenium.Targets;
using Xunit;

namespace Tranquire.Selenium.Tests.Actions;

public class UpdateUIModelsUnitTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("YYYY-MM-DD", "yyyy-MM-dd")]
    [InlineData("YY/M/D", "yy/M/d")]
    [InlineData("dd/MM/yyyy", "dd/MM/yyyy")]
    public void DateTimeFormat_ShouldMapPlaceholderTokens(string placeholder, string expected)
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
        Assert.Equal(expected, ResolveTarget.ResolveBy(method, "field").ToString());
    }

    [Theory]
    [InlineData(ByMethod.Self)]
    [InlineData((ByMethod)int.MaxValue)]
    public void ResolveBy_WithUnsupportedMethod_ShouldExplainFailure(ByMethod method)
    {
        var exception = Assert.Throws<NotSupportedException>(() => ResolveTarget.ResolveBy(method, "field"));

        Assert.Contains(method.ToString(), exception.Message);
    }

    [Fact]
    public void ResolveBy_WithExplicitLocator_ShouldPreserveLocatorAndAttributeValues()
    {
        var locator = By.Id("fixed");
        var parameters = new[] { "Id" };
        var attribute = new TargetWithParametersAttribute(locator, "row_{Id}", parameters);

        Assert.Same(locator, ResolveTarget.ResolveBy(attribute, "unused"));
        Assert.Equal("row_{Id}", attribute.Format);
        Assert.Same(parameters, attribute.FormatParameters);
    }

    [Fact]
    public void TargetAttribute_WithNullLocatorParameters_ShouldUseEmptyParameterArray()
    {
        var attribute = new TargetWithParametersAttribute(By.Id("fixed"), "row_{Id}", null);

        Assert.Empty(attribute.FormatParameters);
    }

    [Fact]
    public void ResolveClassFormat_WithNullDeclaredParameters_ShouldUseModelProperties()
    {
        var attribute = new TargetWithParametersAttribute(ByMethod.Id, "row_{Id}", null);

        Assert.Equal("row_42", ResolveTarget.ResolveClassLevelFormat(attribute, new SelectorModel { Id = 42 }));
    }

    [Theory]
    [InlineData("field_{Missing}", "{Missing}")]
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

    [Theory]
    [InlineData("field_{Key}")]
    [InlineData("field_{1}")]
    public void ResolveClassFormat_WithNullProperty_ShouldUseEmptyString(string format)
    {
        var attribute = new TargetWithParametersAttribute(ByMethod.Name, format);

        Assert.Equal("field_", ResolveTarget.ResolveClassLevelFormat(attribute, new SelectorModel()));
    }

    [Fact]
    public void ResolveNamedFormat_ShouldIgnoreWriteOnlyProperties()
    {
        Assert.Equal("field_alpha", ResolveTarget.ResolveNamedFormat("field_{Key}", new SelectorModel { Key = "alpha" }));
    }

    [Fact]
    public void FieldAction_WithUnsupportedTag_ShouldNotExecuteAnAction()
    {
        var actor = new Mock<IActor>(MockBehavior.Strict);

        UpdateField.PerformFieldAction(actor.Object, "field", "value", "button", "button", null, null);

        actor.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("range")]
    public void FieldAction_WithUnrecognizedInputType_ShouldExecuteDefaultEnterAction(string inputType)
    {
        var actor = new Mock<IActor>();
        var field = Target.The("field").LocatedBy(By.Id("field"));

        UpdateField.PerformFieldAction(actor.Object, "field", "42", "input", inputType, field, null);

        actor.Verify(executor => executor.Execute(It.Is<EnterNewValue>(action => action.Target == field && action.Value == "42")), Times.Once);
        actor.VerifyNoOtherCalls();
    }

    private sealed class SelectorModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string WriteOnly { set => throw new NotSupportedException(); }
    }
}
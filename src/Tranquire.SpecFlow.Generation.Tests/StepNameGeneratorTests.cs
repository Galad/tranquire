using AutoFixture;
using AutoFixture.Idioms;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Tranquire.SpecFlow.Generation.Generator;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Tranquire.SpecFlow.Generation.Tests
{
    public class StepNameGeneratorTests
    {
        [Theory, DomainAutoData]
        public void VerifyGuardClause(GuardClauseAssertion assertion, Fixture fixture)
        {
            fixture.Register(() => MethodDeclaration(
                                    PredefinedType(Token(SyntaxKind.VoidKeyword)),
                                    Identifier("test")
                                ));
            assertion.Verify(typeof(StepSentenceGenerator));
        }

        [Theory]
        [InlineData("ABookInTheLibrary", StepKind.Given, "a book in the library")]
        [InlineData("ABookInTheLibrary", StepKind.When, "I add a book in the library")]
        [InlineData("A", StepKind.Given, "a")]
        [InlineData("A", StepKind.When, "I add a")]
        [InlineData("AB", StepKind.Given, "a b")]
        [InlineData("AB", StepKind.When, "I add a b")]
        [InlineData("ABook_InTheLibrary", StepKind.Given, @"a book in the library")]
        public void ParameterlessAction_ShouldReturnCorrectName(string methodName, StepKind stepKind, string expected)
        {
            string code = @"
public class Add
{
    public void " + methodName + @"()
    {
    }
}
";
            MethodDeclarationSyntax method = GetMethod(code);
            // act
            string actual = StepSentenceGenerator.FromMethod(method, stepKind);
            // assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("TheBook", "string name", StepKind.Given, @"the book ""(.*)""")]
        [InlineData("TheBook", "string name", StepKind.When, @"I add the book ""(.*)""")]
        [InlineData("TheValue", "int value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "short value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "long value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "float value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "double value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "byte value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "sbyte value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "bool value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "ushort value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "uint value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "ulong value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "decimal value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "char value", StepKind.Given, @"the value (.*)")]
        [InlineData("TheValue", "object value", StepKind.Given, @"the value")]
        [InlineData("TheValue", "Value value", StepKind.Given, @"the value")]
        [InlineData("TheBook_InTheLibrary", "string name", StepKind.Given, @"the book ""(.*)"" in the library")]
        [InlineData("TheValue_InTheCalculator", "int value", StepKind.Given, @"the value (.*) in the calculator")]        
        public void ActionWithParameters_ShouldReturnCorrectName(
            string methodName,
            string parameters,
            StepKind stepKind,
            string expected)
        {
            string code = @"
public class Add
{
    public void " + methodName + @"(" + parameters + @")
    {
    }
}
";
            MethodDeclarationSyntax method = GetMethod(code);
            // act
            string actual = StepSentenceGenerator.FromMethod(method, stepKind);
            // assert
            Assert.Equal(expected, actual);
        }

        private static MethodDeclarationSyntax GetMethod(string code)
        {
            return ParseSyntaxTree(code)
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .FirstOrDefault();
        }
    }
}

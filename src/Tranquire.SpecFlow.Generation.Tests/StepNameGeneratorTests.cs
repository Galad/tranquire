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
            assertion.Verify(typeof(StepNameGenerator));
        }

        [Theory]
        [InlineData("ABookInTheLibrary", StepKind.Given, "a book in the library")]
        [InlineData("ABookInTheLibrary", StepKind.When, "I add a book in the library")]
        [InlineData("A", StepKind.Given, "a")]
        [InlineData("A", StepKind.When, "I add a")]
        [InlineData("AB", StepKind.Given, "a b")]
        [InlineData("AB", StepKind.When, "I add a b")]
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
            string actual = StepNameGenerator.FromMethod(method, stepKind);
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

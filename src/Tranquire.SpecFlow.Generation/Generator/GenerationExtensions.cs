using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Tranquire.SpecFlow.Generation.Generator
{
    internal static class GenerationExtensions
    {
        public static IEnumerable<MemberDeclarationSyntax> GetCandidateMethods(this ClassDeclarationSyntax classDeclaration)
        {
            return classDeclaration.Members.OfType<MethodDeclarationSyntax>()
                                           .Where(m => m.Modifiers.Any(mod => mod.Kind() == SyntaxKind.StaticKeyword) &&
                                                       m.Modifiers.Any(mod => mod.Kind() == SyntaxKind.PublicKeyword) &&
                                                       m.IsAction()
                                                 );
        }

        public static IEnumerable<ActionDescription> SelectDescription(this IEnumerable<MemberDeclarationSyntax> memberDeclarationSyntaxes)
        {
            return memberDeclarationSyntaxes.Select(ActionDescription.Create);
        }

        public static IEnumerable<MemberDeclarationSyntax> ToStepMethods(this IEnumerable<ActionDescription> actionDescriptions)
        {
            return actionDescriptions.SelectMany(a =>
                new[]
                {
                    GenerateStepMethod(a, GivenAttributeName, StepKind.Given),
                    GenerateStepMethod(a, WhenAttributeName, StepKind.When)
                }
            );

            MethodDeclarationSyntax GenerateStepMethod(ActionDescription a, NameSyntax attributeName, StepKind stepKind)
            {
                return MethodDeclaration(
                                    PredefinedType(Token(SyntaxKind.VoidKeyword)),
                                    Identifier(stepKind.ToString() + "_" + a.MethodName)
                                )
                                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                .WithAttributeLists(List(new[] 
                                    {
                                        AttributeList(
                                            SeparatedList(new[] 
                                            {
                                                Attribute(attributeName)
                                                    .WithArgumentList(
                                                        AttributeArgumentList(
                                                            SingletonSeparatedList(
                                                                AttributeArgument(
                                                                    LiteralExpression(
                                                                        SyntaxKind.StringLiteralExpression,
                                                                        Literal(a.GetStepRegex(stepKind))
                                                                    )
                                                                )    
                                                            )
                                                        )
                                                    )
                                            })
                                        )
                                    }
                                ))
                                .WithParameterList(ParameterList(SeparatedList(a.Parameters)))
                                .WithBody(Block(GenerateInvokeActionStatement(a, stepKind)));
            }

        }

        private static StatementSyntax GenerateInvokeActionStatement(
            ActionDescription actionDescription,
            StepKind stepKind)
        {
            return ExpressionStatement(
                       InvocationExpression(
                           MemberAccessExpression(
                               SyntaxKind.SimpleMemberAccessExpression,
                               InvocationExpression(
                                   MemberAccessExpression(
                                       SyntaxKind.SimpleMemberAccessExpression,
                                       IdentifierName("_container"),
                                       GenericName(
                                           Identifier("Resolve")
                                       )
                                       .WithTypeArgumentList(
                                           TypeArgumentList(
                                               SingletonSeparatedList<TypeSyntax>(
                                                   IdentifierName("IActorFacade")
                                               )
                                           )
                                       )                                       
                                   )
                               ),
                               IdentifierName(stepKind.ToString())
                           )
                       )
                       .WithArgumentList(
                           ArgumentList(
                               SingletonSeparatedList(
                                   Argument(
                                       InvocationExpression(
                                           MemberAccessExpression(
                                               SyntaxKind.SimpleMemberAccessExpression,
                                               IdentifierName((actionDescription.Method.Parent as ClassDeclarationSyntax).Identifier),
                                               IdentifierName((actionDescription.Method as MethodDeclarationSyntax).Identifier)
                                           )
                                       )
                                       .WithArgumentList(
                                           ArgumentList(
                                               SeparatedList(
                                                   actionDescription.Parameters
                                                                    .Select(p => Argument(IdentifierName(p.Identifier)))
                                               )
                                           )
                                       )
                                   )
                               )
                           )
                       )
                   );
        }

        public static bool IsAction(this MemberDeclarationSyntax memberDeclarationSyntax)
        {
            return true;
        }

        public static NameSyntax SpecFlowName { get; } = QualifiedName(
                    IdentifierName("TechTalk"),
                    IdentifierName("SpecFlow")
                );
        public static NameSyntax GivenAttributeName { get; } =
            QualifiedName(
                SpecFlowName,
                IdentifierName("Given")
            );
        public static NameSyntax WhenAttributeName { get; } =
            QualifiedName(
                SpecFlowName,
                IdentifierName("When")
            );
    }
}

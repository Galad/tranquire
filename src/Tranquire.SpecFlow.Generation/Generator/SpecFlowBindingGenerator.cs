using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Tranquire.SpecFlow.Generation.Generator
{
    /// <summary>
    /// The code generator
    /// </summary>
    public sealed class SpecFlowBindingGenerator : IRichCodeGenerator
    {
        /// <summary>
        /// Creates a new instance of <see cref="SpecFlowBindingGenerator"/>
        /// </summary>
        /// <param name="attributeData"></param>
        public SpecFlowBindingGenerator(AttributeData attributeData)
        {
        }

        /// <inheritdoc />
        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            return Task.FromResult(List<MemberDeclarationSyntax>());
        }

        /// <inheritdoc />
        public Task<RichGenerationResult> GenerateRichAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            ClassDeclarationSyntax classDeclaration = context.ProcessingNode as ClassDeclarationSyntax;
            if (classDeclaration == null)
            {
                return Task.FromResult(new RichGenerationResult());
            }

            return Task.FromResult(new RichGenerationResult()
            {
                Members = List((System.Collections.Generic.IEnumerable<MemberDeclarationSyntax>)(new[]
                {
                    GenerateStepClass(classDeclaration)
                }))
            });
        }

        private static NamespaceDeclarationSyntax GenerateStepClass(ClassDeclarationSyntax classDeclaration)
        {
            var members = classDeclaration.GetCandidateMethods()
                                          .SelectDescription()
                                          .ToStepMethods()
                                          .ToArray();

            string className = classDeclaration.Identifier.ValueText + "_Steps";

            return NamespaceDeclaration(classDeclaration.FirstAncestorOrSelf<NamespaceDeclarationSyntax>().Name)
                                        .AddMembers(
                                                ClassDeclaration(Identifier(className))
                                                             .WithAttributeLists(List(new[] { AttributeList(SeparatedList(new[] { Attribute(BindingAttributeName) })) }))
                                                             .WithModifiers(
                                                                TokenList(Token(SyntaxKind.PublicKeyword))
                                                                )
                                                             .AddMembers(ObjectContainerDeclaration, CreateConstructor(className))
                                                             .AddMembers(members)
                                        );
        }

        private static NameSyntax BindingAttributeName { get; } =
            QualifiedName(
                QualifiedName(
                    IdentifierName("TechTalk"),
                    IdentifierName("SpecFlow")
                ),
                IdentifierName("Binding")
            );

        private static ConstructorDeclarationSyntax CreateConstructor(string className)
        {
            return
                ConstructorDeclaration(
                        Identifier(className)
                    )
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword)
                        )
                    )
                    .WithParameterList(
                        ParameterList(
                            SingletonSeparatedList<ParameterSyntax>(
                                Parameter(
                                    Identifier("container")
                                )
                                .WithType(_objectContainerTypeName)
                            )
                        )
                    )
                    .WithBody(
                        Block(
                            SingletonList<StatementSyntax>(
                                ExpressionStatement(
                                    AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName("_container"),
                                        IdentifierName("container")
                                    )
                                )
                            )
                        )
                    );
        }

        private static QualifiedNameSyntax _objectContainerTypeName =
            QualifiedName(
                IdentifierName("BoDi"),
                IdentifierName("IObjectContainer")
            );

        private static FieldDeclarationSyntax ObjectContainerDeclaration { get; } =
            FieldDeclaration(
                    VariableDeclaration(_objectContainerTypeName)
                        .WithVariables(
                            SingletonSeparatedList(
                                VariableDeclarator(
                                    Identifier("_container")
                                )
                            )
                        )
                    )
                    .WithModifiers(
                        TokenList(
                            new[]{
                                Token(SyntaxKind.PrivateKeyword),
                                Token(SyntaxKind.ReadOnlyKeyword)
                            }
                        )
                    );
    }
}

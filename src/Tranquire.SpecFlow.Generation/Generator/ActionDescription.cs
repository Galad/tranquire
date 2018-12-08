using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tranquire.SpecFlow.Generation.Generator
{
    public class ActionDescription
    {
        public ActionKind Kind { get; }
        public MemberDeclarationSyntax Method { get; }
        public string MethodName { get; }
        
        public ActionDescription(
            ActionKind kind, 
            MemberDeclarationSyntax method, 
            string methodName)
        {
            Kind = kind;
            Method = method ?? throw new System.ArgumentNullException(nameof(method));
            MethodName = methodName ?? throw new System.ArgumentNullException(nameof(methodName));
        }

        public static ActionDescription Create(MemberDeclarationSyntax memberDeclarationSyntax)
        {
            var kind = memberDeclarationSyntax.IsAction() ? ActionKind.Action : ActionKind.Question;
            var methodName = (memberDeclarationSyntax.Parent as ClassDeclarationSyntax).Identifier.ValueText +
                             "_" +
                             (memberDeclarationSyntax as MethodDeclarationSyntax).Identifier.ValueText;
            return new ActionDescription(kind, memberDeclarationSyntax, methodName);
        }

        public string GetStepRegex(StepKind stepKind)
        {
            return StepNameGenerator.FromMethod(Method as MethodDeclarationSyntax, stepKind);
        }
    }
}

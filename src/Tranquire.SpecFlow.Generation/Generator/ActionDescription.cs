using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tranquire.SpecFlow.Generation.Generator
{
    /// <summary>
    /// Describes an action
    /// </summary>
    public class ActionDescription
    {
        /// <summary>
        /// The action kind
        /// </summary>
        public ActionKind Kind { get; }
        /// <summary>
        /// The member declaration that creates the action
        /// </summary>
        public MemberDeclarationSyntax Method { get; }
        /// <summary>
        /// The step method name
        /// </summary>
        public string MethodName { get; }
        /// <summary>
        /// The parameters for the step method
        /// </summary>
        public IReadOnlyList<ParameterSyntax> Parameters { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ActionDescription"/>
        /// </summary>
        /// <param name="kind">The action kind</param>
        /// <param name="method">The member declaration that creates the action</param>
        /// <param name="methodName">The step method name</param>
        /// <param name="parameters">The parameters for the step method</param>
        public ActionDescription(
            ActionKind kind, 
            MemberDeclarationSyntax method, 
            string methodName,
            IReadOnlyList<ParameterSyntax> parameters)
        {
            Kind = kind;
            Method = method ?? throw new System.ArgumentNullException(nameof(method));
            MethodName = methodName ?? throw new System.ArgumentNullException(nameof(methodName));
            Parameters = parameters;
        }

        /// <summary>
        /// Creates a <see cref="ActionDescription"/> from a method declaration
        /// </summary>
        /// <param name="memberDeclarationSyntax"></param>
        /// <returns></returns>
        public static ActionDescription Create(MemberDeclarationSyntax memberDeclarationSyntax)
        {
            var kind = memberDeclarationSyntax.IsAction() ? ActionKind.Action : ActionKind.Question;
            var methodName = (memberDeclarationSyntax.Parent as ClassDeclarationSyntax).Identifier.ValueText +
                             "_" +
                             (memberDeclarationSyntax as MethodDeclarationSyntax).Identifier.ValueText;
            return new ActionDescription(kind, memberDeclarationSyntax, methodName, ((MethodDeclarationSyntax)memberDeclarationSyntax).ParameterList.Parameters);
        }

        /// <summary>
        /// Gets the step regex that is used to map the step in a feature file to a step method
        /// </summary>
        /// <param name="stepKind">The step kind</param>
        /// <returns></returns>
        public string GetStepRegex(StepKind stepKind)
        {
            return StepSentenceGenerator.FromMethod(Method as MethodDeclarationSyntax, stepKind);
        }
    }
}

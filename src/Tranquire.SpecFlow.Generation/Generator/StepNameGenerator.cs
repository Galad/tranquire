using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tranquire.SpecFlow.Generation.Generator
{
    /// <summary>
    /// Generates step sentences
    /// </summary>
    public static class StepSentenceGenerator
    {
        private const string Space = " ";

        /// <summary>
        /// Generate a step sentence from a method declaration
        /// </summary>
        /// <param name="method">The method declaration</param>
        /// <param name="stepKind">The step kind</param>
        /// <returns>A regex representing the step sentence, that matches the method arguments</returns>
        public static string FromMethod(MethodDeclarationSyntax method, StepKind stepKind)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var parts = method.Identifier.Text.Split('_');
            var wordGroups = parts.Select(p => SplitByUppercase(p).Select(w => w.ToLower()))
                                  .ToArray();
            string sentence;            
            if(method.ParameterList.Parameters.Count == 1 && wordGroups.Length == 2)
            {
                sentence = string.Join(
                    Space + GetRegexExpression(method.ParameterList.Parameters[0]) + Space,
                    wordGroups.Select(words => string.Join(Space, words))
                    );
            }
            else if (method.ParameterList.Parameters.Count == 1)
            {
                var parameter = GetRegexExpression(method.ParameterList.Parameters[0]);
                var suffix = parameter == string.Empty ? string.Empty : Space + parameter;
                sentence = string.Join(Space, wordGroups.SelectMany(w => w)) + suffix; 
            }
            else
            {
                sentence = string.Join(Space, wordGroups.SelectMany(w => w));
            }
            if(stepKind == StepKind.Given)
            {
                return sentence;
            }
            return "I " + (method.Parent as ClassDeclarationSyntax).Identifier.Text.ToLower() + Space + sentence;
        }

        private static string GetRegexExpression(ParameterSyntax parameterSyntax)
        {
            if(parameterSyntax.Type is PredefinedTypeSyntax predefinedType)
            {
                switch (predefinedType.Keyword.Kind())
                {
                    case SyntaxKind.BoolKeyword:
                    case SyntaxKind.ByteKeyword:
                    case SyntaxKind.SByteKeyword:
                    case SyntaxKind.ShortKeyword:
                    case SyntaxKind.UShortKeyword:
                    case SyntaxKind.IntKeyword:
                    case SyntaxKind.UIntKeyword:
                    case SyntaxKind.LongKeyword:
                    case SyntaxKind.ULongKeyword:
                    case SyntaxKind.DoubleKeyword:
                    case SyntaxKind.FloatKeyword:
                    case SyntaxKind.DecimalKeyword:                    
                    case SyntaxKind.CharKeyword:
                        return "(.*)";
                    case SyntaxKind.ObjectKeyword:
                        return string.Empty;
                    default:
                        return @"""(.*)""";
                }
            }
            return string.Empty;
        }

        private static IEnumerable<string> SplitByUppercase(string text)
        {
            int i = 0, j = 0;
            char[] chars = text.ToCharArray();
            int length = chars.Length;
            while (i < length)
            {
                while (i + j < length - 1 && !char.IsUpper(text[i + j + 1]))
                {
                    j++;
                }
                yield return new string(chars, i, j + 1);
                i = i + j + 1;
                j = 0;
            }
        }
    }
}

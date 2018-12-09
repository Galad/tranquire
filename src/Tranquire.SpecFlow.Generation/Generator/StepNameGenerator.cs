using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Tranquire.SpecFlow.Generation.Generator
{
    /// <summary>
    /// Generates step sentences
    /// </summary>
    public static class StepSentenceGenerator
    {
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
                throw new System.ArgumentNullException(nameof(method));
            }

            IEnumerable<string> words = GetWords(method.Identifier.Text)
                                            .Select(w => w.ToLower());
            var sentence = string.Join(" ", words);
            if(stepKind == StepKind.Given)
            {
                return sentence;
            }
            return "I " + (method.Parent as ClassDeclarationSyntax).Identifier.Text.ToLower() + " " + sentence;
        }

        private static IEnumerable<string> GetWords(string text)
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

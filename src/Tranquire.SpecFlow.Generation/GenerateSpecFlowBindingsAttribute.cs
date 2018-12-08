using CodeGeneration.Roslyn;
using System;
using System.Diagnostics;
using Tranquire.SpecFlow.Generation.Generator;

namespace Tranquire.SpecFlow.Generation
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    [CodeGenerationAttribute(typeof(SpecFlowBindingGenerator))]
    [Conditional("CodeGeneration")]
    public sealed class GenerateSpecFlowBindingsAttribute : Attribute
    {
    }
}

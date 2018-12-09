using CodeGeneration.Roslyn;
using System;
using System.Diagnostics;
using Tranquire.SpecFlow.Generation.Generator;

namespace Tranquire.SpecFlow.Generation
{
    /// <summary>
    /// Generates SpecFlow step bindings from a static class containing static action factories
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    [CodeGenerationAttribute(typeof(SpecFlowBindingGenerator))]
    [Conditional("CodeGeneration")]    
    public sealed class GenerateSpecFlowBindingsAttribute : Attribute
    {
    }
}

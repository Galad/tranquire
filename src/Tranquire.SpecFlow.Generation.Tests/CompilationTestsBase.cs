using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace Tranquire.SpecFlow.Generation.Tests
{

    public static class CompilationTests
    {
        static CompilationTests()
        {
            // this "core assemblies hack" is from https://stackoverflow.com/a/47196516/4418060
            string coreAssemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
            string[] coreAssemblyNames = new[]
            {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
            "System.Runtime.dll"
        };
            IEnumerable<PortableExecutableReference> coreMetaReferences =
                coreAssemblyNames.Select(x => MetadataReference.CreateFromFile(Path.Combine(coreAssemblyPath, x)));
            Assembly[] otherAssemblies = new[]
            {
            typeof(CSharpCompilation).Assembly,
            typeof(CodeGenerationAttributeAttribute).Assembly,
            typeof(GenerateSpecFlowBindingsAttribute).Assembly,
            typeof(FactAttribute).Assembly,
            typeof(BindingAttribute).Assembly,
            typeof(IAction<>).Assembly
        };
            MetadataReferences = coreMetaReferences
                .Concat<MetadataReference>(otherAssemblies.Select(x => MetadataReference.CreateFromFile(x.Location)))
                .ToImmutableArray();
        }

        internal const string CrLf = "\r\n";
        internal const string Lf = "\n";
        internal const string DefaultFilePathPrefix = "Test";
        internal const string CSharpDefaultFileExt = "cs";
        internal const string TestProjectName = "TestProject";

        internal static readonly string NormalizedPreamble = NormalizeToLf(DocumentTransform.GeneratedByAToolPreamble + Lf);

        internal static readonly ImmutableArray<MetadataReference> MetadataReferences;

        public static async Task AssertGeneratedAsExpected(string source, string expected)
        {
            SyntaxTree generatedTree = await Generate(source);
            // normalize line endings to just LF
            string generatedText = NormalizeToLf(generatedTree.GetText().ToString());
            // and append preamble to the expected
            string expectedText = NormalizedPreamble + NormalizeToLf(expected).Trim();
            Assert.Equal(expectedText, generatedText);
        }

        public static async Task<Assembly> CompileSource(string source)
        {
            SyntaxTree tree = await Generate(source);
            SyntaxTree sourceTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(source));
            string assemblyName = "Generated_" + Guid.NewGuid().ToString();
            CSharpCompilation compilation = CSharpCompilation.Create(assemblyName, new[] { tree, sourceTree }, MetadataReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            Microsoft.CodeAnalysis.Emit.EmitResult result = compilation.Emit(assemblyName + ".dll");
            AssemblyName assemblyName2 = new AssemblyName(assemblyName);
            return Assembly.Load(assemblyName2);
        }

        public static string NormalizeToLf(string input)
        {
            return input?.Replace(CrLf, Lf);
        }

        public static async Task<SyntaxTree> Generate(string source)
        {
            Document document = CreateProject(source).Documents.Single();
            SyntaxTree tree = await document.GetSyntaxTreeAsync();
            CSharpCompilation compilation = (CSharpCompilation)(await document.Project.GetCompilationAsync());
            ImmutableArray<Diagnostic> diagnostics = compilation.GetDiagnostics();
            Assert.Empty(diagnostics.Where(x => x.Severity > DiagnosticSeverity.Warning));
            Progress<Diagnostic> progress = new Progress<Diagnostic>();
            SyntaxTree result = await DocumentTransform.TransformAsync(compilation, tree, null, Assembly.Load, progress);
            return result;
        }

        public static Project CreateProject(params string[] sources)
        {
            ProjectId projectId = ProjectId.CreateNewId(debugName: TestProjectName);
            Solution solution = new AdhocWorkspace()
                .CurrentSolution
                .AddProject(projectId, TestProjectName, TestProjectName, LanguageNames.CSharp)
                .WithProjectCompilationOptions(
                    projectId,
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .WithProjectParseOptions(
                    projectId,
                    new CSharpParseOptions(preprocessorSymbols: new[] { "SOMETHING_ACTIVE" }))
                .AddMetadataReferences(projectId, MetadataReferences);

            int count = 0;
            foreach (string source in sources)
            {
                string newFileName = DefaultFilePathPrefix + count + "." + CSharpDefaultFileExt;
                DocumentId documentId = DocumentId.CreateNewId(projectId, debugName: newFileName);
                solution = solution.AddDocument(documentId, newFileName, SourceText.From(source));
                count++;
            }
            return solution.GetProject(projectId);
        }
    }
}

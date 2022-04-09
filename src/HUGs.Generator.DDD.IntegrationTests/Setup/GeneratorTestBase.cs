using CheckTestOutput;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using HUGs.Generator.DDD.IntegrationTests.Setup.Mocks;

namespace HUGs.Generator.DDD.IntegrationTests.Setup
{
    public abstract class GeneratorTestBase
    {
        protected readonly OutputChecker Check = new("../TestResults");
        protected Compilation EmptyInputCompilation;

        public virtual void Setup()
        {
            EmptyInputCompilation = CreateCompilation(@"class Program { static void Main() {} }");
        }

        protected Compilation CreateCompilation(string source)
        {
            return CSharpCompilation.Create(
                "compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }

        protected static void RunGenerator(
            GeneratorDriver driver,
            Compilation inputCompilation,
            out ImmutableArray<Diagnostic> diagnostics, 
            out ImmutableArray<string> generatedFileTexts)
        {
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out diagnostics);
            
            var result = driver.GetRunResult();
            generatedFileTexts = result.GeneratedTrees.Select(t => t.GetText().ToString()).ToImmutableArray();
        }

        protected GeneratorDriver SetupGeneratorDriver(string schema, params string[] configurations)
            => SetupGeneratorDriver(new[] { schema }, configurations);
        
        protected GeneratorDriver SetupGeneratorDriver(IEnumerable<string> schemas, params string[] configurations)
        {
            var generator = new Generator();
            var additionalFiles = schemas
                .Select((s, i) => new TestAdditionalText(text: s, path: $"dummy{i}.dddschema"))
                .Concat(configurations
                    .Select((config, i) => new TestAdditionalText(text: config, path: $"dummy{i}.dddconfig")));

            return CSharpGeneratorDriver.Create(new List<ISourceGenerator> { generator }, additionalFiles);
        }
    }
}
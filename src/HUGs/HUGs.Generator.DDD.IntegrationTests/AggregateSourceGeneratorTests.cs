using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.Tests.Tools.Mocks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HUGs.Generator.DDD.IntegrationTests
{
    public class AggregateSourceGeneratorTests
    {
        private Compilation emptyInputCompilation;
        private readonly OutputChecker check = new("TestResults");

        [SetUp]
        public void Setup()
        {
            emptyInputCompilation = CreateCompilation(@"class Program { static void Main() {} }");
        }

        [Test]
        [TestCase("SimpleAggregate")]
        [TestCase("OrderAggregate")]
        public void ValidSimpleAggregateSchema_GeneratorRun_GeneratesCorrectAggregate(string fileName)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/Aggregates/{fileName}.dddschema");
            var driver = SetupGeneratorDriver(schema);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(1);
            check.CheckString(generatedFileTexts.First(), checkName: fileName, fileExtension: "cs");
        }

        private static GeneratorDriver SetupGeneratorDriver(string schema)
            => SetupGeneratorDriver(new List<string> { schema });

        private static GeneratorDriver SetupGeneratorDriver(IEnumerable<string> schemas)
        {
            var generator = new Generator();

            return CSharpGeneratorDriver.Create(
                new List<ISourceGenerator> { generator },
                schemas.Select(s => new TestAdditionalText(text: s, path: "dummyPath.dddschema")));
        }

        private static Compilation CreateCompilation(string source)
        {
            return CSharpCompilation.Create(
                "compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}

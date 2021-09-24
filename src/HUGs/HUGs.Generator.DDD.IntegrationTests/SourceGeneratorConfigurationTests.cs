using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.Ddd.Diagnostics;
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
    public class SourceGeneratorConfigurationTests
    {
        private Compilation emptyInputCompilation;
        private readonly OutputChecker check = new("TestResults");

        [SetUp]
        public void Setup()
        {
            emptyInputCompilation = CreateCompilation(@"class Program { static void Main() {} }");
        }

        [Test]
        [TestCase("SimpleValueObject.dddschema", "ValueObjectNamespaceConfig.dddconfig")]
        [TestCase("SimpleValueObject.dddschema", "CompleteNamespaceConfig.dddconfig")]
        public void TargetNamespaceConfiguration_GeneratedInTheNamespace(string schemaFile, string configFile)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/ValueObjects/{schemaFile}");
            var configuration = File.ReadAllText($"../../../TestData/Configuration/{configFile}");
            var driver = SetupGeneratorDriver(schema, configuration);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(1);
            check.CheckString(generatedFileTexts.First(), checkName: $"{schemaFile}_{configFile}", fileExtension: "cs");
        }

        [Test]
        public void VariousSchemas_ConfigurationTargetNamespaceIsSetForAll_ClassesAreGeneratedInDesiredNamespace()
        {
            var valueObjectSchema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var aggregateSchema = File.ReadAllText("../../../TestData/Schemas/Aggregates/SimpleAggregate.dddschema");
            var configuration = File.ReadAllText("../../../TestData/Configuration/CompleteNamespaceConfig.dddconfig");
            var driver = SetupGeneratorDriver(new[] { valueObjectSchema, aggregateSchema }, configuration);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(2);
            check.CheckString(generatedFileTexts.First(), checkName: "First", fileExtension: "cs");
            check.CheckString(generatedFileTexts.Last(), checkName: "Second", fileExtension: "cs");
        }

        [Test]
        public void MultipleConfigurationFiles_DiagnosticErrorReportedAndNothingGenerated()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var configuration1 = File.ReadAllText("../../../TestData/Configuration/ValueObjectNamespaceConfig.dddconfig");
            var configuration2 = File.ReadAllText("../../../TestData/Configuration/ValueObjectNamespaceConfig.dddconfig");
            var driver = SetupGeneratorDriver(new[] { schema }, configuration1, configuration2);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            diagnostics.Should().HaveCount(1);
            diagnostics.Where(d => d.Id == DddDiagnostics.MultipleConfigurationsErrorId).Should().HaveCount(1);
            generatedFileTexts.Should().BeEmpty();
        }

        [Test]
        [TestCase("SyntaxInvalidConfig1.dddconfig")]
        [TestCase("SyntaxInvalidConfig2.dddconfig")]
        public void InvalidConfigurationFile_DiagnosticErrorReportedAndNothingGenerated(string configFile)
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var configuration = File.ReadAllText($"../../../TestData/Configuration/{configFile}");
            var driver = SetupGeneratorDriver(new[] { schema }, configuration);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            diagnostics.Should().HaveCount(1);
            diagnostics.Where(d => d.Id == DddDiagnostics.AdditionalFileParseErrorId).Should().HaveCount(1);
            generatedFileTexts.Should().BeEmpty();
        }

        private static GeneratorDriver SetupGeneratorDriver(string schema, string configuration)
            => SetupGeneratorDriver(new List<string> { schema }, configuration);

        private static GeneratorDriver SetupGeneratorDriver(IEnumerable<string> schemas, params string[] configurations)
        {
            var generator = new Generator();
            var additionalFiles = schemas
                .Select(s => new TestAdditionalText(text: s, path: "dummy.dddschema"))
                .Concat(configurations
                    .Select(config => new TestAdditionalText(text: config, path: "dummy.dddconfig")));

            return CSharpGeneratorDriver.Create(new List<ISourceGenerator> { generator }, additionalFiles);
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
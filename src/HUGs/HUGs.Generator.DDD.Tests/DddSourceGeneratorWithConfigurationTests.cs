using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.Tests.Mocks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HUGs.Generator.DDD.Tests
{
    public class DddSourceGeneratorWithConfigurationTests
    {
        private Compilation emptyInputCompilation;
        private readonly OutputChecker check = new("TestResults/Configuration");

        [SetUp]
        public void Setup()
        {
            emptyInputCompilation = CreateCompilation(@"class Program { static void Main() {} }");
        }

        [Test]
        public void GivenSimpleValueObjectSchema_WhenConfigurationTargetNamespaceIsSet_ThenClassIsGeneratedInDesiredNamespace()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var configuration = File.ReadAllText("../../../TestData/Configuration/SimpleValueObjectNamespaceConfig.dddconfig");
            var driver = SetupGeneratorDriver(schema, configuration);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            generatedFileTexts.Should().HaveCount(1);
            check.CheckString(generatedFileTexts.First(), fileExtension: "cs");
        }

        [Test]
        public void GivenSimpleValueObjectSchema_WhenConfigurationTargetNamespaceIsSetForAll_ThenClassIsGeneratedInDesiredNamespace()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var configuration = File.ReadAllText("../../../TestData/Configuration/CompleteNamespaceConfig.dddconfig");
            var driver = SetupGeneratorDriver(schema, configuration);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            generatedFileTexts.Should().HaveCount(1);
            check.CheckString(generatedFileTexts.First(), fileExtension: "cs");
        }

        [Test]
        public void GivenSimpleValueObjectAndAggregateSchemas_WhenConfigurationTargetNamespaceIsSetForAll_ThenClassesAreGeneratedInDesiredNamespace()
        {
            var valueObjectSchema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var aggregateSchema = File.ReadAllText("../../../TestData/Schemas/Aggregates/SimpleAggregate.dddschema");
            var configuration = File.ReadAllText("../../../TestData/Configuration/CompleteNamespaceConfig.dddconfig");
            var driver = SetupGeneratorDriver(new[] { valueObjectSchema, aggregateSchema}, configuration);

            driver.RunGeneratorsAndUpdateCompilation(emptyInputCompilation, out var outputCompilation, out var diagnostics);

            var generatedTrees = outputCompilation.SyntaxTrees.Where(x => !emptyInputCompilation.SyntaxTrees.Any(y => y.Equals(x))).ToImmutableArray();
            var generatedFileTexts = generatedTrees.Select(x => x.GetText().ToString()).ToImmutableArray();

            generatedFileTexts.Should().HaveCount(1);
            check.CheckString(generatedFileTexts.First(), fileExtension: "cs");
        }

        private static GeneratorDriver SetupGeneratorDriver(string schema, string configuration)
            => SetupGeneratorDriver(new List<string> { schema }, configuration);

        private static GeneratorDriver SetupGeneratorDriver(IEnumerable<string> schemas, string configuration)
        {
            var generator = new Generator();
            var additionalFiles = schemas
                .Select(s => new TestAdditionalText(text: s, path: "dummy.dddschema"))
                .Append(new TestAdditionalText(text: configuration, path: "dummy.dddconfig"));
            
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
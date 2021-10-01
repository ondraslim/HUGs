using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using NUnit.Framework;
using System.IO;
using System.Linq;
using HUGs.Generator.Common.Diagnostics;

namespace HUGs.Generator.DDD.IntegrationTests
{
    public class SourceGeneratorConfigurationTests : GeneratorTestBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [Test]
        [TestCase("SimpleValueObject", "ValueObjectNamespaceConfig")]
        [TestCase("SimpleValueObject", "CompleteNamespaceConfig")]
        public void TargetNamespaceConfiguration_GeneratedInTheNamespace(string schemaFile, string configFile)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/ValueObjects/{schemaFile}.dddschema");
            var configuration = File.ReadAllText($"../../../TestData/Configuration/{configFile}.dddconfig");
            var driver = SetupGeneratorDriver(schema, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(1);

            Check.CheckString(generatedFileTexts.First(), checkName: $"{schemaFile}_{configFile}", fileExtension: "cs");
        }

        [Test]
        public void VariousSchemas_ConfigurationTargetNamespaceIsSetForAll_ClassesAreGeneratedInDesiredNamespace()
        {
            var valueObjectSchema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var aggregateSchema = File.ReadAllText("../../../TestData/Schemas/Aggregates/SimpleAggregate.dddschema");
            var configuration = File.ReadAllText("../../../TestData/Configuration/CompleteNamespaceConfig.dddconfig");
            var driver = SetupGeneratorDriver(new[] { valueObjectSchema, aggregateSchema }, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(2);

            Check.CheckString(generatedFileTexts.First(), checkName: "First", fileExtension: "cs");
            Check.CheckString(generatedFileTexts.Last(), checkName: "Second", fileExtension: "cs");
        }

        [Test]
        [TestCase("ValueObjects/SimpleValueObject.dddschema", "AdditionalUsingsConfig")]
        [TestCase("Aggregates/SimpleAggregate.dddschema", "AdditionalUsingsConfig")]
        [TestCase("Entities/SimpleEntity.dddschema", "AdditionalUsingsConfig")]
        [TestCase("Enumerations/SimpleEnumeration.dddschema", "AdditionalUsingsConfig")]
        public void GivenAdditionalUsings_GeneratedClassesUseTheUsings(string schemaFilePath, string configurationFile)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/{schemaFilePath}");
            var configuration1 = File.ReadAllText($"../../../TestData/Configuration/{configurationFile}.dddconfig");
            var driver = SetupGeneratorDriver(new[] { schema }, configuration1);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(1);

            var checkName = schemaFilePath.SkipWhile(c => c != '/').Skip(1).TakeWhile(c => c != '.').ToArray();
            Check.CheckString(generatedFileTexts.First(), checkName: new string(checkName), fileExtension: "cs");
        }

        [Test]
        public void CompleteConfiguration_GeneratedClassesWorkProperly()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var schema2 = File.ReadAllText("../../../TestData/Schemas/Aggregates/SimpleAggregate.dddschema");
            var schema3 = File.ReadAllText("../../../TestData/Schemas/Entities/SimpleEntity.dddschema");
            var schema4 = File.ReadAllText("../../../TestData/Schemas/Enumerations/SimpleEnumeration.dddschema");
            var configuration = File.ReadAllText("../../../TestData/Configuration/CompleteConfig.dddconfig");

            var driver = SetupGeneratorDriver(new[] { schema, schema2, schema3, schema4 }, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(4);

            for (var index = 0; index < generatedFileTexts.Length; index++)
            {
                var generatedText = generatedFileTexts[index];
                Check.CheckString(generatedText, checkName: index.ToString(), fileExtension: "cs");
            }
        }

        [Test]
        public void MultipleConfigurationFiles_DiagnosticErrorReportedAndNothingGenerated()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var configuration1 = File.ReadAllText("../../../TestData/Configuration/ValueObjectNamespaceConfig.dddconfig");
            var configuration2 = File.ReadAllText("../../../TestData/Configuration/ValueObjectNamespaceConfig.dddconfig");
            var driver = SetupGeneratorDriver(new[] { schema }, configuration1, configuration2);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().HaveCount(1);
            diagnostics.Where(d => d.Id == DddDiagnostics.ConfigurationMultipleErrorId).Should().HaveCount(1);
            generatedFileTexts.Should().BeEmpty();
        }

        [Test]
        [TestCase("SyntaxInvalidConfig1")]
        [TestCase("SyntaxInvalidConfig2")]
        public void InvalidConfigurationFile_DiagnosticErrorReportedAndNothingGenerated(string configFile)
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var configuration = File.ReadAllText($"../../../TestData/Configuration/{configFile}.dddconfig");
            var driver = SetupGeneratorDriver(new[] { schema }, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().HaveCount(1);
            diagnostics.Where(d => d.Id == Diagnostics.AdditionalFileParseErrorId).Should().HaveCount(1);
            generatedFileTexts.Should().BeEmpty();
        }
    }
}
using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using HUGs.Generator.Test.Utils;
using NUnit.Framework;
using System.IO;
using System.Linq;

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
        [TestCase("SimpleValueObject2", "CompleteNamespaceConfig")]
        public void NamespaceConfiguration_GeneratedInTheNamespace(string schemaFile, string configFile)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/ValueObjects/{schemaFile}.dddschema");
            var configuration = File.ReadAllText($"../../../TestData/Configuration/{configFile}.dddconfig");
            var driver = SetupGeneratorDriver(schema, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(3);

            foreach (var generatedFile in generatedFileTexts.Where(f => f.Contains("class ")))
            {
                var checkName = TestHelper.GetGeneratedFileClass(generatedFile);
                Check.CheckString(generatedFile, checkName: checkName, fileExtension: "cs");
            }
        }

        [Test]
        public void VariousSchemas_ConfigurationTargetNamespaceSetForAll_ClassesGeneratedInDesiredNamespace()
        {
            var valueObjectSchema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var aggregateSchema = File.ReadAllText("../../../TestData/Schemas/Aggregates/SimpleAggregate.dddschema");
            var configuration = File.ReadAllText("../../../TestData/Configuration/CompleteNamespaceConfig.dddconfig");
            var driver = SetupGeneratorDriver(new[] { valueObjectSchema, aggregateSchema }, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(5);

            foreach (var generatedFile in generatedFileTexts.Where(f => f.Contains("class ")))
            {
                var checkName = TestHelper.GetGeneratedFileClass(generatedFile);
                Check.CheckString(generatedFile, checkName: checkName, fileExtension: "cs");
            }
        }

        [Test]
        [TestCase("ValueObjects/SimpleValueObject.dddschema", "AdditionalUsingsConfig")]
        [TestCase("Aggregates/SimpleAggregate.dddschema", "AdditionalUsingsConfig")]
        [TestCase("Entities/SimpleEntity.dddschema", "AdditionalUsingsConfig")]
        [TestCase("Enumerations/SimpleEnumeration.dddschema", "AdditionalUsingsConfig", false)]
        public void AdditionalUsings_GeneratedClassesUseTheUsings(string schemaFilePath, string configurationFile, bool dbEntityExpectedToBeGenerated = true)
        {
            var schema = File.ReadAllText($"../../../TestData/Schemas/{schemaFilePath}");
            var configuration1 = File.ReadAllText($"../../../TestData/Configuration/{configurationFile}.dddconfig");
            var driver = SetupGeneratorDriver(new[] { schema }, configuration1);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            var expectedGeneratedFiles = 2;
            if (dbEntityExpectedToBeGenerated)
            {
                expectedGeneratedFiles += 1;
            }

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(expectedGeneratedFiles);

            foreach (var generatedFile in generatedFileTexts.Where(f => f.Contains("class ")))
            {
                var checkName = TestHelper.GetGeneratedFileClass(generatedFile);
                Check.CheckString(generatedFile, checkName: checkName, fileExtension: "cs");
            }
        }

        [Test]
        public void CompleteConfiguration_GeneratedCorrectly()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var schema2 = File.ReadAllText("../../../TestData/Schemas/Aggregates/SimpleAggregate.dddschema");
            var schema3 = File.ReadAllText("../../../TestData/Schemas/Entities/SimpleEntity.dddschema");
            var schema4 = File.ReadAllText("../../../TestData/Schemas/Enumerations/SimpleEnumeration.dddschema");
            var configuration = File.ReadAllText("../../../TestData/Configuration/CompleteConfig.dddconfig");

            var driver = SetupGeneratorDriver(new[] { schema, schema2, schema3, schema4 }, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(8);

            foreach (var generatedFile in generatedFileTexts.Where(f => f.Contains("class ")))
            {
                var checkName = TestHelper.GetGeneratedFileClass(generatedFile);
                Check.CheckString(generatedFile, checkName: checkName, fileExtension: "cs");
            }
        }

        [Test]
        public void MultipleConfigurationFiles_DiagnosticErrorReported()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var configuration1 = File.ReadAllText("../../../TestData/Configuration/ValueObjectNamespaceConfig.dddconfig");
            var configuration2 = File.ReadAllText("../../../TestData/Configuration/ValueObjectNamespaceConfig.dddconfig");
            var driver = SetupGeneratorDriver(new[] { schema }, configuration1, configuration2);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().HaveCount(1);
            diagnostics.Where(d => d.Id is DddDiagnostics.ConfigurationMultipleErrorId).Should().HaveCount(1);
            generatedFileTexts.Should().BeEmpty();
        }

        [Test]
        [TestCase("SyntaxInvalidConfig1")]
        [TestCase("SyntaxInvalidConfig2")]
        public void InvalidConfigurationFile_DiagnosticErrorReported(string configFile)
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/SimpleValueObject.dddschema");
            var configuration = File.ReadAllText($"../../../TestData/Configuration/{configFile}.dddconfig");
            var driver = SetupGeneratorDriver(new[] { schema }, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().HaveCount(1);
            diagnostics.Where(d => d.Id is Diagnostics.AdditionalFileParseErrorId).Should().HaveCount(1);
            generatedFileTexts.Should().BeEmpty();
        }
    }
}
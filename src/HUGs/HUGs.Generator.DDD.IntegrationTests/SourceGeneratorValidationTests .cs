using FluentAssertions;
using HUGs.Generator.Common.Diagnostics;
using HUGs.Generator.DDD.Ddd.Configuration;
using HUGs.Generator.DDD.Ddd.Diagnostics;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Text;

namespace HUGs.Generator.DDD.IntegrationTests
{
    public class SourceGeneratorValidationTests : GeneratorTestBase
    {
        private static readonly string[] Kinds = { "Entity", "Aggregate", "ValueObject", "Enumeration" };


        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }


        [Test]
        public void EmptySchemaFile_WarningReported()
        {
            var schema = File.ReadAllText("../../../TestData/Schemas/Empty.dddschema");
            var driver = SetupGeneratorDriver(schema);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().HaveCount(1);
            diagnostics.Where(d => d.Id == Diagnostics.AdditionalFileEmptyWarningId).Should().HaveCount(1);
            generatedFileTexts.Should().BeEmpty();
        }

        [Test]
        public void PropertyWhitelistedType_NoDiagnosticReported()
        {
            foreach (var kind in Kinds)
            {
                foreach (var whitelistedType in Constants.WhitelistedTypes)
                {
                    var schema = GetSchema(kind, "TestClassName", true, "TestPropertyName", whitelistedType);
                    var driver = SetupGeneratorDriver(schema);

                    RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

                    diagnostics.Should().BeEmpty();
                    generatedFileTexts.Should().HaveCount(kind == "Enumeration" ? 1 : 2);
                }
            }
        }

        [Test]
        public void PropertyDddSchemaType_NoDiagnosticReported()
        {
            const string classOneName = "ClassOne";
            const string classTwoName = "ClassTwo";

            foreach (var kind in Kinds)
            {
                foreach (var kindOther in Kinds)
                {
                    var schema1 = GetSchema(kind, classOneName);
                    var schema2 = GetSchema(kindOther, classTwoName, true, "PropertyDddReference", classOneName);

                    var driver = SetupGeneratorDriver(new[] { schema1, schema2 });

                    RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

                    diagnostics.Should().BeEmpty();

                    var expectedGeneratedFileCount = 4;
                    if (kind == "Enumeration") expectedGeneratedFileCount--;
                    if (kindOther == "Enumeration") expectedGeneratedFileCount--;
                    generatedFileTexts.Should().HaveCount(expectedGeneratedFileCount);
                }
            }
        }

        [Test]
        [TestCase("Entity", "")]
        [TestCase("Aggregate", "")]
        [TestCase("Enumeration", "")]
        [TestCase("ValueObject", "")]
        [TestCase("Entity", "-InvalidName")]
        [TestCase("Aggregate", "-InvalidName")]
        [TestCase("Enumeration", "-InvalidName")]
        [TestCase("ValueObject", "-InvalidName")]
        [TestCase("Entity", "ValidName", true, "", "string")]
        [TestCase("Aggregate", "ValidName", true, "", "string")]
        [TestCase("Enumeration", "ValidName", true, "", "string")]
        [TestCase("ValueObject", "ValidName", true, "", "string")]
        [TestCase("Entity", "ValidName", true, "/InvalidName", "string")]
        [TestCase("Aggregate", "ValidName", true, "/InvalidName", "string")]
        [TestCase("Enumeration", "ValidName", true, "/InvalidName", "string")]
        [TestCase("ValueObject", "ValidName", true, "/InvalidName", "string")]
        [TestCase("Entity", "ValidName", true, "ValidPropertyName", "")]
        [TestCase("Aggregate", "ValidName", true, "ValidPropertyName", "")]
        [TestCase("Enumeration", "ValidName", true, "ValidPropertyName", "")]
        [TestCase("ValueObject", "ValidName", true, "ValidPropertyName", "")]
        [TestCase("Entity", "ValidName", true, "ValidPropertyName", "/InvalidType")]
        [TestCase("ValueObject", "ValidName", true, "ValidName", "UnknownType")]
        [TestCase("Entity", "ValidName", true, "ValidName", "UnknownType")]
        [TestCase("Aggregate", "ValidName", true, "ValidName", "UnknownType")]
        [TestCase("Enumeration", "ValidName", true, "ValidName", "UnknownType")]
        [TestCase("Aggregate", "ValidName", true, "ValidPropertyName", "/InvalidType")]
        [TestCase("Enumeration", "ValidName", true, "ValidPropertyName", "/InvalidType")]
        [TestCase("ValueObject", "ValidName", true, "ValidPropertyName", "/InvalidType")]
        [TestCase("Enumeration", "ValidName", true, "ValidPropertyName", "string", true, "", "ValidPropertyName", "ValidPropertyValue")]
        [TestCase("Enumeration", "ValidName", true, "ValidPropertyName", "string", true, "+InvalidValueName", "ValidPropertyName", "ValidPropertyValue")]
        [TestCase("Enumeration", "ValidName", true, "ValidPropertyName", "string", true, "ValidName", "+InvalidPropertyName", "ValidPropertyValue")]
        public void InvalidSchema_DiagnosticReported(
            string kind, string name,
            bool useProperties = false, string propertyName = null, string propertyType = null,
            bool useValues = false, string valueName = null, string valuePropertyName = null, string valuePropertyValue = null)
        {
            var schema = GetSchema(kind, name, useProperties, propertyName, propertyType, useValues, valueName, valuePropertyName, valuePropertyValue);
            var driver = SetupGeneratorDriver(schema);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            generatedFileTexts.Should().BeEmpty();
            diagnostics.Where(d => d.Id == DddDiagnostics.SchemaInvalidValueErrorId).Should().HaveCount(1);
            diagnostics.Where(d => d.Id == DddDiagnostics.SchemaInvalidErrorId).Should().HaveCount(1);
            diagnostics.Should().HaveCount(2);
        }

        [Test]
        [TestCase(1, true, ".")]
        [TestCase(3, true, ".", ".&.", ".Nop.")]
        [TestCase(1, true, "ValidValueObjectNs", "\\{")]
        [TestCase(1, true, "ValidValueObjectNs", "ValidEntityNs", "/")]
        [TestCase(1, true, "ValidValueObjectNs", "ValidEntityNs", "ValidAggregateNs", "+InvalidEnumNs")]
        [TestCase(1, true, "ValidValueObjectNs", "ValidEntityNs", "ValidAggregateNs", "ValidEnumNs", true, new[] { "$" })]
        [TestCase(1, true, "ValidValueObjectNs", "ValidEntityNs", "ValidAggregateNs", "ValidEnumNs", true, new[] { "OneValid", "Another..Invalid" })]
        [TestCase(1, true, "ValidValueObjectNs", "ValidEntityNs", "ValidAggregateNs", "ValidEnumNs", true, new[] { "OneValid", "/" })]
        [TestCase(1, false, "", "", "", "", true, new[] { "?Invalid" })]
        [TestCase(1, false, "", "", "", "", true, new[] { "With@Invalid@Separators" })]
        [TestCase(1, false, "", "", "", "", true, new[] { "With&Invalid&Separators" })]
        public void InvalidConfiguration_DiagnosticReported(int expectedValueErrorCount,
            bool useNamespaces, string valueObjectNamespace = null, string entityNamespace = null, string aggregateNamespace = null, string enumNamespace = null,
            bool useUsings = false, string[] additionalUsings = null)
        {
            var schema = GetSchema("ValueObject", "EmptyTestValueObject");
            var configuration = GetConfiguration(useNamespaces, valueObjectNamespace, entityNamespace,
                aggregateNamespace, enumNamespace, useUsings, additionalUsings);

            var driver = SetupGeneratorDriver(schema, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            generatedFileTexts.Should().BeEmpty();
            diagnostics.Where(d => d.Id == DddDiagnostics.ConfigurationInvalidValueErrorId).Should().HaveCount(expectedValueErrorCount);
            diagnostics.Where(d => d.Id == DddDiagnostics.ConfigurationInvalidErrorId).Should().HaveCount(1);
            diagnostics.Should().HaveCount(expectedValueErrorCount + 1);
        }

        [Test]
        [TestCase(1, "Entity|TestDuplicateName|2")]
        [TestCase(1, "Aggregate|TestDuplicateName|2")]
        [TestCase(1, "Enumeration|TestDuplicateName|2")]
        [TestCase(1, "ValueObject|TestDuplicateName|2")]
        [TestCase(1, "ValueObject|TestDuplicateName|1", "Entity|TestDuplicateName|1")]
        [TestCase(1, "ValueObject|TestDuplicateName|2", "Entity|TestDuplicateName|2")]
        [TestCase(1, "ValueObject|TestDuplicateName|2", "Entity|TestDuplicateName|1", "Aggregate|TestDuplicateName|1", "Enumeration|TestDuplicateName|1")]
        [TestCase(2, "ValueObject|TestDuplicateValueObject|2", "Entity|TestEntity|1", "Aggregate|TestAggregate|1", "Enumeration|TestDuplicateEnum|2")]
        public void ModelWithDuplicatedNames_DiagnosticReported(int expectedDuplicateCount, params string[] duplicates)
        {
            var schemas = duplicates
                .Select(ds => ds.Split('|'))
                .Select(d => new { Kind = d[0], Name = d[1], Count = int.Parse(d[2]) })
                .Select(d => Enumerable.Repeat(GetSchema(d.Kind, d.Name), d.Count))
                .SelectMany(a => a)
                .ToArray();

            var driver = SetupGeneratorDriver(schemas);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            generatedFileTexts.Should().BeEmpty();
            diagnostics.Where(d => d.Id == DddDiagnostics.DddModelDuplicatedNamesErrorId).Should().HaveCount(expectedDuplicateCount);
            diagnostics.Where(d => d.Id == DddDiagnostics.DddModelInvalidErrorId).Should().HaveCount(1);
            diagnostics.Should().HaveCount(expectedDuplicateCount + 1);
        }

        private static string GetSchema(
            string kind, string name,
            bool useProperties = false, string propertyName = null, string propertyType = null,
            bool useValues = false, string valueName = null, string valuePropertyName = null, string valuePropertyValue = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Kind: {kind}");
            sb.AppendLine($"Name: {name}");

            if (useProperties)
            {
                sb.AppendLine("Properties:");
                sb.AppendLine($"  - Name: {propertyName}");
                sb.AppendLine($"    Type: {propertyType}");
            }

            if (useValues)
            {
                sb.AppendLine("Values:");
                sb.AppendLine($"  - Name: {valueName}");
                sb.AppendLine("    Properties:");
                sb.AppendLine($"       {valuePropertyName}: {valuePropertyValue}");
            }

            return sb.ToString();
        }

        private static string GetConfiguration(
            bool useNamespace = false,
            string valueObjectNamespace = null, string entityNamespace = null, string aggregateNamespace = null, string enumNamespace = null,
            bool useUsings = false, string[] additionalUsings = null)
        {
            var sb = new StringBuilder();
            if (useNamespace)
            {
                sb.AppendLine("TargetNamespaces:");
                if (valueObjectNamespace is not null) sb.AppendLine($"  ValueObject: {valueObjectNamespace}");
                if (entityNamespace is not null) sb.AppendLine($"  Entity: {entityNamespace}");
                if (aggregateNamespace is not null) sb.AppendLine($"  Aggregate: {aggregateNamespace}");
                if (enumNamespace is not null) sb.AppendLine($"  Enumeration: {enumNamespace}");
            }

            if (useUsings)
            {
                sb.AppendLine("AdditionalUsings:");
                if (additionalUsings != null)
                {
                    foreach (var additionalUsing in additionalUsings)
                    {
                        sb.AppendLine($"  - {additionalUsing}");
                    }
                }
            }
            return sb.ToString();
        }
    }
}
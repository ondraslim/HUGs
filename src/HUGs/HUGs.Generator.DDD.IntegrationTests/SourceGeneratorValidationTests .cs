using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using FluentAssertions.Execution;
using HUGs.Generator.DDD.Ddd.Diagnostics;

namespace HUGs.Generator.DDD.IntegrationTests
{
    public class SourceGeneratorValidationTests : GeneratorTestBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
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
        [TestCase("Aggregate", "ValidName", true, "ValidPropertyName", "/InvalidType")]
        [TestCase("Enumeration", "ValidName", true, "ValidPropertyName", "/InvalidType")]
        [TestCase("ValueObject", "ValidName", true, "ValidPropertyName", "/InvalidType")]
        [TestCase("Enumeration", "ValidName", true, "ValidPropertyName", "string", true, "", "ValidPropertyName", "ValidPropertyValue")]
        [TestCase("Enumeration", "ValidName", true, "ValidPropertyName", "string", true, "+InvalidValueName", "ValidPropertyName", "ValidPropertyValue")]
        [TestCase("Enumeration", "ValidName", true, "ValidPropertyName", "string", true, "ValidName", "+InvalidPropertyName", "ValidPropertyValue")]
        public void GivenInvalidSchema_DiagnosticIsReported(
            string kind, string name,
            bool useProperties = false, string propertyName = null, string propertyType = null,
            bool useValues = false, string valueName = null, string valuePropertyName = null, string valuePropertyValue = null)
        {
            var schema = GetInvalidSchema(kind, name, useProperties, propertyName, propertyType, useValues, valueName, valuePropertyName, valuePropertyValue);
            var driver = SetupGeneratorDriver(schema);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);
            
            generatedFileTexts.Should().BeEmpty();
            diagnostics.Should().HaveCount(2);
            diagnostics
                .Where(d => d.Id == DddDiagnostic.SchemaInvalidValueErrorId)
                .Should().HaveCount(1);

            diagnostics
                .Where(d => d.Id == DddDiagnostic.SchemaInvalidErrorId)
                .Should().HaveCount(1);
        }

        private static string GetInvalidSchema(
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
                sb.AppendLine($"    Properties:");
                sb.AppendLine($"       {valuePropertyName}: {valuePropertyValue}");
            }

            return sb.ToString();
        }
    }
}
using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using HUGs.Generator.Test.Utils;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace HUGs.Generator.DDD.IntegrationTests
{
    public class DddSourceGeneratorTests : GeneratorTestBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [Test]
        public void ComplexDddModel_GeneratedCorrectly()
        {
            var orderSchema = File.ReadAllText("../../../TestData/Schemas/Aggregates/OrderAggregate.dddschema");
            var orderItemSchema = File.ReadAllText("../../../TestData/Schemas/Entities/OrderItemEntity.dddschema");
            var countrySchema = File.ReadAllText("../../../TestData/Schemas/Entities/CountryEntity.dddschema");
            var orderStateSchema = File.ReadAllText("../../../TestData/Schemas/Enumerations/OrderStateEnumeration.dddschema");
            var valueObjectSchema = File.ReadAllText("../../../TestData/Schemas/ValueObjects/AddressValueObject.dddschema");
            var configuration = File.ReadAllText("../../../TestData/Configuration/CompleteNamespaceConfig.dddconfig");

            var driver = SetupGeneratorDriver(new[] { orderSchema, orderItemSchema, countrySchema, orderStateSchema, valueObjectSchema }, configuration);

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(14);

            foreach (var generatedFile in generatedFileTexts)
            {
                var checkName = generatedFile.Contains("class ")
                    ? TestHelper.GetGeneratedFileClass(generatedFile)
                    : generatedFile.Trim().Split(" ").First();

                Check.CheckString(generatedFile, checkName: checkName, fileExtension: "cs");
            }
        }
    }
}
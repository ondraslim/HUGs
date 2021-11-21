using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using HUGs.Generator.Test.Utils;
using NUnit.Framework;
using System.IO;

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

            var driver = SetupGeneratorDriver(new[] { orderSchema, orderItemSchema, countrySchema, orderStateSchema, valueObjectSchema });

            RunGenerator(driver, EmptyInputCompilation, out var diagnostics, out var generatedFileTexts);

            diagnostics.Should().BeEmpty();
            generatedFileTexts.Should().HaveCount(13);

            foreach (var generatedFile in generatedFileTexts)
            {
                Check.CheckString(generatedFile, checkName: TestHelper.GetGeneratedFileClass(generatedFile), fileExtension: "cs");
            }
        }
    }
}
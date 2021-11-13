using System.IO;
using System.Linq;
using CheckTestOutput;
using FluentAssertions;
using HUGs.Generator.DDD.IntegrationTests.Setup;
using NUnit.Framework;

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
                var className = generatedFile.Split(' ').SkipWhile(t => t != "class").Skip(1).First().Trim();
                Check.CheckString(generatedFile, checkName: className, fileExtension: "cs");
            }
        }
    }
}
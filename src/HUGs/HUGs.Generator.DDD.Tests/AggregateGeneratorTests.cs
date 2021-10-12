using CheckTestOutput;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class AggregateGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        public void NoPropertySchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.Aggregate,
                Name = "SimpleAggregate",
                Properties = new DddObjectProperty[] { }
            };

            var actualCode = AggregateGenerator.GenerateAggregateCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void PropertyMultipleSchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.Aggregate,
                Name = "PropertiesAggregate",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = AggregateGenerator.GenerateAggregateCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void ArrayPropertySchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.Aggregate,
                Name = "SimpleArrayPropertyAggregate",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Items", Type = "OrderItem[]" },
                }
            };

            var actualCode = AggregateGenerator.GenerateAggregateCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void VariousPropertiesSchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.Aggregate,
                Name = "ArrayPropertyAggregate",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Items", Type = "OrderItem[]" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = AggregateGenerator.GenerateAggregateCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}
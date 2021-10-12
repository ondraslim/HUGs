using CheckTestOutput;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class EntityGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        public void NoPropertySchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.Entity,
                Name = "SimpleEntity",
                Properties = new DddObjectProperty[] { }
            };

            var actualCode = EntityGenerator.GenerateEntityCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void PropertyMultipleSchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.Entity,
                Name = "PropertiesEntity",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = EntityGenerator.GenerateEntityCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void ArrayPropertySchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.Entity,
                Name = "SimpleArrayPropertyEntity",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Items", Type = "OrderItem[]" },
                }
            };

            var actualCode = EntityGenerator.GenerateEntityCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void VariousPropertiesSchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.Entity,
                Name = "ArrayPropertyEntity",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Items", Type = "OrderItem[]" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = EntityGenerator.GenerateEntityCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}
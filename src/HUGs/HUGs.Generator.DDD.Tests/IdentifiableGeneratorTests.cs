using CheckTestOutput;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests
{
    public class IdentifiableGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        [TestCase(DddObjectKind.Entity)]
        [TestCase(DddObjectKind.Aggregate)]
        public void NoPropertySchema_GeneratedCorrectly(DddObjectKind identifiableKind)
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = identifiableKind,
                Name = $"Simple{identifiableKind}",
                Properties = new DddObjectProperty[] { }
            };

            var actualCode = objectSchema.Kind == DddObjectKind.Entity 
                ? IdentifiableGenerator.GenerateEntityCode(objectSchema, generatorConfiguration)
                : IdentifiableGenerator.GenerateAggregateCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, checkName: identifiableKind.ToString(), fileExtension: "cs");
        }

        [Test]
        [TestCase(DddObjectKind.Entity)]
        [TestCase(DddObjectKind.Aggregate)]
        public void PropertyMultipleSchema_GeneratedCorrectly(DddObjectKind identifiableKind)
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = identifiableKind,
                Name = $"Properties{identifiableKind}",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = objectSchema.Kind == DddObjectKind.Entity
                ? IdentifiableGenerator.GenerateEntityCode(objectSchema, generatorConfiguration)
                : IdentifiableGenerator.GenerateAggregateCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, checkName: identifiableKind.ToString(), fileExtension: "cs");
        }

        [Test]
        [TestCase(DddObjectKind.Entity)]
        [TestCase(DddObjectKind.Aggregate)]
        public void ArrayPropertySchema_GeneratedCorrectly(DddObjectKind identifiableKind)
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = identifiableKind,
                Name = $"SimpleArrayProperty{identifiableKind}",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Items", Type = "OrderItem[]" },
                }
            };

            var actualCode = objectSchema.Kind == DddObjectKind.Entity
                ? IdentifiableGenerator.GenerateEntityCode(objectSchema, generatorConfiguration)
                : IdentifiableGenerator.GenerateAggregateCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, checkName: identifiableKind.ToString(), fileExtension: "cs");
        }

        [Test]
        [TestCase(DddObjectKind.Entity)]
        [TestCase(DddObjectKind.Aggregate)]
        public void VariousPropertiesSchema_GeneratedCorrectly(DddObjectKind identifiableKind)
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = identifiableKind,
                Name = $"ArrayProperty{identifiableKind}",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Text", Type = "string" },
                    new() { Name = "Items", Type = "OrderItem[]" },
                    new() { Name = "Number", Optional = true, Type = "double" }
                }
            };

            var actualCode = objectSchema.Kind == DddObjectKind.Entity
                ? IdentifiableGenerator.GenerateEntityCode(objectSchema, generatorConfiguration)
                : IdentifiableGenerator.GenerateAggregateCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, checkName: identifiableKind.ToString(), fileExtension: "cs");
        }
    }
}
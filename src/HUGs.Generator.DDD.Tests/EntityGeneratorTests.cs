using CheckTestOutput;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Ddd.Models.DddTypes;
using HUGs.Generator.DDD.Generators;
using NUnit.Framework;
using System;

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
                Properties = Array.Empty<DddObjectProperty>()
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
                    new() { Name = "Text", Type = "string", ResolvedType = new DddPrimitiveType("string")},
                    new() { Name = "Number", Type = "double?", ResolvedType = new DddPrimitiveType("double?") }
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
                    new() { Name = "Items", Type = "OrderItem[]", ResolvedType = new DddCollectionType(new DomainModelType("OrderItem", DddObjectKind.ValueObject))},
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
                    new() { Name = "Text", Type = "string", ResolvedType = new DddPrimitiveType("string") },
                    new() { Name = "Items", Type = "OrderItem[]", ResolvedType = new DddCollectionType(new DomainModelType("OrderItem", DddObjectKind.ValueObject)) },
                    new() { Name = "Number", Type = "double?", ResolvedType = new DddPrimitiveType("double?") }
                }
            };

            var actualCode = EntityGenerator.GenerateEntityCode(objectSchema, generatorConfiguration);

            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}
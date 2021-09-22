﻿using CheckTestOutput;
using HUGs.Generator.DDD.Common;
using HUGs.Generator.DDD.Common.Configuration;
using HUGs.Generator.DDD.Ddd;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests.GeneratorTests
{
    public class IdentifiableGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults/Identifiable");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        [TestCase(DddObjectKind.Entity)]
        [TestCase(DddObjectKind.Aggregate)]
        public void GivenEmptyIdentifiableSchema_CorrectlyGeneratesIdentifiableClasses(DddObjectKind identifiableKind)
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
        public void GivenIdentifiableSchemaWithProperties_CorrectlyGeneratesIdentifiableClasses(DddObjectKind identifiableKind)
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
        public void GivenIdentifiableSchemaWithArrayProperty_CorrectlyGeneratesIdentifiableClasses(DddObjectKind identifiableKind)
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
        public void GivenIdentifiableSchemaWithVariousProperties_CorrectlyGeneratesIdentifiableClasses(DddObjectKind identifiableKind)
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
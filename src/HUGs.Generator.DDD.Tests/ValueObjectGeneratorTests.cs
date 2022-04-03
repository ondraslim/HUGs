using CheckTestOutput;
using HUGs.Generator.DDD.Ddd;
using HUGs.Generator.DDD.Ddd.Models;
using HUGs.Generator.DDD.Ddd.Models.Configuration;
using HUGs.Generator.DDD.Ddd.Models.DddTypes;
using NUnit.Framework;
using System;
using HUGs.Generator.DDD.Generators;

namespace HUGs.Generator.DDD.Tests
{
    public class ValueObjectGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        public void NoPropertySchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleClass1",
                Properties = Array.Empty<DddObjectProperty>()
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(objectSchema, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void SinglePropertySchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleOptionalPropertyClass",
                Properties = new DddObjectProperty[] { new()
                {
                    Name = "Number", Type = "int", ResolvedType = new DddPrimitiveType("int")
                } }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(objectSchema, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void ComputedPropertySchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleOptionalPropertyClass",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Number", Type = "int", ResolvedType = new DddPrimitiveType("int") },
                    new() { Name = "ComputedNumber", Computed = true, Type = "int", ResolvedType = new DddPrimitiveType("int") }
                }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(objectSchema, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void MultiplePropertiesSchema_GeneratedCorrectly()
        {
            var objectSchema = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "MultiplePropertyClass",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Number", Type = "int", ResolvedType = new DddPrimitiveType("int") },
                    new() { Name = "Number2", Type = "int?", ResolvedType = new DddPrimitiveType("int?") },
                    new() { Name = "Text", Type = "string", ResolvedType = new DddPrimitiveType("string") },
                }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(objectSchema, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}
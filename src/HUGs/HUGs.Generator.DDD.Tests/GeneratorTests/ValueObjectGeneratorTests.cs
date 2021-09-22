﻿using CheckTestOutput;
using HUGs.Generator.DDD.Common;
using HUGs.Generator.DDD.Common.Configuration;
using HUGs.Generator.DDD.Ddd;
using NUnit.Framework;

namespace HUGs.Generator.DDD.Tests.GeneratorTests
{
    public class ValueObjectGeneratorTests
    {
        private readonly OutputChecker check = new("TestResults/ValueObjects");
        private readonly DddGeneratorConfiguration generatorConfiguration = new();

        [Test]
        public void GivenEmptyValueObjectSchema_CorrectlyGeneratesValueObjectClass()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleClass1",
                Properties = new DddObjectProperty[] { }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void GivenValueObjectSchemaWithSingleProperty_CorrectlyGeneratesValueObjectClass()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleOptionalPropertyClass",
                Properties = new DddObjectProperty[] { new() { Name = "Number", Optional = false, Type = "int" } }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void GivenValueObjectSchemaWithComputedProperty_CorrectlyGeneratesValueObjectClass()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "SimpleOptionalPropertyClass",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Number", Optional = false, Type = "int" },
                    new() { Name = "ComputedNumber", Computed = true, Type = "int" }
                }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }

        [Test]
        public void GivenValueObjectSchemaWithMultipleProperties_CorrectlyGeneratesValueObjectClass()
        {
            var inputValueObject = new DddObjectSchema
            {
                Kind = DddObjectKind.ValueObject,
                Name = "MultiplePropertyClass",
                Properties = new DddObjectProperty[]
                {
                    new() { Name = "Number", Optional = false, Type = "int" },
                    new() { Name = "Number2", Optional = true, Type = "int" },
                    new() { Name = "Text", Optional = false, Type = "string" },
                }
            };

            var actualCode = ValueObjectGenerator.GenerateValueObjectCode(inputValueObject, generatorConfiguration);
            check.CheckString(actualCode, fileExtension: "cs");
        }
    }
}